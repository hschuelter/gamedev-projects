using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActionMenu : MonoBehaviour
{
    private List<ActionOption> actionsList = new List<ActionOption>();
    [HideInInspector] public ActionOption lastAction;
    
    [SerializeField] ActionMenuCursor actionCursor;

    void Start()
    {
        actionsList = GetComponentsInChildren<ActionOption>().ToList();
        foreach (var action in actionsList)
        {
            action.actionMenu = this;
        }

        actionsList.First().GetComponent<Button>().Select();
        lastAction = actionsList.First();
        MoveCursorTo(lastAction);
    }

    public void MoveCursorTo(ActionOption actionOption)
    {
        actionCursor.ChangeTarget(actionOption);
        lastAction = actionOption;
    }

    public void ShowMenu(bool active)
    {
        this.gameObject.SetActive(active);
        actionCursor.gameObject.SetActive(active);

        actionCursor.ChangeTarget(lastAction);
    }

    public void EnableAllButtons()
    {
        foreach (var item in actionsList)
        {
            item.GetComponent<Button>().interactable = true;
        }
        lastAction.GetComponent<Button>().Select();
    }

    public void DisableAllButtons()
    {
        foreach (var item in actionsList)
        {
            item.GetComponent<Button>().interactable = false;
        }
    }

}
