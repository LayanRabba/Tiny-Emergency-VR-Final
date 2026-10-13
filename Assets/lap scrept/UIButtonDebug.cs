using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonDebug : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerDownHandler,
    IPointerUpHandler,
    IPointerClickHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("START >>> POINTER ENTER");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("START <<< POINTER EXIT");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("START >>> POINTER DOWN");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("START >>> POINTER UP");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("START !!! CLICK");
    }
}