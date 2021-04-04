using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public float moveSpeed = 1.0f;
    public Vector3 moveVector;
    public GameObject floor;

    public static Text textScore;
    public static int score = 0;

    public void Start()
    {
        // textScore = GameObject.Find("Score").GetComponent<Text>();
        StartCoroutine(time());
    }
    IEnumerator time()
    {
        while (true)
        {
            timeCount();
            // DE SETAT score la acel text "Score: [score]"
            // Debug.Log(textScore);
            yield return new WaitForSeconds(1);
        }
    }
    void timeCount()
    {
        score += 1;
    }
}
