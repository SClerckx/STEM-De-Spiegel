using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class WeightInputField : MonoBehaviour
{
    StorageManager storageManagerScript;

    InputField weightInputField;
   
    void Start()
    {
        storageManagerScript = FindObjectOfType<StorageManager>();
        weightInputField = GetComponent<InputField>();
    }

    //Lees de geselecteerde waarde van de dropdown
    public float GetSelectedOption()
    {
        float selectedOption = float.Parse(weightInputField.text);
        return selectedOption;
    }

    //Wanneer de AccountDropdown die hierbij hoort wordt verandert wordt deze functie getriggerd
    public void SetStoredOption(string accountName)
    {
        //Pas het getoonde gewicht aan naar het gewicht van de geselecteerde speler
        float storedWeight = (int)storageManagerScript.GetAccountData(accountName)[0];
        weightInputField.text = storedWeight.ToString();
    }
}
