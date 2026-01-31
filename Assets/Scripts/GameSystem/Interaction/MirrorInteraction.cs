using UnityEngine;

// ============================================================
// MirrorInteraction.cs - Interacción con el espejo
// ============================================================

/// <summary>
/// Gestiona la interacción con el espejo.
/// Si el jugador tiene ojos de caracol no tratado, no puede usarlo.
/// </summary>
[RequireComponent(typeof(Collider))]
public class MirrorInteraction : MonoBehaviour
{
    [Header("Referencias")]
    [Tooltip("PlayerStatus para verificar estado")]
    public PlayerStatus playerStatus;

    [Header("Mensajes")]
    [TextArea(2, 3)]
    public string disabledMessage = "Desde la enfermedad del caracol no te puedes ni mirar a la cara...";

    void Start()
    {
        if (playerStatus == null)
        {
            playerStatus = FindFirstObjectByType<PlayerStatus>();
        }
    }

    void OnMouseDown()
    {
        if (playerStatus != null && playerStatus.mirrorDisabled)
        {
            // No puede usar el espejo
            GameUIManager.Instance?.ShowMessage(disabledMessage);
            return;
        }
        
        // Usar el espejo normalmente
        UseMirror();
    }

    /// <summary>
    /// Lógica de usar el espejo.
    /// Puedes añadir aquí lo que quieras que pase.
    /// </summary>
    void UseMirror()
    {
        Debug.Log("Usando espejo...");
        // Aquí podrías mostrar UI de autoexamen, ver tu cara, etc.
    }
}
