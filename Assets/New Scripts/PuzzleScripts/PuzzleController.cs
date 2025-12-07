using System.Collections;
using UnityEngine;

public class PuzzleController : MonoBehaviour
{

    [SerializeField] private SlotScripts[] allPuzzleSlots;
    [SerializeField] GameObject puzzle;
    [SerializeField] PuzzleSlotVerifier [] puzzleSlotVerifiers;
    public void CheckPuzzleStatus()
    {
        StartCoroutine(RunCheckDelayed());
    }
    private IEnumerator RunCheckDelayed()
    {

        yield return null;

        if (allPuzzleSlots == null || allPuzzleSlots.Length == 0)
        {
            Debug.LogError("HATA: Puzzle slotları atanmamış!");
            yield break;
        }

        bool isPuzzleSolved = true;

        foreach (SlotScripts slot in allPuzzleSlots)
        {

            PuzzleSlotVerifier rules = slot.GetComponent<PuzzleSlotVerifier>();

            if (rules == null || !rules.VerifyItemRule())
            {
                isPuzzleSolved = false;
                break;
            }
        }

        if (isPuzzleSolved)
        {

            Debug.Log("Yanlış yok");
            puzzle.SetActive(false);
            Destroy(this);

        }
        else
        {
            Debug.Log("Yanlış var");
        }
    }
}