using UnityEngine;
using UnityEngine.EventSystems;

public class DoorKnob : MonoBehaviour, IPointerDownHandler
{
    public bool openDoor;
    
    public void OnPointerDown(PointerEventData eventData)
    {
        openDoor = true;
    }

}
