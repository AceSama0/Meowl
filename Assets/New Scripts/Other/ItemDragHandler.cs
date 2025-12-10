using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class ItemDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform originalParent;
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();

        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();

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

        SlotScripts dropSlot = null;

        if (eventData.pointerEnter != null)
        {
            dropSlot = eventData.pointerEnter.GetComponent<SlotScripts>();

            if (dropSlot == null)
            {
                dropSlot = eventData.pointerEnter.GetComponentInParent<SlotScripts>();
            }
        }

        SlotScripts originalSlot = originalParent?.GetComponent<SlotScripts>();

        if (dropSlot == originalSlot)
        {
            transform.SetParent(originalParent);
            FitItemToSlot();
            return;
        }

        if (dropSlot != null && originalSlot != null)
        {
            GameObject dropSlotItem = dropSlot.currentImage;

            if (dropSlotItem != null && dropSlotItem != gameObject)
            {
                dropSlotItem.transform.SetParent(originalSlot.transform);
                originalSlot.currentImage = dropSlotItem;

                RectTransform swappedRect = dropSlotItem.GetComponent<RectTransform>();
                if (swappedRect != null)
                {
                    swappedRect.anchoredPosition = Vector2.zero;
                    swappedRect.sizeDelta = originalSlot.GetComponent<RectTransform>().sizeDelta * 0.9f;
                    swappedRect.localScale = Vector3.one;
                }

                transform.SetParent(dropSlot.transform);
                dropSlot.currentImage = gameObject;
                FitItemToSlot();
            }
            else
            {
                if (originalSlot.currentImage == gameObject)
                {
                    originalSlot.currentImage = null;
                }

                transform.SetParent(dropSlot.transform);
                dropSlot.currentImage = gameObject;
                FitItemToSlot();
            }


            if (dropSlot.CompareTag("PuzzleSlot") || originalSlot.CompareTag("PuzzleSlot"))
            {
                PuzzleController manager = dropSlot.GetComponentInParent<PuzzleController>();

            if (manager != null)
            {
                // Coroutine'i BAŞLAT
                StartCoroutine(CheckPuzzleDelayed(manager)); 
            }
            }
        }
        else
        {
            transform.SetParent(originalParent);
            FitItemToSlot();
        }
    }

    IEnumerator CheckPuzzleDelayed(PuzzleController manager)
    {
        // 1. Kare bekle: Transform yerleşimini bitirir.
        yield return null;

        // 2. Kare bekle: Bileşenlerin (Item, Verifier) aktifleşmesini sağlar.
        yield return null;

        if (manager != null)
        {
            // Gecikmeden sonra kontrolü çağır
            manager.CheckPuzzleStatus();
        }
    }

    void FitItemToSlot()
    {
        if (rectTransform == null) return;

        SlotScripts parentSlot = transform.parent?.GetComponent<SlotScripts>();
        if (parentSlot == null) return;

        RectTransform slotRect = parentSlot.GetComponent<RectTransform>();
        if (slotRect == null) return;

        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);

        rectTransform.anchoredPosition = Vector2.zero;
        rectTransform.sizeDelta = slotRect.sizeDelta * 0.9f;
        rectTransform.localScale = Vector3.one;
    }
}