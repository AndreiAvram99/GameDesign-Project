using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;


public class MenuScript : MonoBehaviour
{
    public TMP_InputField firstPlayerInputField;
    public TMP_InputField secondPlayerInputField;
    public GameObject playerNameManager;
    
    public void onPlay()
    {
        string firstPlayerName = firstPlayerInputField.text;
        string secondPlayerName = secondPlayerInputField.text;

        if(firstPlayerName.Length == 0 || secondPlayerName.Length == 0)
        {
            EditorUtility.DisplayDialog("Name(s) is(are) empty", "Both names have to be completed", "Ok");
            return;
        }

        playerNameManager.GetComponent<PlayerNameManager>().setPlayerName(firstPlayerName, secondPlayerName);
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

    public void onChosePlayerName()
    {
        SceneManager.LoadScene(Constants.CHOOSE_PLAYER_NAME_SCENE);
    }

    public void onRanking()
    {
        SceneManager.LoadScene(Constants.RANKING_SCENE);
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
