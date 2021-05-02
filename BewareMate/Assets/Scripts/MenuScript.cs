using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public void onPlay()
    {
        SceneManager.LoadScene(Constants.GAME_SCENE);
    }
    public void onHelp()
    {
        SceneManager.LoadScene(Constants.HELP_SCENE);
    }

    public void onGoToMenu()
    {
        SceneManager.LoadScene(Constants.MENU_SCENE);
    }

    public void onQuit()
    {
#if UNITY_EDITOR
        // Application.Quit() does not work in the editor so
        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }
}
