using UnityEngine;
using UnityEngine.SceneManagement;

public class PuzzleController : MonoBehaviour
{
    [SerializeField] GameObject puzzle;
    [SerializeField] private SlotScripts slotA , slotB, slotC, slotD, slotE;

    private const int Reqired_ID_FOR_A = 1;
    private const int Reqired_ID_FOR_B = 2;
    private const int Reqired_ID_FOR_C = 3;
    private const int Reqired_ID_FOR_D = 4;
    private const int Reqired_ID_FOR_E = 5;


    int mistakeCount = 3;

    private PuzzleSetSlots setSlots;
    public void CheckPuzzleStatus()
    {
        if(setSlots == null)
        {
            setSlots = FindAnyObjectByType<PuzzleSetSlots>();
        }
        
        bool isSlotACorrect = setSlots.CheckForCorrectItem(slotA, Reqired_ID_FOR_A);
        bool isSlotBCorrect = setSlots.CheckForCorrectItem(slotB, Reqired_ID_FOR_B);
        bool isSlotCCorrect = setSlots.CheckForCorrectItem(slotC, Reqired_ID_FOR_C);
        bool isSlotDCorrect = setSlots.CheckForCorrectItem(slotD, Reqired_ID_FOR_D);
        bool isSlotECorrect = setSlots.CheckForCorrectItem(slotE, Reqired_ID_FOR_E);

        if (isSlotACorrect && isSlotBCorrect && isSlotCCorrect && isSlotDCorrect && isSlotECorrect)
        {
            Destroy(puzzle);
            //kapı açma kodu buraya gelicek veya bir şey oldu kodu gelicek
            Debug.Log("Tebrikler");
        }
        else if (isSlotACorrect)
        {
            Debug.Log("Yerine oturdu");
        }
        else if (isSlotBCorrect)
        {
            Debug.Log("Yerine oturdu");
        }
        else if (isSlotCCorrect)
        {
            Debug.Log("Yerine oturdu");
        }
        else if (isSlotDCorrect)
        {
            Debug.Log("Yerine oturdu");
        }
        else if (isSlotECorrect)
        {
            Debug.Log("Yerine oturdu");
        }
        else if (!isSlotBCorrect && !isSlotACorrect )
        {
            Debug.Log("Yanlış");
            mistakeCount --;
            if(mistakeCount == 0)
            {
                SceneManager.LoadScene("KızÖlüm");  
            }
        }
    }
}
