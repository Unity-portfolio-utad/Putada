using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// ============================================================
// DayCycleOrchestrator.cs - Orquestador principal del ciclo de días
// ============================================================

/// <summary>
/// Controla el ciclo completo del juego:
/// - Gestiona los días (0-5)
/// - Spawna pacientes según el plan del día
/// - Gestiona la infección del jugador
/// - Evalúa muertes y finales
/// </summary>
public class DayCycleOrchestrator : MonoBehaviour
{
    public static DayCycleOrchestrator Instance { get; private set; }

    [Header("Configuración")]
    [Tooltip("Configuración de la campaña (días, reglas, etc.)")]
    public CampaignConfig campaign;
    
    [Tooltip("Registro de enfermedades")]
    public DiseaseRegistry diseaseRegistry;

    [Header("Referencias de Entidades")]
    [Tooltip("El jugador")]
    public PlayerHuman player;
    
    [Tooltip("Prefab del paciente")]
    public Patient patientPrefab;
    
    [Tooltip("Punto de spawn de pacientes")]
    public Transform patientSpawnPoint;
    
    [Tooltip("Punto de salida de pacientes")]
    public Transform patientExitPoint;

    [Header("UI")]
    [Tooltip("CanvasGroup para fade a negro")]
    public CanvasGroup fadeGroup;

    [Header("Tiempos")]
    public float patientEnterTime = 1f;
    public float patientExitTime = 0.8f;
    public float fadeToBlackTime = 0.6f;
    public float fadeFromBlackTime = 0.6f;

    // ==================== ESTADO ====================
    
    [Header("Estado (Debug)")]
    [SerializeField] private int currentDay = 0;
    [SerializeField] private int currentPatientIndex = -1;
    [SerializeField] private int deadPatientsToday = 0;
    [SerializeField] private bool isProcessing = false;
    [SerializeField] private bool isInSelfExam = false;

    private List<Patient> todaysPatients = new List<Patient>();
    private Patient currentPatient;

    // ==================== PROPIEDADES PÚBLICAS ====================
    
    public int CurrentDay => currentDay;
    public int DeadPatientsToday => deadPatientsToday;
    public bool IsProcessing => isProcessing;
    public Patient CurrentPatient => currentPatient;

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
        // Inicializar fade
        if (fadeGroup != null)
        {
            fadeGroup.alpha = 0f;
            fadeGroup.interactable = false;
            fadeGroup.blocksRaycasts = false;
        }
    }

    // ==================== API PÚBLICA ====================

    /// <summary>
    /// Inicia el juego desde el día 0.
    /// </summary>
    public void StartGame()
    {
        currentDay = 0;
        player?.ResetPlayer();
        StartDay();
    }

    /// <summary>
    /// Trata al paciente actual con una cura específica.
    /// </summary>
    public void TreatCurrentPatient(CureId cure)
    {
        if (currentPatient == null || isProcessing) return;
        
        currentPatient.ApplyCure(cure);
        Debug.Log($"Paciente tratado con {cure}");
        
        StartCoroutine(DismissPatientAndNext());
    }

    /// <summary>
    /// El jugador se trata a sí mismo y termina el día.
    /// </summary>
    public void SelfTreatAndEndDay(CureId cure)
    {
        if (isProcessing || !isInSelfExam) return;
        
        if (cure != CureId.None)
        {
            player.ApplyCure(cure);
            Debug.Log($"Jugador se trató con {cure}");
        }
        
        StartCoroutine(EndDaySequence());
    }

    /// <summary>
    /// Termina el día sin tratarse.
    /// </summary>
    public void EndDayWithoutTreatment()
    {
        if (isProcessing || !isInSelfExam) return;
        
        StartCoroutine(EndDaySequence());
    }

    /// <summary>
    /// Convierte Items del sistema antiguo a CureId.
    /// </summary>
    public CureId ConvertItemToCure(Personaje.Items item)
    {
        switch (item)
        {
            case Personaje.Items.JARABE: return CureId.Syrup;
            case Personaje.Items.CREMA: return CureId.Cream;
            case Personaje.Items.LIJAS: return CureId.Sandpaper;
            case Personaje.Items.SIERRA: return CureId.Saw;
            case Personaje.Items.SAL: return CureId.Salt;
            case Personaje.Items.PICO: return CureId.Pickaxe;
            case Personaje.Items.CERILLAS: return CureId.Candle;
            default: return CureId.None;
        }
    }

    // ==================== LÓGICA INTERNA ====================

    /// <summary>
    /// Inicia un nuevo día.
    /// </summary>
    void StartDay()
    {
        isProcessing = true;
        isInSelfExam = false;
        deadPatientsToday = 0;
        currentPatientIndex = -1;
        
        ClearPatients();

        DayPlan plan = campaign.GetDay(currentDay);
        if (plan == null)
        {
            Debug.LogError($"No hay plan para el día {currentDay}");
            return;
        }

        Debug.Log($"=== COMENZANDO {plan.dayName} ===");
        
        // Mostrar info del día
        GameUIManager.Instance?.ShowDayInfo(currentDay, plan.dayName);
        
        // Mostrar tutorial si aplica
        if (plan.isTutorialDay && !string.IsNullOrEmpty(plan.tutorialText))
        {
            GameUIManager.Instance?.ShowMessage(plan.tutorialText);
        }

        // Generar pacientes
        SpawnPatientsForPlan(plan);
        
        isProcessing = false;
        
        // Siguiente paciente
        StartCoroutine(BringNextPatient());
    }

    /// <summary>
    /// Genera los pacientes para el plan del día.
    /// </summary>
    void SpawnPatientsForPlan(DayPlan plan)
    {
        // Crear pool de enfermedades según el plan
        List<DiseaseId> diseasePool = new List<DiseaseId>();
        
        // Añadir enfermedades leves
        var mildDiseases = new DiseaseId[] { DiseaseId.Catarrh, DiseaseId.Rash, DiseaseId.Otitis };
        for (int i = 0; i < plan.mildCount; i++)
        {
            diseasePool.Add(mildDiseases[Random.Range(0, mildDiseases.Length)]);
        }
        
        // Añadir enfermedades graves
        var severeDiseases = new DiseaseId[] { DiseaseId.BranchArms, DiseaseId.SnailEyes, DiseaseId.StoneLepra };
        for (int i = 0; i < plan.severeCount; i++)
        {
            diseasePool.Add(severeDiseases[Random.Range(0, severeDiseases.Length)]);
        }
        
        // Añadir especial si aplica
        if (plan.includesBodyBugsSpecial)
        {
            diseasePool.Add(DiseaseId.BodyBugs);
        }
        
        // Barajar el pool
        ShuffleList(diseasePool);
        
        // Crear pacientes
        for (int i = 0; i < plan.patientCount; i++)
        {
            Patient patient;
            
            if (patientPrefab != null)
            {
                patient = Instantiate(patientPrefab);
            }
            else
            {
                // Crear paciente básico si no hay prefab
                var go = new GameObject($"Patient_{i}");
                patient = go.AddComponent<Patient>();
            }
            
            patient.gameObject.SetActive(false);
            
            // Asignar enfermedad
            DiseaseId diseaseId = diseasePool[Mathf.Min(i, diseasePool.Count - 1)];
            var definition = diseaseRegistry.Get(diseaseId);
            
            if (definition != null)
            {
                patient.AddDisease(definition);
            }
            
            todaysPatients.Add(patient);
        }
        
        Debug.Log($"Generados {todaysPatients.Count} pacientes para el día {plan.dayIndex}");
    }

    /// <summary>
    /// Trae al siguiente paciente.
    /// </summary>
    IEnumerator BringNextPatient()
    {
        isProcessing = true;
        currentPatientIndex++;
        
        if (currentPatientIndex >= todaysPatients.Count)
        {
            // No más pacientes: ir al autoexamen
            isProcessing = false;
            StartSelfExamPhase();
            yield break;
        }
        
        currentPatient = todaysPatients[currentPatientIndex];
        
        if (patientSpawnPoint != null)
        {
            currentPatient.transform.position = patientSpawnPoint.position;
            currentPatient.transform.rotation = patientSpawnPoint.rotation;
        }
        
        currentPatient.gameObject.SetActive(true);
        
        // Animación de entrada (simple)
        yield return new WaitForSeconds(patientEnterTime);
        
        Debug.Log($"Paciente {currentPatientIndex + 1}/{todaysPatients.Count} listo");
        
        isProcessing = false;
    }

    /// <summary>
    /// Despide al paciente actual y trae el siguiente.
    /// </summary>
    IEnumerator DismissPatientAndNext()
    {
        isProcessing = true;
        
        if (currentPatient != null)
        {
            // Animación de salida
            yield return new WaitForSeconds(patientExitTime);
            currentPatient.gameObject.SetActive(false);
            currentPatient = null;
        }
        
        isProcessing = false;
        
        yield return BringNextPatient();
    }

    /// <summary>
    /// Inicia la fase de autoexamen del jugador.
    /// </summary>
    void StartSelfExamPhase()
    {
        isInSelfExam = true;
        
        Debug.Log("=== FASE DE AUTOEXAMEN ===");
        
        // Mostrar UI de autoexamen
        string summary = player.GetDiseaseSummary(diseaseRegistry);
        GameUIManager.Instance?.ShowSelfExamPanel(summary);
    }

    /// <summary>
    /// Secuencia de fin de día.
    /// </summary>
    IEnumerator EndDaySequence()
    {
        isProcessing = true;
        isInSelfExam = false;
        
        GameUIManager.Instance?.HideSelfExamPanel();
        
        // Fade a negro
        yield return FadeToBlack();
        
        // 1) Resolver pacientes
        foreach (var patient in todaysPatients)
        {
            bool died = patient.ResolveEndOfDay(diseaseRegistry);
            if (died)
            {
                deadPatientsToday++;
            }
        }
        
        Debug.Log($"Pacientes muertos hoy: {deadPatientsToday}");
        GameUIManager.Instance?.UpdateDeadPatients(deadPatientsToday);
        
        // 2) Infectar al jugador según regla del día
        InfectPlayerForDay(campaign.GetDay(currentDay));
        
        // 3) Resolver estado del jugador
        bool isFinalDay = (currentDay >= campaign.TotalDays - 1);
        DeathCause deathCause = player.ResolveEndOfDay(diseaseRegistry, currentDay, isFinalDay);
        
        // 4) ¿Murió el jugador?
        if (deathCause != DeathCause.None)
        {
            yield return HandlePlayerDeath(deathCause);
            yield break;
        }
        
        // 5) Pasar al siguiente día
        currentDay++;
        
        if (currentDay >= campaign.TotalDays)
        {
            // ¡Victoria! (si llegara aquí sin morir en día 5)
            Debug.Log("=== VICTORIA ===");
            // Podrías mostrar un final bueno aquí
            currentDay = 0;
        }
        
        // Fade desde negro
        yield return FadeFromBlack();
        
        isProcessing = false;
        
        // Iniciar nuevo día
        StartDay();
    }

    /// <summary>
    /// Infecta al jugador según la regla del día.
    /// </summary>
    void InfectPlayerForDay(DayPlan plan)
    {
        if (plan == null || plan.infectionRule == InfectionRuleType.None)
        {
            Debug.Log("Sin infección hoy");
            return;
        }
        
        if (todaysPatients.Count == 0) return;
        
        int patientIndex = 0;
        
        switch (plan.infectionRule)
        {
            case InfectionRuleType.GuaranteedFromPatientIndex:
                patientIndex = Mathf.Clamp(plan.guaranteedPatientIndex, 0, todaysPatients.Count - 1);
                break;
                
            case InfectionRuleType.WeightedRandomFromPatients:
                patientIndex = Random.Range(0, todaysPatients.Count);
                break;
        }
        
        var sourcePatient = todaysPatients[patientIndex];
        var diseaseId = sourcePatient.GetPrimaryDisease();
        
        if (diseaseId.HasValue)
        {
            var definition = diseaseRegistry.Get(diseaseId.Value);
            if (definition != null)
            {
                player.AddDisease(definition);
                Debug.Log($"Jugador infectado con {definition.displayName} del paciente {patientIndex}");
            }
        }
    }

    /// <summary>
    /// Maneja la muerte del jugador.
    /// </summary>
    IEnumerator HandlePlayerDeath(DeathCause cause)
    {
        Debug.Log($"=== JUGADOR MUERE: {cause} ===");
        
        string message = campaign.GetDeathMessage(cause);
        GameUIManager.Instance?.ShowDeathScreen(message);
        
        // Esperar
        yield return new WaitForSeconds(campaign.deathScreenDuration);
        
        // Recargar escena
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// Fade a negro.
    /// </summary>
    IEnumerator FadeToBlack()
    {
        if (fadeGroup == null) yield break;
        
        fadeGroup.blocksRaycasts = true;
        float elapsed = 0f;
        
        while (elapsed < fadeToBlackTime)
        {
            elapsed += Time.deltaTime;
            fadeGroup.alpha = Mathf.Clamp01(elapsed / fadeToBlackTime);
            yield return null;
        }
        
        fadeGroup.alpha = 1f;
    }

    /// <summary>
    /// Fade desde negro.
    /// </summary>
    IEnumerator FadeFromBlack()
    {
        if (fadeGroup == null) yield break;
        
        float elapsed = 0f;
        
        while (elapsed < fadeFromBlackTime)
        {
            elapsed += Time.deltaTime;
            fadeGroup.alpha = 1f - Mathf.Clamp01(elapsed / fadeFromBlackTime);
            yield return null;
        }
        
        fadeGroup.alpha = 0f;
        fadeGroup.blocksRaycasts = false;
    }

    /// <summary>
    /// Limpia los pacientes del día.
    /// </summary>
    void ClearPatients()
    {
        foreach (var patient in todaysPatients)
        {
            if (patient != null)
            {
                Destroy(patient.gameObject);
            }
        }
        todaysPatients.Clear();
        currentPatient = null;
    }

    /// <summary>
    /// Baraja una lista.
    /// </summary>
    void ShuffleList<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            T temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }
}
