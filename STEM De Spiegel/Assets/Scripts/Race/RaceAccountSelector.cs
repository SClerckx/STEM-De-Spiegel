using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceAccountSelector : MonoBehaviour
{
    public string levelToLoad;
    StorageManager storageManager;
    PopupManager popupManager;

    AccountDropdown[] accountDropdowns;
    ResistanceDropdown[] resistanceDropdowns;
    AccountSelector[] accountSelectors;

    //Functie die wordt getriggerd door de OkKnop
    public void TerminateSelection()
    {
        //Ga ervan uit dat alles juist is
        bool validPlayerSelection = true;
        bool noDoubleSelection = true;

        Dictionary<string, int> selectedPlayers = new Dictionary<string, int>();

        storageManager = FindObjectOfType<StorageManager>();
        popupManager = FindObjectOfType<PopupManager>();

        accountSelectors = FindObjectsOfType<AccountSelector>();

        //Overloop elke accountselector
        foreach (AccountSelector accountSelector in accountSelectors)
        {
            //Lees de geselecteerde speler en weerstand
            string player = accountSelector.accountDropdown.GetSelectedOption();
            int resistance = accountSelector.resistanceDropdown.GetSelectedOption();

            //Is de selectie van de speler geldig?
            if (player != "" &! selectedPlayers.ContainsKey(player))
            {
                selectedPlayers.Add(player, resistance); //Zo ja, voeg hem toe aan de lijst van geselecteerde spelers
            }
            else
            {
                //Zo nee, kijk wat er mis is
                if (player == "")
                {
                    validPlayerSelection = false; //Er is niets geselecteerd
                }
                if (selectedPlayers.ContainsKey(player))
                {
                    noDoubleSelection = false; //Er is een speler twee keer geselecteerd
                }
            }
        }

        //Is er nooit een probleem geweest met een speler?
        if (validPlayerSelection && noDoubleSelection)
        {
            //Zo nee, Voeg alle geselecteerde spelers toe als actieve spelers en laad het volgende level
            storageManager.activePlayers = selectedPlayers;
            FindObjectOfType<SceneManagerScript>().LoadLevel(levelToLoad);
        }
        else
        {
            //Zo ja, kijk wat er mis is
            if (!validPlayerSelection)
            {
                //Er is een ongeldige selectie gemaakt, gooi een popup met dit resultaat en doe voor de rest niets
                //Waarschuwing, de knop werd niet gedeactiveerd dus dit script kan meerdere keren gerunt worden en dus ook meerdere popups gooien, dit moet opgelost worden
                popupManager.CreateWarningPopup(FindObjectOfType<SubDisplays>().transform, "Alle lege spelers moeten geselecteerd of verwijderd worden");
            }
            if (!noDoubleSelection)
            {
                //Er is een dubbele selectie gemaakt, gooi een popup met dit resultaat en doe voor de rest niets
                popupManager.CreateWarningPopup(FindObjectOfType<SubDisplays>().transform, "Er is een account meerdere keren geselecteerd");
            }
        }
    }
}
//