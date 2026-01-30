using System;
using UnityEngine;

public class Personaje : MonoBehaviour
{

    [SerializeField]
    Armario armario;
    
    public enum Items {
        NULL = 0,
        JARABE = 1,
        CREMA = 2,
        SIERRA = 3,
        LIJAS = 4,
        SAL = 5,
        PICO = 6,
        CERILLAS= 7
    }

    private Items activeItem;

    void Start()
    {
        if (armario == null)
        {
            armario = GameObject.Find("Armario").GetComponent<Armario>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setActiveItem(Items item)
    {

        if (activeItem != Items.NULL)
        {
            armario.showItem(activeItem);
        }

        activeItem = item;
        armario.hideItem(activeItem);
    }
}
