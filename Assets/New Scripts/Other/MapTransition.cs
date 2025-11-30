using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
public class MapTransition : MonoBehaviour
{
    Enemy enemy;
    Player player;
    SpriteRenderer enemysprite;
    [Header("MapChange")]
    [SerializeField] GameObject maptoActivate;
    [SerializeField] GameObject maptoDeactivate;
    [SerializeField] Transform cameraPos;

    [Header("FadeOut")]
    [SerializeField] GameObject Animation;    
    [Header("Camera")]
    
    [SerializeField] float cameraSize;
    private CinemachineConfiner2D confiner;
    private CinemachineCamera vCam;
    [Header("BoundryChange")]
    [SerializeField] PolygonCollider2D mapBoundry;
    [SerializeField]float transformInt; 
    [SerializeField] Vector3 cameraPosition;
    [SerializeField]Direction direction;
    enum Direction{Up,Down,Left,Right}

    void Awake()
    {
        vCam = FindAnyObjectByType<CinemachineCamera>();
        enemy = FindAnyObjectByType<Enemy>();
        player = FindAnyObjectByType<Player>();
        confiner = FindAnyObjectByType<CinemachineConfiner2D>();    
        vCam.Follow = cameraPos;
        if(enemy == null)
        {
            return;
        }
        else
        {
            enemysprite = enemy.GetComponent<SpriteRenderer>();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            MapChange();
            StartCoroutine(TransitionEffect());
            confiner.BoundingShape2D = mapBoundry;
            UpdatePlayerPosition(collision.gameObject);
            enemysprite.enabled = !enemysprite.enabled;

        }
        else if(collision.CompareTag("Enemy"))
        {
            UpdatePlayerPosition(collision.gameObject);
            enemysprite.enabled = !enemysprite.enabled;
        }
    }

    void MapChange()
    {
        if (maptoDeactivate != null)
        {
            maptoDeactivate.SetActive(false);
        }
        if (maptoActivate != null)
        {
            maptoActivate.SetActive(true);
            // vCam.enabled = SetCineMachine;
            vCam.Follow = cameraPos;
        }
    }

    IEnumerator TransitionEffect()
    {
        float currentSpeed = player.speed;
        Animation.SetActive(true);
        player.speed = 0;
        yield return new WaitForSeconds(1);
        Animation.SetActive(false);
        player.speed = currentSpeed;
    }
    
    void UpdatePlayerPosition(GameObject player)
    {
        Vector3 playerPos = player.transform.position;

        switch (direction)
        {
            case Direction.Up:
                playerPos.y += transformInt;
                break;
            case Direction.Down:
                playerPos.y -= transformInt;
                break;
            case Direction.Left:
                playerPos.x -= transformInt;
                break;
            case Direction.Right:
                playerPos.x += transformInt;
                break;
        }

        player.transform.position = playerPos;
        
    }

}
