using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CineDeneme : MonoBehaviour
{
    [SerializeField] Transform target, enemyTarget, enemyEscape;
    [SerializeField] GameObject lantern, enemy, playerLantern, mouseIcon, lanternLight;
    Animator animator;
    Player player;
    private const string IS_WALKING_PARAM = "isWalking";
    [SerializeField] float speed = 20f;
    SpriteRenderer spriteRenderer;
    bool failedBool = false;
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

    public void StartCutScene()
    {
        StartCoroutine(RunCutsceneMovement());
    }


    IEnumerator RunCutsceneMovement()
    {
        player.enabled = false;
        Destroy(lanternLight);
        MusicManager.PauseBackgroundMusic();
        lantern.SetActive(true);
        playerLantern.SetActive(false);
        player.GetComponent<SpriteRenderer>().enabled = false;

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
            player.lightTime = 40;
        }
        yield return new WaitForSeconds(1f);
        spriteRenderer.flipX = true;
        StartCoroutine(EnemyGettingCloser());
        //animation
    }

    IEnumerator Failed()
    {
        while (Vector2.Distance(enemy.transform.position, target.position) > 0.1f)
        {
            enemy.transform.position = Vector2.MoveTowards(
                enemy.transform.position,
                transform.position,
                5f * Time.deltaTime
            );

            yield return null;
        }
        SceneManager.LoadScene("KızÖlüm");
        Destroy(enemy);
    }
    IEnumerator Success()
    {
        Destroy(mouseIcon);
        SpriteRenderer enemySprite = enemy.GetComponent<SpriteRenderer>();
        enemySprite.flipX = true;
        player.enabled = true;
        player.transform.position = transform.position;
        playerLantern.SetActive(true);
        Destroy(lantern);
        while (Vector2.Distance(enemy.transform.position, enemyEscape.position) > 0.1f)
        {
            enemy.transform.position = Vector2.MoveTowards(
                enemy.transform.position,
                enemyEscape.position,
                speed * Time.deltaTime
            );

            yield return null;
        }
        MusicManager.PlayBackgroundMusic(false);
        player.canMove = true;
        SoundEffectManager.Play("deneme");
        Destroy(enemy);
        player.GetComponent<SpriteRenderer>().enabled = true;
        player.animator.SetBool("LanternIdle" , true);
        player.animName = "LanternWalking";
        Destroy(gameObject);

    }

    

    IEnumerator EnemyGettingCloser()
    {
        SoundEffectManager.Play("chaseMother");
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

        yield return StartCoroutine(WaitForPlayerAction(2f));

        if (!failedBool)
        {
            StartCoroutine(Success());
        }
        else
        {
            StartCoroutine(Failed());
        }
    }

    IEnumerator WaitForPlayerAction(float duration)
    {

        mouseIcon.SetActive(true);
        float startTime = Time.time;
        failedBool = true;

        while (Time.time < startTime + duration)
        {
            if (Input.GetMouseButtonDown(1))
            {
                failedBool = false;

                lantern.SetActive(false);

                yield break;
            }

            yield return null;
        }
        Destroy(mouseIcon);
    }


}