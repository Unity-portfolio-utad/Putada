using UnityEngine;

// ============================================================
// LegacyItemAdapter.cs - Adapta ObjetoRecogible al sistema nuevo
// ============================================================

/// <summary>
/// Añade este componente a los objetos que ya tienen ObjetoRecogible
/// para que también funcionen con el sistema nuevo de enfermedades.
/// </summary>
[RequireComponent(typeof(ObjetoRecogible))]
public class LegacyItemAdapter : MonoBehaviour
{
    private ObjetoRecogible objetoRecogible;
    private Personaje.Items lastKnownItem;

    [Header("Configuración")]
    [Tooltip("¿Este objeto está en la balda superior?")]
    public bool isOnTopShelf = false;

    private PlayerStatus playerStatus;

    void Awake()
    {
        objetoRecogible = GetComponent<ObjetoRecogible>();
    }

    void Start()
    {
        playerStatus = FindFirstObjectByType<PlayerStatus>();
        lastKnownItem = objetoRecogible.itemType;
    }

    void Update()
    {
        // Si la balda superior está desactivada y este objeto está ahí
        if (isOnTopShelf && playerStatus != null && playerStatus.topShelfDisabled)
        {
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Obtiene el CureId equivalente de este objeto.
    /// </summary>
    public CureId GetCureId()
    {
        return GameSystemBridge.ConvertItemToCure(objetoRecogible.itemType);
    }

    /// <summary>
    /// Configura qué objetos están en la balda superior.
    /// Según tu diseño: Sierra y Jarabe.
    /// </summary>
    public static void SetupTopShelfItems()
    {
        var adapters = FindObjectsByType<LegacyItemAdapter>(FindObjectsSortMode.None);
        foreach (var adapter in adapters)
        {
            var item = adapter.GetComponent<ObjetoRecogible>();
            if (item != null)
            {
                // Sierra y Jarabe están en la balda superior
                adapter.isOnTopShelf = (item.itemType == Personaje.Items.SIERRA || 
                                        item.itemType == Personaje.Items.JARABE);
            }
        }
    }
}
