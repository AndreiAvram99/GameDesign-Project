using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float moveSpeed = 8.0f;
    public Vector3 moveVector;
    public float jumpHigh = 10f;
    public GameObject lanesSeparator;

    private static int _score;
    private TextMeshProUGUI scoreText;

    public void Start()
    {
        scoreText = GameObject.FindGameObjectWithTag("Score").GetComponent<TextMeshProUGUI>();
        StartCoroutine(scoreCoroutine());
        StartCoroutine(speedCoroutine());
    }
    
    
    
    IEnumerator scoreCoroutine()
    {
        while (true)
        {
            _score += 1;
            scoreText.text = "Score\n" + _score;
            yield return new WaitForSeconds(1);
        }
        // ReSharper disable once IteratorNeverReturns
    }

    IEnumerator speedCoroutine()
    {
        while(true)
        {
            moveSpeed = Math.Min(Constants.MaxSpeed, moveSpeed + 1.5f);
            yield return new WaitForSeconds(5);
        }
        // ReSharper disable once IteratorNeverReturns
    }

    public int getScore()
    {
        return _score;
    }
}
