using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class AccountEditor : MonoBehaviour
{
    StorageManager storageManagerScript;
    PopupManager popupManager;

    AccountDropdown accountDropdownScript;
    WeightInputField weightInputFieldScript;
    ResistanceDropdown resistanceDropdownScript;

    private void Start()
    {
        storageManagerScript = FindObjectOfType<StorageManager>();
        popupManager = FindObjectOfType<PopupManager>();

        accountDropdownScript = GetComponentInChildren<AccountDropdown>();
        weightInputFieldScript = GetComponentInChildren<WeightInputField>();
        resistanceDropdownScript = GetComponentInChildren<ResistanceDropdown>();
    }

    public void EditAccount()
    {
        string accountName = accountDropdownScript.GetSelectedOption();

        //Weight
        bool validWeight = true; float weight = 0;
        try
        {
            weight = float.Parse(weightInputFieldScript.GetComponent<InputField>().text); //weightInputFieldScript.GetSelectedOption() wordt hier niet gebruikt om een error te kunnen catchen. (Kan natuurlijk ook verbeterd worden door .GetSelectedOption een default te laten returnen bij een misgelopen read en dan hier daarop te checken)
        }
        catch
        {
            validWeight = false;
        }
        if (weight < 30 || weight > 150 || weight == null)
        {
            validWeight = false;
        }

        int preferredResistance = resistanceDropdownScript.GetSelectedOption();

        if (validWeight && accountName != "")
        {
            storageManagerScript.CreateOrEditAccount(accountName, weight, preferredResistance);
            popupManager.CreateFeedbackPopup(transform, "Account bewerkt!" + " Naam: " + accountName + ", Gewicht: " + weight + ", Geprefereerde weerstand: " + preferredResistance);
        }
        else
        {
            if (!validWeight)
            {
                popupManager.CreateWarningPopup(transform, "Je moet een geldig gewicht ingeven: 30-150 kg");
            }

            if(accountName == "")
            {
                popupManager.CreateWarningPopup(transform, "Je moet een account selecteren");
            }
        }
    }

    public void RemoveButtonClick()
    {
        string accountName = accountDropdownScript.GetSelectedOption();
        if (accountName != "")
        {
            Popup popup = popupManager.CreateConfirmPopup(transform, "Zeker dat u account " + accountName + " wilt verwijderen?");
            popup.yesEvent.AddListener(RemoveAccount);
        }
        else
        {
            popupManager.CreateWarningPopup(transform, "Je moet een account selecteren");
        }
    }

    public void RemoveAccount()
    {
        string accountName = accountDropdownScript.GetSelectedOption();
        storageManagerScript.DeleteAccount(accountName);
        accountDropdownScript.veranderDropdownOptions();
        popupManager.CreateFeedbackPopup(transform, "Account " + accountName + " verwijderd!");
    }
}
