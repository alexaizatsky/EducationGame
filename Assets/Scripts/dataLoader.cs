using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public delegate void FinishUrlRequest(bool _result);
public class dataLoader : MonoBehaviour
{
    public event FinishUrlRequest OnUrlRequestFinish;
    
    private bool urlEditor;
    
    private string dataToJson;
    private string dataFromJson;
    
    private gameData readMyData, writeMyData;
    
    private string fileName = "myData.json";
    private string filePath;

    private string urlUploadLink;
    private string urlSaveLink;

    private gameManager myManager;
    
    public void Init(gameManager _manager)
    {
        myManager = _manager;
    }
    
    public void SetUrlLink(urlData _data, bool _urlEditor)
    {
        urlUploadLink = _data.inputUrlLink;
        urlSaveLink = _data.outputUrlLink;
        urlEditor = _urlEditor;
    }
    
    public void LoadData()
    {
       ;
        if (Application.isEditor && !urlEditor)
        {
            myManager.SendConsoleText("WORKING ON LOCAL JSON");
            filePath = Path.Combine(Application.dataPath, fileName);
            if (File.Exists(filePath))
            {
                ReadFromJsonFile();
            }
            else
            {
                writeMyData = new gameData();
                WriteToJsonFile(writeMyData);
                ReadFromJsonFile();
            }

            if (OnUrlRequestFinish != null) OnUrlRequestFinish(true);
        }
        else
        {
            myManager.SendConsoleText("WORKING ON URL JSON");
            ReadFromUrl();
 
        }

        
    }

    public void SaveData(int _out)
    {
        writeMyData = new gameData();
        writeMyData.inputValue = readMyData.inputValue;
        writeMyData.outputValue = _out;
        if (Application.isEditor)
        {
            WriteToJsonFile(writeMyData);
        }
        else
        {
            WriteTiUrl(writeMyData);
        }
    }

    void ReadFromJsonFile()
    {
        dataFromJson = File.ReadAllText(filePath);
        readMyData = JsonUtility.FromJson<gameData>(dataFromJson);
        myManager.SendConsoleText("READ LOCAL JSON COMPLETE input "+readMyData.inputValue);
    }
    
    void WriteToJsonFile(gameData _data)
    {
        writeMyData = _data;
        dataToJson = JsonUtility.ToJson(writeMyData);
        File.WriteAllText(filePath, dataToJson);
        myManager.SendConsoleText("WRITE LOCAL JSON COMPLETE output "+writeMyData.outputValue);
    }

    void ReadFromUrl()
    {
        StartCoroutine(GetDataFromUrl());
    }
    
    void WriteTiUrl(gameData _data)
    {
        StartCoroutine(SaveDataToUrl(_data));
    }

    IEnumerator GetDataFromUrl()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(urlUploadLink))
        {
            yield return request.SendWebRequest();
            if (request.isHttpError || request.isNetworkError)
            {
                Debug.Log(request.error);
                myManager.SendConsoleText("URL LOAD ERROR "+request.error);
                if (OnUrlRequestFinish != null) OnUrlRequestFinish(false);
            }
            else
            {
                var text = request.downloadHandler.text;
                readMyData = JsonUtility.FromJson<gameData>(text);
                myManager.SendConsoleText("URL LOAD COMPLETE input "+readMyData.inputValue);
                if (OnUrlRequestFinish != null) OnUrlRequestFinish(true);
            }
        }
    }
  
    IEnumerator SaveDataToUrl(gameData _data)
    {
        string textData = JsonUtility.ToJson(_data);

        using (UnityWebRequest www = UnityWebRequest.Post(urlSaveLink, textData))
        {
            yield return www.SendWebRequest();

            if (www.isHttpError || www.isNetworkError)
            {
                myManager.SendConsoleText("URL SAVE ERROR "+www.error);
                Debug.Log(www.error);
                if (OnUrlRequestFinish != null) OnUrlRequestFinish(false);
            }
            else
            {
                myManager.SendConsoleText("URL SAVE COMPLETE output "+writeMyData.outputValue);
                if (OnUrlRequestFinish != null) OnUrlRequestFinish(true);
            }
        }
    }
    
    
    public gameData GetMyData()
    {
        return readMyData;
    }


}
