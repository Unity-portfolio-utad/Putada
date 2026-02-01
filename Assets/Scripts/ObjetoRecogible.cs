using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class ObjetoRecogible : MonoBehaviour
{
    [SerializeField]
    public Personaje.Items itemType = Personaje.Items.CERILLAS;

    [SerializeField]
    private Personaje personaje;

    [SerializeField]
    private GameObject objeto;
    
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip pickUpSound;
    [SerializeField] AudioClip actionSound;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("Inicio");
        if (personaje == null)
        {
            personaje = GameObject.Find("Player").GetComponent<Personaje>();
        }
    }

    void Update()
    {
        if (objeto == null) return;
        transform.LookAt(objeto.transform.position);
        transform.transform.rotation = Quaternion.Euler( transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + 180, transform.rotation.eulerAngles.z );
    }


    public void OnMouseDown()
    {
        personaje.setActiveItem(itemType);
        Debug.Log(itemType);
        if (audioSource != null && pickUpSound != null)
        {
            audioSource.PlayOneShot(pickUpSound);
        }
        if(personaje.npcCount == personaje.maxNpc)
        {
            StartCoroutine(PlayActionSound());
            
        }
    }

    private IEnumerator PlayActionSound()
    {
        if (audioSource != null && actionSound != null)
        {
            audioSource.PlayOneShot(actionSound);
            yield return new WaitForSeconds(3);
            audioSource.Stop();
            
        }
    }
}
