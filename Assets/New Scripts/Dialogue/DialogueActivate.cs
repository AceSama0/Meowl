using UnityEngine;

public class DialogueActivate : MonoBehaviour
{
   public GameObject DialoguePanel;
    public GameObject SoundManager;

    void Start()
    {
        SoundManager.SetActive(false);
        DialoguePanel.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            DialoguePanel.SetActive(true);
            SoundManager.SetActive(true);
        }
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SoundManager.SetActive(false);
            DialoguePanel.SetActive(false);
        }
    }
}
