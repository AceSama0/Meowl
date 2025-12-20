using UnityEngine;

public class DialogueStarter : MonoBehaviour
{
    [SerializeField] GameObject dialogue;
    bool wasActive = false;
    public void ActivateDialogue()
    {
        dialogue.SetActive(true);
    }

    public bool DialogueActive()
    {
        return dialogue.activeInHierarchy;
    }

    void Update()
    {
        
        if (dialogue.activeInHierarchy)
        {
            wasActive = true;
        }

        
        if (wasActive && !dialogue.activeInHierarchy)
        {
            Destroy(gameObject);
        }
    }
}
