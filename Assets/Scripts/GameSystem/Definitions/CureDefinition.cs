using UnityEngine;

// ============================================================
// CureDefinition.cs - ScriptableObject de definición de cura
// ============================================================

/// <summary>
/// Define las propiedades de un objeto de cura.
/// </summary>
[CreateAssetMenu(fileName = "NewCure", menuName = "Infectio/Cure Definition")]
public class CureDefinition : ScriptableObject
{
    public CureId id;
    
    [Header("Nombre para UI")]
    public string displayName = "Cura";
    
    [TextArea(2, 4)]
    public string description = "Descripción de la cura";
    
    [Header("Ubicación")]
    [Tooltip("¿Está en la balda superior? (se desactiva con lepra tratada)")]
    public bool isOnTopShelf = false;
}
