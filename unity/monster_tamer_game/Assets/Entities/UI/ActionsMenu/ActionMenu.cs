using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActionMenu : MonoBehaviour
{
    [HideInInspector] public List<ActionOption> actionsList = new List<ActionOption>();
    [HideInInspector] public ActionOption lastAction;
    [HideInInspector] public VerticalLayoutGroup verticalLayout;

    public BattleManager battleManager;
    public ActionMenuCursor actionCursor;
    bool isSetup = false;

    void Start()
    {
        verticalLayout = GetComponent<VerticalLayoutGroup>();
        Setup();
        lastAction = actionsList.Where(action => action.gameObject.activeSelf).FirstOrDefault() ?? lastAction;
        lastAction.GetComponent<Button>().Select();
        MoveCursorTo(lastAction);
    }
    public void Setup()
    {
        if (isSetup) return;

        actionsList = GetComponentsInChildren<ActionOption>().ToList();
        foreach (var action in actionsList)
        {
            action.actionMenu = this;
            action.SetInfo();
        }
        lastAction = actionsList.First();
        isSetup = true;
    }

    public void SelectFirstOption()
    {
        if (actionsList.Count == 0) Setup();

        //lastAction = actionsList.First();
        lastAction.GetComponent<Button>().Select();
        actionCursor.Activate();
        MoveCursorTo(lastAction);
    }

    public void MoveCursorTo(ActionOption actionOption)
    {
        actionCursor.ChangeTarget(actionOption);
        lastAction = actionOption;
    }

    public void LoadItems(string[] itemList)
    {
        foreach(var item in itemList)
            Debug.Log($"{item}");
    }

    public void ShowMenu(bool active)
    {
        //this.gameObject.SetActive(active);
        this.transform.parent.gameObject.SetActive(active);
        actionCursor.Activate();

        actionCursor.ChangeTarget(lastAction);
    }

    public virtual void EnableAllButtons()
    {
        foreach (var item in actionsList)
        {
            item.GetComponent<Button>().interactable = true;
        }
        if (!lastAction.gameObject.activeSelf) lastAction = actionsList.Where(action => action.gameObject.activeSelf).FirstOrDefault();
        lastAction.GetComponent<Button>().Select();
        actionCursor.Activate();
    }

    public void DisableAllButtons()
    {
        foreach (var item in actionsList)
        {
            item.GetComponent<Button>().interactable = false;
        }
        actionCursor.Deactivate();
    }

}
