﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lane : MonoBehaviour
{
    public float[] lanesMiddles = new float[4];
    
    public void setLanesMiddles()
    {
        float laneDimension = 18 / 4;
        /*float laneDimension = transform.localScale[Constants.Z] / 4;*/

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
