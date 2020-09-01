using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Variabelen")]
    [Header("Race en training")]
    public int ranking;
    public int fietsnummer;
    public string naam;
    public float afgeledeAfstand;
    public float spelerMassa = 60;
    public float vermogen;
    private float vorigVermogen;
    
    [Header("Race")]
    public float laps = 0; //van spelerBeweging
    public string rondesString; // van raceManager
    public bool finished;
    public int weerstand;

    [Header("Training")]
    public float fietsSnelheid;
    public float ingesteldVermogen;
    public float opgewekteEnergie;
    public float verstrekenTijd;

    [Header("Communicatie")]
    CommunicationManager communicationManager;
    string[] ontvangen;
    List<GameObject> persoonDisplays = new List<GameObject>();

    private void Start()
    {
        communicationManager = FindObjectOfType<CommunicationManager>();
    }

    private void Update()
    {
        if (communicationManager != null)
        {
            ontvangen = communicationManager.ontvangenGesplitst;

            try
            {
                if (ontvangen[1] == fietsnummer.ToString()) //Ontvangen fietsnummer = mijnfietsnummer
                {
                    /*
                    if (ontvangen[0] == "1") //Ontvangen command == snelheid
                    {
                            Debug.Log("mijn nummer" + ontvangen.ToString());
                            fietsSnelheid = float.Parse(ontvangen[2]);

                    }*/

                    if (ontvangen[0] == "2") //Ontvangen command == vermogen
                    {
                        vermogen = float.Parse(ontvangen[2]);
                    }
                }
            }
            catch
            {
            }
        }
    }
}
