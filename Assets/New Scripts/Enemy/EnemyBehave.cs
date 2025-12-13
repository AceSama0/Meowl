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
        // 1. Durum Geçiş Mantığı (Öncelik Flee > Dash > Chase > Roam)
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

        // 2. Durum Değişim Kontrolü
        if (newState != state)
        {
            HandleStateExit(state);
            state = newState;
            HandleStateEnter(state);
        }

        // 3. Durum Eylemleri
        HandleStateAction(state);
    }
    
    // YENİ METOT: Durumdan çıkarken temizlik yapar
    private void HandleStateExit(State exitingState)
    {
        // (Şimdilik boş bırakılabilir, ancak ileride temizlik için kullanılabilir)
    }
    
    // YENİ METOT: Duruma girerken başlatma ve Fizik Yetkisini Yönetir
    private void HandleStateEnter(State enteringState)
    {
        // 🛑 KRİTİK FİZİK YÖNETİMİ: Titremeyi çözmek için
        if (enteringState == State.Roam)
        {
            // Roam'a girerken Rigidbody'yi durdur ve transform.position'a yetki ver
            if (rb != null)
            {
                rb.velocity = Vector2.zero;
                rb.isKinematic = true; 
            }
            wayPointMover.canMove = true;
            wayPointMover.ResumeMovement(); 
        }
        else if (enteringState == State.Chase || enteringState == State.Dash || enteringState == State.Flee)
        {
            // Chase/Dash/Flee'ye girerken Rigidbody'yi aktifleştir
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.WakeUp();
            }
            wayPointMover.canMove = false;
        }
        
        // Diğer Durum Giriş Ayarları
        switch (enteringState)
        {
            case State.Flee:
                hasFled = false; 
                break;
        }
    }
    
    // YENİ METOT: Her Frame'de çalışacak eylemler
    private void HandleStateAction(State currentState)
    {
        // Tüm hareket bayraklarını resetle (yalnızca o anki duruma yetki ver)
        enemy.canMove = false;
        wayPointMover.canMove = false;

        switch (currentState)
        {
            case State.Roam:
                enemy.canDash = true;
                soundPlayed = false;
                hasFled = false;
                wayPointMover.canMove = true; // WayPointMover'a yetki ver
                break;
                
            case State.Chase:
                if (gameObject.name == "Mother" && !soundPlayed) SoundEffectManager.Play("chaseMother");
                soundPlayed = true;
                enemy.canMove = true; // Enemy/Chase scriptine yetki ver
                break;
                
            case State.Dash:
                enemy.canMove = true; // Dash, Enemy scriptinde yönetilir
                break;

            case State.Flee:
                if (!hasFled)
                {
                    wayPointMover.canMove = true;
                    
                    // Geriye doğru WayPoint'e sıçra
                    wayPointMover.currentWayPointIndex -= 3;
                    if (wayPointMover.currentWayPointIndex < 0)
                    {
                        wayPointMover.currentWayPointIndex = 0;
                    }
                    wayPointMover.ResumeMovement(); 

                    hasFled = true;
                }
                wayPointMover.canMove = true; // Kaçış hareketini WayPointMover'a ver
                break;

            case State.Die:
                // Ölüm mantığı buraya gelir
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
        
        // Raycast mesafesi
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 5f, mask); 
        
        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Player"))
            {
                return true;
            }
            else
            {
                return false; // Duvar veya başka bir engel var
            }
        }
        else
        {
            return false;
        }
    }
}