using UnityEngine;

// ============================================================
// DiseaseDefinition.cs - ScriptableObject de definición de enfermedad
// ============================================================

/// <summary>
/// Define las propiedades de una enfermedad: severidad, cura correcta,
/// y consecuencias por tratamiento correcto/incorrecto.
/// </summary>
[CreateAssetMenu(fileName = "NewDisease", menuName = "Infectio/Disease Definition")]
public class DiseaseDefinition : ScriptableObject
{
    [Header("Identificación")]
    public DiseaseId id;
    public Severity severity;
    
    [Header("Nombre para UI")]
    public string displayName = "Enfermedad";
    
    [TextArea(2, 4)]
    public string description = "Descripción de la enfermedad";

    [Header("Progresión Leve")]
    [Tooltip("Solo para enfermedades leves: ¿progresa cada día?")]
    public bool isMildProgressive = true;
    
    [Range(0f, 1f)]
    [Tooltip("Cuánto aumenta el progreso cada día (0.34 = 34%)")]
    public float mildDailyIncrease = 0.34f;

    [Header("Tratamiento")]
    public CureId correctCure;
    
    [Tooltip("¿Esta enfermedad grave mata si no se trata? (false para SnailEyes)")]
    public bool killsIfUntreated = true;

    [Header("Consecuencia si se trata CORRECTAMENTE")]
    [Tooltip("Efecto que se aplica si el tratamiento es correcto (secuela estática para graves)")]
    public ConsequenceDefinition consequenceIfTreatedCorrectly;

    [Header("Consecuencia si se trata MAL o NO se trata")]
    [Tooltip("Efecto que se aplica si el tratamiento es incorrecto (síntomas para leves)")]
    public ConsequenceDefinition consequenceIfWrongOrNotTreated;
}
