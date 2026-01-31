#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

// ============================================================
// GameSystemSetup.cs - Editor para configurar el sistema
// ============================================================

/// <summary>
/// Herramientas de editor para crear y configurar
/// todos los ScriptableObjects del sistema de juego.
/// </summary>
public class GameSystemSetup : EditorWindow
{
    [MenuItem("Tools/Infectio/Setup Game System")]
    public static void ShowWindow()
    {
        GetWindow<GameSystemSetup>("Game System Setup");
    }

    void OnGUI()
    {
        GUILayout.Label("Configuración del Sistema de Juego", EditorStyles.boldLabel);
        GUILayout.Space(10);

        if (GUILayout.Button("Crear Todas las Enfermedades"))
        {
            CreateAllDiseases();
        }

        if (GUILayout.Button("Crear Todas las Consecuencias"))
        {
            CreateAllConsequences();
        }

        if (GUILayout.Button("Crear Registro de Enfermedades"))
        {
            CreateDiseaseRegistry();
        }

        if (GUILayout.Button("Crear Configuración de Campaña"))
        {
            CreateCampaignConfig();
        }

        GUILayout.Space(20);
        
        if (GUILayout.Button("CREAR TODO"))
        {
            CreateAllConsequences();
            CreateAllDiseases();
            CreateDiseaseRegistry();
            CreateCampaignConfig();
            Debug.Log("¡Sistema de juego creado!");
        }
    }

    void CreateAllConsequences()
    {
        string path = "Assets/ScriptableObjects/Consequences";
        EnsureDirectoryExists(path);

        // Blur (Ramas tratado)
        var blur = CreateInstance<AddBlurPercent>();
        blur.consequenceName = "Visión Borrosa";
        blur.description = "Secuela de ramas en brazos tratadas";
        blur.addAmount = 0.2f;
        AssetDatabase.CreateAsset(blur, $"{path}/Consequence_Blur.asset");

        // Fisheye (Caracol tratado)
        var fisheye = CreateInstance<AddFisheyePercent>();
        fisheye.consequenceName = "Visión Distorsionada";
        fisheye.description = "Secuela de ojos de caracol tratados";
        fisheye.addAmount = 0.2f;
        AssetDatabase.CreateAsset(fisheye, $"{path}/Consequence_Fisheye.asset");

        // Breathing (Catarro mal tratado)
        var breathing = CreateInstance<AddBreathingNoise>();
        breathing.consequenceName = "Respiración Ruidosa";
        breathing.description = "Síntoma de catarro no tratado";
        breathing.addAmount = 0.34f;
        AssetDatabase.CreateAsset(breathing, $"{path}/Consequence_Breathing.asset");

        // Ear Ringing (Otitis mal tratado)
        var earRing = CreateInstance<AddEarRinging>();
        earRing.consequenceName = "Pitido en Oídos";
        earRing.description = "Síntoma de otitis no tratada";
        earRing.addAmount = 0.34f;
        AssetDatabase.CreateAsset(earRing, $"{path}/Consequence_EarRinging.asset");

        // Scratch (Sarpullido mal tratado)
        var scratch = CreateInstance<EnablePeriodicScratch>();
        scratch.consequenceName = "Picazón Constante";
        scratch.description = "Síntoma de sarpullido no tratado";
        scratch.intervalSeconds = 10f;
        AssetDatabase.CreateAsset(scratch, $"{path}/Consequence_Scratch.asset");

        // Mirror disabled (Caracol mal tratado)
        var mirror = CreateInstance<DisableMirror>();
        mirror.consequenceName = "No Puedes Mirarte";
        mirror.description = "No puedes usar el espejo por los ojos de caracol";
        AssetDatabase.CreateAsset(mirror, $"{path}/Consequence_DisableMirror.asset");

        // Top shelf disabled (Lepra tratado)
        var shelf = CreateInstance<DisableTopShelf>();
        shelf.consequenceName = "Balda Superior Inaccesible";
        shelf.description = "Secuela de lepra tratada";
        AssetDatabase.CreateAsset(shelf, $"{path}/Consequence_DisableTopShelf.asset");

        // No consequence
        var none = CreateInstance<NoConsequence>();
        none.consequenceName = "Sin Efecto";
        none.description = "Sin consecuencia";
        AssetDatabase.CreateAsset(none, $"{path}/Consequence_None.asset");

        AssetDatabase.SaveAssets();
        Debug.Log("Consecuencias creadas en " + path);
    }

    void CreateAllDiseases()
    {
        string path = "Assets/ScriptableObjects/Diseases";
        EnsureDirectoryExists(path);

        string consPath = "Assets/ScriptableObjects/Consequences";

        // Cargar consecuencias
        var breathing = AssetDatabase.LoadAssetAtPath<ConsequenceDefinition>($"{consPath}/Consequence_Breathing.asset");
        var scratch = AssetDatabase.LoadAssetAtPath<ConsequenceDefinition>($"{consPath}/Consequence_Scratch.asset");
        var earRing = AssetDatabase.LoadAssetAtPath<ConsequenceDefinition>($"{consPath}/Consequence_EarRinging.asset");
        var blur = AssetDatabase.LoadAssetAtPath<ConsequenceDefinition>($"{consPath}/Consequence_Blur.asset");
        var fisheye = AssetDatabase.LoadAssetAtPath<ConsequenceDefinition>($"{consPath}/Consequence_Fisheye.asset");
        var mirror = AssetDatabase.LoadAssetAtPath<ConsequenceDefinition>($"{consPath}/Consequence_DisableMirror.asset");
        var shelf = AssetDatabase.LoadAssetAtPath<ConsequenceDefinition>($"{consPath}/Consequence_DisableTopShelf.asset");
        var none = AssetDatabase.LoadAssetAtPath<ConsequenceDefinition>($"{consPath}/Consequence_None.asset");

        // LEVES
        // Catarro
        var catarrh = CreateInstance<DiseaseDefinition>();
        catarrh.id = DiseaseId.Catarrh;
        catarrh.severity = Severity.Mild;
        catarrh.displayName = "Catarro";
        catarrh.description = "Un resfriado común. Se cura con jarabe.";
        catarrh.isMildProgressive = true;
        catarrh.mildDailyIncrease = 0.34f;
        catarrh.correctCure = CureId.Syrup;
        catarrh.killsIfUntreated = false;
        catarrh.consequenceIfTreatedCorrectly = none;
        catarrh.consequenceIfWrongOrNotTreated = breathing;
        AssetDatabase.CreateAsset(catarrh, $"{path}/Disease_Catarrh.asset");

        // Sarpullido
        var rash = CreateInstance<DiseaseDefinition>();
        rash.id = DiseaseId.Rash;
        rash.severity = Severity.Mild;
        rash.displayName = "Sarpullido";
        rash.description = "Irritación de la piel. Se cura con crema.";
        rash.isMildProgressive = true;
        rash.mildDailyIncrease = 0.34f;
        rash.correctCure = CureId.Cream;
        rash.killsIfUntreated = false;
        rash.consequenceIfTreatedCorrectly = none;
        rash.consequenceIfWrongOrNotTreated = scratch;
        AssetDatabase.CreateAsset(rash, $"{path}/Disease_Rash.asset");

        // Otitis
        var otitis = CreateInstance<DiseaseDefinition>();
        otitis.id = DiseaseId.Otitis;
        otitis.severity = Severity.Mild;
        otitis.displayName = "Otitis";
        otitis.description = "Infección de oído. Se cura con lijas.";
        otitis.isMildProgressive = true;
        otitis.mildDailyIncrease = 0.34f;
        otitis.correctCure = CureId.Sandpaper;
        otitis.killsIfUntreated = false;
        otitis.consequenceIfTreatedCorrectly = none;
        otitis.consequenceIfWrongOrNotTreated = earRing;
        AssetDatabase.CreateAsset(otitis, $"{path}/Disease_Otitis.asset");

        // GRAVES
        // Ramas en brazos
        var branchArms = CreateInstance<DiseaseDefinition>();
        branchArms.id = DiseaseId.BranchArms;
        branchArms.severity = Severity.Severe;
        branchArms.displayName = "Ramas en los Brazos";
        branchArms.description = "Extrañas ramificaciones en los brazos. Se cura con sierra.";
        branchArms.isMildProgressive = false;
        branchArms.correctCure = CureId.Saw;
        branchArms.killsIfUntreated = true;
        branchArms.consequenceIfTreatedCorrectly = blur;
        branchArms.consequenceIfWrongOrNotTreated = null;
        AssetDatabase.CreateAsset(branchArms, $"{path}/Disease_BranchArms.asset");

        // Ojos de caracol
        var snailEyes = CreateInstance<DiseaseDefinition>();
        snailEyes.id = DiseaseId.SnailEyes;
        snailEyes.severity = Severity.Severe;
        snailEyes.displayName = "Ojos de Caracol";
        snailEyes.description = "Ojos que sobresalen. Se cura con sal.";
        snailEyes.isMildProgressive = false;
        snailEyes.correctCure = CureId.Salt;
        snailEyes.killsIfUntreated = false; // NO MATA
        snailEyes.consequenceIfTreatedCorrectly = fisheye;
        snailEyes.consequenceIfWrongOrNotTreated = mirror;
        AssetDatabase.CreateAsset(snailEyes, $"{path}/Disease_SnailEyes.asset");

        // Lepra
        var lepra = CreateInstance<DiseaseDefinition>();
        lepra.id = DiseaseId.StoneLepra;
        lepra.severity = Severity.Severe;
        lepra.displayName = "Lepra";
        lepra.description = "Lepra medieval. Se cura con pico de minero.";
        lepra.isMildProgressive = false;
        lepra.correctCure = CureId.Pickaxe;
        lepra.killsIfUntreated = true;
        lepra.consequenceIfTreatedCorrectly = shelf;
        lepra.consequenceIfWrongOrNotTreated = null;
        AssetDatabase.CreateAsset(lepra, $"{path}/Disease_Lepra.asset");

        // ESPECIAL
        // Bichos en el cuerpo
        var bodyBugs = CreateInstance<DiseaseDefinition>();
        bodyBugs.id = DiseaseId.BodyBugs;
        bodyBugs.severity = Severity.Special;
        bodyBugs.displayName = "Bichos en el Cuerpo";
        bodyBugs.description = "Infestación parasitaria. La única cura es la vela... pero igual mueres.";
        bodyBugs.isMildProgressive = false;
        bodyBugs.correctCure = CureId.Candle;
        bodyBugs.killsIfUntreated = true;
        bodyBugs.consequenceIfTreatedCorrectly = null;
        bodyBugs.consequenceIfWrongOrNotTreated = null;
        AssetDatabase.CreateAsset(bodyBugs, $"{path}/Disease_BodyBugs.asset");

        AssetDatabase.SaveAssets();
        Debug.Log("Enfermedades creadas en " + path);
    }

    void CreateDiseaseRegistry()
    {
        string path = "Assets/ScriptableObjects";
        EnsureDirectoryExists(path);

        var registry = CreateInstance<DiseaseRegistry>();
        
        string disPath = "Assets/ScriptableObjects/Diseases";
        registry.allDiseases = new DiseaseDefinition[]
        {
            AssetDatabase.LoadAssetAtPath<DiseaseDefinition>($"{disPath}/Disease_Catarrh.asset"),
            AssetDatabase.LoadAssetAtPath<DiseaseDefinition>($"{disPath}/Disease_Rash.asset"),
            AssetDatabase.LoadAssetAtPath<DiseaseDefinition>($"{disPath}/Disease_Otitis.asset"),
            AssetDatabase.LoadAssetAtPath<DiseaseDefinition>($"{disPath}/Disease_BranchArms.asset"),
            AssetDatabase.LoadAssetAtPath<DiseaseDefinition>($"{disPath}/Disease_SnailEyes.asset"),
            AssetDatabase.LoadAssetAtPath<DiseaseDefinition>($"{disPath}/Disease_Lepra.asset"),
            AssetDatabase.LoadAssetAtPath<DiseaseDefinition>($"{disPath}/Disease_BodyBugs.asset"),
        };

        AssetDatabase.CreateAsset(registry, $"{path}/DiseaseRegistry.asset");
        AssetDatabase.SaveAssets();
        Debug.Log("Registro de enfermedades creado");
    }

    void CreateCampaignConfig()
    {
        string path = "Assets/ScriptableObjects";
        EnsureDirectoryExists(path);

        var campaign = CreateInstance<CampaignConfig>();
        
        campaign.days = new DayPlan[]
        {
            // Día 0: Tutorial
            new DayPlan
            {
                dayIndex = 0,
                dayName = "Tutorial",
                patientCount = 1,
                mildCount = 1,
                severeCount = 0,
                includesBodyBugsSpecial = false,
                infectionRule = InfectionRuleType.None,
                isTutorialDay = true,
                tutorialText = "Bienvenido. Trata al paciente con la cura correcta."
            },
            // Día 1: 2 pacientes leves, te infectas de 1
            new DayPlan
            {
                dayIndex = 1,
                dayName = "Día 1",
                patientCount = 2,
                mildCount = 2,
                severeCount = 0,
                includesBodyBugsSpecial = false,
                infectionRule = InfectionRuleType.WeightedRandomFromPatients,
                isTutorialDay = false
            },
            // Día 2: 3 pacientes (2 leves, 1 grave), te infectas del grave
            new DayPlan
            {
                dayIndex = 2,
                dayName = "Día 2",
                patientCount = 3,
                mildCount = 2,
                severeCount = 1,
                includesBodyBugsSpecial = false,
                infectionRule = InfectionRuleType.GuaranteedFromPatientIndex,
                guaranteedPatientIndex = 2, // El grave
                isTutorialDay = false
            },
            // Día 3: 4 pacientes (2 leves, 2 graves), 25% cada uno
            new DayPlan
            {
                dayIndex = 3,
                dayName = "Día 3",
                patientCount = 4,
                mildCount = 2,
                severeCount = 2,
                includesBodyBugsSpecial = false,
                infectionRule = InfectionRuleType.WeightedRandomFromPatients,
                isTutorialDay = false
            },
            // Día 4: 5 pacientes (3 leves, 1 grave, 1 BodyBugs)
            new DayPlan
            {
                dayIndex = 4,
                dayName = "Día Final",
                patientCount = 5,
                mildCount = 3,
                severeCount = 1,
                includesBodyBugsSpecial = true,
                infectionRule = InfectionRuleType.WeightedRandomFromPatients,
                isTutorialDay = false
            }
        };

        campaign.deathMessageUntreatedSevere = "HAS MUERTO POR UNA ENFERMEDAD GRAVE NO TRATADA.";
        campaign.deathMessageMildOverflow = "HAS MUERTO POR ACUMULAR ENFERMEDADES LEVES HASTA EL LÍMITE.";
        campaign.deathMessageBodyBugs = "FINAL DEL JUEGO.\n\nHAS MUERTO.\n\nNo siempre hay una cura para todo...";
        campaign.deathMessageCandle = "FINAL DEL JUEGO.\n\nHAS MUERTO.\n\nNo siempre hay una cura para todo...";
        campaign.deathScreenDuration = 5f;

        AssetDatabase.CreateAsset(campaign, $"{path}/CampaignConfig.asset");
        AssetDatabase.SaveAssets();
        Debug.Log("Configuración de campaña creada");
    }

    void EnsureDirectoryExists(string path)
    {
        if (!AssetDatabase.IsValidFolder(path))
        {
            string[] parts = path.Split('/');
            string current = parts[0];
            
            for (int i = 1; i < parts.Length; i++)
            {
                string next = current + "/" + parts[i];
                if (!AssetDatabase.IsValidFolder(next))
                {
                    AssetDatabase.CreateFolder(current, parts[i]);
                }
                current = next;
            }
        }
    }
}
#endif
