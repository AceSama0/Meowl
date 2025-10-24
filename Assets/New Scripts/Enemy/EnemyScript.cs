
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyScript : MonoBehaviour
{
    public static EnemyScript instance { get; private set; }
    public float speed = 6f;
    [SerializeField] float orijinalSpeed;

    [SerializeField] float overlapRadius = 10f;
    [SerializeField] float distance;

    [SerializeField] bool isStopped;
    // [SerializeField] bool isFleeing = false;

    [SerializeField] Rigidbody2D rb;

    [SerializeField] Player target;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Animator animator;


    // Düşmana kaçma kodu ekle
    //table kodunu düzenle
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        orijinalSpeed = speed;
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        Table();
        distance = Vector2.Distance(target.transform.position, transform.position);


        if (target.lightTime >= 0f && distance < 5f && !isStopped)
        {
            StopEnemy();
            CancelInvoke("ResumMove");
            Invoke("ResumeMove", 5f);
        }
        LookAtPlayer();
    }
    void LookAtPlayer()
    {
        if (Physics2D.OverlapCircle(transform.position, overlapRadius, LayerMask.GetMask("Player")))
        {
            if (target.transform.position.x > transform.position.x)
            {
                spriteRenderer.flipX = false;
            }
            else if (target.transform.position.x < transform.position.x)
            {
                spriteRenderer.flipX = true;
            }
        }
    }

    void Table()
    {
        Collider2D hitTable = Physics2D.OverlapCircle(transform.position, overlapRadius, LayerMask.GetMask("Table"));


        if (hitTable != null)
        {
            Transform tableTransform = hitTable.transform;
            RaycastHit2D distanceT = Physics2D.Raycast(transform.position, tableTransform.position, LayerMask.GetMask("Table"));

            // Vector2 flee = (transform.position - distance * speed * Time.deltaTime).normalized;

            if (distanceT && !isStopped)
            {
                // transform.position = Vector2.MoveTowards(transform.position, distance, speed * Time.deltaTime);
                CancelInvoke("ResumeMove");
                Invoke("ResumeMove", 5f);
            }
        }
        else
        {

        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene("KızÖlüm");

        }
    }
    void FixedUpdate()
    {
        if (!isStopped)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
        }
    }

    void StopEnemy()
    {
        speed = 0f;
        isStopped = true;
    }
    void ResumeMove()
    {
        speed = orijinalSpeed;
        isStopped = false;
    }
}
