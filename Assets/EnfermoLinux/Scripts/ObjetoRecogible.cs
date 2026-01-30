using System;
using Unity.VisualScripting;
using UnityEngine;

public class ObjetoRecogible : MonoBehaviour
{
    [SerializeField]
    Personaje.Items itemType = Personaje.Items.CERILLAS;

    [SerializeField]
    private Personaje personaje;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (personaje == null)
        {
            personaje = GameObject.Find("Player").GetComponent<Personaje>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
    {
        personaje.setActiveItem(itemType);
    }
}
