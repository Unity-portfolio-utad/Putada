using System;
using UnityEngine;
using UnityEngine.UI;

public class NightShift : MonoBehaviour
{
    [SerializeField] private AudioClip[] audioDias;
    [SerializeField] private AudioSource audioSource;

    //Un canva que se activa al morir
    [SerializeField] private GameObject canvas;

    //Tiene un texto que dice cuantos pacientes han muerto
    [SerializeField] private TMPro.TextMeshProUGUI textoMuertes;
    private int currentIndex;
    private int nextIndex;


    private void Start()
    {
        if (audioDias != null && audioDias.Length > 0 && audioSource != null)
        {
            currentIndex = System.Array.IndexOf(audioDias, audioSource.clip);
            nextIndex = (currentIndex + 1) % audioDias.Length;
            audioSource.clip = audioDias[nextIndex];
            audioSource.Play();
        }
    }

    public void ActivarNightShift(int muertes)
    {
        StartCoroutine(FadeCanvasAndChangeAudio(muertes));
    }

    private System.Collections.IEnumerator FadeCanvasAndChangeAudio(int muertes)
    {
        //Activar canvas
        canvas.SetActive(true);

        //Fade in
        float duration = 1f; // Duración del fade
        float currentTime = 0f;
        CanvasGroup canvasGroup = canvas.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = canvas.AddComponent<CanvasGroup>();

        }

        canvasGroup.alpha = 0f;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(currentTime / duration);
            yield return null;
        }

        //Mostrar texto
        textoMuertes.text = "";
        string fullText = "Patients deceased: " + muertes;
        for (int i = 0; i < fullText.Length; i++)
        {
            textoMuertes.text += fullText[i];
            yield return new WaitForSeconds(0.1f);
        }

        //Esperar 2 segundos
        yield return new WaitForSeconds(2f);

        //Cambiar audio al siguiente dia
        //Aqui solo debes coger el siguiente audio del array
        if (audioDias != null && audioDias.Length > 0 && audioSource != null)
        {
            currentIndex = System.Array.IndexOf(audioDias, audioSource.clip);
            nextIndex = (currentIndex + 1) % audioDias.Length;
            audioSource.clip = audioDias[nextIndex];
            audioSource.Play();
        }

        //Fade out
        currentTime = 0f;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            canvasGroup.alpha = 1f - Mathf.Clamp01(currentTime / duration);
            yield return null;
        }

        //Desactivar canvas
        canvas.SetActive(false);
    }
    
    //USO
    
    //[SerializeField] NightShift nightShift;
    
    // if (nightShift == null)
    // {
    //     // intentar recuperar una referencia válida en la escena
    //     nightShift = FindFirstObjectByType<NightShift>();
    // }
    //
    // if (nightShift != null)
    // {
    //     nightShift.ActivarNightShift(3, true);
    // }
    // else
    // {
    //     Debug.LogWarning($"MirrorInteraction: nightShift no asignado en Inspector ni encontrado en escena. GameObject: {gameObject.name}", this);
    // }
}
