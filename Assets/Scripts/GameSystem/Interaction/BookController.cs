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
        [SerializeField] AudioSource pageTurnSound;
        [SerializeField] AudioClip pageTurnClip;


        public void NextPage()
        {
            if (currentPageIndex < pages.Length - 1)
            {
                currentPageIndex++;
                pageDisplay.sprite = pages[currentPageIndex];
            }
            else
            {
                currentPageIndex = 0;
                pageDisplay.sprite = pages[currentPageIndex];
            }
        }
        
        public void PreviousPage()
        {
            if (currentPageIndex > 0)
            {
                currentPageIndex--;
                pageDisplay.sprite = pages[currentPageIndex];
            }
            else
            {
                currentPageIndex = pages.Length - 1;
                pageDisplay.sprite = pages[currentPageIndex];
            }
        }
        
        
        public void PageAnim()
        {   
                pageTurnSound.PlayOneShot(pageTurnClip);
                Transform animationObject = transform.Find("Animacion");
                if (animationObject != null)
                {
                    //Corrutine for wait until animation ends
                    StartCoroutine(PlayAnimationAndWait(animationObject.gameObject));
                }
        }
        private IEnumerator PlayAnimationAndWait(GameObject animationObject)
        {
            animationObject.SetActive(true);
            Animator animator = animationObject.GetComponent<Animator>();
            if (animator != null)
            {
                animator.Play("Animacion");
                // Wait until the animation is finished
                yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
            }
            animationObject.SetActive(false);
        }
    }
}

