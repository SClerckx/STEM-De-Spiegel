using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PersoonSelector : MonoBehaviour
    
{
    CommunicationManager communicationManager;
    StorageManager storageManager;
    List<string> activePlayers;
    List<int> actieveFietsen;

    PopupManager popupManager;

    public Dropdown persoonDropdownDropdown;
    public InputField vermogenInputField;

    Player player;
    int fietsNummer;

    string naam;
    int inTeStellenvermogen = 0;

    public void OkButtonClicked() //Training
    {
        player = transform.parent.GetComponent<Player>();
        storageManager = FindObjectOfType<StorageManager>();
        communicationManager = FindObjectOfType<CommunicationManager>();
        popupManager = FindObjectOfType<PopupManager>();

        activePlayers = new List<string>(storageManager.activePlayers.Keys);
        actieveFietsen = new List<int>(storageManager.activePlayers.Values);

        //Bepaalfietsnummer door alle nummers te overlopen en het eerste te nemen dat vrij is
        int i = 1;
        bool fietsNummerFound = false;
        while (i < 7 &! fietsNummerFound)
        {
            if (!actieveFietsen.Contains(i))
            {
                fietsNummer = i;
                fietsNummerFound = true;
            }
            else
            {
                i += 1;
            }
        }

        //Leees de geselecteerde naam
        naam = persoonDropdownDropdown.options[persoonDropdownDropdown.value].text; //Kan in vervolg beter AccountDropdown gebruiken
        
        //Lees het geselecteerd vermogen
        try //Kan ook beter naar AccountDropdown
        {
            inTeStellenvermogen = int.Parse(vermogenInputField.text); 
        }
        catch
        {
            inTeStellenvermogen = 0;
        }

        //Zijn de naam en het vermogen geldig?
        if (naam != "" && inTeStellenvermogen > 0 && inTeStellenvermogen < 500 &! activePlayers.Contains(naam))
        {
            //Zo ja, zet informatie in Player, instantieer tekst en activeer fiets
            player.fietsnummer = fietsNummer;
            player.naam = naam;
            player.spelerMassa = storageManager.GetAccountData(naam)[0];
            player.ingesteldVermogen = inTeStellenvermogen;

            //Instantieer tekst
            transform.parent.gameObject.GetComponent<PersoonDisplay>().InstantieerText();
            try
            {
                //Activeer fiets
                communicationManager.SendPort(4, fietsNummer, 1);
                communicationManager.SendPort(5, fietsNummer, inTeStellenvermogen); //zend vermogen naar fiets
            }
            catch { Debug.Log("COMMUNICATIONERROR"); }

            //Verwijder de selector (dit object)
            Destroy(gameObject);
        }
        else
        {
            //Zo nee, zoek wat er mis is en gooi een popup
            if (naam == "")
            {
                popupManager.CreateWarningPopup(transform, "Je moet een account selecteren");
            }
            if (inTeStellenvermogen <= 0 || inTeStellenvermogen >= 500)
            {
                popupManager.CreateWarningPopup(transform, "Je moet een geldig vermogen invoeren: 1 tot 500 watt");
            }
            if (activePlayers.Contains(naam))
            {
                popupManager.CreateWarningPopup(transform, naam + " is al aan het trainen");
            }
        }
    }
}
