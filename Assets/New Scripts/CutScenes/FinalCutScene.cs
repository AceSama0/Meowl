using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class FinalCutScene : MonoBehaviour
{
    [SerializeField] float speed = 2f;
    Animator animator;

    [SerializeField] Transform target;
    [SerializeField] GameObject Enemy, Dialogue1;

    void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(FinalStart());
        Debug.Log("Başladı");
    }
    void Update()
    {
        if (!Dialogue1.activeInHierarchy)
        {
            SoundEffectManager.Play("dialogue");
        }
    }
    IEnumerator FinalStart()
    {
        Debug.Log("Başladı");
        animator.SetBool("walk", true);
        yield return StartCoroutine(GettingCloser(gameObject, target));
        animator.SetBool("walk", false);
        Dialogue1.SetActive(true);
        
    }

    IEnumerator AfterDialogue()
    {

        SoundEffectManager.Play("deneme");
        Debug.Log("Yeap");
        yield return null;

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
