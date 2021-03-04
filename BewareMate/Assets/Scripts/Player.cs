using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private int leftKeyCode;
    private int rightKeyCode;
    private int upKeyCode;
    private string playerTag;

    private Player matePlayer;
    private GameObject playerObject;
    private int startPositionIndex;
    private int currentPositionIndex;
    private float[] lanesMiddles; 
    private Vector3 transformPosition;

    public Player(GameManager gameManager, string playerTag)
    {
        this.playerTag = playerTag;
        playerObject = GameObject.FindGameObjectWithTag(this.playerTag);
        setLanesMiddles(gameManager);
        setStartPositionIndexAndKeys();
        setPosition(new Vector3(lanesMiddles[startPositionIndex],
                                    playerObject.transform.position[Constants.Y],
                                    playerObject.transform.position[Constants.Z]));
    }

    public void addMatePlayer(Player player)
    {
        matePlayer = player;
    }

    public void moveForward(GameManager gameManager)
    {
        transformPosition = gameManager.moveVector * gameManager.moveSpeed * Time.deltaTime;
        playerObject.transform.Translate(transformPosition);
    }

    public void changeLane()
    {
        bool moveFlag = false;
        if (Input.GetKeyUp((KeyCode)leftKeyCode) &&
                            currentPositionIndex != 0 &&
                            currentPositionIndex - 1 != matePlayer.currentPositionIndex)
        {
            setCurrentPositionIndex(currentPositionIndex - 1);
            moveFlag = true;
        }

        else if (Input.GetKeyUp((KeyCode)rightKeyCode) &&
                                 currentPositionIndex != 3 &&
                                 currentPositionIndex + 1 != matePlayer.currentPositionIndex)
        {
            setCurrentPositionIndex(currentPositionIndex + 1);
            moveFlag = true;
        }

        if (moveFlag)
        {
            Vector3 playerMoveDirection = new Vector3(lanesMiddles[currentPositionIndex],
                                                        playerObject.transform.position[Constants.Y],
                                                        playerObject.transform.position[Constants.Z]);
            setPosition(playerMoveDirection);
        }
    }

    private void setCurrentPositionIndex(int index)
    {
        currentPositionIndex = index;
    }

    private void setLanesMiddles(GameManager gameManager)
    {
        lanesMiddles = new float[4];
        float laneDimension = gameManager.floor.transform.localScale[Constants.Z] / 4;

        for (int i = 0; i < lanesMiddles.Length; i++)
        {
            if (i < 2)
            {
                lanesMiddles[i] = 0 - (1 - i) * laneDimension - laneDimension / 2;
            }
            else
            {
                lanesMiddles[i] = 0 + (i - 2) * laneDimension + laneDimension / 2;
            }

        }
    }

    private void setFirstPlayerKeys() {
        leftKeyCode = Constants.A_KEYCODE;
        rightKeyCode = Constants.D_KEYCODE;
        upKeyCode = Constants.W_KEYCODE;
    }

    private void setSecondPlayerKeys() {
        leftKeyCode = Constants.LEFT_ARROW_KEYCODE;
        rightKeyCode = Constants.RIGHT_ARROW_KEYCODE;
        upKeyCode = Constants.UP_ARROW_KEYCODE;
    }

    private void setPosition(Vector3 position) {
        playerObject.transform.position = position;
    }

    private void setStartPositionIndexAndKeys() {
        if (playerTag == "FirstPlayer")
        {
            startPositionIndex = Constants.FIRST_PLAYER_POSITION_INDEX;
            setFirstPlayerKeys();
        }
        else
        {
            startPositionIndex = Constants.SECOND_PLAYER_POSITION_INDEX;
            setSecondPlayerKeys();
        }
        currentPositionIndex = startPositionIndex;
    }
}
