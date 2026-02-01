using UnityEngine;

// ============================================================
// MirrorInteraction.cs - Interacción con el espejo
// ============================================================

/// <summary>
/// Gestiona la interacción con el espejo.
/// Si el jugador tiene ojos de caracol no tratado, no puede usarlo.
/// </summary>
[RequireComponent(typeof(Collider))]
public class MirrorInteraction : MonoBehaviour
{
    public string disabledMessage = "Desde la enfermedad del caracol no te puedes ni mirar a la cara...";

    [SerializeField] PatientController patientController;
    [SerializeField] SpriteRenderer mirrorSprite;
    public Sprite[] pics;
    private int currentPic = 0;
    bool isViewing = false;
    [SerializeField] CamMovement camMovement;

    void Start()
    {
        if (mirrorSprite != null)
        {
            var c = mirrorSprite.color; c.a = 0f; mirrorSprite.color = c; mirrorSprite.gameObject.SetActive(false);
        }
    }

    void OnMouseDown()
    {
        if (isViewing) return;
        
        // Usar el espejo normalmente
        StartCoroutine(ShowSequence());
    }

    System.Collections.IEnumerator ShowSequence()
    {
        Debug.Log("Usando espejo...");

        currentPic = (int)FindAnyObjectByType<Personaje>().enfermedad;//(int)patientController.enfermedad;
        
        if (mirrorSprite != null && pics != null && currentPic >= 0 && currentPic < pics.Length)
            mirrorSprite.sprite = pics[currentPic];
        isViewing = true;
        if (camMovement != null) camMovement.enabled = false;
        Cursor.lockState = CursorLockMode.Locked; Cursor.visible = false;
        if (mirrorSprite != null) mirrorSprite.gameObject.SetActive(true);

        //scriptDialog.dialogoTemporal(currentPic);
        
        float target = 220f/255f;
        float t = 0f;
        while (t < 2f)
        {
            t += Time.deltaTime; if (mirrorSprite != null){var c=mirrorSprite.color;c.a=Mathf.Lerp(0f,target,t/2f);mirrorSprite.color=c;} yield return null;
        }
        if (mirrorSprite != null){var c2=mirrorSprite.color;c2.a=target;mirrorSprite.color=c2;}
        
        yield return new WaitForSeconds(2);
        t = 0f;
        while (t < 5f)
        {
            t += Time.deltaTime; if (mirrorSprite != null){var c=mirrorSprite.color;c.a=Mathf.Lerp(target,0f,t/5f);mirrorSprite.color=c;} yield return null;
        }
        if (mirrorSprite != null){var c3=mirrorSprite.color;c3.a=0f;mirrorSprite.color=c3;mirrorSprite.gameObject.SetActive(false);}

        if (camMovement != null) camMovement.enabled = true;
        Cursor.lockState = CursorLockMode.None; Cursor.visible = true;
        isViewing = false;
    }
}
