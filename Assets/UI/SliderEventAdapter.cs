using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class SliderEventAdapter : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public event Action onPointerDown;
    public event Action onPointerUp;
    public event Action onDrag;

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        onPointerDown?.Invoke();
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        onPointerUp?.Invoke();
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        onDrag?.Invoke();
    }
}
