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
    [SerializeField] private Button restartButton;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button endButton;

    [SerializeField] private TMP_Text totalExpGained;

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
        characterExpHUDManager.ClearChildren();
    }

    public IEnumerator ShowExpGained(int expGained)
    {
        characterExpHUDManager.DrawHUD(expGained);
        totalExpGained.text = $"{expGained}";
        yield return delay(1.0f);
        characterExpHUDManager.StartExp();
    }

    public void ShowRewards()
    {
        rewardsWindow.ShowWindow();
    }

    IEnumerator delay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

}