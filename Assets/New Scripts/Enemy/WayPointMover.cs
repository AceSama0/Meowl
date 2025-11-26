using System.Collections;
using UnityEngine;

public class WayPointMover : MonoBehaviour
{
    [SerializeField] Transform wayPointParent;
    Enemy enemy;
    Player player;
    [SerializeField] float movementSpeed = 2f;
    [SerializeField] float waitTime = 1f;
    public bool loopWayPoints = true;
    [SerializeField] Transform[] wayPoints;
    private int currentWayPointIndex;
    private bool isWaiting;
    void Awake()
    {
    
        enemy = GetComponent<Enemy>();
        player = FindAnyObjectByType<Player>();
    }
    void Start()
    {
        wayPoints = new Transform[wayPointParent.childCount];

        for (int i = 0; i < wayPointParent.childCount; i++)
        {
            wayPoints[i] = wayPointParent.GetChild(i);
        }
    }

    void Update()
    {
        Roam();
        // if (SeesPlayer())
        // {
        //     loopWayPoints = false;
        //     enemy.enabled = true;
        // }
        // else
        // {
        //     loopWayPoints = true;
        //     MoveToWayPoint();
        //     enemy.enabled = false;
        // }

    }

    public void Roam()
    {
        if (PauseMenuUI.instance.isGamePause || isWaiting)
        {
            return;
        }
        MoveToWayPoint();
    }

    void MoveToWayPoint()
    {
        Transform target = wayPoints[currentWayPointIndex];

        transform.position = Vector2.MoveTowards(transform.position, target.transform.position, movementSpeed * Time.deltaTime);
        if (Vector2.Distance(transform.position, target.transform.position) < 0.1f)
        {
            StartCoroutine(WaitAtWayPoint());
        }
    }


    IEnumerator WaitAtWayPoint()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTime);
        currentWayPointIndex = loopWayPoints ? (currentWayPointIndex + 1) % wayPoints.Length : Mathf.Min(currentWayPointIndex + 1, wayPoints.Length - 1);
        isWaiting = false;
    }
    
}
