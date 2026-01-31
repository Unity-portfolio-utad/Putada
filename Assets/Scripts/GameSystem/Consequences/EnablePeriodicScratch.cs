using UnityEngine;

// ============================================================
// EnablePeriodicScratch.cs - Consecuencia: activar sonido rascar
// Usado por: Sarpullido (mal tratado o no tratado)
// ============================================================

[CreateAssetMenu(fileName = "EnableScratch", menuName = "Infectio/Consequences/Enable Periodic Scratch")]
public class EnablePeriodicScratch : ConsequenceDefinition
{
    [Tooltip("Intervalo en segundos entre cada sonido de rascar")]
    public float intervalSeconds = 10f;

    public override void Apply(PlayerStatus playerStatus, int dayIndex)
    {
        if (playerStatus == null) return;
        playerStatus.scratchEnabled = true;
        playerStatus.scratchInterval = intervalSeconds;
    }
}
