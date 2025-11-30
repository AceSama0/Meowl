using UnityEngine;

public class BlockScript : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] float Force = 1f;
    [SerializeField] PointerController pC;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        PushBlock();
    }

    void PushBlock()
    {
        if (pC.success)
        {
            rb.AddForce(Vector2.up * Force, ForceMode2D.Impulse);
        }
        else
        {
            return;
        }
    }
}
