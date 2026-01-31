using UnityEngine;

// ============================================================
// AddBreathingNoise.cs - Consecuencia: aumentar ruido respiración
// Usado por: Catarro (mal tratado o no tratado)
// ============================================================

[CreateAssetMenu(fileName = "AddBreathingNoise", menuName = "Infectio/Consequences/Add Breathing Noise")]
public class AddBreathingNoise : ConsequenceDefinition
{
    [Range(0f, 1f)]
    [Tooltip("Cantidad a añadir al porcentaje de ruido respiración")]
    public float addAmount = 0.34f;

    public override void Apply(PlayerStatus playerStatus, int dayIndex)
    {
        if (playerStatus == null) return;
        playerStatus.breathingNoise = Mathf.Clamp01(playerStatus.breathingNoise + addAmount);
    }
}
