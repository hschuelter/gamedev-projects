using UnityEngine;
using UnityEngine.EventSystems;

public class HoverUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        //Cursor.SetCursor(pointCursor, Vector2.zero, CursorMode.Auto);
        transform.localScale = new Vector2(1.1f, 1.1f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = new Vector2(1.0f, 1.0f);
        //Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}
