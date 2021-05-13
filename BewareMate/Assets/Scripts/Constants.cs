using UnityEngine;

public class Constants : MonoBehaviour
{
    public const int NumberOfLivesPerPlayer = 3;
    public const int X = 0;
    public const int Y = 1;
    public const int Z = 2;

    public const int AKeycode = 97;
    public const int WKeycode = 119;
    public const int DKeycode = 100;
    public const int LeftArrowKeycode = 276;
    public const int UpArrowKeycode = 273;
    public const int RightArrowKeycode = 275;

    public const int FirstPlayerPositionIndex = 0;
    public const int SecondPlayerPositionIndex = 2;

    public const int ShuffleIterations = 20;
    public const float FloorLength = 40f;

    public const float CameraInitX = 0.0f;
    public const float CameraInitY = 5.4f;
    public const float CameraInitZ = -46.6f;
    public const float DistFromCamera = 9.5f;

    public const float PlayersInitY = 2.4f;
    public const float PlayersInitZ = -37f;

    public const float FenceStart = -10f;
    public const float FenceDifference = -20f;
    

    public const float MaxSpeed = 25f;

    public const float FloorWidth = 18f;
    public const float NumberOfLanes = 4f;


    public const int MENU_SCENE = 0;
    public const int GAME_SCENE = 1;
    public const int GAME_OVER_SCENE = 2;
    public const int HELP_SCENE = 3;
    public const int CHOOSE_PLAYER_NAME_SCENE = 4;
    public const int RANKING_SCENE = 5;
}
