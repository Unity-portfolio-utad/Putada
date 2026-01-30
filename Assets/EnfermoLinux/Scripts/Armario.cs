using System.Collections.Generic;
using UnityEngine;

public class Armario : MonoBehaviour
{
    [SerializeField]
    Dictionary<Personaje.Items, GameObject> objetos;

    void Start()
    {
        
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
