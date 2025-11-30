using System.Collections;
using UnityEngine;
enum State
{
    Chase,
    Roam,
    Flee,

}
public class EnemyBehave : MonoBehaviour
{
    Player player;
    Enemy enemy;
    WayPointMover wayPointMover;
    State state;
    Rigidbody2D rb;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = FindAnyObjectByType<Player>();
        state = State.Roam;
    }
    void Start()
    {
        enemy = GetComponent<Enemy>();
        wayPointMover = GetComponent<WayPointMover>();
    }
    void Update()
    {
        if (ChaseDistance() < 10f && SeesPlayer())
        {
            state = State.Chase;
        }
        else
        {
            state = State.Roam;
        }
        switch (state)
        {
            case State.Roam:
                enemy.enabled = false;
                wayPointMover.enabled = true;
                break;
            case State.Chase:
                enemy.enabled = true;
                wayPointMover.enabled = false;
                break;
        }
    }
    
    float ChaseDistance()
    {
        return Vector2.Distance(transform.position, player.transform.position);
    }

    public bool SeesPlayer()
    {
        LayerMask mask = LayerMask.GetMask("Player", "Wall");
        Vector2 direction = (player.transform.position - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 5f, mask);
        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Player"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    IEnumerator QuickTimeEvent()
    {

        Time.timeScale = 0.8f;
        enemy.movementSpeed = 5f;
        enemy.enabled = true;
        float qteDureation = 3f;
        bool success = false;
        while (qteDureation > 0)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                success = true;
                break;
            }

            yield return null;

        }

        Time.timeScale = 1f;

        if (success)
        {
            enemy.enabled = false;
            Debug.Log("Başarılı");
            yield return new WaitForSeconds(5f);
            state = State.Roam;
        }
    }

}
