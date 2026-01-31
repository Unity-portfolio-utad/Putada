using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.InputSystem;
public class ScriptDialog : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
     public string[] lines;
    public float textSpeed;
    private int index;
    void Start()
    {
       textComponent.text = string.Empty;
       comenzarDialogo();
    }

    // Update is called once per frame
    void Update()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (textComponent.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
    }


    void comenzarDialogo()
    {
        index =  0;
        StartCoroutine(Typeline());
    }

    IEnumerator Typeline()
    {

      // tipea cada char 1 por uno

      foreach(char c in lines[index].ToCharArray())
        {
        textComponent.text += c;
        yield return new WaitForSeconds(textSpeed);
        } 

    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(Typeline());
        }

        else
        {
            gameObject.SetActive(false);
        }
 
    }

}


