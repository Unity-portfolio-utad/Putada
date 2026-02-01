using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Personaje : MonoBehaviour
{
    bool locke = false;
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

    [SerializeField] public int maxNpc = 3;
    public int npcCount = 0;

    [SerializeField]  public GameObject [] prefab ;
  

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
                Debug.Log((int)item + " vs " + (int)enfermedad);
                if (locke) return;
                locke = true;
                // comparación explícita, evitando casteos entre enums distintos
                bool cures = true;
                switch (enfermedad)
                {
                    case Enfermedad.Enfermedades.CATARRO:
                        cures = (item == Items.JARABE);
                        break;
                    case Enfermedad.Enfermedades.SARPULLIDO:
                        cures = (item == Items.CREMA);
                        break;
                    case Enfermedad.Enfermedades.OTITIS:
                        cures = (item == Items.LIJAS);
                        break;
                    case Enfermedad.Enfermedades.RAMAS_BRAZOS:
                        cures = (item == Items.SIERRA);
                        break;
                    case Enfermedad.Enfermedades.OJOS_CARACOL:
                        cures = (item == Items.SAL);
                        break;
                    case Enfermedad.Enfermedades.LEPRA:
                        cures = (item == Items.PICO);
                        break;
                    case Enfermedad.Enfermedades.BICHOS_OJOS:
                        cures = (item == Items.CERILLAS);
                        break;
                }

                if (!cures)
                {
                    nightShift.ActivarNightShift(dead, true);
                }
                else
                {
                    npcCount = 0;
                    maxNpc += 1;
                    nightShift.ActivarNightShift(dead, false);
                    dead = 0;

                    // Devolver el objeto a la balda y limpiar el item activo antes de generar el siguiente NPC
                    if (armario != null)
                    {
                        armario.showItem(item);
                    }
                    activeItem = Items.NULL;

                    nextNpc(null);
                    locke = false;
                    return;
                }
            }
            else
            {
                Debug.LogWarning(
                    $"MirrorInteraction: nightShift no asignado en Inspector ni encontrado en escena. GameObject: {gameObject.name}",
                    this);
            }
        }
        if (activeItem != Items.NULL)
        {
            armario.showItem(activeItem);
        }
        if (activeItem == item)
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
        if (old != null)
            GameObject.Destroy(old);
        
        setActiveItem(Items.NULL);

        
        npcCount++;
        if (npcCount >= maxNpc)
        {
            if (nightShift == null)
            {
                // intentar recuperar una referencia válida en la escena
                nightShift = FindFirstObjectByType<NightShift>();
            }
            nightShift.FastMsg("That was the last one, I should check myself in the mirror...");
            Array v = Enum.GetValues(typeof(Enfermedad.Enfermedades));
            enfermedad = (Enfermedad.Enfermedades)v.GetValue(UnityEngine.Random.Range(1, v.Length));
        }
        else
        {

           GameObject temp = GameObject.Instantiate(prefab[Random.Range(0, prefab.Length)]);
            temp.GetComponent<ScriptDialog>().leve = Random.value > 0.5f;


            if (npcCount == maxNpc)
            {
                Array v = Enum.GetValues(typeof(Enfermedad.Enfermedades));
                enfermedad = (Enfermedad.Enfermedades)v.GetValue(UnityEngine.Random.Range(1, 7));

            }
        }
    }
}
