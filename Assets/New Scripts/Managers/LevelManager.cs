using Unity.VisualScripting;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance { get; private set; }
    // [SerializeField] GameObject door1;
    public bool[] keys = new bool[5];
    public int keyIndex = 0;
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
        StartLevel();
    }
    void Update()
    {
        SetActiveDoor();
    }

    void SetActiveDoor()
    {
        if (keys[0])
        {
            // door1.SetActive(true);
        }
    }
    public void StartLevel()
    {
        GameManager.instance.PlayerPos();
        // Enemy.instance.transform.position = new Vector3(0, 4f , 0);
    }
    public void EnemyDelay(float time)
    {
        Invoke("StopEnemies", time);
    }
    void StopEnemies()
    {
        EnemyManager.instance.StopEnemy();
    }
}
