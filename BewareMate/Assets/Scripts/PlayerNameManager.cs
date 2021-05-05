using UnityEngine;

public class PlayerNameManager : MonoBehaviour
{
    static string _firstPlayerName;
    static string _secondPlayerName;
    
    public void setPlayerName(string fPlayerName, string sPlayerName)
    {
        _firstPlayerName = fPlayerName;
        _secondPlayerName = sPlayerName;
    }

    public string getFirstPlayerName()
    {
        return _firstPlayerName;
    }
    
    public string getSecondPlayerName()
    {
        return _secondPlayerName;
    }
    
}
