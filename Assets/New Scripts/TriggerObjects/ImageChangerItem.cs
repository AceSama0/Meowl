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
    void Awake()
    {
        playerInteract = FindAnyObjectByType<PlayerInteract>();
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerNear = true;
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
            LetterRead.sprite = newSprite;
            LetterText.sprite = newSprite;
            panel.SetActive(true);
            LetterButton.sprite = imageButtonSprite;
        }

        if (Input.GetMouseButtonDown(1))
        {
            panel.SetActive(false);
            Destroy(gameObject);

        }
    }
}
