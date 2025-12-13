using UnityEngine;
using UnityEngine.UI;
namespace DiaglogueSystem
{
    public class DialogueLine : DialogueBaseClass
    {
        private Text textHolder;
        
        [Header("Text Options")]
        [SerializeField] Color textColor;
        [SerializeField] Font textFont;
        [SerializeField] string input;

        [Header("Time parametres")]
        [SerializeField] float delayBetween;
        [SerializeField] float delay;

        [Header("Character Image")]
        [SerializeField] Sprite characterSprite;
        [SerializeField] Image imageHolder;
        void Awake()
        {
            textHolder = GetComponent<Text>();
            textHolder.text = "";

            imageHolder.sprite = characterSprite;
            // imageHolder.preserveAspect = true;
        }
        void Start()
        {
            StartCoroutine(WriteText(input, textHolder, delay, textColor, textFont));
        }
    }
}

