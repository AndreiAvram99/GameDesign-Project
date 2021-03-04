using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    GameManager gameManager;

    public string playerTag = "";

    private int leftKeyCode;
    private int rightKeyCode;
    private int upKeyCode;

    private GameObject matePlayer;

    private int startLane;
    private int currentLane;

    private bool onGround;
    private Rigidbody playerRigidBody;


    private void initPlayer()
    {
        onGround = true;
        playerRigidBody = GetComponent<Rigidbody>();
        setPlayerStartLane();
        setPlayerStartPosition();
        setPLayerInputKeys();
        setMatePlayer();

        
    }

    private void setPlayerStartPosition() {
        gameManager.floor.GetComponent<Lane>().setLanesMiddles();

        Vector3 startPositionVector = new Vector3(gameManager.floor.GetComponent<Lane>().lanesMiddles[startLane],
                                                  transform.position[Constants.Y],
                                                  transform.position[Constants.Z]);
        setPlayerPosition(startPositionVector);
    }

    private void setPlayerStartLane()
    {
        if (playerTag == "FirstPlayer") startLane = Constants.FIRST_PLAYER_POSITION_INDEX;
        else startLane = Constants.SECOND_PLAYER_POSITION_INDEX;
        currentLane = startLane;
    }

    private void setPLayerInputKeys() {
        if (playerTag == "FirstPlayer") setFirstPlayerKeys();
        else setSecondPlayerKeys();
    }

    private void setFirstPlayerKeys()
    {
        leftKeyCode = Constants.A_KEYCODE;
        rightKeyCode = Constants.D_KEYCODE;
        upKeyCode = Constants.W_KEYCODE;
    }

    private void setSecondPlayerKeys()
    {
        leftKeyCode = Constants.LEFT_ARROW_KEYCODE;
        rightKeyCode = Constants.RIGHT_ARROW_KEYCODE;
        upKeyCode = Constants.UP_ARROW_KEYCODE;
    }

    private void setMatePlayer() {
        if (playerTag == "FirstPlayer") 
            matePlayer = GameObject.FindGameObjectWithTag("SecondPlayer");
        else
            matePlayer = GameObject.FindGameObjectWithTag("FirstPlayer");
    }

    private void setPlayerPosition(Vector3 positionVector)
    {
        transform.position = positionVector;
    }

    public void moveForward()
    {
        Vector3 transformPosition = gameManager.moveVector * gameManager.moveSpeed * Time.deltaTime;
        transform.Translate(transformPosition);
    }

    public void changeLane()
    {
        bool moveFlag = false;

        if (Input.GetKeyUp((KeyCode)leftKeyCode) &&
                            currentLane != 0 && 
                            (currentLane - 1 != matePlayer.GetComponent<Player>().currentLane || 
                            (onGround && !matePlayer.GetComponent<Player>().onGround) || 
                            (!onGround && matePlayer.GetComponent<Player>().onGround)))
        {
            currentLane -= 1;
            moveFlag = true;
        }

        else if (Input.GetKeyUp((KeyCode)rightKeyCode) &&
                                 currentLane != 3 &&
                                 (currentLane + 1 != matePlayer.GetComponent<Player>().currentLane || 
                                 (onGround && !matePlayer.GetComponent<Player>().onGround) || 
                                 (!onGround && matePlayer.GetComponent<Player>().onGround)))
        {
            currentLane += 1;
            moveFlag = true;
        }

        if (moveFlag)
        {
            Vector3 playerMoveDirectionVecotr = new Vector3(gameManager.floor.GetComponent<Lane>().lanesMiddles[currentLane],
                                                            transform.position[Constants.Y],
                                                            transform.position[Constants.Z]);
            setPlayerPosition(playerMoveDirectionVecotr);
        }
    }

    public void jump()
    {
        if (onGround)
        {
            if (Input.GetKeyDown((KeyCode)upKeyCode))
            {
                playerRigidBody.velocity = new Vector3(0f, 8f, 0f);
                onGround = false;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor" ||
            collision.gameObject.tag == "FirstPlayer" ||
            collision.gameObject.tag == "SecondPlayer")
        {
            onGround = true;
        }
    }

    void Start()
    {
        GameObject gameController = GameObject.FindGameObjectWithTag("GameController");
        gameManager = gameController.GetComponent<GameManager>();
        initPlayer();
    }

    void Update()
    {
        moveForward();
        changeLane();
        jump();
    }
}
