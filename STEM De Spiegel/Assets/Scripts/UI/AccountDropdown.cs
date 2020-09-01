using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AccountDropdown : MonoBehaviour
{
    GameObject storageManager;
    StorageManager storageManagerScript;

    Dropdown accountDropdown;

    List<string> playerOptions = new List<string>();

    public ResistanceDropdown resistanceDropdownScript;
    public WeightInputField weightInputFieldScript;

    void Start()
    {
        storageManagerScript = FindObjectOfType<StorageManager>();

        accountDropdown = GetComponent<Dropdown>();

        //Voeg luisterfunctie toe zodat wanneer de Dropdown verandert de OnDropdownChanged() functie wordt gerunt
        accountDropdown.onValueChanged.AddListener(delegate
        {
            OnDropdownChanged();
        });

        veranderDropdownOptions();
    }

    //Lees de geselecteerde waarde van de dropdown
    public string GetSelectedOption()
    {
        Dropdown accountDropdown = GetComponent<Dropdown>();
        string selectedOption = accountDropdown.options[accountDropdown.value].text; //Lees de text van de DropdownOptie die geselecteerd is (accountDropdown.value = nummer van geselecteerde dropdown)
        return selectedOption;
    }

    //Pas de opties van de dropdowns aan naar alle accounts
    public void veranderDropdownOptions()
    {
        storageManagerScript = FindObjectOfType<StorageManager>();

        accountDropdown = GetComponent<Dropdown>();
        //Verwijder alle opties die nu al in de AccountDropdown staan
        accountDropdown.ClearOptions();

        //Vraag de speleropties aan de StorageManager
        List<string> personen = new List<string>();
        personen = storageManagerScript.GetPlayerOptions();

        List<Dropdown.OptionData> dropdownOptiondata = new List<Dropdown.OptionData>();

        //Overloop elke speler in de speleropties
        foreach (string persoon in personen)
        {
            //Voeg deze persoon toe als een AccountDropdown optie
            Dropdown.OptionData newDropdownOptionData = new Dropdown.OptionData();
            newDropdownOptionData.text = persoon;
            accountDropdown.options.Add(newDropdownOptionData);
        }
    }

    //Wanneer de dropdown verandert
    void OnDropdownChanged()
    {
        storageManagerScript = FindObjectOfType<StorageManager>();

        //Check of er een weerstandDropdown is
        if (resistanceDropdownScript != null)
        {
            //Geef de het ResistanceDropDownScript het commando om de geprefereerde weerstand van de speler te selecteren (GetSelectedOption returnd de geselecteerde speler)
            resistanceDropdownScript.SetStoredOption(GetSelectedOption());
        }

        if (weightInputFieldScript != null)
        {
            //Geef de het ResistanceDropDownScript het commando om het gewicht van de speler te tonen (GetSelectedOption returnd de geselecteerde speler)
            weightInputFieldScript.SetStoredOption(GetSelectedOption());
        }
    }
}
