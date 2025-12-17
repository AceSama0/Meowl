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
        SoundEffectManager.Play("HoldingMirror");
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
                // SWAP işlemi
                dropSlotItem.transform.SetParent(originalSlot.transform);
                originalSlot.currentImage = dropSlotItem;
                Debug.Log($"SWAP: {originalSlot.name} artık {dropSlotItem.name} içeriyor"); // DEBUG

                // ... rect transform kodları ...

                transform.SetParent(dropSlot.transform);
                dropSlot.currentImage = gameObject;
                Debug.Log($"SWAP: {dropSlot.name} artık {gameObject.name} içeriyor"); // DEBUG
                FitItemToSlot();
            }
            else
            {
                // YENİ YERLEŞTIRME
                if (originalSlot.currentImage == gameObject)
                {
                    originalSlot.currentImage = null;
                    Debug.Log($"ESKİ SLOT TEMİZLENDİ: {originalSlot.name}"); // DEBUG
                }

                transform.SetParent(dropSlot.transform);
                dropSlot.currentImage = gameObject;
                Debug.Log($"YENİ YERLEŞTIRME: {dropSlot.name} artık {gameObject.name} içeriyor"); // DEBUG
                FitItemToSlot();
            }

            // DÜZELTME: Her iki slot için de kontrol et
            if (dropSlot.CompareTag("PuzzleSlot") || originalSlot.CompareTag("PuzzleSlot"))
            {
                // Önce dropSlot'tan ara
                PuzzleController manager = dropSlot.GetComponentInParent<PuzzleController>();

                // Bulamazsan originalSlot'tan ara
                if (manager == null)
                {
                    manager = originalSlot.GetComponentInParent<PuzzleController>();
                }

                // Hala bulamazsan tüm sahnede ara (son çare)
                if (manager == null)
                {
                    manager = FindAnyObjectByType<PuzzleController>();
                }

                if (manager != null)
                {
                    Debug.Log("PuzzleController bulundu, kontrol ediliyor..."); // DEBUG
                    StartCoroutine(CheckPuzzleDelayed(manager));
                }
                else
                {
                    Debug.LogError("PuzzleController bulunamadı!"); // DEBUG
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
        // Bir frame bekle (UI güncellensin diye)
        yield return null;

        if (manager != null)
        {
            Debug.Log("CheckPuzzleStatus çağrılıyor..."); // DEBUG
            manager.CheckPuzzleStatus();
        }
        else
        {
            Debug.LogError("Manager null oldu!"); // DEBUG
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