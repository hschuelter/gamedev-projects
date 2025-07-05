using UnityEngine;
using UnityEngine.EventSystems;

public class ActionOption : MonoBehaviour, ISelectHandler
{
    [HideInInspector] public ActionMenu actionMenu;

    public void OnSelect(BaseEventData eventData)
    {
        actionMenu.MoveCursorTo(this);
        if (actionMenu != null)
            actionMenu.lastAction = this;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
