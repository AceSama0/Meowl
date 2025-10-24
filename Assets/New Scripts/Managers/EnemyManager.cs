using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance { get; private set; }
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
    public void StopEnemy()
    {
        EnemyScript.instance.speed = 0;
    }
    public void EnemySpawn()
    {
        var pos = EnemyScript.instance.transform.position;
        pos.x = Random.Range(-7f, 7f);
        pos.y = Random.Range(-3f, 3f);
        EnemyScript.instance.transform.position = pos;
    }

}
