using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public delegate void SendDefaultURLs(string _in, string _out);
public delegate void SendConsoleText(string _text);

public class gameManager : MonoBehaviour
{
    [SerializeField] private urlData defaultUrlData;
    [SerializeField] private uiManager _uiManager;
    [SerializeField] private dataLoader _dataLoader;

    private gameData myData;

    public event SendConsoleText OnConsoleTextSend;
    private void Awake()
    {
        _dataLoader.Init(this);
        _uiManager.Init(this, defaultUrlData);
        _uiManager.OnGetData += PressGetData;
        _uiManager.OnSaveData += PressSendData;
    }
    
    void PressGetData(urlData _data)
    {
        _dataLoader.SetUrlLink(_data, _uiManager.GetUrlEditorToggle());
        _dataLoader.OnUrlRequestFinish += FinishUrlRequest;
        _dataLoader.LoadData();

    }

    void FinishUrlRequest(bool _result)
    {
        _dataLoader.OnUrlRequestFinish -= FinishUrlRequest;
        if (_result)
        {
            SendConsoleText("REQUEST COMPLETE");  
            _uiManager.SetInputValue(_dataLoader.GetMyData().inputValue);
        }
        else
        {
            SendConsoleText("REQUEST ERROR");
        }
    }
    
    
    void PressSendData(urlData _data, int _out)
    {
        _dataLoader.SetUrlLink(_data, _uiManager.GetUrlEditorToggle());
        _dataLoader.SaveData(_out);
        _dataLoader.OnUrlRequestFinish += FinishUrlRequest;
    }

    public void SendConsoleText(string _text)
    {
        if (OnConsoleTextSend != null) OnConsoleTextSend(_text);
    }
}
