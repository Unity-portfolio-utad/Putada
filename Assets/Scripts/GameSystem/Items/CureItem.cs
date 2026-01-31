using UnityEngine;

// ============================================================
// CureItem.cs - Objeto de cura interactivo
// ============================================================

/// <summary>
/// Representa un objeto de cura en la estantería.
/// Reemplaza/complementa ObjetoRecogible para integrarse
/// con el sistema de enfermedades.
/// </summary>
public class CureItem : MonoBehaviour
{
    [Header("Configuración")]
    [Tooltip("ID de la cura que representa este objeto")]
    public CureId cureId = CureId.Syrup;
    
    [Tooltip("¿Está en la balda superior?")]
    public bool isOnTopShelf = false;
    
    [Tooltip("Definición de cura (opcional, para info adicional)")]
    public CureDefinition cureDefinition;

    [Header("Estado")]
    [SerializeField]
    private bool isHeld = false;
    
    [SerializeField]
    private bool isDisabled = false;

    // Referencia al sistema
    private DayCycleOrchestrator orchestrator;
    private PlayerStatus playerStatus;

    void Start()
    {
        orchestrator = DayCycleOrchestrator.Instance;
        playerStatus = FindFirstObjectByType<PlayerStatus>();
    }

    void Update()
    {
        // Si está en balda superior y está desactivada, deshabilitar
        if (isOnTopShelf && playerStatus != null && playerStatus.topShelfDisabled && !isDisabled)
        {
            isDisabled = true;
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Llamado cuando el jugador hace clic en el objeto.
    /// </summary>
    public void OnMouseDown()
    {
        if (isDisabled) return;
        
        if (!isHeld)
        {
            // Recoger objeto
            PickUp();
        }
        else
        {
            // Soltar objeto
            PutDown();
        }
    }

    /// <summary>
    /// Recoge el objeto.
    /// </summary>
    void PickUp()
    {
        isHeld = true;
        Debug.Log($"Recogido: {cureId}");
        
        // Aquí podrías cambiar visual del objeto o moverlo a la mano
    }

    /// <summary>
    /// Suelta el objeto.
    /// </summary>
    void PutDown()
    {
        isHeld = false;
        Debug.Log($"Soltado: {cureId}");
    }

    /// <summary>
    /// Usa el objeto en un paciente o en el jugador.
    /// </summary>
    public void UseOnPatient()
    {
        if (!isHeld || orchestrator == null) return;
        
        orchestrator.TreatCurrentPatient(cureId);
        isHeld = false;
        
        Debug.Log($"Usado {cureId} en paciente");
    }

    /// <summary>
    /// Usa el objeto en el jugador (autoexamen).
    /// </summary>
    public void UseOnSelf()
    {
        if (!isHeld || orchestrator == null) return;
        
        orchestrator.SelfTreatAndEndDay(cureId);
        isHeld = false;
        
        Debug.Log($"Usado {cureId} en jugador");
    }

    /// <summary>
    /// ¿Está siendo sostenido?
    /// </summary>
    public bool IsHeld => isHeld;
}
