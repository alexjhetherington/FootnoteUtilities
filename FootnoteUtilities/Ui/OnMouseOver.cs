using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnMouseOver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Action OnEnter;
    public Action OnExit;

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnEnter.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnExit.Invoke();
    }
}
