using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

// ============================================================
// GameUIManager.cs - Gestor de UI del juego
// ============================================================

/// <summary>
/// Gestiona toda la UI del juego: mensajes, paneles de muerte,
/// información de día, etc.
/// </summary>
public class GameUIManager : MonoBehaviour
{
    public static GameUIManager Instance { get; private set; }

    [Header("Paneles")]
    [Tooltip("Panel de muerte/game over")]
    public CanvasGroup deathPanel;
    
    [Tooltip("Panel de información de día")]
    public CanvasGroup dayInfoPanel;
    
    [Tooltip("Panel de autoexamen")]
    public CanvasGroup selfExamPanel;
    
    [Tooltip("Panel de mensajes temporales")]
    public CanvasGroup messagePanel;

    [Header("Textos")]
    [Tooltip("Texto de muerte")]
    public TextMeshProUGUI deathText;
    
    [Tooltip("Texto de día actual")]
    public TextMeshProUGUI dayText;
    
    [Tooltip("Texto de pacientes muertos")]
    public TextMeshProUGUI deadPatientsText;
    
    [Tooltip("Texto de mensaje temporal")]
    public TextMeshProUGUI messageText;
    
    [Tooltip("Texto de enfermedades del jugador")]
    public TextMeshProUGUI playerDiseasesText;

    [Header("Botones")]
    [Tooltip("Botón para terminar día (después de autoexamen)")]
    public Button endDayButton;

    [Header("Tiempos")]
    public float messageDuration = 3f;
    public float panelFadeDuration = 0.5f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        // Ocultar todos los paneles al inicio
        HideAllPanels();
    }

    /// <summary>
    /// Oculta todos los paneles.
    /// </summary>
    public void HideAllPanels()
    {
        SetPanelVisible(deathPanel, false);
        SetPanelVisible(dayInfoPanel, false);
        SetPanelVisible(selfExamPanel, false);
        SetPanelVisible(messagePanel, false);
    }

    /// <summary>
    /// Muestra el panel de muerte con el mensaje.
    /// </summary>
    public void ShowDeathScreen(string message)
    {
        if (deathText != null)
        {
            deathText.text = message;
        }
        SetPanelVisible(deathPanel, true);
    }

    /// <summary>
    /// Muestra la información del día.
    /// </summary>
    public void ShowDayInfo(int dayIndex, string dayName)
    {
        if (dayText != null)
        {
            dayText.text = $"DÍA {dayIndex}: {dayName}";
        }
        StartCoroutine(ShowPanelTemporary(dayInfoPanel, 3f));
    }

    /// <summary>
    /// Muestra el panel de autoexamen.
    /// </summary>
    public void ShowSelfExamPanel(string diseaseSummary)
    {
        if (playerDiseasesText != null)
        {
            playerDiseasesText.text = diseaseSummary;
        }
        SetPanelVisible(selfExamPanel, true);
    }

    /// <summary>
    /// Oculta el panel de autoexamen.
    /// </summary>
    public void HideSelfExamPanel()
    {
        SetPanelVisible(selfExamPanel, false);
    }

    /// <summary>
    /// Actualiza el contador de pacientes muertos.
    /// </summary>
    public void UpdateDeadPatients(int count)
    {
        if (deadPatientsText != null)
        {
            deadPatientsText.text = $"Pacientes muertos hoy: {count}";
        }
    }

    /// <summary>
    /// Muestra un mensaje temporal.
    /// </summary>
    public void ShowMessage(string message)
    {
        if (messageText != null)
        {
            messageText.text = message;
        }
        StartCoroutine(ShowPanelTemporary(messagePanel, messageDuration));
    }

    /// <summary>
    /// Muestra un panel temporalmente.
    /// </summary>
    IEnumerator ShowPanelTemporary(CanvasGroup panel, float duration)
    {
        SetPanelVisible(panel, true);
        yield return new WaitForSeconds(duration);
        SetPanelVisible(panel, false);
    }

    /// <summary>
    /// Activa/desactiva un panel.
    /// </summary>
    void SetPanelVisible(CanvasGroup panel, bool visible)
    {
        if (panel == null) return;
        
        panel.alpha = visible ? 1f : 0f;
        panel.interactable = visible;
        panel.blocksRaycasts = visible;
    }

    /// <summary>
    /// Fade de un panel.
    /// </summary>
    public IEnumerator FadePanel(CanvasGroup panel, float from, float to, float duration)
    {
        if (panel == null) yield break;

        float elapsed = 0f;
        panel.alpha = from;
        
        if (to > 0)
        {
            panel.interactable = true;
            panel.blocksRaycasts = true;
        }

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            panel.alpha = Mathf.Lerp(from, to, elapsed / duration);
            yield return null;
        }

        panel.alpha = to;
        
        if (to <= 0)
        {
            panel.interactable = false;
            panel.blocksRaycasts = false;
        }
    }
}
