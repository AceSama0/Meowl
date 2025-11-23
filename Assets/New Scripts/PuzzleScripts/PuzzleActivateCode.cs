using UnityEngine;

public class PuzzleActivateCode : MonoBehaviour
{
    public GameObject PuzzleName;

    void Start()
    {
        PuzzleName.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PuzzleName.SetActive(true);
            Pause();
        }
    }

    public void Resume()
    {
        Time.timeScale = 1f;
    }
    void Pause()
    {
        Time.timeScale = 0f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PuzzleName.SetActive(false);
            Resume();
        }
    }
}
