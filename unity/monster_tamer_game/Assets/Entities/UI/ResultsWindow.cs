using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class ResultsWindow : MonoBehaviour
{

    private TMP_Text label;

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // loads current scene
    }

    public void ShowWindow(bool playersAlive)
    {
        string resultString = playersAlive ? "You Win" : "You Lose";
        GetComponentInChildren<TMP_Text>().text = resultString;

        gameObject.SetActive(true);
    }
    public void HideWindow()
    {
        gameObject.SetActive(false);
    }

}