using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class SafeOpen : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    SafePuzzleController safePuzzleController;
    public bool touchDedected = false;
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        safePuzzleController = FindAnyObjectByType<SafePuzzleController>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        StartCoroutine(SuccessLockAnimation());


        //animasyonlar
    }

    public void OnPointerUp(PointerEventData eventData)
    {

        touchDedected = false;
        SoundEffectManager.Play("failed");
        animator.SetBool("open", false);

        //animasyonlar
    }

    IEnumerator SuccessLockAnimation()
    {
        touchDedected = true;
        // SoundEffectManager.Play("walking"); // Buraya kasa zımbırtısını döndürme gelecek
        animator.SetBool("open", true);
        yield return new WaitForSeconds(2);
        safePuzzleController.CheckPuzzleCompletion();
    }

}
