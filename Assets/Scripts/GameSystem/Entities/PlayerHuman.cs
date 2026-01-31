using System.Collections.Generic;
using UnityEngine;

// ============================================================
// PlayerHuman.cs - El jugador con sistema de enfermedades
// ============================================================

/// <summary>
/// El jugador del juego. Gestiona sus enfermedades, progresión,
/// secuelas y evaluación de muerte al final de cada día.
/// </summary>
public class PlayerHuman : BaseHuman
{
    [Header("Referencias")]
    public PlayerStatus status;

    [Header("Debug")]
    [SerializeField]
    private DeathCause lastDeathCause = DeathCause.None;

    /// <summary>
    /// Última causa de muerte evaluada.
    /// </summary>
    public DeathCause LastDeathCause => lastDeathCause;

    void Awake()
    {
        if (status == null)
        {
            status = GetComponent<PlayerStatus>();
        }
    }

    /// <summary>
    /// Resuelve el estado del jugador al final del día.
    /// Aplica consecuencias, progresa enfermedades y determina si muere.
    /// </summary>
    /// <param name="registry">Registro de enfermedades</param>
    /// <param name="dayIndex">Día actual (0-5)</param>
    /// <param name="isFinalDay">¿Es el día final (5)?</param>
    /// <returns>Causa de muerte o None si sobrevive</returns>
    public DeathCause ResolveEndOfDay(DiseaseRegistry registry, int dayIndex, bool isFinalDay)
    {
        lastDeathCause = DeathCause.None;
        
        // Lista para enfermedades a eliminar (tratadas correctamente)
        var toRemove = new List<DiseaseId>();
        
        // Contar cuántas enfermedades habrá al amanecer
        int diseasesAtDawn = activeDiseases.Count;

        // ========== FASE 1: EVALUAR GRAVES Y ESPECIALES ==========
        
        foreach (var disease in activeDiseases)
        {
            var definition = registry.Get(disease.id);
            if (definition == null) continue;

            bool treatedCorrectly = disease.treatedToday && 
                                    disease.cureUsedToday == definition.correctCure;

            // ESPECIAL: BodyBugs (día 5)
            if (definition.severity == Severity.Special)
            {
                if (isFinalDay)
                {
                    // Siempre mueres, pero el mensaje cambia
                    if (disease.treatedToday && disease.cureUsedToday == CureId.Candle)
                    {
                        lastDeathCause = DeathCause.CandleFinal;
                    }
                    else
                    {
                        lastDeathCause = DeathCause.BodyBugsFinal;
                    }
                    return lastDeathCause;
                }
            }

            // GRAVES: muerte si no se tratan (excepto SnailEyes)
            if (definition.severity == Severity.Severe)
            {
                if (!treatedCorrectly && definition.killsIfUntreated)
                {
                    lastDeathCause = DeathCause.UntreatedSevere;
                    return lastDeathCause;
                }
            }
        }

        // ========== FASE 2: APLICAR CONSECUENCIAS Y PROGRESIÓN ==========
        
        foreach (var disease in activeDiseases)
        {
            var definition = registry.Get(disease.id);
            if (definition == null) continue;

            bool treatedCorrectly = disease.treatedToday && 
                                    disease.cureUsedToday == definition.correctCure;

            // ----- LEVES -----
            if (definition.severity == Severity.Mild)
            {
                if (treatedCorrectly)
                {
                    // Leve tratada correctamente: se cura, sin secuelas
                    toRemove.Add(disease.id);
                    diseasesAtDawn--;
                    Debug.Log($"Jugador curado de {definition.displayName}");
                }
                else
                {
                    // Leve no tratada o mal tratada: progresa
                    if (definition.isMildProgressive)
                    {
                        disease.mildProgress += definition.mildDailyIncrease;
                        Debug.Log($"{definition.displayName} progresa: {disease.mildProgress:P0}");
                    }
                    
                    // Aplicar consecuencia de mal tratamiento
                    if (definition.consequenceIfWrongOrNotTreated != null)
                    {
                        definition.consequenceIfWrongOrNotTreated.Apply(status, dayIndex);
                    }
                }
            }
            
            // ----- GRAVES -----
            else if (definition.severity == Severity.Severe)
            {
                if (treatedCorrectly)
                {
                    // Grave tratada correctamente: aplicar secuela estática
                    toRemove.Add(disease.id);
                    diseasesAtDawn--;
                    
                    if (definition.consequenceIfTreatedCorrectly != null)
                    {
                        definition.consequenceIfTreatedCorrectly.Apply(status, dayIndex);
                        Debug.Log($"Jugador curado de {definition.displayName} con secuela");
                    }
                }
                else
                {
                    // SnailEyes no mata pero aplica consecuencia
                    if (!definition.killsIfUntreated)
                    {
                        if (definition.consequenceIfWrongOrNotTreated != null)
                        {
                            definition.consequenceIfWrongOrNotTreated.Apply(status, dayIndex);
                        }
                        // No se elimina, sigue activa
                    }
                }
            }

            // Incrementar días activa
            disease.daysActive++;
        }

        // ========== FASE 3: ELIMINAR ENFERMEDADES CURADAS ==========
        
        foreach (var id in toRemove)
        {
            RemoveDisease(id);
        }

        // ========== FASE 4: EVALUAR MUERTE POR LEVES ==========
        
        // Muerte si alguna leve supera 100% Y hay otra enfermedad al amanecer
        bool hasAnotherAtDawn = diseasesAtDawn > 1;
        
        foreach (var disease in activeDiseases)
        {
            if (disease.severity == Severity.Mild && disease.mildProgress > 1f)
            {
                if (hasAnotherAtDawn)
                {
                    lastDeathCause = DeathCause.MildOverflowPlusAnotherAtDawn;
                    return lastDeathCause;
                }
            }
        }

        // ========== FASE 5: LIMPIAR FLAGS DIARIOS ==========
        
        foreach (var disease in activeDiseases)
        {
            disease.treatedToday = false;
            disease.cureUsedToday = CureId.None;
        }

        return DeathCause.None;
    }

    /// <summary>
    /// Reinicia completamente al jugador.
    /// </summary>
    public void ResetPlayer()
    {
        ClearAllDiseases();
        lastDeathCause = DeathCause.None;
        
        if (status != null)
        {
            status.ResetAllEffects();
        }
    }

    /// <summary>
    /// Obtiene un resumen de las enfermedades activas para UI.
    /// </summary>
    public string GetDiseaseSummary(DiseaseRegistry registry)
    {
        if (activeDiseases.Count == 0)
        {
            return "Estás sano.";
        }

        var summary = "Tus enfermedades:\n";
        foreach (var disease in activeDiseases)
        {
            var def = registry.Get(disease.id);
            if (def != null)
            {
                summary += $"- {def.displayName}";
                if (disease.severity == Severity.Mild)
                {
                    summary += $" ({disease.mildProgress:P0})";
                }
                summary += "\n";
            }
        }
        return summary;
    }
}
