using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RankingManager : MonoBehaviour
{
    public struct scoreData
    {
        public string playerName1;
        public string playerName2;
        public int score;
    };

    public TextMeshProUGUI rankingTextField;

    private scoreData[] scores;
    private int count = 0;
    private const string PATH_TO_DATA = "data/data.txt";

    // Start is called before the first frame update
    void Start()
    {
        scores = new scoreData[15];
        string[] lines = System.IO.File.ReadAllLines(PATH_TO_DATA);

        foreach (string line in lines)
        {
            parseAndSave(line);
        }

        if (count != 10)
        {
            Debug.LogError("Ceva nu e ok. Verifica fisierul " + PATH_TO_DATA);
        }

        putIntoScene();
    }

    private void putIntoScene()
    {
        string rankingString = "";

        for(int i = 0; i < 10; i++)
        {
            rankingString += scores[i].playerName1;
            rankingString += " & ";
            rankingString += scores[i].playerName2;
            rankingString += " - ";
            rankingString += scores[i].score;
            rankingString += '\n';
        }

        rankingTextField.text = rankingString;
    }

    private void parseAndSave(string line)
    {
        scoreData data;
        data.playerName1 = "";

        int pos = 0;

        while (line[pos] != ',')
        {
            data.playerName1 += line[pos];
            pos++;
        }

        pos++;

        data.playerName2 = "";

        while (line[pos] != ',')
        {
            data.playerName2 += line[pos];
            pos++;
        }

        pos++;

        data.score = 0;

        while (pos < line.Length)
        {
            data.score = data.score * 10 + line[pos] - '0';
            pos++;
        }

        scores[count] = data;
        count++;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
