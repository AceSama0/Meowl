using System.Collections;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEditor.Connect;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private Coroutine activeCoroutine;
    public static Player instance { get; private set; }
    [Header("Lantern")]
    [SerializeField] Transform[] lightWayPoints = new Transform[3];
    [SerializeField] int lightWPIndex = 3;

    [SerializeField] float lightfloat = 5f;
    [SerializeField] bool lightBool = true;
    [SerializeField] bool switchHand;


    [SerializeField] Light2D lantern;

    public float lightTime;
    [Header("Puzzle")]
    private int keyValue = 0;
    [SerializeField] Vector2 movement;
    [SerializeField] float speed = 5f; 
    [SerializeField] public int fuels = 0;

    [SerializeField] Rigidbody2D rb;

    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Animator animator;

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
    }

    void Start()
    {
        lantern.transform.position = lightWayPoints[0].position;
        lantern.pointLightInnerRadius = 5f;
        StartCoroutine(switchingLight());    
    }
    void Update()
    {

        playerMovement();
        LightOnOff();
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
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }

        if (inputX == -1)
        {
            spriteRenderer.flipX = true;
        }

        else if (inputX == 1)
        {
            spriteRenderer.flipX = false;
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

        if (collision.gameObject.CompareTag("Table")) //masa etkileşimi
        {
            // GameManager.instance.canvas = true;
        }

        if (collision.gameObject.CompareTag("Key")) // anahtar
        {
            Destroy(collision.gameObject);
            LevelManager.instance.keys[LevelManager.instance.keyIndex] = true;
            keyValue++;

            if (keyValue == 5)
            {
                GameManager.instance.doorBool = true;
            }
        }
        if (collision.gameObject.CompareTag("Door") && GameManager.instance.doorBool) // kapı
        {
            GameManager.instance.NextScene();

        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            SceneManager.LoadScene("KızÖlüm");
        }
        if (collision.gameObject.CompareTag("Drawer"))
        {
            UIManager.instance.activateDialogue = true;
        }
    }

    IEnumerator RightHandLight(Light2D lantern)
    {
        if (lightWPIndex < 0) { lightWPIndex = 0; }
        while (lightWPIndex <= lightWayPoints.Length-1)
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
        while (true)
        {
            if (activeCoroutine != null)
            {
                StopCoroutine(activeCoroutine);
            }
            activeCoroutine = StartCoroutine(RightHandLight(lantern));
            yield return new WaitForSeconds(5);
            if (activeCoroutine != null)
            {
                StopCoroutine(activeCoroutine);
            }
            activeCoroutine = StartCoroutine(LeftHandLight(lantern));   
            yield return new WaitForSeconds(5);
        }
    }
    

}
