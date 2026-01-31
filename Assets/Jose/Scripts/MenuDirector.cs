using System.Collections;
using UnityEngine;

public class MenuDirector : MonoBehaviour
{
    [Header("Root (player)")]
    public Transform playerRoot;
    public Transform menuPlayerPoint;
    public Transform gamePlayerPoint; // usaremos SOLO su rotación

    [Header("Gameplay scripts (desactivados en menú/transición)")]
    public MonoBehaviour[] gameplayScripts;

    [Header("UI")]
    public CanvasGroup menuGroup;
    public CanvasGroup fadeGroup; // negro fullscreen

    [Header("Timings")]
    public float menuFadeOutTime = 0.35f;
    public float travelDuration = 1.2f;
    public float fadeToBlackTime = 0.6f;
    public float fadeFromBlackTime = 0.6f;

    [Header("Travel")]
    public float travelForwardDistance = 1.5f;

    bool busy;

    void Start()
    {
        // Colocar root en el menú
        if (playerRoot && menuPlayerPoint)
            playerRoot.SetPositionAndRotation(menuPlayerPoint.position, menuPlayerPoint.rotation);

        SetGameplayEnabled(false);

        // Cursor libre
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // UI inicial
        if (menuGroup)
        {
            menuGroup.alpha = 1f;
            menuGroup.interactable = true;
            menuGroup.blocksRaycasts = true;
        }

        if (fadeGroup)
        {
            fadeGroup.alpha = 0f;
            fadeGroup.interactable = false;
            fadeGroup.blocksRaycasts = false;
        }
    }

    public void OnPlayPressed()
    {
        if (busy) return;
        StartCoroutine(PlaySequence());
    }

    public void OnQuitPressed()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    IEnumerator PlaySequence()
    {
        busy = true;

        // asegúrate de que gameplay no pisa transform durante la animación
        SetGameplayEnabled(false);

        // Cursor libre
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // 1) Fade out menú
        yield return Fade(menuGroup, 1f, 0f, menuFadeOutTime);
        if (menuGroup)
        {
            menuGroup.interactable = false;
            menuGroup.blocksRaycasts = false;
        }

        // 2) Travelling + fade to black al mismo tiempo
        Vector3 startPos = playerRoot.position;
        Quaternion startRot = playerRoot.rotation;

        Vector3 endPos = startPos + (playerRoot.forward * travelForwardDistance);
        Quaternion endRot = startRot;

        float tTravel = 0f;
        float tFade = 0f;

        if (fadeGroup) fadeGroup.blocksRaycasts = true;

        while (tTravel < 1f || tFade < 1f)
        {
            if (tTravel < 1f)
            {
                tTravel += Time.deltaTime / Mathf.Max(0.0001f, travelDuration);
                float k = Mathf.Clamp01(tTravel);

                playerRoot.SetPositionAndRotation(
                    Vector3.Lerp(startPos, endPos, k),
                    Quaternion.Slerp(startRot, endRot, k)
                );
            }

            if (fadeGroup && tFade < 1f)
            {
                tFade += Time.deltaTime / Mathf.Max(0.0001f, fadeToBlackTime);
                fadeGroup.alpha = Mathf.Clamp01(tFade);
            }

            yield return null;
        }

        // 3) YA ESTÁ NEGRO -> TELEPORT A (0,0,0) + ROTACIÓN DEL PUNTO
        if (playerRoot)
        {
            Quaternion targetRot = gamePlayerPoint ? gamePlayerPoint.rotation : playerRoot.rotation;
            playerRoot.SetPositionAndRotation(gamePlayerPoint.position, targetRot);
        }

        // 5) Activar gameplay
        SetGameplayEnabled(true);
        
        // 4) Fade in desde negro
        yield return Fade(fadeGroup, 1f, 0f, fadeFromBlackTime);
        if (fadeGroup) fadeGroup.blocksRaycasts = false;


        // Cursor libre y visible (como pediste)
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        busy = false;
    }

    void SetGameplayEnabled(bool enabled)
    {
        if (gameplayScripts == null) return;
        foreach (var s in gameplayScripts)
            if (s != null) s.enabled = enabled;
    }

    IEnumerator Fade(CanvasGroup cg, float from, float to, float duration)
    {
        if (cg == null) yield break;

        float t = 0f;
        cg.alpha = from;

        while (t < 1f)
        {
            t += Time.deltaTime / Mathf.Max(0.0001f, duration);
            cg.alpha = Mathf.Lerp(from, to, Mathf.Clamp01(t));
            yield return null;
        }

        cg.alpha = to;
    }
}
