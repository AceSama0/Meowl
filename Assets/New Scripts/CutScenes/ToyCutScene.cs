using System.Collections;
using UnityEngine;

public class ToyCutScene : MonoBehaviour
{
    [SerializeField] GameObject player, enemies;
    Player playerSC;
    [SerializeField] MapTransition mapTransition;
    [SerializeField] GameObject enemy, toy, NextCutScene;
    [SerializeField] Transform playerBack, enemyT, enemyRun;
    [SerializeField] float speed = 2f;
    [SerializeField] SpriteRenderer eSpriteR;
    Animator animator;
    [SerializeField] Animator eAnimator;

    void Start()
    {
        playerSC = player.GetComponent<Player>();
        playerSC.canMove = false;
        animator = GetComponent<Animator>();
        StartCoroutine(StartCutScene());
    }
    // Beklemeleri ayarla 
    IEnumerator StartCutScene()
    {
        
        player.SetActive(false);
        SoundEffectManager.Play("door");
        yield return new WaitForSeconds(1);
        yield return StartCoroutine(GettingCloser(enemy, enemyT));
        Debug.Log("Tamamlandı 1");
        toy.SetActive(true);
        animator.SetBool("Walk", true);
        speed /= 2;
        yield return StartCoroutine(GettingCloser(gameObject, playerBack));
        animator.SetBool("Walk", false);
        Debug.Log("Tamamlandı 2");
        yield return null;
        speed *= 2;
        yield return StartCoroutine(GettingCloser(enemy, toy.transform));
        Destroy(toy);
        Debug.Log("Tamamlandı 3");
        eSpriteR.flipX = false;
        yield return StartCoroutine(GettingCloser(enemy, enemyRun));
        Debug.Log("Tamamlandı 4");
        SoundEffectManager.Play("door");
        enemy.SetActive(false);
        player.SetActive(true);
        playerSC.canMove = true;
        player.transform.position = transform.position;
        playerSC.animator.SetBool("isWalking", false);
        yield return null;
        playerSC.animator.CrossFade("LanternIdle", 0.1f);
        yield return null;
        playerSC.animName = "LanternWalking";
        Destroy(enemies);
        NextCutScene.SetActive(true);
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
