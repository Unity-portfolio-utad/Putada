using UnityEngine;

// ============================================================
// Patient.cs - Paciente que viene a ser tratado
// ============================================================

/// <summary>
/// Representa un paciente que viene cada día a ser tratado.
/// Si no se le trata correctamente, aumenta el contador de muertes.
/// </summary>
public class Patient : BaseHuman
{
    [Header("Estado")]
    [SerializeField]
    private bool willDieAtEndOfDay = false;

    /// <summary>
    /// ¿Morirá este paciente al final del día?
    /// </summary>
    public bool WillDieAtEndOfDay => willDieAtEndOfDay;

    /// <summary>
    /// Resuelve el estado del paciente al final del día.
    /// Determina si muere basándose en si fue tratado correctamente.
    /// </summary>
    /// <param name="registry">Registro de enfermedades</param>
    /// <returns>true si el paciente muere</returns>
    public bool ResolveEndOfDay(DiseaseRegistry registry)
    {
        willDieAtEndOfDay = false;

        foreach (var disease in activeDiseases)
        {
            var definition = registry.Get(disease.id);
            if (definition == null) continue;

            // Verificar si fue tratado correctamente
            bool treatedCorrectly = disease.treatedToday && 
                                    disease.cureUsedToday == definition.correctCure;

            if (!treatedCorrectly)
            {
                willDieAtEndOfDay = true;
                Debug.Log($"Paciente muere: {definition.displayName} no tratada correctamente");
                break;
            }
        }

        // Limpiar flags diarios
        foreach (var disease in activeDiseases)
        {
            disease.ResetDailyFlags();
        }

        return willDieAtEndOfDay;
    }

    /// <summary>
    /// Obtiene la primera enfermedad del paciente (para infección).
    /// </summary>
    public DiseaseId? GetPrimaryDisease()
    {
        if (activeDiseases.Count > 0)
        {
            return activeDiseases[0].id;
        }
        return null;
    }

    /// <summary>
    /// Reinicia el paciente para reutilizar.
    /// </summary>
    public void Reset()
    {
        ClearAllDiseases();
        willDieAtEndOfDay = false;
    }
}
