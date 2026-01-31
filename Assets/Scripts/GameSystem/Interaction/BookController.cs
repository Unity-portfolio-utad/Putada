using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace GameSystem.Interaction
{

    public class BookController : MonoBehaviour
    {
        //Continue
        public Sprite[] pages;
        public SpriteRenderer pageDisplay;
        private int currentPageIndex = 0;

        [SerializeField] private Collider nextPageCollider;
        [SerializeField] private Collider previousPageCollider;


        //     public void NextPage()
        //     {
        //         pageDisplay.gameObject.GetComponent<Animator>().SetTrigger("NextPage");
        //         if (currentPageIndex < pages.Length - 1)
        //         {
        //             currentPageIndex++;
        //             pageDisplay.sprite = pages[currentPageIndex];
        //         }
        //     }
        //
        //     public void PreviousPage()
        //     {
        //         pageDisplay.gameObject.GetComponent<Animator>().SetTrigger("NextPage");
        //         if (currentPageIndex > 0)
        //         {
        //             currentPageIndex--;
        //             pageDisplay.sprite = pages[currentPageIndex];
        //         }
        //     }
        //
        //
        //     private void OnTriggerEnter(Collider other)
        //     {
        //         if (other == nextPageCollider)
        //         {
        //             NextPage();
        //         }
        //         else if (other == previousPageCollider)
        //         {
        //             PreviousPage();
        //         }
        //     }

        //Rehazlo pero en vez de OnTriggerEnter, cuando el jugador lo pulse con el ratón
        private void Update()
        {
            if (Input.GetMouseButtonDown(0)) // Botón izquierdo del ratón
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider == nextPageCollider)
                    {
                        NextPage();
                    }
                    else if (hit.collider == previousPageCollider)
                    {
                        PreviousPage();
                    }
                }
            }
        }

        // public void NextPage()
        // {
        //     if (currentPageIndex < pages.Length - 1)
        //     {
        //         currentPageIndex++;
        //         pageDisplay.sprite = pages[currentPageIndex];
        //     }
        // }
        //
        // public void PreviousPage()
        // {
        //     if (currentPageIndex > 0)
        //     {
        //         currentPageIndex--;
        //         pageDisplay.sprite = pages[currentPageIndex];
        //     }
        // }

        //Rewrite the functions adding the activation of a child of this GameObject that when its active it plays the animation by itself
        public void NextPage()
        {
            if (currentPageIndex < pages.Length - 1)
            {
                currentPageIndex++;
                pageDisplay.sprite = pages[currentPageIndex];
                Transform animationObject = transform.Find("Animacion");
                if (animationObject != null)
                {
                    animationObject.gameObject.SetActive(true);
                }

            }

        }

        public void PreviousPage()
        {
            if (currentPageIndex > 0)
            {
                currentPageIndex--;
                pageDisplay.sprite = pages[currentPageIndex];
                Transform animationObject = transform.Find("Animacion");
                if (animationObject != null)
                {
                    animationObject.gameObject.SetActive(true);
                }
            }
        }
    }
}

