using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager : MonoBehaviour
{
    [SerializeField] private uiManager _uiManager;
    [SerializeField] private dataLoader _dataLoader;

    private gameData myData;
    private void Awake()
    {
        _dataLoader.LoadData();
        myData = _dataLoader.GetMyData();
        _uiManager.Init(myData);
        _uiManager.OnFinishButton += PressFinish;
    }

    void PressFinish(int _out)
    {
        _uiManager.OnFinishButton -= PressFinish;
        _dataLoader.SaveData(_out);
    }
}
