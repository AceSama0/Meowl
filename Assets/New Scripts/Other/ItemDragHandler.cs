using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform originalParent;
    CanvasGroup canvasGroup;
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        transform.SetParent(transform.root);
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.6f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;
        
        SlotScripts dropSlot = eventData.pointerEnter?.GetComponent<SlotScripts>();
        if (dropSlot == null)
        {
            // transform.localScale = Vector3.one;
            GameObject dropItem = eventData.pointerEnter;
            if (dropItem != null)
            {
                dropSlot = dropItem.GetComponentInParent<SlotScripts>();
            }
        }
        SlotScripts originalSlot = originalParent.GetComponent<SlotScripts>();

        if (dropSlot != null)
        {
            if (dropSlot.currentImage != null)
            {
                dropSlot.currentImage.transform.SetParent(originalSlot.transform);
                originalSlot.currentImage = dropSlot.currentImage;
                dropSlot.currentImage.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            }
            else
            {
                originalSlot.currentImage = null;
            }

            transform.SetParent(dropSlot.transform);
            dropSlot.currentImage = gameObject;
        }
        else
        {
            transform.SetParent(originalParent);
        }

        GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }
}
