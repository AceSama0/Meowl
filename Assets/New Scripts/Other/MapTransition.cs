using System.Collections.Generic;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
public class MapTransition : MonoBehaviour
{
    Enemy enemy;
    SpriteRenderer enemysprite;
    [Header("MapChange")]
    [SerializeField] GameObject maptoActivate;
    [SerializeField] GameObject maptoDeactivate;
    [SerializeField] bool SetCineMachine ;
    
    [Header("Camera")]
    private Camera camera1;
    [SerializeField] GameObject cineMachine;
    [SerializeField] float cameraSize;
    private CinemachineConfiner2D confiner;
    [Header("BoundryChange")]
    [SerializeField] PolygonCollider2D mapBoundry;
    [SerializeField]Direction direction;
    [SerializeField]float transformInt; 
    [SerializeField] Vector3 cameraPosition;
    enum Direction{Up,Down,Left,Right}

    void Awake()
    {
        enemy = FindAnyObjectByType<Enemy>();
        confiner = FindAnyObjectByType<CinemachineConfiner2D>();    
        camera1 = FindAnyObjectByType<Camera>();
        enemysprite = enemy.GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            MapChange();
            SetCamera();
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
            cineMachine.SetActive(SetCineMachine);
        }
    }

    void SetCamera()
    {
        camera1.transform.position = cameraPosition;
        camera1.orthographicSize = cameraSize;
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
