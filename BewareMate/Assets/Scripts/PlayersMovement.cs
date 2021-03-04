using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersMovement: MonoBehaviour
{
    GameManager gameManager;
    Player firstPlayer;
    Player secondPlayer;

    void Start()
    {
        GameObject gameController = GameObject.FindGameObjectWithTag("GameController");     
        gameManager = gameController.GetComponent<GameManager>();

        firstPlayer = new Player(gameManager, "FirstPlayer");
        secondPlayer = new Player(gameManager, "SecondPlayer");

        firstPlayer.addMatePlayer(secondPlayer);
        secondPlayer.addMatePlayer(firstPlayer);
    }


    void Update()
    {
        firstPlayer.moveForward(gameManager);
        secondPlayer.moveForward(gameManager);

        firstPlayer.changeLane();
        secondPlayer.changeLane();

    }
}
