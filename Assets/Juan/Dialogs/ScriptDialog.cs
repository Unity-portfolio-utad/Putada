using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.InputSystem;
using UnityEditor.Animations;


public class ScriptDialog : MonoBehaviour
{
    private bool ready = false;

    private float opacity = 0.0f;

    private bool isDialogActive = false;
    public TextMeshPro textComponent;
    public float textSpeed = 0.05f;

    [SerializeField]
    AnimatorController[] aniControlllers;

    public List<SintomaData> baseDeDatosSintomas;

    Animator anim; 

    [SerializeField]
    private Personaje perj;

    public bool leve = false;
    public Enfermedad.Enfermedades nakim;

    private List<string> lines = new List<string>();
    private int index = -1;
    [SerializeField]
    private Enfermedad enfermedad;

    public enum TipoSintoma { A_Calor, B_Flujos, C_Cuerpo, D_Dolor, E_Conducta }

    public enum Enfermedades { NULL, CATARRO, SARPULLIDO, RAMAS_BRAZOS, OTITIS, OJOS_CARACOL, LEPRA, BICHOS_OJOS }

    [System.Serializable]
    public struct SintomaData
    {
        public Enfermedad.TipoSintoma tipo;
        public TextAsset archivoTexto;
    }



    void Start()
    {
        leve = Random.value > 0.5f;
        perj = GameObject.FindFirstObjectByType<Personaje>();
        enfermedad = GameObject.FindFirstObjectByType<Enfermedad>();
        GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0);
        StartCoroutine(fadeIn());
        textComponent.text = string.Empty;
        GenerarDialogoEnfermedad();
        anim = GetComponent<Animator>();
        anim.runtimeAnimatorController = aniControlllers[UnityEngine.Random.Range(0, aniControlllers.Length - 1)];
    }

    void Update()
    {
        if (index == -1 || lines.Count == 0) return;
        if (!isDialogActive) return;
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (textComponent.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
    }

    void GenerarDialogoEnfermedad()
    {
        lines.Clear();
        NightShift nightShift = perj.nightShift;
        if (nightShift == null)
        {
            // intentar recuperar una referencia válida en la escena
            nightShift = FindFirstObjectByType<NightShift>();
        }

        Enfermedad.Enfermedades enfermedadElegida;

        if (leve && nightShift.diaActual < 5)
        {
            enfermedadElegida = Enfermedad.leves[UnityEngine.Random.Range(0, Enfermedad.leves.Length)];
        }
        else
        {
            
            if(nightShift.diaActual == 5)
            {
                enfermedadElegida = Enfermedad.Enfermedades.BICHOS_OJOS;
            }
            else
            enfermedadElegida = Enfermedad.graves[UnityEngine.Random.Range(0, Enfermedad.graves.Length-1)];
        }

        if (nakim == Enfermedad.Enfermedades.NULL) {
            nakim = enfermedadElegida;
        }


        List<Enfermedad.TipoSintoma> sintomasMezclados = new List<Enfermedad.TipoSintoma>(enfermedad.getSintomas(nakim));
        Shuffle(sintomasMezclados);
        Debug.LogWarning("sfefvr");
        foreach (Enfermedad.TipoSintoma tipo in sintomasMezclados)
        {
            Debug.LogWarning("dentro");
            string frase = ObtenerFraseRandomDeSintoma(tipo);
            if (!string.IsNullOrEmpty(frase))
            {
                Debug.LogWarning("Hola");
                lines.Add(frase);
            }
        }
    }

    string ObtenerFraseRandomDeSintoma(Enfermedad.TipoSintoma tipoBuscado)
    {
        Debug.LogWarning("A");
        foreach (var data in baseDeDatosSintomas)
        {
            Debug.LogWarning("h");
            if (data.tipo == tipoBuscado && data.archivoTexto != null)
            {
                string[] posiblesFrases = data.archivoTexto.text.Split(new char[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);

                if (posiblesFrases.Length > 0)
                {
                    return posiblesFrases[UnityEngine.Random.Range(0, posiblesFrases.Length)];
                }
            }
        }
        return "Error: No se encontró texto para el síntoma " + tipoBuscado;
    }

    void Shuffle<T>(List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = UnityEngine.Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public void comenzarDialogo()
    {
        index = 0;
        if (lines.Count > 0)
            StartCoroutine(Typeline());
    }

    IEnumerator Typeline()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    // IEnumerator fadeIn()
    // {
    //     while (true)
    //     {
    //         yield return new WaitForSeconds(0.1f);
    //         transform.position -= new Vector3(0.1f, 0f, 0);
    //     }
    //
    //     transform.position = new Vector3(transform.position.x, 0.7f, transform.position.z);
    // }
    //
    // IEnumerator fadeOut()
    // {
    //     while (transform.position.x < 3f)
    //     {
    //         yield return new WaitForSeconds(0.1f);
    //         transform.position += new Vector3(0.1f, 0, 0);
    //     }
    //
    //     transform.position = new Vector3(0.7f, transform.position.y, transform.position.z);
    //     //Hacer que venga el siguiente
    // }
    
    //Vamos a hacer el fade in y fade out con la opacidad del sprite renderer
    IEnumerator fadeIn()
    {
        yield return new WaitForSeconds(3f);
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        while (opacity < 1.0f)
        {
            yield return new WaitForSeconds(0.1f);
            opacity += 0.1f;
            sr.color = new Color(1.0f, 1.0f, 1.0f, opacity);
        }
        opacity = 1.0f;
        sr.color = new Color(1.0f, 1.0f, 1.0f, opacity);
        ready = true;
    }
    IEnumerator fadeOut()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        while (opacity > 0.0f)
        {
            yield return new WaitForSeconds(0.1f);
            opacity -= 0.1f;
            sr.color = new Color(1.0f, 1.0f, 1.0f, opacity);
        }
        opacity = 0.0f;
        sr.color = new Color(1.0f, 1.0f, 1.0f, opacity);

        textComponent.SetText("");
        perj.nextNpc(this.gameObject);
        //Hacer que venga el siguiente
    }
    
    //Haz que funcione correctamente el fade ou

    void NextLine()
    {
        if (index < lines.Count - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(Typeline());
        }
        else
        {
            isDialogActive = false;
            textComponent.SetText("");
            GenerarDialogoEnfermedad();
            // gameObject.SetActive(false);
        }
    }

    void OnMouseDown()
    {
        if (perj.activeItem == Personaje.Items.NULL)
        {
            if (!ready || isDialogActive) {}
            else {
                isDialogActive = true;
                comenzarDialogo();
                anim.SetTrigger("quitarMascara");
            }
        }

        else
        {
            ready = false;
            StartCoroutine(fadeOut());
            if ((int)perj.activeItem != (int)nakim)
            {
                perj.dead++;
            }
        }
    }
}