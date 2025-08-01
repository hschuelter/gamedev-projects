using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class ResultsWindow : MonoBehaviour
{

    private TMP_Text label;
    [SerializeField] private CharacterExpHUDManager characterExpHUDManager;
    [SerializeField] private BattleManager battleManager;
    [SerializeField] private EncounterController encounterController;
    [SerializeField] private RewardsWindow rewardsWindow;
    [SerializeField] private ExpMultiplierController expMultiplierController;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button endButton;

    [SerializeField] private TMP_Text totalExpGained;
    private int totalExpGainedValue;
    private float totalExpMultiplier;

    private void Update()
    {
        CheckNextButton();
    }

    void CheckNextButton()
    {
        nextButton.interactable = characterExpHUDManager.isOK();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void NextBattle()
    {
        HideWindow();
        battleManager.ShowMapWindow();
        characterExpHUDManager.ResetExp();
    }

    public void EndGame()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ShowWindow(bool playersAlive, int expGained)
    {
        string resultString = playersAlive ? "Victory!" : "Defeat...";
        GetComponentInChildren<TMP_Text>().text = resultString;

        gameObject.SetActive(true);
        StartCoroutine(ShowExpGained(expGained));

        var isEnd = !encounterController.IsNext();
        restartButton.gameObject.SetActive(!isEnd);
        nextButton.gameObject.SetActive(!isEnd);
        endButton.gameObject.SetActive(isEnd);
    }
    public void HideWindow()
    {
        gameObject.SetActive(false);
        expMultiplierController.HideExpItems();
        characterExpHUDManager.ClearChildren();
    }

    public IEnumerator ShowExpGained(int expGained)
    {
        characterExpHUDManager.DrawHUD(expGained);
        StartCoroutine(expMultiplierController.ShowExpItems());
        totalExpGainedValue = expGained;
        UpdateTotalExp(1f);

        yield return new WaitUntil(() => expMultiplierController.ShowExpItemsCompleted);

        yield return delay(0.3f);
        expMultiplierController.ShowExpItemsCompleted = false;
        characterExpHUDManager.StartExp(Mathf.RoundToInt(totalExpGainedValue * totalExpMultiplier));
    }

    public void ShowRewards()
    {
        rewardsWindow.ShowWindow();
    }
    public void UpdateTotalExp(float multiplier)
    {
        totalExpGained.text = $"{Mathf.RoundToInt(totalExpGainedValue * multiplier)}";
        totalExpMultiplier = multiplier;
    }

    IEnumerator delay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

}