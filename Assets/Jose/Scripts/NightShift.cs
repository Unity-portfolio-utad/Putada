using System;
using UnityEngine;
using UnityEngine.UI;

public class NightShift : MonoBehaviour
{
    [SerializeField] private AudioClip[] audioDias;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioMuerte;

    //Un canva que se activa al morir
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject canvasRapido;

    //Tiene un texto que dice cuantos pacientes han muerto
    [SerializeField] private TMPro.TextMeshProUGUI textoMuertes;
    [SerializeField] private TMPro.TextMeshProUGUI textoRapido;
    private int currentIndex;
    private int nextIndex;
    private bool deathActivated = false;


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

    public void ActivarNightShift(int muertes, bool death)
    {
        StartCoroutine(FadeCanvasAndChangeAudio(muertes, death));
    }

    private System.Collections.IEnumerator FadeCanvasAndChangeAudio(int muertes, bool death)
    {
        deathActivated = death;
        //Activar canvas
        canvas.SetActive(true);

        //Fade in
        float duration = 2f; // Duración del fade
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
    
        if(!deathActivated)
        {
            //Mostrar texto
            textoMuertes.text = "";
            string fullText = "Patients deceased tonight: " + muertes;
            for (int i = 0; i < fullText.Length; i++)
            {
                textoMuertes.text += fullText[i];
                yield return new WaitForSeconds(0.1f);
            }

            //Esperar 2 segundos
            yield return new WaitForSeconds(3f);

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
            textoMuertes.text = "";
        }else if(deathActivated)
        {
            //Mostrar texto
            textoMuertes.text = "";
            string fullText = "You DIED, choose wisely next time.";
            for (int i = 0; i < fullText.Length; i++)
            {
                textoMuertes.text += fullText[i];
                yield return new WaitForSeconds(0.1f);
            }

            audioSource.clip = audioMuerte;
            audioSource.Play();
            //Esperar 2 segundos
            yield return new WaitForSeconds(3f);
            
            //Restart scene
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }
        
    }
    
    //Haz una funcion llamada FastMsg(string msg) que muestre un mensaje rapido en el canvas, //Mostrar texto
    // textoMuertes.text = "";
    // string fullText = "Patients deceased tonight: " + muertes;
    //     for (int i = 0; i < fullText.Length; i++)
    // {
    //     textoMuertes.text += fullText[i];
    //     yield return new WaitForSeconds(0.1f);
    // }
    public void FastMsg(string msg)
    {
        StartCoroutine(FastMsgCoroutine(msg));
    }

    private System.Collections.IEnumerator FastMsgCoroutine(string msg)
    {
        canvasRapido.SetActive(true);
        
        //Fade in
        float duration = 3f; // Duración del fade
        float currentTime = 0f;
        CanvasGroup canvasGroup = canvasRapido.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = canvasRapido.AddComponent<CanvasGroup>();

        }

        canvasGroup.alpha = 0f;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(currentTime / duration);
            yield return null;
        }
        
        textoRapido.text = "";
        string fullText = msg;
        for (int i = 0; i < fullText.Length; i++)
        {
            textoRapido.text += fullText[i];
            yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitForSeconds(3f);
        
        //Fade out
        currentTime = 0f;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            canvasGroup.alpha = 1f - Mathf.Clamp01(currentTime / duration);
            yield return null;
        }

        //Desactivar canvas
        canvasRapido.SetActive(false);
        textoRapido.text = "";
        
        // coroutine finalizada
        yield break;
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
