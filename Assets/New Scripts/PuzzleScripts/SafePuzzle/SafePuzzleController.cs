using UnityEngine;
using UnityEngine.EventSystems;

public class SafePuzzleController : MonoBehaviour , IPointerClickHandler
{
    Lock[] locks;
    [SerializeField] GameObject puzzleUI;
    SafeOpen safeOpen;
    public bool IsPuzzleCompleted { get; private set; }
    void Awake()
    {
        safeOpen = FindAnyObjectByType<SafeOpen>();
        locks = FindObjectsOfType<Lock>();
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void CheckPuzzleCompletion()
    {
        if (IsPuzzleCompleted)
        {
            return; 
        }
        
        int completedCount = 0;

        foreach (Lock lockPiece in locks)
        {
            if(lockPiece.success)
            {
                completedCount++;
            }
        }

        if (completedCount == locks.Length && safeOpen.touchDedected)
        {
            SolveMainPuzzle();
        }
        else if (completedCount != locks.Length && safeOpen.touchDedected)
        {
            FailedMainPuzzle();
        }
    }

    private void SolveMainPuzzle()
    {
        Player player = FindAnyObjectByType<Player>();
        player.canMove = true;
        IsPuzzleCompleted = true;  
        SoundEffectManager.Play("success");
        Debug.Log("Kasa Şifresi Bulundu");
        Destroy(puzzleUI);     
    }
    private void FailedMainPuzzle()
    {    
        IsPuzzleCompleted = false;      
        SoundEffectManager.Play("walking");  
        Debug.Log("Kasa Şifresi Bulunamadı");
    }

    
}
