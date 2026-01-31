using UnityEngine;

// ============================================================
// GameSystemBridge.cs - Puente entre sistemas antiguo y nuevo
// ============================================================

/// <summary>
/// Conecta el sistema de items existente (Personaje, Armario, etc.)
/// con el nuevo sistema de enfermedades y días.
/// </summary>
public class GameSystemBridge : MonoBehaviour
{
    public static GameSystemBridge Instance { get; private set; }

    [Header("Referencias Sistema Antiguo")]
    public Personaje personaje;
    public Armario armario;

    [Header("Referencias Sistema Nuevo")]
    public DayCycleOrchestrator orchestrator;
    public PlayerHuman playerHuman;
    public PlayerStatus playerStatus;

    [Header("Mapeo Items a Curas")]
    [Tooltip("¿Usar el sistema nuevo directamente?")]
    public bool useNewSystem = true;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        // Auto-encontrar referencias si no están asignadas
        if (personaje == null)
            personaje = FindFirstObjectByType<Personaje>();
        
        if (armario == null)
            armario = FindFirstObjectByType<Armario>();
            
        if (orchestrator == null)
            orchestrator = DayCycleOrchestrator.Instance;
            
        if (playerHuman == null)
            playerHuman = FindFirstObjectByType<PlayerHuman>();
            
        if (playerStatus == null)
            playerStatus = FindFirstObjectByType<PlayerStatus>();
    }

    /// <summary>
    /// Convierte un Item del sistema antiguo a CureId del nuevo.
    /// </summary>
    public static CureId ConvertItemToCure(Personaje.Items item)
    {
        switch (item)
        {
            case Personaje.Items.JARABE:
                return CureId.Syrup;
            case Personaje.Items.CREMA:
                return CureId.Cream;
            case Personaje.Items.LIJAS:
                return CureId.Sandpaper;
            case Personaje.Items.SIERRA:
                return CureId.Saw;
            case Personaje.Items.SAL:
                return CureId.Salt;
            case Personaje.Items.PICO:
                return CureId.Pickaxe;
            case Personaje.Items.CERILLAS:
                return CureId.Candle;
            default:
                return CureId.None;
        }
    }

    /// <summary>
    /// Convierte CureId a Item del sistema antiguo.
    /// </summary>
    public static Personaje.Items ConvertCureToItem(CureId cure)
    {
        switch (cure)
        {
            case CureId.Syrup:
                return Personaje.Items.JARABE;
            case CureId.Cream:
                return Personaje.Items.CREMA;
            case CureId.Sandpaper:
                return Personaje.Items.LIJAS;
            case CureId.Saw:
                return Personaje.Items.SIERRA;
            case CureId.Salt:
                return Personaje.Items.SAL;
            case CureId.Pickaxe:
                return Personaje.Items.PICO;
            case CureId.Candle:
                return Personaje.Items.CERILLAS;
            default:
                return Personaje.Items.NULL;
        }
    }

    /// <summary>
    /// Aplica una cura al paciente actual usando el item activo del sistema antiguo.
    /// </summary>
    public void ApplyCureFromActiveItem()
    {
        if (!useNewSystem || orchestrator == null) return;
        
        // Obtener item activo del personaje (necesitaría exposer esto)
        // Por ahora, esto es un placeholder
        Debug.Log("ApplyCureFromActiveItem llamado");
    }

    /// <summary>
    /// Notifica al sistema nuevo que se usó un item del sistema antiguo.
    /// </summary>
    public void NotifyItemUsed(Personaje.Items item, bool onPatient)
    {
        if (!useNewSystem || orchestrator == null) return;
        
        CureId cure = ConvertItemToCure(item);
        
        if (onPatient)
        {
            orchestrator.TreatCurrentPatient(cure);
        }
        else
        {
            orchestrator.SelfTreatAndEndDay(cure);
        }
    }
}
