using UnityEngine;

public class FinalRotoStarter : MonoBehaviour
{
    [SerializeField] GameObject DialogueHolder , finalRoto, beforeCutScene;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!DialogueHolder.activeInHierarchy)
        {
            finalRoto.SetActive(true);
        }
    }
}
