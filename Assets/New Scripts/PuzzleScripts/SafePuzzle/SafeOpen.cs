using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class SafeOpen : MonoBehaviour , IPointerDownHandler , IPointerUpHandler
{
    SafePuzzleController safePuzzleController;
    public bool touchDedected = false ;
    private void Awake() 
    {
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
        
        //animasyonlar
    }

    IEnumerator SuccessLockAnimation()
    {
        touchDedected = true;
        // SoundEffectManager.Play("walking"); // Buraya kasa zımbırtısını döndürme gelecek
        yield return new WaitForSeconds(1);
        safePuzzleController.CheckPuzzleCompletion();
    }
    
}
