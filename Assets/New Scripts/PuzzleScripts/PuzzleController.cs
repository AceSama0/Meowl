using System.Collections;
using UnityEngine;

public class PuzzleController : MonoBehaviour
{
    [SerializeField] private SlotScripts[] allPuzzleSlots;
    Player player;
    [SerializeField] GameObject puzzle;
    private InventoryScript inventoryScript;
    void Awake()
    {
        inventoryScript = FindAnyObjectByType<InventoryScript>();
        player = FindAnyObjectByType<Player>();
    }

    private bool EnsureSlotsAreLoaded()
    {
        if (allPuzzleSlots == null || allPuzzleSlots.Length == 0)
        {
            allPuzzleSlots = GetComponentsInChildren<SlotScripts>(true); 
        }
        
        if (allPuzzleSlots == null || allPuzzleSlots.Length == 0)
        {
            Debug.LogError($"SLOT HATASI: {gameObject.name} için puzzle slotları bulunamadı! Hierarchy kontrol edin.");
            return false;
        }
        return true;
    }

    public void CheckPuzzleStatus()
    {
        StartCoroutine(RunCheckDelayed());
    }

    private IEnumerator RunCheckDelayed()
    {
        yield return null; 
        yield return null; 

        if (!EnsureSlotsAreLoaded())
        {
            yield break;
        }

        bool isPuzzleSolved = true;

        foreach (SlotScripts slot in allPuzzleSlots)
        {
            PuzzleSlotVerifier rules = slot.GetComponent<PuzzleSlotVerifier>();

            if (rules == null)
            {
                 Debug.LogError($"KRİTİK HATA: {slot.name} üzerinde PuzzleSlotVerifier yok!");
                 isPuzzleSolved = false;
                 break;
            }

            if (!rules.VerifyItemRule())
            {
                isPuzzleSolved = false;
                break;
            }
        }

        if (isPuzzleSolved)
        {
            // Debug.Log("Yanlış yok");
            
            if (puzzle != null)
            {
                SoundEffectManager.Play("success");
                inventoryScript.isAnotherScreenOpened = false;
                puzzle.SetActive(false);
                player.canMove = true;
                
            }
            Destroy(gameObject);
        }
        else
        {
            // Debug.Log("Yanlış var");
        }
    }
}