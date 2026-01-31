using UnityEngine;

// ============================================================
// AddFisheyePercent.cs - Consecuencia: aumentar efecto ojo de pez
// Usado por: Ojos de caracol (tratado correctamente)
// ============================================================

[CreateAssetMenu(fileName = "AddFisheye", menuName = "Infectio/Consequences/Add Fisheye Percent")]
public class AddFisheyePercent : ConsequenceDefinition
{
    [Range(0f, 1f)]
    [Tooltip("Cantidad a añadir al porcentaje de fisheye (0.15 = 15%)")]
    public float addAmount = 0.15f;

    public override void Apply(PlayerStatus playerStatus, int dayIndex)
    {
        if (playerStatus == null) return;
        playerStatus.fisheye = Mathf.Clamp01(playerStatus.fisheye + addAmount);
    }
}
