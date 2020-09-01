using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class AccountCreator : MonoBehaviour
{
    public InputField nameInputField;
    public InputField weightInputField;
    public Dropdown preferredResistanceDropdown;

    StorageManager storageManagerScript;
    PopupManager popupManagerScript;

    string dataPath;
    List<float> toSaveFloats;

    string name;
    string[] illigalItems = { "*", ".", "/", "\"", "[", "]", "\\", ":", ";", "|", "=", "," };

    float weight;
    int preferredResistance;

    public void CreateAccount()
    {
        storageManagerScript = FindObjectOfType<StorageManager>();
        popupManagerScript = FindObjectOfType<PopupManager>();

        bool validName = true;
        name = nameInputField.text;
        foreach (string illigalItem in illigalItems)
        {
            if (name.Contains(illigalItem))
            {
                validName = false;
            }
        }
        if (name.Length > 20 || name == "")
        {
            validName = false;
        }

        bool validWeight = true;
        try
        {
            weight = float.Parse(weightInputField.text);
        }
        catch
        {
            validWeight = false;
        }
        if (weight < 30 || weight > 150 || weight == null) //weight == null kan nooit dus mag weg
        {
            validWeight = false;
        }

        preferredResistance = int.Parse(preferredResistanceDropdown.options[preferredResistanceDropdown.value].text);

        if (validName & !storageManagerScript.GetPlayerOptions().Contains(name) && validWeight)
        {
            storageManagerScript.CreateOrEditAccount(name, weight, preferredResistance);

            AccountDropdown[] accountDropdowns = FindObjectsOfType<AccountDropdown>();
            foreach (AccountDropdown accountDropdown in accountDropdowns)
            {
                accountDropdown.veranderDropdownOptions();
            }

            popupManagerScript.CreateFeedbackPopup(transform, "Nieuw account gemaakt!" + " Naam: " + name + ", Gewicht: " + weight + ", Geprefereerde weerstand: " + preferredResistance);
        }
        else
        {
            if (!validName)
            {
                // Dit mogen ook gewoon drie if statements worden, geen idee wat ik hier aan het denken was
                if (name.Length > 20 || name == "")
                {
                    if (name.Length > 20)
                    {
                        popupManagerScript.CreateWarningPopup(transform, "De naam is te lang");
                    }
                    else
                    {
                        popupManagerScript.CreateWarningPopup(transform, "Er moet een naam ingegeven worden");
                    }
                }
                else
                {
                    popupManagerScript.CreateWarningPopup(transform, "De naam bevat een illegaal teken (* . / \" \\ [ ] : ; | = ,)");
                }

            }
            if (storageManagerScript.GetPlayerOptions().Contains(name))
            {
                popupManagerScript.CreateWarningPopup(transform, "Er bestaat al een account met deze naam");
            }

            if (!validWeight)
            {
                popupManagerScript.CreateWarningPopup(transform, "Je moet een geldig gewicht ingeven: 30-150 kg");
            }

        }
        
    }
}
