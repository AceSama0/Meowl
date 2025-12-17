using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    bool idleing;
    private float footStepTimer;
    [SerializeField] float footStepDuration = 0.2f;
    bool isPlayingFootSteps;
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

    [SerializeField] float lightfloat = 5f;
    [SerializeField] bool lightBool = true;
    [SerializeField] bool switchHand;
    [SerializeField] Light2D lantern;
    public float lightTime = 0;
    [Header("Puzzle")]
    [SerializeField] Vector2 movement;
    public float speed = 5f;
    [SerializeField] public int fuels = 0;

    [SerializeField] Rigidbody2D rb;

    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField]public Animator animator;
    [Header("QTE")]
    public bool isQTEActive = false;
    private float qteStartTime;
    private float qteDuration = 1.0f;
    public bool success = false;

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

    void Start()
    {
        lantern.transform.position = lightWayPoints[0].position;
        lantern.pointLightInnerRadius = 5f;
    }
    void Update()
    {
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
        LightOnOff();

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


        if (Input.GetMouseButtonDown(1))
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
        success = true;
    }

    private void OnQTEFailed()
    {
        success = false;
    }

    void LightOnOff()
    {
        if (lightTime > 0 || lantern.pointLightInnerRadius > 0)
        {
            lightTime -= 1f * Time.deltaTime;
        }
        if (lightTime >= 20f)
        {
            lantern.pointLightInnerRadius = 2f;
        }
        else if (lightTime < 0f) // alan bitmesi smooth olmalı
        {
            lantern.pointLightInnerRadius = 0f;
        }
        else if (lightTime < 20f)
        {
            LightBreath();
        }
    }
    void LightBreath()
    {
        if (lantern.pointLightInnerRadius >= 3f)
        {
            lightBool = true;
        }
        else if (lantern.pointLightInnerRadius < 2f)
        {
            lightBool = false;
        }

        if (lightBool && lightTime > 0)
        {
            lantern.pointLightInnerRadius -= lightfloat * Time.deltaTime * 0.5f;
        }
        else if (!lightBool && lightTime > 0)
        {
            lantern.pointLightInnerRadius += lightfloat * Time.deltaTime * 0.5f;
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
        if (collision.gameObject.CompareTag("Fuel")) // yakıt
        {
            lightTime = 40f;
            Debug.Log("Fenere yakıt eklendi");
            fuels++;
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            MusicManager.PauseBackgroundMusic();
            if(collision.gameObject.name == "Daughter") SceneManager.LoadScene("GirlKill");
            if(collision.gameObject.name == "Mother") SceneManager.LoadScene("MotherKill");
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
