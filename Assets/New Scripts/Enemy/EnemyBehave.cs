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
    bool soundPlayed = false;
    public Vector2 lastSeen;
    Enemy enemy;
    bool hasFled = false;

    [SerializeField] Transform spawnPoint;
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
        State newState;
        
        if (ChaseDistance() < 10f && SeesPlayer() && player.lightTime > 0)
        {
            newState = State.Flee;
        }
        else if (ChaseDistance() < 5f && SeesPlayer() && enemy.canDash)
        {
            newState = State.Dash;
        }
        else if (ChaseDistance() < 10f && SeesPlayer())
        {
            newState = State.Chase;
            lastSeen = player.transform.position;
        }
        else
        {
            newState = State.Roam;
        }

        if (newState != state)
        {
            HandleStateExit(state);
            state = newState;
            HandleStateEnter(state);
        }

        HandleStateAction(state);
    }
    
    private void HandleStateExit(State exitingState)
    {
    }
    
    private void HandleStateEnter(State enteringState)
    {
        if (enteringState == State.Roam)
        {
            
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.bodyType = RigidbodyType2D.Kinematic;
            }
            wayPointMover.canMove = true;
            wayPointMover.ResumeMovement(); 
        }
        else if (enteringState == State.Chase || enteringState == State.Dash || enteringState == State.Flee)
        {
            
            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
                rb.WakeUp();
            }
            wayPointMover.canMove = false;
        }
        
        
        switch (enteringState)
        {
            case State.Flee:
                hasFled = false; 
                break;
        }
    }
    
    
    private void HandleStateAction(State currentState)
    {
        
        enemy.canMove = false;
        wayPointMover.canMove = false;

        switch (currentState)
        {
            case State.Roam:
                enemy.canDash = true;
                soundPlayed = false;
                hasFled = false;
                wayPointMover.canMove = true; 
                break;
                
            case State.Chase:
                if (gameObject.name == "Mother" && !soundPlayed) SoundEffectManager.Play("chaseMother");
                if (gameObject.name == "Daughter" && !soundPlayed) SoundEffectManager.Play("success");
                soundPlayed = true;
                enemy.canMove = true; 
                break;
                
            case State.Dash:
                enemy.canMove = true; 
                break;

            case State.Flee:
                if (!hasFled)
                {
                    wayPointMover.canMove = true;
                    
                    
                    wayPointMover.currentWayPointIndex -= 3;
                    if (wayPointMover.currentWayPointIndex < 0)
                    {
                        wayPointMover.currentWayPointIndex = 0;
                    }
                    wayPointMover.ResumeMovement(); 

                    hasFled = true;
                }
                wayPointMover.canMove = true; 
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
}