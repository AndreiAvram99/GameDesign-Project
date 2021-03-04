using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    GameManager gameManager;
    
    void Start()
    {
        GameObject gameController = GameObject.FindGameObjectWithTag("GameController");
        gameManager = gameController.GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(gameManager.moveVector * gameManager.moveSpeed * Time.deltaTime);
    }
}
