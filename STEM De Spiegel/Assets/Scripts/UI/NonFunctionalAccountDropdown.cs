using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonFunctionalAccountDropdown : MonoBehaviour
{
    /*
     * string previousSelection = "";
    int previousIndex;
    string currentSelection = "";
    int currentIndex;
    string removedItem;
    Start is called before the first frame update
    void Start()
    {
        accountDropdown.onValueChanged.AddListener(delegate
        {
            OnDropdownChanged();
        });
    }

    // in VeranderDropdownOptieons
    List<string> selectedPlayers = new List<string>();
        selectedPlayers = storageManagerScript.selectedPlayers;
        List<string> activePlayers = new List<string>(storageManagerScript.activePlayers.Keys);

    en check in spawn loop voor dubbels

    //Weggehaalde functies

    private void Update()
    {
        List<string> activePlayersLocal = new List<string>(storageManagerScript.activePlayers.Keys);
        List<string> previousActivePlayersLocal = new List<string>();

        if (activePlayersLocal != previousActivePlayersLocal && activePlayersLocal.Count < previousActivePlayersLocal.Count)
        {
            foreach (string previousSelectedPlayer in previousActivePlayersLocal)
            {
                if (!previousActivePlayersLocal.Contains(previousSelectedPlayer))
                {
                    removedItem = previousSelectedPlayer;
                }
            }

            AccountDropdown[] accountDropdowns = FindObjectsOfType<AccountDropdown>();

            foreach (AccountDropdown accountDropdown in accountDropdowns)
            {
                if (accountDropdown != GetComponent<AccountDropdown>())
                {
                    Dropdown dropdown = accountDropdown.gameObject.GetComponent<Dropdown>();

                    string priorSelection = accountDropdown.GetSelectedOption();
                    Dropdown.OptionData dropdownOptionData = new Dropdown.OptionData();
                    dropdownOptionData.text = removedItem;
                    dropdown.options.Insert(previousIndex, dropdownOptionData);
                    ChangeDropdownValueToSelection(priorSelection, dropdown);//Has to happen every time dropdown.options is changed, unity does not handle this itself
                }
            }
        }

        previousActivePlayersLocal = activePlayersLocal;
    }

    void OnDropdownChanged()
    {
        storageManagerScript = FindObjectOfType<StorageManager>();
        currentSelection = GetSelectedOption();
        storageManagerScript.selectedPlayers.Add(currentSelection);

        if (previousSelection != "" && previousSelection != null)
        {
            storageManagerScript.selectedPlayers.Remove(previousSelection);
        }

        if (currentSelection != previousSelection)
        {
            if (resistanceDropdownScript != null)
            {
                resistanceDropdownScript.SetStoredOption(currentSelection);
            }

            if (weightInputFieldScript != null)
            {
                weightInputFieldScript.SetStoredOption(currentSelection);
            }

            AccountDropdown[] accountDropdowns = FindObjectsOfType<AccountDropdown>();

            foreach (AccountDropdown accountDropdown in accountDropdowns)
            {
                if (accountDropdown != GetComponent<AccountDropdown>())
                {
                    Dropdown dropdown = accountDropdown.gameObject.GetComponent<Dropdown>();

                    for (int x = 0; x < dropdown.options.Count; x++)
                    {
                        if (dropdown.options[x].text == currentSelection)
                        {
                            string priorSelection = accountDropdown.GetSelectedOption();
                            dropdown.options.RemoveAt(x);
                            currentIndex = x;
                            ChangeDropdownValueToSelection(priorSelection, dropdown); //Has to happen every time dropdown.options is changed, unity does not handle this itself
                        }
                    }

                    if (previousSelection != "")
                    {
                        string priorSelection = accountDropdown.GetSelectedOption();
                        Dropdown.OptionData dropdownOptionData = new Dropdown.OptionData();
                        dropdownOptionData.text = previousSelection;
                        dropdown.options.Insert(previousIndex, dropdownOptionData);
                        ChangeDropdownValueToSelection(priorSelection, dropdown);//Has to happen every time dropdown.options is changed, unity does not handle this itself
                    }
                }
            }
        }

        previousIndex = currentIndex;
        previousSelection = currentSelection;
    }

    void ChangeDropdownValueToSelection(string selection, Dropdown dropdownLocal)
    {
        for (int i = 0; i < dropdownLocal.options.Count; ++i)
        {
            if (dropdownLocal.options[i].text == selection)
            {
                dropdownLocal.value = i;
            }
        }
    }

    private void OnDestroy()
    {
        if (previousSelection != "" && previousSelection != null)
        {
            storageManagerScript.selectedPlayers.Remove(previousSelection);

            List<string> activePlayers = new List<string>(storageManagerScript.activePlayers.Keys);

            if (!activePlayers.Contains(previousSelection))
            {
                AccountDropdown[] accountDropdowns = FindObjectsOfType<AccountDropdown>();

                foreach (AccountDropdown accountDropdown in accountDropdowns)
                {
                    if (accountDropdown != GetComponent<AccountDropdown>())
                    {
                        Dropdown dropdown = accountDropdown.gameObject.GetComponent<Dropdown>();

                        if (previousSelection != "")
                        {
                            string priorSelection = accountDropdown.GetSelectedOption();
                            Dropdown.OptionData dropdownOptionData = new Dropdown.OptionData();
                            dropdownOptionData.text = previousSelection;
                            dropdown.options.Insert(previousIndex, dropdownOptionData);
                            ChangeDropdownValueToSelection(priorSelection, dropdown);//Has to happen every time dropdown.options is changed, unity does not handle this itself
                        }
                    }
                }
            }
        }
    }*/
}
