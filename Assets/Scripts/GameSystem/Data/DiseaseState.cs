using System;

// ============================================================
// DiseaseState.cs - Estado de runtime de una enfermedad activa
// ============================================================

/// <summary>
/// Representa el estado en tiempo de ejecución de una enfermedad
/// que un humano (jugador o paciente) tiene activa.
/// </summary>
[Serializable]
public class DiseaseState
{
    public DiseaseId id;
    public Severity severity;

    /// <summary>
    /// Progreso de enfermedad leve (0..1). Aumenta 0.34 cada día no tratada.
    /// Si supera 1.0 y tienes otra enfermedad al amanecer, mueres.
    /// </summary>
    public float mildProgress;

    /// <summary>
    /// Si la enfermedad fue tratada hoy.
    /// </summary>
    public bool treatedToday;

    /// <summary>
    /// Qué cura se usó hoy (si alguna).
    /// </summary>
    public CureId cureUsedToday;

    /// <summary>
    /// Días que lleva activa esta enfermedad.
    /// </summary>
    public int daysActive;

    public DiseaseState()
    {
        id = DiseaseId.Catarrh;
        severity = Severity.Mild;
        mildProgress = 0f;
        treatedToday = false;
        cureUsedToday = CureId.None;
        daysActive = 0;
    }

    public DiseaseState(DiseaseId diseaseId, Severity sev)
    {
        id = diseaseId;
        severity = sev;
        mildProgress = 0f;
        treatedToday = false;
        cureUsedToday = CureId.None;
        daysActive = 0;
    }

    /// <summary>
    /// Resetea los flags diarios al pasar al siguiente día.
    /// </summary>
    public void ResetDailyFlags()
    {
        treatedToday = false;
        cureUsedToday = CureId.None;
        daysActive++;
    }
}
