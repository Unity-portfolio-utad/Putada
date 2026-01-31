using UnityEngine;

// ============================================================
// CampaignConfig.cs - Configuración de toda la campaña
// ============================================================

/// <summary>
/// ScriptableObject que define la campaña completa del juego:
/// todos los días y sus configuraciones.
/// </summary>
[CreateAssetMenu(fileName = "CampaignConfig", menuName = "Infectio/Campaign Config")]
public class CampaignConfig : ScriptableObject
{
    [Header("Configuración de Campaña")]
    [Tooltip("Todos los planes de día (0-5)")]
    public DayPlan[] days;

    [Header("Mensajes de Muerte")]
    [TextArea(2, 4)]
    public string deathMessageUntreatedSevere = "HAS MUERTO POR UNA ENFERMEDAD GRAVE NO TRATADA.";
    
    [TextArea(2, 4)]
    public string deathMessageMildOverflow = "HAS MUERTO POR ACUMULAR ENFERMEDADES LEVES HASTA EL LÍMITE.";
    
    [TextArea(2, 4)]
    public string deathMessageBodyBugs = "FINAL DEL JUEGO.\nHAS MUERTO.\nNo siempre hay una cura para todo...";
    
    [TextArea(2, 4)]
    public string deathMessageCandle = "FINAL DEL JUEGO.\nHAS MUERTO.\nNo siempre hay una cura para todo...";

    [Header("Tiempos")]
    [Tooltip("Segundos antes de recargar la escena tras morir")]
    public float deathScreenDuration = 5f;

    /// <summary>
    /// Obtiene el plan de un día específico.
    /// </summary>
    public DayPlan GetDay(int index)
    {
        if (days == null || index < 0 || index >= days.Length)
        {
            return null;
        }
        return days[index];
    }

    /// <summary>
    /// Número total de días en la campaña.
    /// </summary>
    public int TotalDays => days?.Length ?? 0;

    /// <summary>
    /// Obtiene el mensaje de muerte según la causa.
    /// </summary>
    public string GetDeathMessage(DeathCause cause)
    {
        switch (cause)
        {
            case DeathCause.UntreatedSevere:
                return deathMessageUntreatedSevere;
            case DeathCause.MildOverflowPlusAnotherAtDawn:
                return deathMessageMildOverflow;
            case DeathCause.BodyBugsFinal:
                return deathMessageBodyBugs;
            case DeathCause.CandleFinal:
                return deathMessageCandle;
            default:
                return "HAS MUERTO.";
        }
    }
}
