using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Player : MonoBehaviour
{
    GameManager gameManager;

    public string playerTag = "";

    private int lives;
    private int leftKeyCode;
    private int rightKeyCode;
    private int upKeyCode;

    private GameObject matePlayer;

    private int startLane;
    private int currentLane;

    private bool onGround;
    private bool underMate;
    private bool isDead = false; 
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
        if (playerTag == "FirstPlayer") startLane = Constants.FirstPlayerPositionIndex;
        else startLane = Constants.SecondPlayerPositionIndex;
        currentLane = startLane;
    }

    private void setPLayerInputKeys() {
        if (playerTag == "FirstPlayer") setFirstPlayerKeys();
        else setSecondPlayerKeys();
    }

    private void setFirstPlayerKeys()
    {
        leftKeyCode = Constants.AKeycode;
        rightKeyCode = Constants.DKeycode;
        upKeyCode = Constants.WKeycode;
    }

    private void setSecondPlayerKeys()
    {
        leftKeyCode = Constants.LeftArrowKeycode;
        rightKeyCode = Constants.RightArrowKeycode;
        upKeyCode = Constants.UpArrowKeycode;
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

    public bool isInHole()
    {
        return transform.position.y < 0.75;
    }

    public bool getIsDead() {
        return isDead;
    }

    public void moveForward()
    {
        if (!isInHole() && !isDead)
        {
            Vector3 transformPosition = gameManager.moveVector * (gameManager.moveSpeed * Time.deltaTime);
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
                            !isInHole() &&
                            (currentLane - 1 != matePlayer.GetComponent<Player>().currentLane || 
                            (onGround && !matePlayer.GetComponent<Player>().onGround) || 
                            (!onGround && matePlayer.GetComponent<Player>().onGround) ||
                             matePlayer.GetComponent<Player>().getIsDead()))
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
                                 !isInHole() &&
                                 (currentLane + 1 != matePlayer.GetComponent<Player>().currentLane || 
                                 (onGround && !matePlayer.GetComponent<Player>().onGround) || 
                                 (!onGround && matePlayer.GetComponent<Player>().onGround) ||
                                  matePlayer.GetComponent<Player>().getIsDead()))
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
                float jumpHigh = gameManager.jumpHigh;

                if(underMate)
                {
                    jumpHigh *= 1.5f;
                }

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

    private bool isLostLife()
    {
        float cameraZ = GameObject.FindGameObjectWithTag("MainCamera").transform.position.z;
        float playerZ = transform.position.z;
        return (Constants.DistFromCamera - Math.Abs(cameraZ - playerZ)) > 2.0;
    }

    IEnumerator respawnPlayers()
    {

        GameObject.FindGameObjectWithTag("MainCamera").transform.position = new Vector3(Constants.CameraInitX,
                                                                                           Constants.CameraInitY,
                                                                                           Constants.CameraInitZ);

        Vector3 respawnPosition = new Vector3(gameManager.floor.GetComponent<Lane>().lanesMiddles[startLane],
                                              Constants.PlayersInitY,
                                              Constants.PlayersInitZ);
        setPlayerPosition(respawnPosition);
        setCurrentLane(startLane);

        if (!matePlayer.GetComponent<Player>().getIsDead())
        {
            int matePlayerStartLane = matePlayer.GetComponent<Player>().getStartLane();
            respawnPosition.x = gameManager.floor.GetComponent<Lane>().lanesMiddles[matePlayerStartLane];
            matePlayer.GetComponent<Player>().setPlayerPosition(respawnPosition);
            matePlayer.GetComponent<Player>().setCurrentLane(matePlayerStartLane);
        }
        
        yield return new WaitForSecondsRealtime(3);
    }

    private void checkIfLostLife()
    {
        if(isLostLife() && lives != 0)
        {

            string hearthStr = playerTag + "_" + Convert.ToString(3 - lives);
            Debug.Log(hearthStr);
            GameObject hearth = GameObject.FindGameObjectWithTag(hearthStr);
            hearth.SetActive(false);

            if (lives == 1)
            {
                Debug.Log("You lost" + playerTag);
                isDead = true;
                this.transform.position = new Vector3(-40f, 10f, -15f);
                if (matePlayer.GetComponent<Player>().getIsDead()) {
                    SceneManager.LoadScene(2);
                }
            }

            else
            {    
                Debug.Log("You lost one life" + playerTag);
                GameObject FloorsManager = GameObject.FindGameObjectWithTag("FloorsManager");
                FloorsManager.GetComponent<FloorGenerator>().resetStart();
                StartCoroutine(respawnPlayers());
            }

            lives--;
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
        moveForward();
        checkIfAboveHole();
        checkIfLostLife();
        changeLane();
        jump();
    }
}
