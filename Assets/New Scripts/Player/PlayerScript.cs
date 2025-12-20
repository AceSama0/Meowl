using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] GameObject GirlKill, FailedKill, MotherKill;
    bool idleing;
    private float footStepTimer;
    [SerializeField] float footStepDuration = 0.2f;
    public string animName = "isWalking";
    public bool canMove = true;
    public bool canRotate = true;
    float basicSpeed;
    private Coroutine activeCoroutine;
    public static Player instance { get; private set; }
    [Header("Lantern")]
    [SerializeField] Transform[] lightWayPoints = new Transform[3];
    [SerializeField] Transform lanternPosition;
    [SerializeField] int lightWPIndex = 3;

    [SerializeField] public float lightfloat = 0;
    [SerializeField] bool lightBool = true;
    [SerializeField] Light2D lantern;
    public float lightTime = 0;
    public float lightCount = 1;
    [Header("Puzzle")]
    [SerializeField] Vector2 movement;
    public float speed = 5f;
    [SerializeField] public int fuels = 0;

    [SerializeField] Rigidbody2D rb;

    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] public Animator animator;
    [Header("QTE")]
    public bool isQTEActive = false;
    private float qteStartTime;
    private float qteDuration = 1.0f;
    public bool success = false;
    [Header("Daughter")]
    [SerializeField] GameObject enemy;
    Enemy enemyMove;
    [SerializeField] WayPointMover wayPointMover;
    EnemyBehave enemyBehave;
    SpriteRenderer EspriteRenderer;
    Collider2D enemyCollider;
    DialogueStarter dialogueStarter;

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
        basicSpeed = speed;
    }
    void Update()
    {
        if (inDialogue())
        {
            return;
        }
        if (isQTEActive)
        {
            HandleQTEInput();
        }
        if (canMove)
        {
            speed = basicSpeed;
        }
        else
        {
            speed = 0;
        }

        playerMovement();
        LightBehave();
        LightTimer();

        bool isMoving = rb.linearVelocity.magnitude > 0;

        if (isMoving && canMove)
        {

            footStepTimer -= Time.deltaTime;

            if (footStepTimer <= 0)
            {
                FootSteps();

                footStepTimer = footStepDuration;
            }
        }


        if (rb.linearVelocity.magnitude == 0)
        {

        }

    }
    private void HandleQTEInput()
    {

        if (Time.time > qteStartTime + qteDuration)
        {

            isQTEActive = false;
            OnQTEFailed();
            return;
        }


        if (Input.GetMouseButtonDown(1))//&& lightCount > 0)
        {

            isQTEActive = false;
            OnQTESuccess();
        }
    }


    public void StartQTE(float duration)
    {
        if (isQTEActive) return;

        qteDuration = duration;
        isQTEActive = true;
        qteStartTime = Time.time;

    }


    private void OnQTESuccess()
    {
        wayPointMover.canMove = false;
        enemyMove.canMove = false;
        animator.SetBool("JumpedChild", false);
        wayPointMover.currentWayPointIndex = 8;
        enemy.transform.position = wayPointMover.wayPoints[7].position;
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        yield return new WaitForSeconds(10f);
        enemyCollider.enabled = true;
        wayPointMover.canMove = true;
        enemyBehave.enabled = true;
        EspriteRenderer.enabled = true;

    }

    private void OnQTEFailed()
    {
        success = false;
        FailedKill.SetActive(true);
        SoundEffectManager.Play("jumpScare");
        Debug.Log("Failed");
    }
    void LightBehave()
    {
        if (Input.GetMouseButtonDown(1) && lightCount > 0)
        {
            lightCount--;
            lightTime = 10f;
        }
    }
    void LightTimer()
    {
        // Dış ışığı (menzili) hep sabit tutuyoruz
        lantern.pointLightOuterRadius = lightfloat;

        if (lightTime > 0)
        {
            // Süreyi azalt
            lightTime -= Time.deltaTime;

            // Işığın iç halkasını (parlak kısmını) 3'ten 0'a doğru küçült
            // 10f senin fenerinin toplam yanma süresi
            float ratio = lightTime / 10f;
            lantern.pointLightInnerRadius = Mathf.Lerp(0, 3f, ratio);

            if (lightTime <= 0)
            {
                lightTime = 0;
                lantern.pointLightInnerRadius = 0;
                // Dış ışık kalsın ama iç ışık 0 olduğu için fener 'bitmiş' gibi görünür
            }
        }
        else
        {
            // Süre yoksa iç ışık hep 0 kalsın
            lantern.pointLightInnerRadius = 0;
        }
    }

    void playerMovement()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");
        Vector2 direction = new Vector2(inputX, inputY).normalized;
        rb.linearVelocity = direction * speed;

        if (inputX != 0 || inputY != 0)
        {
            animator.SetBool(animName, true);
            idleing = false;
        }
        else
        {
            animator.SetBool(animName, false);
            idleing = true;
        }

        if (canRotate)
        {
            // Yön sola: inputX < 0
            if (inputX < 0)
            {
                // Eğer sağa bakıyorsak (yani pozitif ölçekteyiz) sola çevir.
                if (transform.localScale.x > 0)
                {
                    transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
                }
            }

            else if (inputX > 0)
            {

                if (transform.localScale.x < 0)
                {
                    transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);
                }
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (collision.gameObject.name == "Daughter")
            {
                enemyBehave = collision.gameObject.GetComponent<EnemyBehave>();
                enemyBehave.enabled = false;
                enemyCollider = collision.gameObject.GetComponent<Collider2D>();
                enemyCollider.enabled = false;
                EspriteRenderer = collision.gameObject.GetComponent<SpriteRenderer>();
                EspriteRenderer.enabled = false;
                enemyMove = collision.gameObject.GetComponent<Enemy>();
                float grabNumber = UnityEngine.Random.Range(0, 3);
                if (grabNumber == 0)
                {
                    SoundEffectManager.Play("grab");
                    animator.SetBool("JumpedChild", true);
                    StartQTE(3);
                }
                else
                {
                    MusicManager.PauseBackgroundMusic();
                    GirlKill.SetActive(true);
                    SoundEffectManager.Play("jumpScare");
                }

            }
            if (collision.gameObject.name == "Mother")
            {
                SoundEffectManager.Play("MotherKills");
                MotherKill.SetActive(true);
            }
        }
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Dialogue"))
        {
            dialogueStarter = collision.gameObject.GetComponent<DialogueStarter>();
            if (Input.GetKeyDown(KeyCode.E))
            {

                collision.gameObject.GetComponent<DialogueStarter>().ActivateDialogue();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Candle"))
        {
            Destroy(collision.gameObject);
            Debug.Log("Fenere yakıt eklendi");
            lightCount++;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        dialogueStarter = null;
    }

    private bool inDialogue()
    {
        if (dialogueStarter != null)
        {
            animator.SetBool(animName, false);
            return dialogueStarter.DialogueActive();
        }
        else
        {
            return false;
        }
    }

    void FootSteps()
    {
        SoundEffectManager.Play("walking", true);
    }



    IEnumerator RightHandLight(Light2D lantern)
    {
        if (lightWPIndex < 0) { lightWPIndex = 0; }
        while (lightWPIndex <= lightWayPoints.Length - 1)
        {
            lantern.transform.position = Vector2.MoveTowards(lantern.transform.position, lightWayPoints[lightWPIndex].position, 2f * Time.deltaTime);
            if (Vector2.Distance(lantern.transform.position, lightWayPoints[lightWPIndex].position) < 0.01f)
            {
                lightWPIndex++;
            }
            yield return null;
        }
    }
    IEnumerator LeftHandLight(Light2D lantern)
    {
        if (lightWPIndex > 2) { lightWPIndex = 2; }
        while (lightWPIndex >= 0)
        {
            lantern.transform.position = Vector2.MoveTowards(lantern.transform.position, lightWayPoints[lightWPIndex].position, 2f * Time.deltaTime);
            if (Vector2.Distance(lantern.transform.position, lightWayPoints[lightWPIndex].position) < 0.01f)
            {
                lightWPIndex--;
            }
            yield return null;
        }

    }
    IEnumerator switchingLight()
    {
        if (idleing)
        {
            while (true)
            {
                if (activeCoroutine != null)
                {
                    StopCoroutine(activeCoroutine);
                }
                animator.SetBool("SwitchingLantern", true);
                activeCoroutine = StartCoroutine(RightHandLight(lantern));
                yield return new WaitForSeconds(5);
                animator.SetBool("SwitchingLantern", false);
                if (activeCoroutine != null)
                {
                    StopCoroutine(activeCoroutine);
                }
                activeCoroutine = StartCoroutine(LeftHandLight(lantern));
                animator.SetBool("SwitchingLantern", true);
                yield return new WaitForSeconds(5);
                animator.SetBool("SwitchingLantern", false);
            }
        }

    }
}
