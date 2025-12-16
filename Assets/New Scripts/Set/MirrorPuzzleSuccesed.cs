using UnityEngine;
using UnityEngine.UI;

public class MirrorPuzzleSuccesed : MonoBehaviour
{
    [SerializeField] Image LetterRead, LetterText, LetterButton;
    [SerializeField] Sprite newSprite, imageButtonSprite;
    [SerializeField] GameObject panel,brokenMirror;
    void Start()
    {
        Destroy(brokenMirror);
        panel.SetActive(true);
        LetterRead.sprite = newSprite;
        LetterText.sprite = newSprite;
        LetterButton.sprite = imageButtonSprite;
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            Destroy(panel);
        }
    }
}
