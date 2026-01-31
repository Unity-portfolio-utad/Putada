using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace GameSystem.Interaction
{
    /*
        Nuevo BookController:
        - Usa un único AnimationClip (arrastrado en el inspector).
        - Tiene un GameObject objetivo (el hijo que contiene el SpriteRenderer) donde se samplea la animación.
        - Avanza o retrocede N frames (por defecto 10) cuando se pulsa el collider correspondiente.
        - No usa PlayableGraph ni legacy; usa AnimationClip.SampleAnimation(target, time).
    */
    [DisallowMultipleComponent]
    public class BookController : MonoBehaviour
    {
        [Header("Clip y Sprite target")]
        [Tooltip("AnimationClip que contiene la animación de sprites (una sola animación)")]
        public AnimationClip clip;

        [Tooltip("GameObject que contiene el SpriteRenderer (normalmente hijo del libro). La SampleAnimation se aplicará aquí.")]
        public GameObject spriteTarget;

        [Header("Input via Colliders")]
        public Collider nextCollider; // avanza frames
        public Collider prevCollider; // retrocede frames

        [Header("Opciones de stepping")]
        [Tooltip("Cuántos frames avanza/retrocede por pulsación")]
        public int frameStep = 10;

        [Tooltip("Pausa en segundos tras mover frames antes de aceptar otra interacción")]
        public float inputCooldown = 0.05f;

        [Header("Eventos")]
        public UnityEvent<int> onFrameChanged; // envía el frame actual

        // estado
        [SerializeField]
        private int currentFrame = 0;

        private int totalFrames = 1;
        private float frameRate = 30f;
        private bool isCoolingDown = false;

        void Reset()
        {
            frameStep = 10;
            inputCooldown = 0.05f;
        }

        void Awake()
        {
            // auto-assign spriteTarget si hay un SpriteRenderer hijo único
            if (spriteTarget == null)
            {
                var sr = GetComponentInChildren<SpriteRenderer>();
                if (sr != null) spriteTarget = sr.gameObject;
            }

            // conectar colliders con helper
            if (nextCollider != null)
            {
                var btn = nextCollider.gameObject.GetComponent<BookColliderButton>();
                if (btn == null) btn = nextCollider.gameObject.AddComponent<BookColliderButton>();
                btn.controller = this;
                btn.isNext = true;
            }

            if (prevCollider != null)
            {
                var btn = prevCollider.gameObject.GetComponent<BookColliderButton>();
                if (btn == null) btn = prevCollider.gameObject.AddComponent<BookColliderButton>();
                btn.controller = this;
                btn.isNext = false;
            }

            InitializeClipInfo();
            ApplyFrame(currentFrame);
        }

        void InitializeClipInfo()
        {
            if (clip == null)
            {
                totalFrames = 1;
                frameRate = 30f;
                return;
            }

            frameRate = clip.frameRate > 0f ? clip.frameRate : 30f;
            // totalFrames = duración * frameRate, redondeamos a entero
            totalFrames = Mathf.Max(1, Mathf.RoundToInt(clip.length * frameRate));

            // clamp currentFrame
            currentFrame = Mathf.Clamp(currentFrame, 0, totalFrames - 1);
        }

        /// <summary>
        /// Avanza frameStep frames hacia adelante (clamp en límites)
        /// </summary>
        public void StepForward()
        {
            if (isCoolingDown) return;
            if (totalFrames <= 1) return;
            int target = currentFrame + frameStep;
            currentFrame = Mathf.Clamp(target, 0, totalFrames - 1);
            ApplyFrame(currentFrame);
            StartCoroutine(InputCooldown());
        }

        /// <summary>
        /// Retrocede frameStep frames hacia atrás (clamp en límites)
        /// </summary>
        public void StepBackward()
        {
            if (isCoolingDown) return;
            if (totalFrames <= 1) return;
            int target = currentFrame - frameStep;
            currentFrame = Mathf.Clamp(target, 0, totalFrames - 1);
            ApplyFrame(currentFrame);
            StartCoroutine(InputCooldown());
        }

        IEnumerator InputCooldown()
        {
            isCoolingDown = true;
            yield return new WaitForSeconds(inputCooldown);
            isCoolingDown = false;
        }

        void ApplyFrame(int frame)
        {
            if (clip == null || spriteTarget == null)
            {
                // nada que mostrar
                onFrameChanged?.Invoke(frame);
                return;
            }

            // calcular tiempo en segundos del frame
            float time = frame / frameRate;
            time = Mathf.Clamp(time, 0f, clip.length);

            // Sample the animation at that time onto the spriteTarget
            clip.SampleAnimation(spriteTarget, time);

            onFrameChanged?.Invoke(frame);
        }

        [ContextMenu("Recalculate Clip Info")]
        public void Recalculate()
        {
            InitializeClipInfo();
            ApplyFrame(currentFrame);
        }

        // métodos públicos para compatibilidad con nombres anteriores
        public void NextPage() => StepForward();
        public void PrevPage() => StepBackward();

        // Exponer estado
        public int CurrentFrame => currentFrame;
        public int TotalFrames => totalFrames;
    }

    // helper collider
    public class BookColliderButton : MonoBehaviour
    {
        [HideInInspector]
        public BookController controller;

        [HideInInspector]
        public bool isNext = true;

        [Tooltip("Si quieres que el collider responda a triggers, pon aquí la tag del objeto que lo activará (por ejemplo 'Player'). Vacío = no usar trigger.")]
        public string triggerTag = "Player";

        void OnMouseDown()
        {
            if (controller == null) return;
            if (isNext) controller.StepForward(); else controller.StepBackward();
        }

        void OnTriggerEnter(Collider other)
        {
            if (controller == null) return;
            if (string.IsNullOrEmpty(triggerTag)) return;
            if (other.CompareTag(triggerTag))
            {
                if (isNext) controller.StepForward(); else controller.StepBackward();
            }
        }
    }
}
