using UnityEngine;

public class StepPuzzle : MonoBehaviour
{
    
    private BreakingWood[] allWoodPieces;
    
    
    public bool IsPuzzleCompleted { get; private set; } = false;

    void Start()
    {
        
        allWoodPieces = FindObjectsOfType<BreakingWood>();
        
        Debug.Log($"Toplam {allWoodPieces.Length} adet odun parçası bulundu.");
        
        if (allWoodPieces.Length != 5)
        {
             Debug.LogWarning("Bulmaca için beklenen 5 odun parçası bulunamadı. Kontrol edin.");
        }
    }

    
    public void CheckPuzzleCompletion()
    {
        if (IsPuzzleCompleted)
        {
            return; 
        }
        
        int completedCount = 0;
        
        
        foreach (BreakingWood woodPiece in allWoodPieces)
        {
            if (woodPiece.success)
            {
                completedCount++;
            }
        }

        
        if (completedCount == allWoodPieces.Length)
        {
            SolveMainPuzzle();
        }
    }

    private void SolveMainPuzzle()
    {
        IsPuzzleCompleted = true;
        
        Debug.Log("TEBRİKLER! TÜM 5 ODUN PARÇASI TAMAMLANDI VE BULMACA ÇÖZÜLDÜ!");
        
        
    }
}
