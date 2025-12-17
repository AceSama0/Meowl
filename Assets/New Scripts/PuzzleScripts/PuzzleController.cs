using System.Collections;
using UnityEngine;

public class PuzzleController : MonoBehaviour
{
    [SerializeField] private SlotScripts[] allPuzzleSlots;
    Player player;
    [SerializeField] GameObject puzzle,finalHint;

    private InventoryScript inventoryScript;
    void Awake()
    {
        inventoryScript = FindAnyObjectByType<InventoryScript>();
        player = FindAnyObjectByType<Player>();
    }

    public void CheckPuzzleStatus()
    {
        bool isPuzzleSolved = true;

        foreach (SlotScripts slot in allPuzzleSlots)
        {
            PuzzleSlotVerifier rules = slot.GetComponent<PuzzleSlotVerifier>();
            bool isValid = rules.VerifyItemRule();
            string itemName = (slot.currentImage != null) ? slot.currentImage.name : "BOŞ";
            Debug.Log($"Slot: {slot.name} | Item: {itemName} | Doğru: {isValid}");
            if (!isValid)
            {
                isPuzzleSolved = false;
            }
        }

        if (isPuzzleSolved)
        {

            if (puzzle != null)
            {
                SoundEffectManager.Play("success");
                inventoryScript.isAnotherScreenOpened = false;
                player.canMove = true;
                Destroy(puzzle);
                if (finalHint != null)
                {
                    finalHint.SetActive(true);
                }
            }
            Destroy(gameObject);
        }
    }
}