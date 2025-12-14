using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class MapTransition : MonoBehaviour
{
    [SerializeField] MapTransition previousMapTransition;
    public Coroutine lockedDoorCoroutine;
    [SerializeField] Enemy enemy;
    bool isEnemyInside = false;
    Player player;
    DoorKnob doorKnob;
    [SerializeField] GameObject door;
    WayPointMover wayPointMover;
    public bool isPlayerInside;
    [SerializeField] bool corridorBool;

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
    [SerializeField] float transformInt;
    [SerializeField] Vector3 cameraPosition;
    [SerializeField] Direction direction;

    [Header("CutScene")]
    [SerializeField] GameObject CutSceneObject;
    bool animatonPlayed;
    CineDeneme cineDeneme;
    bool soundPlayed = false;

    enum Direction { Up, Down, Left, Right }

    void Awake()
    {

        vCam = FindAnyObjectByType<CinemachineCamera>();
        player = FindAnyObjectByType<Player>();
        confiner = FindAnyObjectByType<CinemachineConfiner2D>();

        if (vCam != null && cameraPos != null)
        {
            vCam.Follow = cameraPos;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            HandlePlayerEnter();
            soundPlayed = false;
        }
        else if (collision.CompareTag("Enemy"))
        {
            
            HandleEnemyEnter(collision);
            soundPlayed = false;
        }
    }

    private void HandlePlayerEnter()
    {

        if (previousMapTransition != null)
        {
            if (previousMapTransition.lockedDoorCoroutine != null)
            {
                previousMapTransition.StopCoroutine(previousMapTransition.lockedDoorCoroutine);
                previousMapTransition.lockedDoorCoroutine = null;
            }
            previousMapTransition.isPlayerInside = false;
        }


        StartCoroutine(SeesDoor());
        lockedDoorCoroutine = StartCoroutine(LockedDoor());
    }

    private void HandleEnemyEnter(Collider2D collision)
    {
        if (corridorBool || !isPlayerInside)
        {
            UpdatePlayerPosition(collision.gameObject);
            if(!soundPlayed && corridorBool)SoundEffectManager.Play("door");
            soundPlayed = true;
            isEnemyInside = true;

            if (previousMapTransition != null)
            {
                previousMapTransition.isEnemyInside = false;
            }
        }
        else if (isPlayerInside)
        {
            StartCoroutine(TryOpeningDoor());
        }
    }

    IEnumerator TryOpeningDoor()
    {

        if (wayPointMover == null && enemy != null)
        {
            wayPointMover = enemy.GetComponent<WayPointMover>();
        }

        if (wayPointMover == null) yield break;

        float originalSpeed = wayPointMover.movementSpeed;
        wayPointMover.movementSpeed = 0;

        SoundEffectManager.Play("knocking");
        if (enemy.name == "Mother")
        {
            SoundEffectManager.Play("enemyScream");
        }

        yield return new WaitForSeconds(3);


        wayPointMover.currentWayPointIndex += 5;
        if (wayPointMover.wayPoints != null && wayPointMover.currentWayPointIndex >= wayPointMover.wayPoints.Length)
        {
            wayPointMover.currentWayPointIndex = 0;
        }

        wayPointMover.movementSpeed = originalSpeed;
    }

    public IEnumerator LockedDoor()
    {
        yield return new WaitForSeconds(5);
        isPlayerInside = true;
    }

    IEnumerator SeesDoor()
    {
        player.canMove = false;
        door.SetActive(true);


        if (doorKnob == null)
        {
            doorKnob = FindAnyObjectByType<DoorKnob>();
        }

        while (doorKnob != null && !doorKnob.openDoor && !Input.GetMouseButtonDown(1))
        {
            yield return null;
        }

        if (doorKnob != null && doorKnob.openDoor)
        {
            doorKnob.openDoor = false;

            MapChange();

            if (confiner != null && mapBoundry != null)
            {
                confiner.BoundingShape2D = mapBoundry;
            }

            UpdatePlayerPosition(player.gameObject);
            StartCoroutine(TransitionEffect());
            if(!soundPlayed) SoundEffectManager.Play("door");
            soundPlayed = true;
            StartCoroutine(CutSceneShower());

            
            lockedDoorCoroutine = StartCoroutine(LockedDoor());
        }
        else if (Input.GetMouseButtonDown(1))
        {
            if (corridorBool)
            {
                isPlayerInside = true;
            }    
            if (lockedDoorCoroutine != null)
            {
                StopCoroutine(lockedDoorCoroutine);
                lockedDoorCoroutine = null;
            }
        }

        door.SetActive(false);
        player.canMove = true;
    }

    IEnumerator TransitionEffect()
    {
        player.canMove = false;
        Animation.SetActive(true);
        SoundEffectManager.Play("door");

        yield return new WaitForSeconds(1);

        Animation.SetActive(false);
        player.canMove = true;
    }

    IEnumerator CutSceneShower()
    {
        if (CutSceneObject != null && !animatonPlayed)
        {
            yield return new WaitForSeconds(1);

            CutSceneObject.SetActive(true);

            
            if (cineDeneme == null)
            {
                cineDeneme = FindAnyObjectByType<CineDeneme>();
            }

            animatonPlayed = true;
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

            if (vCam != null && cameraPos != null)
            {
                vCam.Follow = cameraPos;
            }
        }
    }

    void UpdatePlayerPosition(GameObject targetObject)
    {
        if (targetObject == null) return;

        Vector3 pos = targetObject.transform.position;

        switch (direction)
        {
            case Direction.Up:
                pos.y += transformInt;
                break;
            case Direction.Down:
                pos.y -= transformInt;
                break;
            case Direction.Left:
                pos.x -= transformInt;
                break;
            case Direction.Right:
                pos.x += transformInt;
                break;
        }

        targetObject.transform.position = pos;
    }
}