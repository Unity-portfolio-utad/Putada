using UnityEngine;
using System.Collections;
using System.Collections.Generic; // Necesario para listas
using TMPro;
using UnityEngine.InputSystem;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public float textSpeed = 0.05f;

    public List<SintomaData> baseDeDatosSintomas; 

    public List<Enfermedad> listaEnfermedades;

    private List<string> lines = new List<string>(); 
    private int index;


    public enum TipoSintoma { A_Calor, B_Flujos, C_Cuerpo, D_Dolor, E_Conducta }

    [System.Serializable]
    public struct SintomaData
    {
        public TipoSintoma tipo;
        public TextAsset archivoTexto; 
    }

    [System.Serializable]
    public struct Enfermedad
    {
        public string nombre;
        public TipoSintoma[] combinacion;
    }

    

    void Start()
    {
        textComponent.text = string.Empty;
        GenerarDialogoEnfermedad();
        comenzarDialogo();
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

        // enfermedad Random
        if (listaEnfermedades.Count == 0) return;
        Enfermedad enfermedadElegida = listaEnfermedades[Random.Range(0, listaEnfermedades.Count)];
        

        List<TipoSintoma> sintomasMezclados = new List<TipoSintoma>(enfermedadElegida.combinacion);
        Shuffle(sintomasMezclados); 

        // de cada síntoma saco archivo y una frase random
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
        // busco el textAsset
        foreach (var data in baseDeDatosSintomas)
        {
            if (data.tipo == tipoBuscado && data.archivoTexto != null)
            {
                // separar el txt por líneas
                string[] posiblesFrases = data.archivoTexto.text.Split(new char[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);
                
                if (posiblesFrases.Length > 0)
                {
                    // frase random de este archivo
                    return posiblesFrases[Random.Range(0, posiblesFrases.Length)];
                }
            }
        }
        return "Error: No se encontró texto para el síntoma " + tipoBuscado;
    }

    // barajar listas
    void Shuffle<T>(List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }


    void comenzarDialogo()
    {
        index = 0;
        if(lines.Count > 0)
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
            gameObject.SetActive(false);
        }
    }
}
