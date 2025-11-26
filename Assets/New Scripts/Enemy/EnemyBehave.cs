using UnityEngine;
enum State
{
    Chase,
    Roam,
}
public class EnemyBehave : MonoBehaviour
{
    Player player;
    Enemy enemy;
    WayPointMover wayPointMover;
    State state;
    void Awake()
    {
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
        if(ChaseDistance() < 10f && SeesPlayer())
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
        LayerMask mask = LayerMask.GetMask("Player" , "Wall");
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

}
