using System;
using GameSystem.Interaction;
using UnityEngine;

public class LeftBookCollider : MonoBehaviour
{
    [SerializeField] private BookController bookController;

    void OnMouseDown()
    {
        Debug.Log("LeftBookCollider clicked");
        if (bookController != null)
        {
            bookController.PageAnim();
            bookController.PreviousPage();
        }
    }
}
