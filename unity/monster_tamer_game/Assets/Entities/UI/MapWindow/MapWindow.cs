using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapWindow : MonoBehaviour
{
    [SerializeField] private BattleManager battleManager;

    private List<Button> buttonOptions = new List<Button>();
    private int position = 0;

    private void Start()
    {
        buttonOptions = GetComponentsInChildren<Button>().ToList();
        buttonOptions.ForEach(b => b.interactable = false);
    }

    public void ShowWindow()
    {
        buttonOptions.ForEach(b => b.interactable = false);
        buttonOptions.ElementAt(position++).interactable = true;

        gameObject.SetActive(true);
    }

    public void HideWindow()
    {
        gameObject.SetActive(false);
    }

    public void NextBattle()
    {
        battleManager.NextBattle();
    }
}
