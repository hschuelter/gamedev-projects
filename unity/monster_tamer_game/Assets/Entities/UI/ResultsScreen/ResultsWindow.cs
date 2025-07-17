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
    [SerializeField] private Button nextButton;

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
        battleManager.NextBattle();
    }

    public void ShowWindow(bool playersAlive, int expGained)
    {
        string resultString = playersAlive ? "You Win" : "You Lose";
        GetComponentInChildren<TMP_Text>().text = resultString;

        gameObject.SetActive(true);
        ShowExpGained(expGained);
    }
    public void HideWindow()
    {
        gameObject.SetActive(false);
        characterExpHUDManager.ClearChildren();
    }

    public void ShowExpGained(int expGained)
    {
        characterExpHUDManager.DrawHUD(expGained);
    }

}