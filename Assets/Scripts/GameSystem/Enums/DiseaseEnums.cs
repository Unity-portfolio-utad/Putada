// ============================================================
// DiseaseEnums.cs - Enumeraciones del sistema de enfermedades
// ============================================================

/// <summary>
/// Identificadores únicos de cada enfermedad en el juego.
/// </summary>
public enum DiseaseId
{
    // Leves (se pueden acumular, progresan 34% por día)
    Catarrh,     // Catarro - cura: Jarabe
    Rash,        // Sarpullido - cura: Crema
    Otitis,      // Otitis - cura: Lijas

    // Graves (si no se tratan bien ese día → muerte, excepto SnailEyes)
    BranchArms,  // Ramas en los brazos - cura: Sierra
    SnailEyes,   // Ojos de caracol - cura: Sal (NO mata, pero desactiva espejo)
    StoneLepra,  // Lepra - cura: Pico

    // Especial (día 5, siempre mueres)
    BodyBugs     // Bichos en el cuerpo - cura: Vela (pero igual mueres)
}

/// <summary>
/// Severidad de la enfermedad.
/// </summary>
public enum Severity
{
    Mild,    // Leve: progresa 34% por día, muerte si >100% + otra al amanecer
    Severe,  // Grave: muerte ese día si no se trata correctamente (excepto SnailEyes)
    Special  // Especial: BodyBugs, muerte siempre (cambia mensaje final)
}

/// <summary>
/// Identificadores de cada cura/objeto curativo.
/// </summary>
public enum CureId
{
    None,      // Sin cura aplicada
    Syrup,     // Jarabe (catarro)
    Cream,     // Crema (sarpullido)
    Sandpaper, // Lijas (otitis)
    Saw,       // Sierra (ramas brazos)
    Salt,      // Sal (ojos caracol)
    Pickaxe,   // Pico (lepra)
    Candle     // Vela (bichos cuerpo) - final
}

/// <summary>
/// Causa de muerte del jugador.
/// </summary>
public enum DeathCause
{
    None,
    UntreatedSevere,               // Grave no tratada bien ese día
    MildOverflowPlusAnotherAtDawn, // Leve >100% + otra enfermedad al amanecer
    BodyBugsFinal,                 // Final especial: bichos sin tratar
    CandleFinal                    // Final especial: te quemaste con vela
}

/// <summary>
/// Tipo de regla de infección para cada día.
/// </summary>
public enum InfectionRuleType
{
    None,                         // Día 0 tutorial: no te infectas
    GuaranteedFromPatientIndex,   // Te infectas del paciente en índice específico
    WeightedRandomFromPatients    // 25% de cada paciente (uno garantizado)
}
