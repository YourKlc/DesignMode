using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script_od : SerializedMonoBehaviour
{
    [TableMatrix]
    public int[,] mat;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < mat.GetLength(1); i++)
        {
            string s = "";
            for (int j = 0; j < mat.GetLength(0); j++)
            {
                s += " " + mat[j, i];
            }

            Debug.Log(s);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
