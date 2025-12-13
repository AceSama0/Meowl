using System.Collections;
using UnityEngine;

public class BreakingWood : MonoBehaviour
{
    [SerializeField] private StepPuzzle stepPuzzle;

    public bool success { get; private set; } = false;
    
    private bool hasSucceeded = false; 

    void Start()
    {
        stepPuzzle = FindAnyObjectByType<StepPuzzle>();
    }
    

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !hasSucceeded && stepPuzzle != null && !stepPuzzle.IsPuzzleCompleted)
        {
            hasSucceeded = true;
            
            success = true;
            SoundEffectManager.Play("deneme");
            
            stepPuzzle.CheckPuzzleCompletion();
        }
    }
}