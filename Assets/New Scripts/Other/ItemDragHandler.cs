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

        // Drop edilen slotu bul
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

        // Aynı slot'a mı bırakıldı?
        if (dropSlot == originalSlot)
        {
            transform.SetParent(originalParent);
            FitItemToSlot();
            return;
        }

        // ✅ GEÇERLİ BİR SLOT'A BIRAKILDIYSA
        if (dropSlot != null && originalSlot != null)
        {
            GameObject dropSlotItem = dropSlot.currentImage;

            // 🔄 DROP SLOT DOLU MU? → SWAP
            if (dropSlotItem != null && dropSlotItem != gameObject)
            {
                // 1. Drop slot'taki item'ı al ve original slot'a yerleştir
                dropSlotItem.transform.SetParent(originalSlot.transform);
                originalSlot.currentImage = dropSlotItem;
                
                RectTransform swappedRect = dropSlotItem.GetComponent<RectTransform>();
                if (swappedRect != null)
                {
                    swappedRect.anchoredPosition = Vector2.zero;
                    swappedRect.sizeDelta = originalSlot.GetComponent<RectTransform>().sizeDelta * 0.9f;
                    swappedRect.localScale = Vector3.one;
                }

                // 2. Sürüklenen item'ı drop slot'a yerleştir
                transform.SetParent(dropSlot.transform);
                dropSlot.currentImage = gameObject;
                FitItemToSlot();
            }
            // 📭 DROP SLOT BOŞ → TAŞI
            else
            {
                // Original slot'u temizle
                if (originalSlot.currentImage == gameObject)
                {
                    originalSlot.currentImage = null;
                }

                // Drop slot'a yerleştir
                transform.SetParent(dropSlot.transform);
                dropSlot.currentImage = gameObject;
                FitItemToSlot();
            }

            // 🧩 PUZZLE KONTROLÜ - BİR FRAME SONRA! (KRİTİK!)
            if (dropSlot.CompareTag("PuzzleSlot") || originalSlot.CompareTag("PuzzleSlot"))
            {
                StartCoroutine(CheckPuzzleDelayed());
            }
        }
        // ❌ GEÇERSİZ YER → GERİ DÖN
        else
        {
            transform.SetParent(originalParent);
            FitItemToSlot();
        }
    }

    // 🔥 KRİTİK: Bir frame bekle, sonra kontrol et
    IEnumerator CheckPuzzleDelayed()
    {
        // Unity'nin tüm transform değişikliklerini tamamlaması için bekle
        yield return null;

        PuzzleController puzzleController = FindAnyObjectByType<PuzzleController>();
        if (puzzleController != null)
        {
            puzzleController.CheckPuzzleStatus();
        }
    }

    void FitItemToSlot()
    {
        if (rectTransform == null) return;

        SlotScripts parentSlot = transform.parent?.GetComponent<SlotScripts>();
        if (parentSlot == null) return;

        RectTransform slotRect = parentSlot.GetComponent<RectTransform>();
        if (slotRect == null) return;

        // Anchor ve pivot ayarla
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        
        // Pozisyon ve boyut
        rectTransform.anchoredPosition = Vector2.zero;
        rectTransform.sizeDelta = slotRect.sizeDelta * 0.9f; // Slot boyutunun %90'ı
        rectTransform.localScale = Vector3.one;
    }
}