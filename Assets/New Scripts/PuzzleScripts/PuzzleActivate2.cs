using System.Collections;
using UnityEngine;

public class PuzzleActivate2 : MonoBehaviour
{
    public GameObject PuzzleName;
    InventoryScript inventoryScript;
    PauseMenuUI pause;

    void Start()
    {
        pause = FindAnyObjectByType<PauseMenuUI>();
        PuzzleName.SetActive(false);
        inventoryScript = FindAnyObjectByType<InventoryScript>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (PuzzleName != null)
            {

                PuzzleName.SetActive(true);
            }
            inventoryScript.isAnotherScreenOpened = true;
        }
    }

    IEnumerator WaitSome()
    {
        yield return new WaitForSeconds(1);
        inventoryScript.isAnotherScreenOpened = false;
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape) && inventoryScript.isAnotherScreenOpened && !pause.isGamePause)
        {
            PuzzleName.SetActive(false);
            StartCoroutine(WaitSome());
        }
    }
}
