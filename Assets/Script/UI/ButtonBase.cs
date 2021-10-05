using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonBase : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //누름 여부
    [HideInInspector]
    public bool pressed = false;
    
    //누름
    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        pressed = true;
    }

    //땜
    public virtual void OnPointerExit(PointerEventData eventData)
    {
        pressed = false;
    }
}
