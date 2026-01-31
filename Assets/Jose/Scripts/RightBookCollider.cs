using GameSystem.Interaction;
using UnityEngine;

public class RightBookCollider : MonoBehaviour
{
    [SerializeField] private BookController bookController;

    void OnMouseDown()
    {
        Debug.Log("RightBookCollider clicked");
        if (bookController != null)
        {
            bookController.PageAnim();
            bookController.NextPage();
        }
    }
}
