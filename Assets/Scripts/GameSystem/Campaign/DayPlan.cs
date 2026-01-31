using System;
using UnityEngine;

// ============================================================
// DayPlan.cs - Plan de un día específico
// ============================================================

/// <summary>
/// Define la configuración de un día específico:
/// cuántos pacientes, tipos de enfermedades, y regla de infección.
/// </summary>
[Serializable]
public class DayPlan
{
    [Header("Identificación")]
    [Tooltip("Índice del día (0-5)")]
    public int dayIndex;
    
    [Tooltip("Nombre para mostrar (ej: 'Día 1 - Tutorial')")]
    public string dayName = "Día";

    [Header("Pacientes")]
    [Tooltip("Número total de pacientes este día")]
    public int patientCount = 1;

    [Header("Composición de Enfermedades")]
    [Tooltip("Cuántos pacientes tienen enfermedad leve")]
    public int mildCount = 1;
    
    [Tooltip("Cuántos pacientes tienen enfermedad grave")]
    public int severeCount = 0;
    
    [Tooltip("¿Incluir al paciente especial con BodyBugs? (solo día 5)")]
    public bool includesBodyBugsSpecial = false;

    [Header("Regla de Infección del Jugador")]
    [Tooltip("Tipo de regla de infección para este día")]
    public InfectionRuleType infectionRule = InfectionRuleType.None;
    
    [Tooltip("Índice del paciente del que te infectas (si rule = Guaranteed)")]
    public int guaranteedPatientIndex = 0;

    [Header("Texto Tutorial (opcional)")]
    [TextArea(2, 4)]
    public string tutorialText = "";
    
    [Tooltip("¿Es este día un tutorial?")]
    public bool isTutorialDay = false;
}
