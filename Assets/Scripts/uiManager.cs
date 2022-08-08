using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void PressGetDataButton(urlData _data);
public delegate void PressSaveDataButton(urlData _data, int _out);
public class uiManager : MonoBehaviour
{
    [Header("----------------- QUEST -------------------")]
    [SerializeField] private Text inputText;
    [SerializeField] private InputField inputField;

    [Header("----------------- URL -------------------")]
    [SerializeField] private InputField inputUrlField;
    [SerializeField] private InputField outputUrlField;
    [SerializeField] private Toggle urlEditorToggle;

    [Header("----------------- CONSOLE -------------------")] 
    [SerializeField] private Text consoleText;

    private gameManager myManager;

    public event PressGetDataButton OnGetData;
    public event PressSaveDataButton OnSaveData;
    
    List<string> consoleList = new List<string>();
    
    public void Init(gameManager _man, urlData _defaultData)
    {
        myManager = _man;
        inputText.text = "";
        consoleText.text = " fill URL fields, press Get, solve quest, press Send";
        inputUrlField.text = _defaultData.inputUrlLink;
        outputUrlField.text = _defaultData.outputUrlLink;
        myManager.OnConsoleTextSend += GetConsoleText;
    }

    public void SetInputValue(int _input)
    {
        inputText.text = _input.ToString();
    }

    public bool GetUrlEditorToggle()
    {
        return urlEditorToggle.isOn;
    }
    
    public void PressGetData()
    {
        if (OnGetData!=null)
        {
            urlData ud = new urlData();
            ud.inputUrlLink = inputUrlField.text;
            ud.outputUrlLink = outputUrlField.text;
            OnGetData(ud);
        }
    }

    public void PressSaveData()
    {
        if (OnSaveData!=null)
        {
            int outValue = 0;
        
            if (inputField.text!=null)
                outValue = int.Parse(inputField.text);
            
            urlData ud = new urlData();
            ud.inputUrlLink = inputUrlField.text;
            ud.outputUrlLink = outputUrlField.text;
            OnSaveData(ud, outValue);
        }
    }

    public void GetConsoleText(string _text)
    {
        if (consoleList.Count < 5)
        {
            consoleList.Add(_text);
        }
        else
        {
            consoleList.RemoveAt(0);
            consoleList.Add(_text);
        }

        string tex = "";
        for (int i = 0; i < consoleList.Count; i++)
        {
            tex += "\n" + consoleList[i];
        }

        consoleText.text = tex;
    }
}
