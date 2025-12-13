using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class Lock : MonoBehaviour , IPointerClickHandler
{
    [SerializeField] Sprite[] sprites;
    SafePuzzleController safePuzzleController;
    int spriteIndex = 0;
    [SerializeField] int wantedInt;
    Image image; 
    Animator animator;
    public bool success = false;
    void Awake()
    {
        safePuzzleController = FindAnyObjectByType<SafePuzzleController>();
        animator = GetComponent<Animator>();
        image = GetComponent<Image>();
        image.sprite = sprites[0];
    }
    public void OnPointerClick(PointerEventData eventData)
    {

        StartCoroutine(changeAnimaton());
        SoundEffectManager.Play("walking");
        if(sprites != null)
        {
            spriteIndex = (spriteIndex + 1) % sprites.Length;
            image.sprite = sprites[spriteIndex];
            if (spriteIndex == wantedInt)
            {
                success = true;
                // safePuzzleController.CheckPuzzleCompletion();
            }
        }   
    }

    IEnumerator changeAnimaton()
    {
        animator.SetBool("Lock", true);
        yield return new WaitForSeconds(1);
        animator.SetBool("Lock", false);
    }
}
