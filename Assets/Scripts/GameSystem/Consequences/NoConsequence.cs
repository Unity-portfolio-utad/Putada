using UnityEngine;

// ============================================================
// NoConsequence.cs - Consecuencia vacía (sin efecto)
// ============================================================

/// <summary>
/// Consecuencia que no hace nada.
/// Útil para enfermedades leves tratadas correctamente.
/// </summary>
[CreateAssetMenu(fileName = "NoConsequence", menuName = "Infectio/Consequences/No Consequence")]
public class NoConsequence : ConsequenceDefinition
{
    public override void Apply(PlayerStatus playerStatus, int dayIndex)
    {
        // No hace nada
    }
}
