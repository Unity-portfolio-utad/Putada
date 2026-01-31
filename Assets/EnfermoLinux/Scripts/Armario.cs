using System;
using System.Collections.Generic;
using UnityEngine;

public class Armario : MonoBehaviour
{
    [SerializeField]
    Dictionary<Personaje.Items, GameObject> objetos = new Dictionary<Personaje.Items, GameObject>();

    [SerializeField]
    ObjetoRecogible[] recogibles;

    void Start()
    {
        for (int i = 0; i < recogibles.Length; i++)
        {
            objetos.Add(recogibles[i].itemType, recogibles[i].gameObject);
            Debug.Log(i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void hideItem(Personaje.Items item)
    {
        if (objetos.ContainsKey(item))
        {
            objetos[item].SetActive(false);
        }
    }

    public void showItem(Personaje.Items item)
    {
        if (objetos.ContainsKey(item))
        {
            objetos[item].SetActive(true);
        }   
    }
}
