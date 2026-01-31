using System.Collections.Generic;
using UnityEngine;

// ============================================================
// BaseHuman.cs - Clase base para jugador y pacientes
// ============================================================

/// <summary>
/// Clase base abstracta para entidades que pueden tener enfermedades.
/// </summary>
public abstract class BaseHuman : MonoBehaviour
{
    [Header("Enfermedades Activas")]
    [SerializeField]
    protected List<DiseaseState> activeDiseases = new List<DiseaseState>();

    /// <summary>
    /// Lista de enfermedades activas (solo lectura).
    /// </summary>
    public IReadOnlyList<DiseaseState> Diseases => activeDiseases;

    /// <summary>
    /// Añade una enfermedad a este humano.
    /// </summary>
    public virtual void AddDisease(DiseaseDefinition definition)
    {
        if (definition == null) return;
        
        // Evitar duplicados
        foreach (var existing in activeDiseases)
        {
            if (existing.id == definition.id)
            {
                Debug.Log($"{gameObject.name} ya tiene {definition.id}");
                return;
            }
        }
        
        var state = new DiseaseState(definition.id, definition.severity);
        activeDiseases.Add(state);
        
        Debug.Log($"{gameObject.name} ha contraído {definition.displayName}");
    }

    /// <summary>
    /// Aplica una cura a todas las enfermedades activas.
    /// Marca que fueron tratadas hoy con esa cura.
    /// </summary>
    public virtual void ApplyCure(CureId cure)
    {
        foreach (var disease in activeDiseases)
        {
            disease.treatedToday = true;
            disease.cureUsedToday = cure;
        }
        
        Debug.Log($"{gameObject.name} fue tratado con {cure}");
    }

    /// <summary>
    /// Aplica una cura específica a una enfermedad concreta.
    /// </summary>
    public virtual void ApplyCureToDisease(CureId cure, DiseaseId targetDisease)
    {
        foreach (var disease in activeDiseases)
        {
            if (disease.id == targetDisease)
            {
                disease.treatedToday = true;
                disease.cureUsedToday = cure;
                Debug.Log($"{gameObject.name}: {targetDisease} tratada con {cure}");
                return;
            }
        }
    }

    /// <summary>
    /// Elimina una enfermedad específica.
    /// </summary>
    public virtual void RemoveDisease(DiseaseId id)
    {
        activeDiseases.RemoveAll(d => d.id == id);
    }

    /// <summary>
    /// Elimina todas las enfermedades.
    /// </summary>
    public virtual void ClearAllDiseases()
    {
        activeDiseases.Clear();
    }

    /// <summary>
    /// Comprueba si tiene una enfermedad específica.
    /// </summary>
    public bool HasDisease(DiseaseId id)
    {
        foreach (var d in activeDiseases)
        {
            if (d.id == id) return true;
        }
        return false;
    }

    /// <summary>
    /// Obtiene el número de enfermedades activas.
    /// </summary>
    public int DiseaseCount => activeDiseases.Count;
}
