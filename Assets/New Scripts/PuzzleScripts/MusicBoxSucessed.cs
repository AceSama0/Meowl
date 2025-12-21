using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class MusicBoxSucessed : MonoBehaviour
{
    [SerializeField] AudioClip musicBoxMusic;
    [SerializeField] AudioClip main;
    [SerializeField] MapTransition mapTransition;
    [SerializeField] GameObject musicBox, daughter, dialogue;
    void Start()
    {
        MusicManager.PlayBackgroundMusic(false, musicBoxMusic);
        mapTransition.doorCanOpen = true;
        Destroy(musicBox);
    }
    void Update()
    {
        if (gameObject.activeInHierarchy && Input.GetMouseButtonDown(0))
        {
            MusicManager.PauseBackgroundMusic();
            daughter.SetActive(true);
            StartCoroutine(SetDialogueActive());
        }
    }

    IEnumerator SetDialogueActive()
    {
        yield return new WaitForSeconds(3f);
        MusicManager.PlayBackgroundMusic(false, main);
        dialogue.SetActive(true);
        Destroy(gameObject);
    }
}
