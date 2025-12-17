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
}

public class EnemyBehave : MonoBehaviour
{
    float originalSpeed;
    [SerializeField] Player player;
    bool soundPlayed = false;
    public Vector2 lastSeen;
    Enemy enemy;
    bool hasFled = false;
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

    [Header("QTE")]
    bool QTEStarter = false;
    [SerializeField] GameObject QTE;
    Animator animator;

    void Awake()
    {
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
        // Sprite yönünü ayarla (Eğer özel bir durum yoksa)
        if (settingSprite) SetEnemySprite();

        // Görüş kontrolü (Performans için timer ile)
        visionCheckTimer += Time.deltaTime;
        if (visionCheckTimer >= VISION_CHECK_INTERVAL)
        {
            visionCheckTimer = 0f;
            distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
            canSeePlayer = CheckSeesPlayer();
        }

        // Oyuncuyu görmüyorsa gizle (İsteğe bağlı bir mekanik gibi duruyor)
        spriteRenderer.enabled = canSeePlayer;

        // State Değişimi
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
        // 1. Ölüm kontrolü (En öncelikli)
        if (state == State.Roam && wayPointMover.currentWayPointIndex == 0 && wayPointMover.hasStartedPath) 
        {
            return State.Die;
        }

        // 2. Kaçma (Işık varsa)
        if (distanceToPlayer < 10f && canSeePlayer && player.lightTime > 0)
        {
            return State.Flee;
        }

        // 3. Atılma (Yakınsa)
        if (distanceToPlayer < 5f && canSeePlayer && enemy.canDash)
        {
            return State.Dash;
        }

        // 4. Takip
        if (distanceToPlayer < 10f && canSeePlayer)
        {
            lastSeen = player.transform.position;
            return State.Chase;
        }

        // 5. Normal devriye
        return State.Roam;
    }

    private void HandleStateExit(State exitingState)
    {
        if (exitingState == State.Flee)
        {
            settingSprite = true; // Kaçma bitince sprite kontrolünü geri ver
            wayPointMover.movementSpeed = originalSpeed;
        }
    }

    private void HandleStateEnter(State enteringState)
    {
        // Fizik ayarları
        if (enteringState == State.Roam)
        {
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.bodyType = RigidbodyType2D.Kinematic;
            }
        }
        else if (enteringState == State.Chase || enteringState == State.Dash || enteringState == State.Flee)
        {
            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
                rb.WakeUp();
            }
        }

        // State'e giriş anında bir kez yapılacak işlemler
        switch (enteringState)
        {
            case State.Flee:
                StartCoroutine(FleeProcess());
                break;
            case State.Die:
                if (gameObject.name == "Mother") animator.SetBool("Death", true);
                StartCoroutine(KillObject());
                break;
        }
    }

    private void HandleStateAction(State currentState)
    {
        // Varsayılan olarak hareketleri kapat, state içinde gerekirse aç
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
                wayPointMover.canMove = true; // Kaçarken wayPointMover hareket eder
                break;
        }
    }

    IEnumerator FleeProcess()
    {
        // Bir kez çalışacak kaçış mekaniği
        settingSprite = false;
        
        // Oyuncunun tersine bak
        spriteRenderer.flipX = (player.transform.position.x > transform.position.x);

        // Geri gitme mantığı
        wayPointMover.currentWayPointIndex = Mathf.Max(0, wayPointMover.currentWayPointIndex - 3);
        if (wayPointMover.isWaiting) wayPointMover.isWaiting = false;
        
        wayPointMover.movementSpeed = 20f; // Hızlıca uzaklaş
        wayPointMover.ResumeMovement();

        yield return new WaitForSeconds(1.5f); // 1.5 saniye boyunca kaç

        wayPointMover.movementSpeed = originalSpeed;
        settingSprite = true;
    }

    IEnumerator KillObject()
    {
        // Animasyonun bitmesi için 1 saniye bekle ve sonra objeyi yok et/kapat
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }

    private bool CheckSeesPlayer()
    {
        Vector2 direction = (player.transform.position - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 15f, visionMask);

        return hit.collider != null && hit.collider.CompareTag("Player");
    }
}