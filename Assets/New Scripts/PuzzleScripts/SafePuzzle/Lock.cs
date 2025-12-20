using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class Lock : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] Sprite[] sprites;

    int spriteIndex = 0;
    [SerializeField] int wantedInt;
    Image image;
    [SerializeField] Animator animator;
    [SerializeField] string LockName;
    public bool success = false;
    void Awake()
    {

        image = GetComponent<Image>();
        image.sprite = sprites[0];
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        StartCoroutine(changeAnimaton());
        SoundEffectManager.Play("rollingNumbers");
        
    }

    IEnumerator changeAnimaton()
    {
        image.enabled = false;
        if (gameObject.name == LockName)
        {
            animator.SetBool(LockName, true);
            yield return new WaitForSeconds(0.5f);
            animator.SetBool(LockName, false);
            image.enabled = true;
        }

        if (sprites != null)
        {
            spriteIndex = (spriteIndex + 1) % sprites.Length;
            image.sprite = sprites[spriteIndex];
            if (spriteIndex == wantedInt)
            {
                success = true;

            }
        }
    }
}
