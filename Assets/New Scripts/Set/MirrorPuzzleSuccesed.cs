using UnityEngine;
using UnityEngine.UI;

public class MirrorPuzzleSuccesed : MonoBehaviour
{
    [SerializeField] Image LetterRead, LetterText, LetterButton;
    [SerializeField] Sprite newSprite, imageButtonSprite;
    [SerializeField] GameObject panel;
    void Awake()
    {
        panel.SetActive(true);
        LetterRead.sprite = newSprite;
        LetterText.sprite = newSprite;
        LetterButton.sprite = imageButtonSprite;
    }

    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            panel.SetActive(false);
        }
    }
}
