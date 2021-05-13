using System;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameManager gameManager;
    public PlayerNameManager playerNameManager;
    public GameObject matePlayer;
    public GameObject mainCamera;
    public GameObject scoreManager;
    public GameObject floorsManager;
    public string playerTag = "";
    
    private FloorGenerator floorGenerator;
    private Player matePlayerScript;
    private ScoreScript scoreScript;
    private Lane laneScript;
    
    private int lives;

    private int startLane;
    private int currentLane;

    private int leftKeyCode;
    private int rightKeyCode;
    private int upKeyCode;
    
    private bool onGround;
    private bool underMate;
    private bool dead; 
    
    private Rigidbody playerRigidBody;

    
    // Set player's init components
    private void setFloorGenerator()
    {
        floorGenerator = floorsManager.GetComponent<FloorGenerator>();
    }

    private void setScoreScript()
    {
        scoreScript = scoreManager.GetComponent<ScoreScript>();
    }

    private void setMatePlayerScript()
    {
        matePlayerScript = matePlayer.GetComponent<Player>();
    }

    private void setLaneScript()
    {
        laneScript = gameManager.lanesSeparator.GetComponent<Lane>();
    }

    private void setLives()
    {
        lives = Constants.NumberOfLivesPerPlayer;
    }

    
    // Set player's input keys 
    private void setPlayerInputKeys() {
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
    
    
    // Set & change player's position
    private int getStartLane() {
        return startLane; 
    }

    private void setMiddles()
    {
        laneScript.setLanesMiddles();
    }
    
    private void setPlayerPosition(Vector3 positionVector)
    {
        transform.position = positionVector;
    }
    
    private void setPlayerStartPosition() {
        startLane = playerTag == "FirstPlayer" ? Constants.FirstPlayerPositionIndex : Constants.SecondPlayerPositionIndex;
        currentLane = startLane;
        
        var startPosition = transform.position;
        
        var startPositionVector = new Vector3(laneScript.lanesMiddles[startLane],
            startPosition[Constants.Y],
            startPosition[Constants.Z]);
        setPlayerPosition(startPositionVector);
    }
    
    private void setCurrentLane(int lane) {
        currentLane = lane;
    }

    private void changeLane()
    {
        var moveCurrentPlayer = false;
        var moveMatePlayer = false;

        if (Input.GetKeyUp((KeyCode)leftKeyCode) &&
                            currentLane != 0 && 
                            !isInHole() &&
                            (currentLane - 1 != matePlayerScript.currentLane || 
                            (onGround && !matePlayerScript.onGround) || 
                            (!onGround && matePlayerScript.onGround) ||
                             matePlayerScript.isDead()))
        {
            if (underMate)
            {
                moveMatePlayer = true;
                matePlayerScript.currentLane -= 1;
            }

            currentLane -= 1;

            moveCurrentPlayer = true;
        }

        else if (Input.GetKeyUp((KeyCode)rightKeyCode) &&
                                 currentLane != 3 &&
                                 !isInHole() &&
                                 (currentLane + 1 != matePlayerScript.currentLane || 
                                 (onGround && !matePlayerScript.onGround) || 
                                 (!onGround && matePlayerScript.onGround) ||
                                  matePlayerScript.isDead()))
        {
            if (underMate)
            {
                moveMatePlayer = true;
                matePlayerScript.currentLane += 1;
            }

            currentLane += 1;

            moveCurrentPlayer = true;
        }

        if (moveCurrentPlayer)
        {
            var transformPosition = transform.position;
            var playerMoveDirectionVecotr = new Vector3(laneScript.lanesMiddles[currentLane],
                                                        transformPosition[Constants.Y],
                                                        transformPosition[Constants.Z]);
            setPlayerPosition(playerMoveDirectionVecotr);
        }

        if (moveMatePlayer)
        {
            var mateLane = matePlayerScript.currentLane;
            var mateTransformPosition = matePlayerScript.transform.position;
            
            var playerMoveDirectionVecotr = new Vector3(laneScript.lanesMiddles[mateLane],
                                                        mateTransformPosition[Constants.Y],
                                                        mateTransformPosition[Constants.Z]);
            matePlayerScript.setPlayerPosition(playerMoveDirectionVecotr);
        }
    }

    
    // Get flags values
    private bool isInHole()
    {
        if (transform.position.y < 0.75) 
        {
            return true;
        }
        else 
        {
            return false;
        }
    }
    
    private bool isUnderMate()
    {
        return underMate;
    }

    private bool isOnGround()
    {
        return onGround;
    }
    
    public bool isDead() {
        return dead;
    }


    // Player's interactions
    private void moveForward()
    {
        if (!isInHole() && !isDead())
        {
            var transformPosition = gameManager.moveVector * (gameManager.moveSpeed * Time.deltaTime);
            transform.Translate(transformPosition);
        }
    }

    private void jump()
    {
        if (isOnGround() && !isInHole())
        {
            if (Input.GetKeyDown((KeyCode)upKeyCode))
            {
                var jumpHigh = gameManager.jumpHigh;

                if(isUnderMate())
                {
                    jumpHigh *= 2.0f;
                }

                playerRigidBody.velocity = new Vector3(0f, jumpHigh, 0f);
                onGround = false;
            }
        }
    }

    
    // Check if player lost one live
    private bool lostLife()
    {
        var cameraZ = mainCamera.transform.position.z;
        var playerZ = transform.position.z;
        return (Constants.DistFromCamera - Math.Abs(cameraZ - playerZ)) > 2.0;
    }
    
    private void checkIfPlayerIsAlive()
    {
        if(lostLife() && lives != 0)
        {
            string hearthStr = playerTag + "_" + Convert.ToString(3 - lives);
            GameObject hearth = GameObject.FindGameObjectWithTag(hearthStr);
            hearth.SetActive(false);

            if (lives == 1)
            {
                dead = true;
                this.transform.position = new Vector3(-40f, 10f, -15f);
                if (matePlayerScript.isDead()) {
                    scoreScript.addScore(playerNameManager.getFirstPlayerName(), playerNameManager.getSecondPlayerName(), gameManager.getScore());
                    SceneManager.LoadScene(Constants.GAME_OVER_SCENE);
                }
            }

            else
            {    
                floorGenerator.resetStart();
                StartCoroutine(respawnPlayers());
            }
            lives--;
        }
    }


    private void fixedZAxis()
    {
        playerRigidBody.constraints = (RigidbodyConstraints) 122;
        matePlayerScript.playerRigidBody.constraints = (RigidbodyConstraints) 122;
    }

    private void ofFixedZAxis()
    {
        playerRigidBody.constraints = (RigidbodyConstraints) 114;
        matePlayerScript.playerRigidBody.constraints = (RigidbodyConstraints) 114;
    }

    private IEnumerator respawnPlayers()
    {
        gameManager.moveSpeed = 0;
        fixedZAxis();
        
        mainCamera.transform.position = new Vector3(Constants.CameraInitX, Constants.CameraInitY, Constants.CameraInitZ);
        
        Vector3 respawnPosition = new Vector3(laneScript.lanesMiddles[startLane],
                                                Constants.PlayersInitY,
                                                Constants.PlayersInitZ);
        setPlayerPosition(respawnPosition);
        setCurrentLane(startLane);
        
        if (!matePlayerScript.isDead())
        {
            int matePlayerStartLane = matePlayerScript.getStartLane();
            respawnPosition.x = laneScript.lanesMiddles[matePlayerStartLane];
            matePlayerScript.setPlayerPosition(respawnPosition);
            matePlayerScript.setCurrentLane(matePlayerStartLane);
        }

       
        yield return new WaitForSeconds(0.5f);
        
        gameManager.moveSpeed = 8;
        ofFixedZAxis();
    }

    
    
    // Player collision events
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("FirstPlayer") ||
            collision.gameObject.CompareTag("SecondPlayer"))
        {
            underMate = false;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if ((collision.gameObject.CompareTag("FirstPlayer") ||
             collision.gameObject.CompareTag("SecondPlayer")) &&
             transform.position.y < matePlayer.transform.position.y)
        {
            underMate = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((collision.gameObject.CompareTag("FirstPlayer") ||
             collision.gameObject.CompareTag("SecondPlayer")) &&
             transform.position.y < matePlayer.transform.position.y)
        {
            underMate = true;
        }

        if (collision.gameObject.CompareTag("Floor") ||
            collision.gameObject.CompareTag("FirstPlayer") ||
            collision.gameObject.CompareTag("SecondPlayer"))
        {
            if (underMate && collision.gameObject.CompareTag("Floor"))
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
        playerRigidBody = GetComponent<Rigidbody>();
        onGround = true;
        underMate = false;
        dead = false;
        setFloorGenerator();
        setMatePlayerScript();
        setScoreScript();
        setLaneScript();
        setLives();
        setPlayerInputKeys();
        setMiddles();
        setPlayerStartPosition();
        gameManager.clearScore();
    }

    void Update()
    {
        checkIfPlayerIsAlive();
        moveForward();
        changeLane();
        jump();
    }
}
