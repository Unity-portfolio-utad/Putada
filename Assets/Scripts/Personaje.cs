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

    public Items activeItem;

    public Enfermedad.Enfermedades enfermedad = Enfermedad.Enfermedades.NULL;

    [SerializeField]
    int maxNpc = 3;
    int npcCount = 0;

    [SerializeField]
    GameObject prefab;


    void Start()
    {
        Array v = Enum.GetValues(typeof (Enfermedad.Enfermedades));
        enfermedad = (Enfermedad.Enfermedades) v.GetValue(UnityEngine.Random.Range(1, v.Length));
        if (armario == null)
        {
            armario = GameObject.Find("estanteria").GetComponent<Armario>();
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

        if (activeItem == item) {
            armario.showItem(activeItem);
            activeItem = Items.NULL;

            return;
        }

        activeItem = item;
        armario.hideItem(activeItem);
    }

    public void nextNpc(GameObject old)
    {
        GameObject.Destroy(old);

        GameObject.Instantiate(prefab);
        npcCount++;

        setActiveItem(Items.NULL);
    }
}
