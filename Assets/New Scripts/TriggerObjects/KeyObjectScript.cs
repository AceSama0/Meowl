using UnityEngine;

public class KeyObjectScript : MonoBehaviour
{
    [SerializeField] MapTransition mapTransition;
    [SerializeField] GameObject keyVisual;
    bool isAtKey = false; 
    bool hasPickedUp = false; 

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isAtKey = true;
            Debug.Log("Anahtarı almak için E'ye bas.");
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isAtKey = false;
        }
    }

    void Update()
    {
        if (isAtKey && !hasPickedUp && Input.GetKeyDown(KeyCode.E))
        {
            hasPickedUp = true;
            if (keyVisual != null) keyVisual.SetActive(true);

            if (mapTransition != null)
            {
                mapTransition.doorCanOpen = true;
            }

   
            GetComponent<SpriteRenderer>().enabled = false;
        }
  
        else if (hasPickedUp && Input.GetMouseButtonDown(0))
        {
            if (keyVisual != null) keyVisual.SetActive(false);
            Destroy(gameObject);
        }
    }
}