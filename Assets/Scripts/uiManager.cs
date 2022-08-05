using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void PressFinishButton(int _out);
public class uiManager : MonoBehaviour
{
    [SerializeField] private Text inputText;
    [SerializeField] private InputField inputField;

    public event PressFinishButton OnFinishButton;
    
    public void Init(gameData _data)
    {
        inputText.text = _data.inputValue.ToString();

    }

    public void PressFinish()
    {
        int outValue = 0;
        
        if (inputField.text!=null)
            outValue = int.Parse(inputField.text);
        
        if (OnFinishButton != null)
            OnFinishButton(outValue);
    }
}
