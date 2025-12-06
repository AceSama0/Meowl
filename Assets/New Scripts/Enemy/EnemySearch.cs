using UnityEngine;

public class EnemySearch : MonoBehaviour
{
    EnemyBehave enemyBehave;
    Vector2 Direction;
    Rigidbody2D rb;
    float movementSpeed = 5f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyBehave = GetComponent<EnemyBehave>();
        Direction = enemyBehave.lastSeen;
    }

    void FixedUpdate()
    {
        Vector2 direction = (Direction - (Vector2)transform.position).normalized;
        rb.linearVelocity = direction * movementSpeed;
    }


}
