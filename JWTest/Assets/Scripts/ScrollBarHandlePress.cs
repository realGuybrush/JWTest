using UnityEngine;
using UnityEngine.EventSystems;

public class ScrollBarHandlePress : MonoBehaviour, IPointerDownHandler, IDropHandler, IPointerClickHandler
{
    public MainManager mainManager;

    public void OnPointerDown(PointerEventData eventData)
    {
        mainManager.OnScrollBarHandlePress();
    }

    public void OnDrop(PointerEventData data)
    {
        mainManager.OnScrollBarHandleRelease();
    }

    public void OnPointerClick(PointerEventData data)
    {
        mainManager.OnScrollBarHandleRelease();
    }
}
