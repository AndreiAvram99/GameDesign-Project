using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public float moveSpeed = 8.0f;
    public Vector3 moveVector;
    public float jumpHigh = 10f;
    public GameObject floor;

    public static Text textScore;
    public static int score = 0;

    public void Start()
    {
        // textScore = GameObject.Find("Score").GetComponent<Text>();
        StartCoroutine(scoreCoroutine());
        StartCoroutine(speedCoroutine());
    }
    
    
    
    IEnumerator scoreCoroutine()
    {
        while (true)
        {
            score += 1;
            GameObject.FindGameObjectWithTag("Score").GetComponent<TextMeshProUGUI>().text = "Score\n" + score;
            yield return new WaitForSeconds(1);
        }
    }

    IEnumerator speedCoroutine()
    {
        while(true)
        {
           // moveSpeed = Math.Min(Constants.MAX_SPEED, moveSpeed + 1.5f);
            Debug.Log(moveSpeed + "LOL VITEZA");
            yield return new WaitForSeconds(5);
        }
    }
}
