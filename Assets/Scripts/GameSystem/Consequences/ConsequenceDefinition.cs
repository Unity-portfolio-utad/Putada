using UnityEngine;

// ============================================================
// ConsequenceDefinition.cs - Base abstracta para consecuencias
// ============================================================

/// <summary>
/// Clase base abstracta para definir consecuencias/secuelas.
/// Cada consecuencia específica hereda de esta clase.
/// </summary>
public abstract class ConsequenceDefinition : ScriptableObject
{
    [Header("Info")]
    public string consequenceName = "Consecuencia";
    
    [TextArea(2, 3)]
    public string description = "Descripción del efecto";

    /// <summary>
    /// Aplica la consecuencia al estado del jugador.
    /// </summary>
    /// <param name="playerStatus">Estado del jugador a modificar</param>
    /// <param name="dayIndex">Día actual (0-5)</param>
    public abstract void Apply(PlayerStatus playerStatus, int dayIndex);
}
