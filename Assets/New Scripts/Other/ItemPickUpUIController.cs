using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class ItemPickUpUIController : MonoBehaviour
{
    public static ItemPickUpUIController Instance { get; private set; }
    public GameObject popUpPrefabs;
    public int maxPopup = 5;
    public float popUpduration = 3f;
    private readonly Queue<GameObject> activePopUps = new();
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowItemPickUp(string itemName, Sprite itemIcon)
    {
        GameObject newPopUp = Instantiate(popUpPrefabs, transform);
        newPopUp.GetComponentInChildren<Text>().text = itemName;

        Image itemImage = newPopUp.transform.Find("ItemIcon")?.GetComponent<Image>();
        if (itemImage)
        {
            itemImage.sprite = itemIcon;
        }

        activePopUps.Enqueue(newPopUp);

        if (activePopUps.Count > maxPopup)
        {
            Destroy(activePopUps.Dequeue());
        }

        StartCoroutine(FadeOutAndDestroy(newPopUp));
    }
    private IEnumerator FadeOutAndDestroy(GameObject popup)
    {
        yield return new WaitForSeconds(popUpduration);
        if (popup == null) yield break;

        CanvasGroup canvasGroup = popup.GetComponent<CanvasGroup>();
        for (float timePassed = 0f; timePassed < 1f; timePassed += Time.deltaTime)
        {
            if (popup == null) yield break;
            canvasGroup.alpha = 1f - timePassed;
            yield return null;
        }

        Destroy(popup);
    }
}
