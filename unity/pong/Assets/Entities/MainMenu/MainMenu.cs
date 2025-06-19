using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private int GAME_SCENE = 1;

    public void StartGame()
    {
        SceneManager.LoadScene(GAME_SCENE);
    }
    public void QuitGame()
    {
        Debug.Log("Game Quit");
        Application.Quit();
    }
}
