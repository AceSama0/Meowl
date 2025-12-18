using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

enum State
{
    Chase,
    Roam,
    Search,
    Dash,
    Flee,
    Die,
    Spawn,
}

public class EnemyBehave : MonoBehaviour
{
    CapsuleCollider2D colliderEnemy;
    float originalSpeed;
    [SerializeField] Player player;
    bool soundPlayed = false;
    public Vector2 lastSeen;
    Enemy enemy;
    bool settingSprite = true;

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

    Animator animator;

    void Awake()
    {
        colliderEnemy = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        enemy = GetComponent<Enemy>();
        wayPointMover = GetComponent<WayPointMover>();
        state = State.Roam;
        originalSpeed = wayPointMover.movementSpeed;

        visionMask = LayerMask.GetMask("Player", "Wall");
    }

    void Update()
    {
        // Ölüyken veya doğarken tüm mantığı durdur
        if (state == State.Die || state == State.Spawn) return;

        if (settingSprite) SetEnemySprite();

        visionCheckTimer += Time.deltaTime;
        if (visionCheckTimer >= VISION_CHECK_INTERVAL)
        {
            visionCheckTimer = 0f;
            distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
            canSeePlayer = CheckSeesPlayer();
        }

        // Oyuncuyu görmüyorsa görünmez yap (Mekaniğine göre)
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

    void SetEnemySprite()
    {
        if (canSeePlayer && player.transform.position.x > transform.position.x)
            spriteRenderer.flipX = false;
        else
            spriteRenderer.flipX = true;
    }

    private State DetermineState()
    {
        if (state == State.Die || state == State.Spawn) return state;

        // Ölüm: Başlangıç noktasındayken (0. waypoint) oyuncu ışık tutuyorsa
        if (state == State.Roam && wayPointMover.currentWayPointIndex == 0 && canSeePlayer && player.lightTime > 0)
        {
            return State.Die;
        }

        // Kaçma: Işık varsa
        if (distanceToPlayer < 10f && canSeePlayer && player.lightTime > 0)
        {
            return State.Flee;
        }

        // Atılma, Takip ve Devriye
        if (distanceToPlayer < 5f && canSeePlayer && enemy.canDash) return State.Dash;
        if (distanceToPlayer < 10f && canSeePlayer) return State.Chase;

        return State.Roam;
    }

    private void HandleStateExit(State exitingState)
    {
        if (exitingState == State.Flee)
        {
            settingSprite = true;
            wayPointMover.movementSpeed = originalSpeed;
        }
    }

    private void HandleStateEnter(State enteringState)
    {
        switch (enteringState)
        {
            case State.Roam:
                if (rb != null) { rb.linearVelocity = Vector2.zero; rb.bodyType = RigidbodyType2D.Kinematic; }
                break;

            case State.Chase:
            case State.Dash:
            case State.Flee:
                if (rb != null) { rb.bodyType = RigidbodyType2D.Dynamic; rb.WakeUp(); }
                StartCoroutine(FleeProcess());
                break;

            case State.Die:
                StartCoroutine(KillObject());
                break;

            case State.Spawn:
                StartCoroutine(SpawnProcess());
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
                wayPointMover.canMove = true;
                
                break;
        }
    }

    IEnumerator FleeProcess()
    {
        settingSprite = false;
        spriteRenderer.flipX = (player.transform.position.x > transform.position.x);
        wayPointMover.currentWayPointIndex = Mathf.Max(0, wayPointMover.currentWayPointIndex - 3);
        if (wayPointMover.isWaiting) wayPointMover.isWaiting = false;
        wayPointMover.movementSpeed = 20f;
        wayPointMover.ResumeMovement();
        yield return new WaitForSeconds(1.5f);
        wayPointMover.movementSpeed = originalSpeed;
        settingSprite = true;
    }

    IEnumerator KillObject()
    {
        if (gameObject.name == "Mother") animator.SetBool("Death", true);

        colliderEnemy.enabled = false;
        wayPointMover.canMove = false;

        yield return new WaitForSeconds(1f);

        spriteRenderer.enabled = false;

        // Öldükten sonra Spawn sürecine geç
        state = State.Spawn;
        HandleStateEnter(State.Spawn);
    }

    IEnumerator SpawnProcess()
    {
        yield return new WaitForSeconds(5f);

        if (spawnPoint != null) transform.position = spawnPoint.position;

        if (gameObject.name == "Mother") animator.SetBool("Death", false);
        spriteRenderer.enabled = true;
        colliderEnemy.enabled = true;
        wayPointMover.currentWayPointIndex = 0;

        state = State.Roam;
    }

    private bool CheckSeesPlayer()
    {
        Vector2 direction = (player.transform.position - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 15f, visionMask);
        return hit.collider != null && hit.collider.CompareTag("Player");
    }
}