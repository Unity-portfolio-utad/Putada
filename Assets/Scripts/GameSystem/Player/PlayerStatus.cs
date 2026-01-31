using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

// ============================================================
// PlayerStatus.cs - Estado del jugador (efectos, secuelas, flags)
// ============================================================

/// <summary>
/// Gestiona todos los efectos visuales y de audio que afectan al jugador
/// como resultado de enfermedades y secuelas.
/// </summary>
public class PlayerStatus : MonoBehaviour
{
    [Header("Referencias de Post-Processing")]
    [Tooltip("Volume de post-procesado para efectos visuales")]
    public Volume postProcessVolume;
    
    [Header("Referencias de Audio")]
    [Tooltip("AudioSource para sonido de respiración")]
    public AudioSource breathingAudioSource;
    
    [Tooltip("AudioSource para pitido de oídos")]
    public AudioSource earRingingAudioSource;
    
    [Tooltip("AudioSource para sonido de rascar")]
    public AudioSource scratchAudioSource;
    
    [Tooltip("Clip de sonido de rascar")]
    public AudioClip scratchClip;

    [Header("Referencias de Gameplay")]
    [Tooltip("GameObject del espejo (se desactiva con ojos caracol no tratado)")]
    public GameObject mirrorObject;
    
    [Tooltip("GameObjects de la balda superior (se desactivan con lepra tratada)")]
    public GameObject[] topShelfObjects;

    // ==================== VALORES PORCENTUALES (0..1) ====================
    
    [Header("Efectos Visuales (0-1)")]
    [Range(0f, 1f)]
    [Tooltip("Porcentaje de borrosidad de cámara")]
    public float cameraBlur = 0f;
    
    [Range(0f, 1f)]
    [Tooltip("Porcentaje de efecto ojo de pez")]
    public float fisheye = 0f;

    [Header("Efectos de Audio (0-1)")]
    [Range(0f, 1f)]
    [Tooltip("Porcentaje de ruido de respiración")]
    public float breathingNoise = 0f;
    
    [Range(0f, 1f)]
    [Tooltip("Porcentaje de pitido en oídos")]
    public float earRinging = 0f;

    // ==================== FLAGS DE GAMEPLAY ====================
    
    [Header("Flags de Estado")]
    [Tooltip("¿El espejo está desactivado?")]
    public bool mirrorDisabled = false;
    
    [Tooltip("¿La balda superior está desactivada?")]
    public bool topShelfDisabled = false;
    
    [Tooltip("¿Está activo el sonido periódico de rascar?")]
    public bool scratchEnabled = false;
    
    [Tooltip("Intervalo entre sonidos de rascar")]
    public float scratchInterval = 10f;

    // ==================== PRIVADOS ====================
    
    private float scratchTimer = 0f;
    private DepthOfField depthOfField;
    private LensDistortion lensDistortion;

    void Start()
    {
        // Intentar obtener efectos de post-processing
        if (postProcessVolume != null && postProcessVolume.profile != null)
        {
            postProcessVolume.profile.TryGet(out depthOfField);
            postProcessVolume.profile.TryGet(out lensDistortion);
        }
        
        // Inicializar timer de rascar
        scratchTimer = scratchInterval;
    }

    void Update()
    {
        // Aplicar efectos visuales
        ApplyVisualEffects();
        
        // Aplicar efectos de audio
        ApplyAudioEffects();
        
        // Gestionar gameplay flags
        ApplyGameplayFlags();
        
        // Sonido periódico de rascar
        HandlePeriodicScratch();
    }

    /// <summary>
    /// Aplica los efectos visuales basados en los valores porcentuales.
    /// </summary>
    void ApplyVisualEffects()
    {
        // Blur (Depth of Field)
        if (depthOfField != null)
        {
            // Ajustar apertura basado en blur (más blur = apertura más baja)
            depthOfField.active = cameraBlur > 0.01f;
            if (depthOfField.active)
            {
                // Focal length y aperture para simular blur
                depthOfField.focusDistance.value = Mathf.Lerp(10f, 0.5f, cameraBlur);
            }
        }
        
        // Fisheye (Lens Distortion)
        if (lensDistortion != null)
        {
            lensDistortion.active = fisheye > 0.01f;
            if (lensDistortion.active)
            {
                // Distorsión negativa para efecto fisheye
                lensDistortion.intensity.value = Mathf.Lerp(0f, -0.5f, fisheye);
            }
        }
    }

    /// <summary>
    /// Aplica los efectos de audio basados en los valores porcentuales.
    /// </summary>
    void ApplyAudioEffects()
    {
        // Respiración
        if (breathingAudioSource != null)
        {
            breathingAudioSource.volume = breathingNoise;
            if (breathingNoise > 0.01f && !breathingAudioSource.isPlaying)
            {
                breathingAudioSource.loop = true;
                breathingAudioSource.Play();
            }
            else if (breathingNoise <= 0.01f && breathingAudioSource.isPlaying)
            {
                breathingAudioSource.Stop();
            }
        }
        
        // Pitido de oídos
        if (earRingingAudioSource != null)
        {
            earRingingAudioSource.volume = earRinging;
            if (earRinging > 0.01f && !earRingingAudioSource.isPlaying)
            {
                earRingingAudioSource.loop = true;
                earRingingAudioSource.Play();
            }
            else if (earRinging <= 0.01f && earRingingAudioSource.isPlaying)
            {
                earRingingAudioSource.Stop();
            }
        }
    }

    /// <summary>
    /// Aplica los flags de gameplay (espejo, balda superior).
    /// </summary>
    void ApplyGameplayFlags()
    {
        // Espejo
        if (mirrorObject != null)
        {
            mirrorObject.SetActive(!mirrorDisabled);
        }
        
        // Balda superior
        if (topShelfObjects != null && topShelfDisabled)
        {
            foreach (var obj in topShelfObjects)
            {
                if (obj != null)
                {
                    obj.SetActive(false);
                }
            }
        }
    }

    /// <summary>
    /// Gestiona el sonido periódico de rascar (sarpullido mal tratado).
    /// </summary>
    void HandlePeriodicScratch()
    {
        if (!scratchEnabled) return;
        
        scratchTimer -= Time.deltaTime;
        
        if (scratchTimer <= 0f)
        {
            scratchTimer = scratchInterval;
            
            if (scratchAudioSource != null && scratchClip != null)
            {
                scratchAudioSource.PlayOneShot(scratchClip);
            }
        }
    }

    /// <summary>
    /// Resetea todos los efectos a valores por defecto.
    /// Útil para reiniciar el juego.
    /// </summary>
    public void ResetAllEffects()
    {
        cameraBlur = 0f;
        fisheye = 0f;
        breathingNoise = 0f;
        earRinging = 0f;
        mirrorDisabled = false;
        topShelfDisabled = false;
        scratchEnabled = false;
        scratchInterval = 10f;
        scratchTimer = 10f;
    }

    /// <summary>
    /// Intenta usar el espejo. Devuelve false si está desactivado.
    /// </summary>
    public bool TryUseMirror()
    {
        if (mirrorDisabled)
        {
            // Mostrar mensaje: "Desde la enfermedad del caracol no te puedes ni mirar a la cara"
            GameUIManager.Instance?.ShowMessage("Desde la enfermedad del caracol no te puedes ni mirar a la cara...");
            return false;
        }
        return true;
    }
}
