using UnityEngine;

// ============================================================
// DisableMirror.cs - Consecuencia: desactivar espejo
// Usado por: Ojos de caracol (mal tratado o no tratado)
// ============================================================

[CreateAssetMenu(fileName = "DisableMirror", menuName = "Infectio/Consequences/Disable Mirror")]
public class DisableMirror : ConsequenceDefinition
{
    public override void Apply(PlayerStatus playerStatus, int dayIndex)
    {
        if (playerStatus == null) return;
        playerStatus.mirrorDisabled = true;
    }
}
