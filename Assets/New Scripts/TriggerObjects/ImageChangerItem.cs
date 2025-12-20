using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

public class ImageChangerItem : MonoBehaviour
{
    bool isPlayerNear;
    [SerializeField] GameObject panel;
    PlayerInteract playerInteract;
    [SerializeField] Image LetterRead, LetterText, LetterButton;
    [SerializeField] Sprite newSprite, imageButtonSprite;
    [SerializeField] GameObject keyObject;
    void Awake()
    {
        playerInteract = FindAnyObjectByType<PlayerInteract>();
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerNear = true;
            keyObject.SetActive(true);
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        isPlayerNear = false;
    }

    void Update()
    {
        if (isPlayerNear && playerInteract.interacting)
        {
            SoundEffectManager.Play("OpenPaper");
            LetterRead.sprite = newSprite;
            LetterText.sprite = newSprite;
            panel.SetActive(true);
            LetterButton.sprite = imageButtonSprite;
        }

        if (Input.GetMouseButtonDown(0) && isPlayerNear && gameObject.activeInHierarchy)
        {
            SoundEffectManager.Play("ClosePaper");
            panel.SetActive(false);
            Destroy(gameObject);
        }
    }
}
