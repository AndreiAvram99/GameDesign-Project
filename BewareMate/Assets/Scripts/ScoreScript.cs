using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreScript : MonoBehaviour
{

    public struct scoreData
    {
        public string playerName1;
        public string playerName2;
        public int score;
    };

    private scoreData[] scores;
    private int count = 0;
    private const string PATH_TO_DATA = "data/data.txt";

    // Start is called before the first frame update
    void Start()
    {
        scores = new scoreData[15];
        string[] lines = System.IO.File.ReadAllLines(PATH_TO_DATA);

        foreach(string line in lines)
        {
            parseAndSave(line);
        }

        if(count != 10)
        {
            Debug.LogError("Ceva nu e ok. Verifica fisierul " + PATH_TO_DATA);
        }
    }

    private void parseAndSave(string line)
    {
        scoreData data;
        data.playerName1 = "";

        int pos = 0;

        while(line[pos] != ',')
        {
            data.playerName1 += line[pos];
            pos++;
        }

        pos++;

        data.playerName2 = "";

        while(line[pos] != ',')
        {
            data.playerName2 += line[pos];
            pos++;
        }

        pos++;

        data.score = 0;

        while(pos < line.Length)
        {
            data.score = data.score * 10 + line[pos] - '0';
            pos++;
        }

        scores[count] = data;
        count++;
    }

    public void addScore(string playerName1, string playerName2, int score)
    {
        for(int i = 0; i < 10; i++)
        {
            if(scores[i].score <= score)
            {
                for(int j = 9; j > i; --j)
                {
                    scores[j] = scores[j - 1];
                }

                scores[i].playerName1 = playerName1;
                scores[i].playerName2 = playerName2;
                scores[i].score = score;
                break;
            }
        }

        string[] dataString = convertScoresToString();

        System.IO.File.WriteAllLines(PATH_TO_DATA, dataString);
    }

    private string[] convertScoresToString()
    {
        string[] scoresString = new string[10];

        int cnt = 0;
        foreach(scoreData data in scores)
        {
            if (cnt > 9) break;
            scoresString[cnt] = data.playerName1 + "," + data.playerName2 + "," + data.score;
            cnt++;
        }

        return scoresString;
    }

}
