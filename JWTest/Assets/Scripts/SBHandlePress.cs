using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SBHandlePress : MonoBehaviour, IPointerDownHandler, IDropHandler, IPointerClickHandler
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
