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
    [HideInInspector] public int position = 1;

    public void Load()
    {
        buttonOptions = GetComponentsInChildren<Button>().ToList();
        buttonOptions.ForEach(b => b.interactable = false);
    }

    public void ShowWindow()
    {
        gameObject.SetActive(true);
        var nextStage = buttonOptions.ElementAt(position);
        nextStage.interactable = true;
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
