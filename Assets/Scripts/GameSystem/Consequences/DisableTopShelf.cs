using UnityEngine;

// ============================================================
// DisableTopShelf.cs - Consecuencia: desactivar balda superior
// Usado por: Lepra (tratado correctamente)
// ============================================================

[CreateAssetMenu(fileName = "DisableTopShelf", menuName = "Infectio/Consequences/Disable Top Shelf")]
public class DisableTopShelf : ConsequenceDefinition
{
    public override void Apply(PlayerStatus playerStatus, int dayIndex)
    {
        if (playerStatus == null) return;
        playerStatus.topShelfDisabled = true;
    }
}
