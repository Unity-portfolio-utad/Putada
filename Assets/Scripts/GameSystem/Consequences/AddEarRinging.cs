using UnityEngine;

// ============================================================
// AddEarRinging.cs - Consecuencia: aumentar pitido en oídos
// Usado por: Otitis (mal tratado o no tratado)
// ============================================================

[CreateAssetMenu(fileName = "AddEarRinging", menuName = "Infectio/Consequences/Add Ear Ringing")]
public class AddEarRinging : ConsequenceDefinition
{
    [Range(0f, 1f)]
    [Tooltip("Cantidad a añadir al porcentaje de pitido")]
    public float addAmount = 0.34f;

    public override void Apply(PlayerStatus playerStatus, int dayIndex)
    {
        if (playerStatus == null) return;
        playerStatus.earRinging = Mathf.Clamp01(playerStatus.earRinging + addAmount);
    }
}
