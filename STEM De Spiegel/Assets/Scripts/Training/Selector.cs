using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Selector : MonoBehaviour
{
    public Dropdown persoonDropdownDropdown;
    public Dropdown vermogenInputField;

    public void veranderDropdownOptions(List<string> personen)
    {
        persoonDropdownDropdown.ClearOptions();

        List<Dropdown.OptionData> dropdownOptiondata = new List<Dropdown.OptionData>();

        foreach (string persoon in personen)
        {
            Dropdown.OptionData newDropdownOptionData = new Dropdown.OptionData();
            newDropdownOptionData.text = persoon;
            dropdownOptiondata.Add(newDropdownOptionData);
        }

        foreach (Dropdown.OptionData dropdownData in dropdownOptiondata)
        {
            persoonDropdownDropdown.options.Add(dropdownData);
        }

    }
}
