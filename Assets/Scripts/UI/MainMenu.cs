using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Playgame()
    {
        SceneManager.LoadScene("Game");
    }

    public void Credits()
    {

    }

    public void QuitGame()
    {
        DatabaseController.Exit();
        Application.Quit();
    }
}
