using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class FirstCutScene : MonoBehaviour
{
    [SerializeField] GameObject Player, Enemy, Armature, dialogue, UI, SecondCutScene, PlayerLight;
    [SerializeField] AudioClip audioClip, main;

    [SerializeField] Animator animator;
    [SerializeField] float speed = 2f;
    [SerializeField] Transform first, Second;
    bool dialogueStarted = false;
    void Start()
    {
        MusicManager.PlayBackgroundMusic(false, audioClip);
        Player.GetComponent<SpriteRenderer>().enabled = false;
        Enemy.SetActive(false);
        StartCoroutine(First());
    }

    void Update()
    {
        if (dialogueStarted && !dialogue.activeInHierarchy)
        {
            dialogueStarted = false;
            StartCoroutine(SecondC());
        }
    }

    IEnumerator First()
    {
        yield return null;
        animator.SetBool("walk", true);
        yield return StartCoroutine(GettingCloser(Armature, first));
        Debug.Log("Ulaştım");
        yield return null;
        animator.SetBool("walk", false);
        dialogue.SetActive(true);
        dialogueStarted = true;

    }

    IEnumerator SecondC()
    {
        yield return null;
        animator.SetBool("walk", true);
        yield return StartCoroutine(GettingCloser(Armature, Second));
        UI.SetActive(true);
        Enemy.SetActive(true);
        SoundEffectManager.Play("door");
        SecondCutScene.SetActive(true);
        Player.GetComponent<SpriteRenderer>().enabled = true;
        PlayerLight.SetActive(true);
        MusicManager.PlayBackgroundMusic(false, main);
        Destroy(gameObject);
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
