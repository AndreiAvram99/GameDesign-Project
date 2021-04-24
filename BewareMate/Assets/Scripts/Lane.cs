﻿using UnityEngine;

public class Lane : MonoBehaviour
{
    public float[] lanesMiddles = new float[4];


    public void generateTraps() {
        
    }

    public void setLanesMiddles()
    {
        float laneDimension = Constants.FloorWidth / Constants.NumberOfLanes;
   
        for (int i = 0; i < lanesMiddles.Length; i++)
        {
            if (i < 2)
            {
                lanesMiddles[i] = 0 - (1 - i) * laneDimension - laneDimension / 2;
            }
            else
            {
                lanesMiddles[i] = 0 + (i - 2) * laneDimension + laneDimension / 2;
            }

        }
    }
}
