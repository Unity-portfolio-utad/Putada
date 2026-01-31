using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.InputSystem;


public class ScriptDialog : MonoBehaviour
{
    public TextMeshPro textComponent;
    public float textSpeed = 0.05f;

    public List<SintomaData> baseDeDatosSintomas;

    private List<Enfermedad.Enfermedades> enfermedadesGraves;
    private List<Enfermedad.Enfermedades> enfermedadesLeves;

    public bool leve = false; 
    public Enfermedad.Enfermedades nakim; 

    private List<string> lines = new List<string>();
    private int index;
    private  Enfermedad enfermedad;

    public enum TipoSintoma { A_Calor, B_Flujos, C_Cuerpo, D_Dolor, E_Conducta }

    public enum Enfermedades { CATARRO, SARPULLIDO, RAMAS_BRAZOS, OTITIS, OJOS_CARACOL, LEPRA, BICHOS_OJOS }

    [System.Serializable]
    public struct SintomaData
    {
        public TipoSintoma tipo;
        public TextAsset archivoTexto;
    }

 

    void Awake()
    {
        textComponent.text = string.Empty;
        GenerarDialogoEnfermedad();
    }

    void Update()
    {
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

        Enfermedad.Enfermedades enfermedadElegida;

        if (leve)
        {
            if (enfermedadesLeves.Count == 0)
            {
                return;
            }
            enfermedadElegida = enfermedadesLeves[UnityEngine.Random.Range(0, enfermedadesLeves.Count)];
        }
        else
        {
            if (enfermedadesGraves.Count == 0)
            {
                return;
            }
            enfermedadElegida = enfermedadesGraves[UnityEngine.Random.Range(0, enfermedadesGraves.Count)];
        }

        nakim = enfermedadElegida;
        

        List<Enfermedad.TipoSintoma> sintomasMezclados = new List<Enfermedad.TipoSintoma>(enfermedad.getSintomas(nakim));
        Shuffle(sintomasMezclados);

        foreach (TipoSintoma tipo in sintomasMezclados)
        {
            string frase = ObtenerFraseRandomDeSintoma(tipo);
            if (!string.IsNullOrEmpty(frase))
            {
                lines.Add(frase);
            }
        }
    }

    string ObtenerFraseRandomDeSintoma(TipoSintoma tipoBuscado)
    {
        foreach (var data in baseDeDatosSintomas)
        {
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

    public void NextLine()
    {
        if (index < lines.Count - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(Typeline());
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}