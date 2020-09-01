using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PersoonDisplay : MonoBehaviour
{
    public GameObject persoonSelectorPrefab;
    public GameObject selector;
    AccountDropdown accountDropdownScript;

    public GameObject persoonTekstPrefab;
    public GameObject text;

    [Header("RemovePopup")]
    public Button removeButton;
    public GameObject removePopupPrefab;
    Popup removePopupScript;
    public UnityEvent yesEvent;
    public UnityEvent noEvent;

    Player playerScript;

    StorageManager storageManagerScript;
    CommunicationManager communicationManager;

    GameObject display;
    SubDisplays subDisplaysScript;

    List<string> personen = new List<string>();
    List<Player> spelersPerDisplay = new List<Player>();

    public bool textInstantiated;

    void Start()
    {
        display = transform.parent.gameObject;
        subDisplaysScript = display.GetComponent<SubDisplays>();

        storageManagerScript = FindObjectOfType<StorageManager>();
        communicationManager = FindObjectOfType<CommunicationManager>();
        playerScript = GetComponent<Player>();

        InstantieerSelector();
    }

    //Selector wordt geinstantieert bij de start van dit script
    void InstantieerSelector()
    {
        selector = Instantiate(persoonSelectorPrefab, transform);
        //Verander de opties in de dropdown naar alle speleropties
        accountDropdownScript = selector.GetComponentInChildren<AccountDropdown>();
        accountDropdownScript.veranderDropdownOptions();
    }

    //Wordt getriggerd vanuit de selector
    public void InstantieerText()
    {
        text = Instantiate(persoonTekstPrefab, transform);
        storageManagerScript.activePlayers.Add(playerScript.naam, playerScript.fietsnummer);
        textInstantiated = true;
    }

    //Verwijder deze subdisplay wanneer een persoon stopt met trainen (getriggerd door de RemoveButton)
    public void RemovePersoonDisplay()
    {
        //Is de persoon nog aan het kiezen?
        if (textInstantiated)
        {
            //Zo nee, gooi een popup zodat de persoon zijn training niet perongeluk stopt
            PopupManager popupManagerScript = FindObjectOfType<PopupManager>();
            removePopupScript = popupManagerScript.CreateConfirmPopup(transform, "Zeker dat u wilt stoppen met trainen?");
            removePopupScript.yesEvent.AddListener(RemoveThisDisplay);
            removePopupScript.noEvent.AddListener(ReactivateRemoveButton);
            removeButton.interactable = false;
        }
        else
        {
            //Zo, ja verwijder zonder popup
            subDisplaysScript.RemoveSubDisplay(gameObject);
            Destroy(gameObject);
        }
    }

    //Verwijder de Display nu echt
    void RemoveThisDisplay()
    {
        //Sla de training van de persoon op
        playerScript = GetComponent<Player>();
        storageManagerScript.activePlayers.Remove(playerScript.naam);
        storageManagerScript.SaveTraining(playerScript.naam, playerScript.afgeledeAfstand, playerScript.verstrekenTijd, playerScript.opgewekteEnergie);

        //Deactiveer zijn fiets in de arduino
        communicationManager.SendPort(4, playerScript.fietsnummer, 0);

        //Verwijder dit object
        subDisplaysScript.RemoveSubDisplay(gameObject);
        Destroy(gameObject);
    }

    //Wanneer de persoon op nee drukt in de bevestigingspopup moet de de RemoveButton geheractiveerd worden
    void ReactivateRemoveButton()
    {
        removeButton.interactable = true;
        Destroy(removePopupScript.gameObject);
    }
}
