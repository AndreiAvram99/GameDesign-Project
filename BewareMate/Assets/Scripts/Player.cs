using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    GameManager gameManager;

    public string playerTag = "";
    public bool onMate = false;

    public static bool isMateColission;

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

    public void setPlayerPosition(Vector3 positionVector)
    {
        transform.position = positionVector;
    }

    public void setOnMate(bool onMate)
    {
        this.onMate = onMate;
    }

    public bool getOnMate()
    {
        return onMate;
    }

    public void moveForward()
    {
        Vector3 transformPosition = gameManager.moveVector * gameManager.moveSpeed * Time.deltaTime;
        transform.Translate(transformPosition);
    }

    public bool isMateAboveMe()
    {
        return transform.position.y < matePlayer.transform.position.y && 
            currentLane == matePlayer.GetComponent<Player>().currentLane;
    }

    public int getCurrentLane()
    {
        return currentLane;
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
            if (isMateAboveMe())
            {
                Debug.Log($"{playerTag} e dedesubt");
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
            if (isMateAboveMe())
            {
                Debug.Log($"{playerTag} e dedesubt");
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
                playerRigidBody.velocity = new Vector3(0f, 10f, 0f);
                onGround = false;
            }
        }
    }
    private void checkIfAboveHole()
    {
        if (transform.position.y < 0.45)
        {
            onGround = false;
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

    public bool isCollisionWithMate(Collision collision)
    {
        return  isMateColission =   collision.gameObject.tag == "FirstPlayer" ||
                                    collision.gameObject.tag == "SecondPlayer";
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            onGround = true;
        }

        //isCollisionWithMate(collision);
    }

    void Start()
    {
        GameObject gameController = GameObject.FindGameObjectWithTag("GameController");
        gameManager = gameController.GetComponent<GameManager>();
        initPlayer();
    }

    void Update()
    {
        // checkIfOneIsBehind();
        checkIfAboveHole();
        moveForward();
        changeLane();
        jump();
    }
}
