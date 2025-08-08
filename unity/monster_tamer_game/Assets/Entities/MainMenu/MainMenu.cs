using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private void Awake()
    {
        MusicPlayerController.Instance.PlayMusic((int)TrackNumber.TitleTheme);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Battle");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
