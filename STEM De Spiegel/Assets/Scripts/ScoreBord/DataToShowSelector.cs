using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DataToShowSelector : MonoBehaviour
{
    AccountDropdown accountDropdown;
    public Dropdown modeDropdown;
    public Dropdown periodDropdown;

    List<string> dataToShow = new List<string>();

    //Lees alle data van de dropdowns, getriggerd door ScoreBord
    public List<string> getInputData() //mode, accountName, period
    {
        List<string> inputData = new List<string>();

        accountDropdown = FindObjectOfType<AccountDropdown>();

        //Lees de geselecteerde mode
        string mode = modeDropdown.options[modeDropdown.value].text;
        inputData.Add(mode);

        if (accountDropdown != null)
        {
            //Wanneer er een accountDropdown is, lees deze
            string accountName = accountDropdown.GetSelectedOption();
            inputData.Add(accountName);
        }
        else
        {
            inputData.Add("");
        }

        //Lees de geselecteerde periode
        string period = periodDropdown.options[periodDropdown.value].text;
        inputData.Add(period);

        return inputData;
    }
}
