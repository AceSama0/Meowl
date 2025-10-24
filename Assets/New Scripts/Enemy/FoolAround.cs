using UnityEngine;
using System.Collections;

public class FoolAround : MonoBehaviour
{
    [SerializeField] Transform[] wayPoints;
    Enemy enemy;
    [SerializeField] float speed = 2f;
    [SerializeField] float orijinalSpeed;
    [SerializeField] float dedectRadius;
    private Vector3 StartingPosition;

    private int wayPointIndex = 0;
    Animator animator;
    SpriteRenderer spriteRenderer;
    int randomInt;
    void Awake()
    {
        enemy = GetComponent<Enemy>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }
    void Start()
    {
        StartingPosition = transform.position;
        orijinalSpeed = speed;
        animator.SetBool("isWalking", true);
        transform.position = wayPoints[wayPointIndex].position;
    }

    void Update()
    {
        DetectPlayer();
        StartCoroutine(EnemyFoolingAround());
    }

    void DetectPlayer()
    {
        if (Physics2D.OverlapCircle(transform.position, dedectRadius, LayerMask.GetMask("Player")))
        {
            speed = orijinalSpeed;
            // enemy.enabled = true;
        }
    }
    // Vector3 GetRandomDirection()
    // {
    //     return new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized;
    // }
    IEnumerator EnemyFoolingAround()
    {
        if (wayPointIndex <= wayPoints.Length - 1)
        {
            flipEnemy();

            transform.position = Vector2.MoveTowards(transform.position, wayPoints[wayPointIndex].position, speed * Time.deltaTime);

            if (transform.position == wayPoints[wayPointIndex].position)
            {
                randomInt = Random.Range(0, 5);
                if (randomInt == 0)
                {
                    speed = 0;
                    animator.SetBool("isWalking", false);
                    yield return new WaitForSeconds(3);
                    animator.SetBool("isWalking", true);
                    speed = orijinalSpeed;
                    wayPointIndex--;
                }
                wayPointIndex++;
            }
        }
        else
        {
            wayPointIndex = 0;
        }
    }

    void flipEnemy()
    {
        if (transform.position.x > wayPoints[wayPointIndex].position.x)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
    }
    public static Vector2 Lerp(Vector2 a, Vector2 b, float t)
    {
        return a + b;
    }
}
