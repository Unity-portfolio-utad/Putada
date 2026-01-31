using UnityEngine;

// ============================================================
// PatientInteraction.cs - Permite interactuar con el paciente
// ============================================================

/// <summary>
/// Componente que permite al jugador interactuar con un paciente
/// para aplicarle una cura.
/// </summary>
[RequireComponent(typeof(Collider))]
public class PatientInteraction : MonoBehaviour
{
    [Header("Referencias")]
    [Tooltip("Componente Patient de este objeto")]
    public Patient patient;

    void Start()
    {
        if (patient == null)
        {
            patient = GetComponent<Patient>();
        }
        
        // Asegurar que tiene collider
        var collider = GetComponent<Collider>();
        if (collider != null)
        {
            collider.isTrigger = false; // Queremos clicks, no triggers
        }
    }

    /// <summary>
    /// Llamado cuando el jugador hace clic en el paciente.
    /// </summary>
    void OnMouseDown()
    {
        if (DayCycleOrchestrator.Instance == null) return;
        if (DayCycleOrchestrator.Instance.IsProcessing) return;
        
        // Buscar si el jugador tiene un objeto de cura seleccionado
        var cureItems = FindObjectsByType<CureItem>(FindObjectsSortMode.None);
        CureItem heldItem = null;
        
        foreach (var item in cureItems)
        {
            if (item.IsHeld)
            {
                heldItem = item;
                break;
            }
        }
        
        if (heldItem != null)
        {
            // Usar el objeto en el paciente
            heldItem.UseOnPatient();
        }
        else
        {
            // Mostrar mensaje de que necesita seleccionar una cura
            GameUIManager.Instance?.ShowMessage("Selecciona una cura de la estantería primero.");
        }
    }
}
