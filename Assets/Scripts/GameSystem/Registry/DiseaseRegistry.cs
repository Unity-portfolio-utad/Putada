using System.Collections.Generic;
using UnityEngine;

// ============================================================
// DiseaseRegistry.cs - Registro central de enfermedades
// ============================================================

/// <summary>
/// ScriptableObject que contiene todas las definiciones de enfermedades.
/// Proporciona acceso rápido por ID.
/// </summary>
[CreateAssetMenu(fileName = "DiseaseRegistry", menuName = "Infectio/Disease Registry")]
public class DiseaseRegistry : ScriptableObject
{
    [Header("Todas las Enfermedades")]
    public DiseaseDefinition[] allDiseases;
    
    private Dictionary<DiseaseId, DiseaseDefinition> diseaseMap;

    void OnEnable()
    {
        BuildMap();
    }

    /// <summary>
    /// Construye el diccionario de lookup.
    /// </summary>
    void BuildMap()
    {
        diseaseMap = new Dictionary<DiseaseId, DiseaseDefinition>();
        
        if (allDiseases == null) return;
        
        foreach (var disease in allDiseases)
        {
            if (disease != null && !diseaseMap.ContainsKey(disease.id))
            {
                diseaseMap[disease.id] = disease;
            }
        }
    }

    /// <summary>
    /// Obtiene la definición de una enfermedad por su ID.
    /// </summary>
    public DiseaseDefinition Get(DiseaseId id)
    {
        if (diseaseMap == null) BuildMap();
        
        if (diseaseMap.TryGetValue(id, out var definition))
        {
            return definition;
        }
        
        Debug.LogWarning($"DiseaseRegistry: No se encontró definición para {id}");
        return null;
    }

    /// <summary>
    /// Obtiene todas las enfermedades leves.
    /// </summary>
    public List<DiseaseDefinition> GetAllMild()
    {
        var result = new List<DiseaseDefinition>();
        foreach (var d in allDiseases)
        {
            if (d != null && d.severity == Severity.Mild)
                result.Add(d);
        }
        return result;
    }

    /// <summary>
    /// Obtiene todas las enfermedades graves (excluyendo especiales).
    /// </summary>
    public List<DiseaseDefinition> GetAllSevere()
    {
        var result = new List<DiseaseDefinition>();
        foreach (var d in allDiseases)
        {
            if (d != null && d.severity == Severity.Severe)
                result.Add(d);
        }
        return result;
    }
}
