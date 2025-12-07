using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
enum State
{
    Chase,
    Roam,
    Search,
    Dash,
    Flee,
    Die,
}
public class EnemyBehave : MonoBehaviour
{
    Player player;
    public Vector2 lastSeen;
    Enemy enemy;
    bool hasFled = false;

    [SerializeField] Transform spawnPoint;
    [SerializeField] float waitingTime;
    WayPointMover wayPointMover;
    State state;
    Rigidbody2D rb;



    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = FindAnyObjectByType<Player>();
        enemy = GetComponent<Enemy>();
        wayPointMover = GetComponent<WayPointMover>();
        state = State.Roam;
    }

    void Update()
    {
        if (ChaseDistance() < 10f&& SeesPlayer() && player.lightTime > 0)
        {
            state = State.Flee;
        }
        else if (ChaseDistance() < 5f && SeesPlayer() && enemy.canDash)
        {
            state = State.Dash;
        }

        else if (ChaseDistance() < 10f && SeesPlayer())
        {
            state = State.Chase;
            lastSeen = player.transform.position;
        }

        else
        {
            state = State.Roam;
        }

        switch (state)
        {
            case State.Roam:
                hasFled = false;
                enemy.enabled = false;
                wayPointMover.enabled = true;
                enemy.canDash = true;
                break;
            case State.Chase:
                enemy.enabled = true;
                wayPointMover.enabled = false;
                break;
            case State.Dash:
                break;

            case State.Flee:
                if (!hasFled)
                {
                    enemy.enabled = false;
                    wayPointMover.currentWayPointIndex -= 3; 
                    if (wayPointMover.currentWayPointIndex < 0)
                    {
                        wayPointMover.currentWayPointIndex = 0;
                    }
                    wayPointMover.enabled = true;
                    hasFled = true;
                }
                break;

            case State.Die:
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


    IEnumerator Waiting()
    {
        float oldSpeed = enemy.movementSpeed;
        enemy.movementSpeed = 0f;
        yield return new WaitForSeconds(waitingTime);
        enemy.movementSpeed = oldSpeed;
    }

}
