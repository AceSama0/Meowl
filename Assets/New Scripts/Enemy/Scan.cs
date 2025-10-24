using UnityEngine;

public class Scan : MonoBehaviour
{
    [SerializeField] float radius;
    LayerMask layerMask;
    [SerializeField] string StringMask;
    void Start()
    {
        StringMask = layerMask.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        Scanning();
    }
    public void Scanning()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(gameObject.transform.position, radius, LayerMask.GetMask(StringMask));
        if (hits.Length > 0)
        {
            Collider2D closest = null;
            float minDistance = Mathf.Infinity;
            foreach (Collider2D hit in hits)
            {
                float distance = Vector2.Distance(hit.transform.position, transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closest = hit;
                }
            }
            if (closest != null)
            {
                Debug.Log($"En yakın Dolap {closest.name}" );
            }
        }
    }
}
