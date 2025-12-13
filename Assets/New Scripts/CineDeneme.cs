using System.Collections;
using UnityEngine;

public class CineDeneme : MonoBehaviour
{
    [SerializeField] Transform target, enemyTarget, enemyEscape;
    [SerializeField] GameObject lantern, enemy, playerLantern;
    Animator animator;
    Player player;
    private const string IS_WALKING_PARAM = "isWalking";
    [SerializeField] float speed = 20f;
    SpriteRenderer spriteRenderer;
    void Awake()
    {
        player = FindAnyObjectByType<Player>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        if (animator != null)
        {
            animator.SetBool(IS_WALKING_PARAM, true);
        }
        StartCoroutine(RunCutsceneMovement());
    }


    IEnumerator RunCutsceneMovement()
    {

        MusicManager.PauseBackgroundMusic();
        lantern.SetActive(true);

        while (Vector2.Distance(transform.position, target.position) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                target.position,
                speed * Time.deltaTime
            );

            yield return null;
        }


        transform.position = target.position;

        SoundEffectManager.Play("walking");

        if (animator != null)
        {
            animator.SetBool(IS_WALKING_PARAM, false);
            animator.SetBool("RaiseLanternLeft", true);
            lantern.SetActive(false);
            playerLantern.SetActive(true);
            player.lightTime = 40;

        }
        StartCoroutine(EnemyGettingCloser());

        yield return new WaitForSeconds(2f);
        spriteRenderer.flipX = true;
        yield return new WaitForSeconds(2f);
        MusicManager.PlayBackgroundMusic(false);
        player.canMove = true;
        player.GetComponent<SpriteRenderer>().enabled = true;
        player.transform.position = transform.position;

        Destroy(gameObject);
    }

    IEnumerator EnemyGettingCloser()
    {
        enemy.SetActive(true);
        while (Vector2.Distance(enemy.transform.position, enemyTarget.position) > 0.1f)
        {
            enemy.transform.position = Vector2.MoveTowards(
                enemy.transform.position,
                enemyTarget.position,
                speed * Time.deltaTime
            );

            yield return null;
        }
        yield return new WaitForSeconds(1);
        while (Vector2.Distance(enemy.transform.position, enemyEscape.position) > 0.1f)
        {
            enemy.transform.position = Vector2.MoveTowards(
                enemy.transform.position,
                enemyEscape.position,
                speed * Time.deltaTime
            );

            yield return null;
        }
        SoundEffectManager.Play("deneme");
        Destroy(enemy);
    }
}