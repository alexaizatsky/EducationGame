using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class dataLoader : MonoBehaviour
{
    private string dataToJson;
    private string dataFromJson;
    
    private gameData readMyData, writeMyData;
    
    private string fileName = "myData.json";
    private string filePath;

    [SerializeField] private string urlUploadLink = "https://www.spivak.com/inputData.json";
    [SerializeField] private string urlSaveLink = "https://www.spivak.com/outputData.json";
    public void LoadData()
    {
        if (Application.isEditor)
        {
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
        }
        else
        {
            ReadFromUrl();
            if (readMyData ==null)
            {
                writeMyData = new gameData();
                WriteTiUrl(writeMyData);
                ReadFromUrl();
            }
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
    }
    
    void WriteToJsonFile(gameData _data)
    {
        writeMyData = _data;
        dataToJson = JsonUtility.ToJson(writeMyData);
        File.WriteAllText(filePath, dataToJson);
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
            }
            else
            {
                var text = request.downloadHandler.text;
                readMyData = JsonUtility.FromJson<gameData>(text);
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
                Debug.Log(www.error);
            }
        }
    }
    
    
    public gameData GetMyData()
    {
        return readMyData;
    }
}
