using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TableScript : MonoBehaviour
{
    Light2D candle;
    bool candleBurning;
    void Start()
    {
        candle = GetComponentInChildren<Light2D>();
        candle.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            candle.enabled = true;
            
        }
    }
}
