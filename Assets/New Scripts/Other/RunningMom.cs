using UnityEngine;

public class RunningMom : MonoBehaviour
{
    [SerializeField] float speed = 10f;
    [SerializeField] Transform point;
    [SerializeField] Animator animator;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, point.transform.position , speed * Time.deltaTime);
    }
}
