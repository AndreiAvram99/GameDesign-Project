using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    GameManager gameManager;

    public string playerTag = "";
    public float jumpHigh = 10f;

    private int golaneala = 0;
    private int lives;
    private int leftKeyCode;
    private int rightKeyCode;
    private int upKeyCode;

    private GameObject matePlayer;

    private int startLane;
    private int currentLane;
    private double DIST_FROM_CAMERA = 11.5;

    private bool onGround;
    private bool underMate;
    private Rigidbody playerRigidBody;


    private void initPlayer()
    {
        onGround = true;
        playerRigidBody = GetComponent<Rigidbody>();
        setPlayerStartLane();
        setPlayerStartPosition();
        setPLayerInputKeys();
        setMatePlayer();
        lives = 3;
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

    public void setPlayerPosition(Vector3 positionVector)
    {
        transform.position = positionVector;
    }

    public bool IsInHole()
    {
        return transform.position.y < 0.0;
    }

    public void moveForward()
    {
        if (!IsInHole())
        {
            Vector3 transformPosition = gameManager.moveVector * gameManager.moveSpeed * Time.deltaTime;
            transform.Translate(transformPosition);
        }
    }

    public int getCurrentLane()
    {
        return currentLane;
    }

    public void setCurrentLane(int lane) {
        currentLane = lane;
    }

    public void changeLane()
    {
        bool moveFlag = false;
        bool moveMate = false;

        if (Input.GetKeyUp((KeyCode)leftKeyCode) &&
                            currentLane != 0 && 
                            (currentLane - 1 != matePlayer.GetComponent<Player>().currentLane || 
                            (onGround && !matePlayer.GetComponent<Player>().onGround) || 
                            (!onGround && matePlayer.GetComponent<Player>().onGround)))
        {
            if (underMate)
            {
                moveMate = true;
                matePlayer.GetComponent<Player>().currentLane -= 1;
            }

            currentLane -= 1;

            moveFlag = true;
        }

        else if (Input.GetKeyUp((KeyCode)rightKeyCode) &&
                                 currentLane != 3 &&
                                 (currentLane + 1 != matePlayer.GetComponent<Player>().currentLane || 
                                 (onGround && !matePlayer.GetComponent<Player>().onGround) || 
                                 (!onGround && matePlayer.GetComponent<Player>().onGround)))
        {
            if (underMate)
            {
                moveMate = true;
                matePlayer.GetComponent<Player>().currentLane += 1;
            }

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

        if(moveMate)
        {
            int mateLane = matePlayer.GetComponent<Player>().currentLane;

            Vector3 playerMoveDirectionVecotr = new Vector3(gameManager.floor.GetComponent<Lane>().lanesMiddles[mateLane],
                                                            matePlayer.GetComponent<Player>().transform.position[Constants.Y],
                                                            matePlayer.GetComponent<Player>().transform.position[Constants.Z]);
            matePlayer.GetComponent<Player>().setPlayerPosition(playerMoveDirectionVecotr);
        }
    }

    public void jump()
    {
        if (onGround)
        {
            if (Input.GetKeyDown((KeyCode)upKeyCode))
            {
                playerRigidBody.velocity = new Vector3(0f, jumpHigh, 0f);
                onGround = false;
            }
        }
    }

    public int getStartLane() {
        return startLane; 
    }

    private void checkIfAboveHole()
    {
        if (transform.position.y < 0.45)
        {
            onGround = false;
        }
    }

    private bool IsLostLife()
    {
        float cameraZ = GameObject.FindGameObjectWithTag("MainCamera").transform.position.z;
        float playerZ = transform.position.z;
        if (golaneala < 1)
        {
            golaneala += 1;
            Debug.Log(playerTag + ":" + (DIST_FROM_CAMERA - Math.Abs(cameraZ - playerZ)));
            Debug.Log(playerTag + ":" + (DIST_FROM_CAMERA - Math.Abs(cameraZ - playerZ) > 2.0));
        
        }
        return (DIST_FROM_CAMERA - Math.Abs(cameraZ - playerZ)) > 2.0;
    }

    private void respawnPlayers()
    {
        Vector3 respawnPosition = new Vector3(gameManager.floor.GetComponent<Lane>().lanesMiddles[startLane],
                                              Constants.PLAYERS_INIT_Y,
                                              Constants.PLAYERS_INIT_Z);
        setPlayerPosition(respawnPosition);
        setCurrentLane(startLane);


        int matePlayerStartLane = matePlayer.GetComponent<Player>().getStartLane();
        respawnPosition.x = matePlayerStartLane;
        matePlayer.GetComponent<Player>().setPlayerPosition(respawnPosition);
        matePlayer.GetComponent<Player>().setCurrentLane(matePlayerStartLane);

        GameObject.FindGameObjectWithTag("MainCamera").transform.position = new Vector3(Constants.CAMERA_INIT_X,
                                                                                        Constants.CAMERA_INIT_Y,
                                                                                        Constants.CAMERA_INIT_Z);

    }

    private void checkIfLostLife()
    {
        if(IsLostLife())
        {
            if(lives == 1)
            {
                Debug.Log("Ai pierdut fraiere " + playerTag);
            }
            else
            {
                Debug.Log("Ai pierdut o viata fraiere " + playerTag);
                lives--;
                respawnPlayers();
            }
        }
    }

    private void checkIfOneIsBehind()
    {
        if(transform.position.y != matePlayer.GetComponent<Player>().transform.position.y)
        {
            float maxZ = Math.Max(transform.position.z, matePlayer.GetComponent<Player>().transform.position.z);
            transform.position = moveToZ(maxZ);
            matePlayer.GetComponent<Player>().transform.position = matePlayer.GetComponent<Player>().moveToZ(maxZ);
        }
    }

    public Vector3 moveToZ(float Z)
    {
        return new Vector3(transform.position.x, transform.position.y, Z);

    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "FirstPlayer" ||
            collision.gameObject.tag == "SecondPlayer")
        {
            underMate = false;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if ((collision.gameObject.tag == "FirstPlayer" ||
             collision.gameObject.tag == "SecondPlayer") &&
             transform.position.y < matePlayer.transform.position.y)
        {
            underMate = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((collision.gameObject.tag == "FirstPlayer" ||
             collision.gameObject.tag == "SecondPlayer") &&
             transform.position.y < matePlayer.transform.position.y)
        {
            underMate = true;
        }

        if (collision.gameObject.tag == "Floor" ||
            collision.gameObject.tag == "FirstPlayer" ||
            collision.gameObject.tag == "SecondPlayer")
        {
            if (underMate && collision.gameObject.tag == "Floor")
            {
                onGround = true;
            }
            else if(!underMate)
            {
                onGround = true;
            }
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
        checkIfAboveHole();
        checkIfLostLife();
        moveForward();
        changeLane();
        jump();
    }
}
