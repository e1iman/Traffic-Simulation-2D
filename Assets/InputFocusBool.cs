using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class InputFocusBool : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler{
    public void OnPointerEnter(PointerEventData eventData)
    {
        GameMaster.GM.inputFocus = true;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        GameMaster.GM.inputFocus = false;
    }
}
