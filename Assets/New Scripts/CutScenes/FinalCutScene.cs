using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class FinalCutScene : MonoBehaviour
{
    [SerializeField] float speed = 2f;
    Animator animator;
    CinemachineCamera vCam;

    [SerializeField] Transform target;
    [SerializeField] GameObject Enemy, Dialogue1, Rotoscope1, UI, player;
    [SerializeField] AudioClip rotoscopeSounds;
    bool isDialogueActivated;
    private void Start()
    {
        vCam = FindAnyObjectByType<CinemachineCamera>();
        animator = GetComponent<Animator>();

    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(FinalStart());
        }
    }
    void Update()
    {
        if (isDialogueActivated && !Dialogue1.activeInHierarchy)
        {
            Rotoscope1.SetActive(true);
            MusicManager.PlayBackgroundMusic(false, rotoscopeSounds);
        }
    }
    IEnumerator FinalStart()
    {
        player.SetActive(false);
        vCam.Follow = transform;
        Destroy(UI);
        MusicManager.PauseBackgroundMusic();
        Debug.Log("Başladı");
        animator.SetBool("walk", true);
        yield return StartCoroutine(GettingCloser(gameObject, target));
        animator.SetBool("walk", false);
        Dialogue1.SetActive(true);
        // Dialogue1.SetActive(false);
        isDialogueActivated = true;
    }


    IEnumerator GettingCloser(GameObject gameObject, Transform transform)
    {
        while (Vector2.Distance(gameObject.transform.position, transform.transform.position) > 0.01f)
        {
            gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, transform.transform.position, speed * Time.deltaTime);
            yield return null;
        }
        gameObject.transform.position = transform.transform.position;

    }
}
