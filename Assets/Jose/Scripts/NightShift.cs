using UnityEngine;
using UnityEngine.UI;

public class NightShift : MonoBehaviour
{
    [SerializeField] private AudioClip[] audioDias;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Image dieImage;

    //Un canva que se activa al morir
    [SerializeField] private GameObject canvas;

    //Tiene un texto que dice cuantos pacientes han muerto
    [SerializeField] private TMPro.TextMeshProUGUI textoMuertes;
    
    public void ActivarNightShift(int muertes, bool playerDied = false)
    {
        StartCoroutine(FadeCanvasAndChangeAudio(muertes, playerDied));
    }

    private System.Collections.IEnumerator FadeCanvasAndChangeAudio(int muertes, bool playerDied = false)
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
            if(playerDied)
                //Si el jugador a muerto poner la imagen de muerte como fondo
                dieImage.enabled = true;
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
            int currentIndex = System.Array.IndexOf(audioDias, audioSource.clip);
            int nextIndex = (currentIndex + 1) % audioDias.Length;
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
}
