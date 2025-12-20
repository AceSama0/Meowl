using UnityEngine;
using UnityEngine.InputSystem;

public class MusicBoxSucessed : MonoBehaviour
{
    [SerializeField] AudioClip musicBoxMusic;
    [SerializeField] AudioClip main;
    [SerializeField] MapTransition mapTransition;
    [SerializeField] GameObject key;
    void Start()
    {
        MusicManager.PlayBackgroundMusic(false, musicBoxMusic);
        mapTransition.doorCanOpen = true;
    }
    void Update()
    {
        if (gameObject.activeInHierarchy && Input.GetMouseButtonDown(0))
        {
            MusicManager.PlayBackgroundMusic(false, main);
            key.SetActive(true);
            Destroy(gameObject);
        }
    }
}
