using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ResistanceDropdown : MonoBehaviour
{
    GameObject storageManager;
    StorageManager storageManagerScript;

    Dropdown resistanceDropdown;

    void Start()
    {
        storageManager = GameObject.FindGameObjectWithTag("StorageManager");
        storageManagerScript = storageManager.GetComponent<StorageManager>();

        resistanceDropdown = GetComponent<Dropdown>();
        setOptions();
    }

    //Lees de geselecteerde waarde van de dropdown
    public int GetSelectedOption()
    {
        int selectedOption;
        resistanceDropdown = GetComponent<Dropdown>();
        selectedOption = int.Parse(resistanceDropdown.options[resistanceDropdown.value].text); //Lees de text van de DropdownOptie die geselecteerd is (resistanceDropdown.value = nummer van geselecteerde dropdown)
        return selectedOption;
    }

    //Zet 1-5 in opties van de dropdown
    public void setOptions() 
    {
        List<Dropdown.OptionData> dropdownDataList = new List<Dropdown.OptionData>();
        int i = 1;
        while(i <= 5)
        {
            Dropdown.OptionData dropdownOptionData = new Dropdown.OptionData();
            dropdownOptionData.text = i.ToString();
            dropdownDataList.Add(dropdownOptionData);
            i += 1;
        }
        resistanceDropdown.ClearOptions();
        resistanceDropdown.options.AddRange(dropdownDataList);
        resistanceDropdown.value = 0;
    }

    //Wanneer de AccountDropdown die hierbij hoort wordt verandert wordt deze functie getriggerd
    public void SetStoredOption(string accountName)
    {
        //Pas de geselecteerde weerstand aan naar de geprefereerde weerstand van de geselecteerde speler
        int storedResistance = (int)storageManagerScript.GetAccountData(accountName)[1];
        resistanceDropdown.value = storedResistance - 1;
    }
}
