using UnityEngine;

public class AfterToyPuzzle : MonoBehaviour
{
    [SerializeField] GameObject BeforeFinalCutScene, puzzle , toy; 
    bool dollCompleted;
    void Start()
    {
        dollCompleted = true;
        Destroy(toy);
    }
    void Update()
    {
        if(dollCompleted && Input.GetMouseButtonDown(0))
        {
            BeforeFinalCutScene.SetActive(true);
            Destroy(puzzle);
        }
    }
}
