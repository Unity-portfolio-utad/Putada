using UnityEngine;

public class PatientController : MonoBehaviour
{

    public Enfermedad.Enfermedades enfermedad;
    
    [SerializeField]
    private ScriptDialog dialog;

    private Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (dialog == null)
        {
            dialog = GetComponent<ScriptDialog>();

        }

        enfermedad = dialog.nakim;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
    {
        dialog.comenzarDialogo();
        animator.SetTrigger("quitarMascara");

    }
}