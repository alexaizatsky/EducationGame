using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameData
{
    public int inputValue;
    public int outputValue;

    public gameData()
    {
        inputValue = 5;
        outputValue = 0;
    }
}

[System.Serializable]
public class urlData
{
    public string inputUrlLink;
    public string outputUrlLink;
}
