using UnityEngine;
using UnityEngine.SceneManagement;

public class TrainPuzzle : MonoBehaviour
{
    [SerializeField] GameObject puzzle;

    [SerializeField] private SlotScripts[] slots;

    private const int Reqired_ID_FOR_A = 4;
    private const int Reqired_ID_FOR_B = 1;
    private const int Reqired_ID_FOR_C = 3;
    private const int Reqired_ID_FOR_D = 2;
    private const int Reqired_ID_FOR_E = 5;

    public bool CheckForCorrectItem(SlotScripts targetSlot, int requiredItemID)
    {
        if (targetSlot.currentImage == null)
        {
            return false;
        }

        GameObject itemObject = targetSlot.currentImage;
        Item itemComponent = itemObject.GetComponent<Item>();

        if (itemComponent != null)
        {
            if (itemComponent.ID == requiredItemID)
            {
                return true;
            }
        }
        return false;
    }

    public void CheckPuzzleStatus()
    {
        // Array'in atanıp atanmadığını kontrol edin (Inspector'da unuttuysanız NullReference verir)
        if (slots == null || slots.Length == 0)
        {
            Debug.LogError("SLOTS DİZİSİ EKSİK: Inspector'da slots[] dizisini atadığınızdan emin olun!");
            return;
        }

        // Sadece Slot A'nın kontrolünü yap
        bool isSlotACorrect = CheckForCorrectItem(slots[0], Reqired_ID_FOR_A);

        // ⚠️ KRİTİK DEBUG ÇIKTISI
        Debug.Log($"--- SLOT A KONTROLÜ ---");
        Debug.Log($"1. Sonuç: {isSlotACorrect} (True olmalı)");
        Debug.Log($"2. Aranan ID: {Reqired_ID_FOR_A}");

        // Kontrol: Slot A'nın içindeki objenin ID'sini de görelim.
        if (slots[0].currentImage != null)
        {
            Item currentItem = slots[0].currentImage.GetComponent<Item>();
            if (currentItem != null)
            {
                Debug.Log($"3. Slottaki Gerçek ID: {currentItem.ID}");
            }
            else
            {
                Debug.Log($"3. HATA: Slottaki obje üzerinde 'Item.cs' scripti yok.");
            }
        }
        else
        {
            Debug.Log($"3. HATA: Slot A Boş (currentImage = null)");
        }
        Debug.Log(slots[0].name);
        // bool isSlotACorrect = CheckForCorrectItem(slots[0], Reqired_ID_FOR_A);
        bool isSlotBCorrect = CheckForCorrectItem(slots[1], Reqired_ID_FOR_B);
        bool isSlotCCorrect = CheckForCorrectItem(slots[2], Reqired_ID_FOR_C);
        bool isSlotDCorrect = CheckForCorrectItem(slots[3], Reqired_ID_FOR_D);
        bool isSlotECorrect = CheckForCorrectItem(slots[4], Reqired_ID_FOR_E);

        if (isSlotACorrect)//&& isSlotBCorrect && isSlotCCorrect && isSlotDCorrect && isSlotECorrect)
        {
            Destroy(puzzle);
            //kapı açma kodu buraya gelicek veya bir şey oldu kodu gelicek
            Debug.Log("Tebrikler");
        }
    }




}
