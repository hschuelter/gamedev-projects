using Microsoft.Unity.VisualStudio.Editor;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ExpMultiplierController : MonoBehaviour
{
    List<ExpMultiplierItem> expMultiplierObjs;

    [HideInInspector] public bool ShowExpItemsCompleted = false;

    [SerializeField] private ResultsWindow resultsWindow;
    private float _expMultiplier = 1f;

    void Awake()
    {
        expMultiplierObjs = new List<ExpMultiplierItem>();
        expMultiplierObjs = GetComponentsInChildren<ExpMultiplierItem>().ToList();
        expMultiplierObjs.ForEach(item => item.Show(false));   
    }

    public IEnumerator ShowExpItems()
    {
        _expMultiplier = 1f;
        foreach (var item in expMultiplierObjs.Where(bonus => bonus.IsActive))
        {
            item.Show(true);
            _expMultiplier *= item.Multiplier;
            yield return delay(0.5f);
            resultsWindow.UpdateTotalExp(_expMultiplier);
        }
        yield return delay(0.3f);
        ShowExpItemsCompleted = true;
    }
    public void HideExpItems()
    {
        expMultiplierObjs.ForEach(item => item.Show(false));
        expMultiplierObjs.ForEach(item => item.IsActive = false);
        NotifySingleRound(true);
        NotifyUntouched(true);
    }

    public void NotifyUntouched(bool value)
    {
        Debug.Log($"Untouched {value}");
        UpdateActiveMultiplier("Untouched", value);
    }

    public void NotifyChargedAttack(bool value)
    {
        UpdateActiveMultiplier("Charged Attack", value);
    }

    public void NotifyRecklessAttack(bool value)
    {
        UpdateActiveMultiplier("Reckless Attack", value);
    }

    public void NotifySingleRound(bool value)
    {
        UpdateActiveMultiplier("Single Round", value);
    }

    public void NotifyWeakness(bool value)
    {
        UpdateActiveMultiplier("Weakness", value);
    }

    private void UpdateActiveMultiplier(string description, bool value)
    {
        expMultiplierObjs.Where(item => item.Description == description).ToList().ForEach(bonus => bonus.IsActive = value);
    }

    IEnumerator delay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

}
