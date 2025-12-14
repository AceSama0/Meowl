using System.Collections;
using UnityEngine;

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
    float originalSpeed;
    Player player;
    bool soundPlayed = false;
    public Vector2 lastSeen;
    Enemy enemy;
    bool hasFled = false;

    [SerializeField] Transform spawnPoint;
    WayPointMover wayPointMover;
    State state;
    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;
    private LayerMask visionMask;
    private bool canSeePlayer;
    private float distanceToPlayer;
    private float visionCheckTimer;
    private const float VISION_CHECK_INTERVAL = 0.1f;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        player = FindAnyObjectByType<Player>();
        enemy = GetComponent<Enemy>();
        wayPointMover = GetComponent<WayPointMover>();
        state = State.Roam;
        originalSpeed = wayPointMover.movementSpeed;

        
        visionMask = LayerMask.GetMask("Player", "Wall");
    }

    void Update()
    {
        SetEnemyLook();
        visionCheckTimer += Time.deltaTime;
        if (visionCheckTimer >= VISION_CHECK_INTERVAL)
        {
            visionCheckTimer = 0f;
            distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
            canSeePlayer = CheckSeesPlayer();
        }

        spriteRenderer.enabled = canSeePlayer;

        
        State newState = DetermineState();

        if (newState != state)
        {
            HandleStateExit(state);
            state = newState;
            HandleStateEnter(state);
        }

        HandleStateAction(state);
    }

    void SetEnemyLook()
    {
        if(canSeePlayer && player.transform.position.x > transform.position.x)
        {
            spriteRenderer.flipX = false;
        }
        else
        {
            spriteRenderer.flipX = true;
        }
    }

    
    private State DetermineState()
    {
        if (distanceToPlayer < 10f && canSeePlayer && player.lightTime > 0)
        {
            return State.Flee;
        }
        else if (distanceToPlayer < 5f && canSeePlayer && enemy.canDash)
        {
            return State.Dash;
        }
        else if (distanceToPlayer < 10f && canSeePlayer)
        {
            lastSeen = player.transform.position;
            return State.Chase;
        }
        else
        {
            return State.Roam;
        }
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
                wayPointMover.movementSpeed = originalSpeed;
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
                    wayPointMover.movementSpeed = 10f;
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
    private bool CheckSeesPlayer()
    {
        Vector2 direction = (player.transform.position - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 15f, visionMask);

        return hit.collider != null && hit.collider.CompareTag("Player");
    }
}