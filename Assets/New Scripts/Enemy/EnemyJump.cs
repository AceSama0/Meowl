using System.Collections;
using UnityEngine;

public class EnemyJump : MonoBehaviour
{
    Rigidbody2D rb;
    public float movementSpeed = 2f;
    Vector2 moveDirection;
    Transform target;
    public float originalSpeed { get; private set; }
    [SerializeField] float lungePower = 10f;

    void Start()
    {
        target = FindAnyObjectByType<Player>().transform;
    }

    public IEnumerator WaitBeforeJump()
    {
        yield return new WaitForSeconds(1);

        if (target == null) yield break;


        Vector3 startPosition = transform.position;
        Vector3 direction = (target.position - startPosition).normalized;

        float distanceToTravel = lungePower;
        Vector3 endPosition = startPosition + direction * distanceToTravel;

        float timeElapsed = 0;

        while (timeElapsed < 1)
        {
            float t = timeElapsed / 1;

            transform.position = Vector3.Lerp(startPosition, endPosition, t);

            timeElapsed += Time.deltaTime;

            yield return null;
        }

        transform.position = endPosition;

        yield return new WaitForSeconds(0.5f);
    }
}
