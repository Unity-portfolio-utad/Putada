using UnityEngine;

// ============================================================
// AddBlurPercent.cs - Consecuencia: aumentar borrosidad de cámara
// Usado por: Ramas en brazos (tratado correctamente)
// ============================================================

[CreateAssetMenu(fileName = "AddBlur", menuName = "Infectio/Consequences/Add Blur Percent")]
public class AddBlurPercent : ConsequenceDefinition
{
    [Range(0f, 1f)]
    [Tooltip("Cantidad a añadir al porcentaje de blur (0.15 = 15%)")]
    public float addAmount = 0.15f;

    public override void Apply(PlayerStatus playerStatus, int dayIndex)
    {
        if (playerStatus == null) return;
        playerStatus.cameraBlur = Mathf.Clamp01(playerStatus.cameraBlur + addAmount);
    }
}
