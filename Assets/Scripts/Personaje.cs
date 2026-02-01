using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Personaje : MonoBehaviour
{

    [SerializeField] Armario armario;
    [SerializeField] NightShift nightShift;

    public enum Items
    {
        NULL = 0,
        JARABE = 1,
        CREMA = 2,
        SIERRA = 3,
        LIJAS = 4,
        SAL = 5,
        PICO = 6,
        CERILLAS = 7
    }

    public Items activeItem;

    public Enfermedad.Enfermedades enfermedad = Enfermedad.Enfermedades.NULL;

    [SerializeField] int maxNpc = 3;
    int npcCount = 0;

    [SerializeField] GameObject prefab;

    public int dead = 0;


    void Start()
    {
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
        if (npcCount >= maxNpc)
        {
            if (nightShift == null)
            {
                // intentar recuperar una referencia válida en la escena
                nightShift = FindFirstObjectByType<NightShift>();
            }

            if (nightShift != null)
            {
                if ((int)item != (int)enfermedad)
                {
                    nightShift.ActivarNightShift(dead, true);
                }
                else
                {
                    nightShift.ActivarNightShift(dead, false);
                }

                npcCount = 0;
                maxNpc += 1;
            }
            else
            {
                Debug.LogWarning(
                    $"MirrorInteraction: nightShift no asignado en Inspector ni encontrado en escena. GameObject: {gameObject.name}",
                    this);
            }
        }
        else if (activeItem != Items.NULL)
        {
            armario.showItem(activeItem);
        }
        else if (activeItem == item)
        {
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

        setActiveItem(Items.NULL);
        npcCount++;
        if (npcCount >= maxNpc)
        {
            Array v = Enum.GetValues(typeof(Enfermedad.Enfermedades));
            enfermedad = (Enfermedad.Enfermedades)v.GetValue(UnityEngine.Random.Range(1, v.Length));
        }
        else
        {
            GameObject.Instantiate(prefab);


            GameObject temp = GameObject.Instantiate(prefab);
            temp.GetComponent<ScriptDialog>().leve = Random.Range(0, 1) > 0.5;
            npcCount++;

            setActiveItem(Items.NULL);

            if (npcCount == maxNpc)
            {
                Array v = Enum.GetValues(typeof(Enfermedad.Enfermedades));
                enfermedad = (Enfermedad.Enfermedades)v.GetValue(UnityEngine.Random.Range(1, v.Length));

            }
        }
    }
}

