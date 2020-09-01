using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBord : MonoBehaviour
{
    DataToShowSelector dataToShowSelector;
    List<string> dataToShow;
    string mode;
    string accountName;
    string period;

    StorageManager storageManagerScript;

    DateTime periodStartDate;

    [Header("Panels")]
    GameObject panel;
    
    public GameObject individualPanelPrefab;
    public GameObject collectivePanelPrefab;
    public GameObject competitivePanelPrefab;

    [Header("Dropdown")]
    public GameObject AccountDropdownPrefab;
    GameObject accountDropdown;

    private void Start()
    {
        dataToShowSelector = FindObjectOfType<DataToShowSelector>();

        //Voeg een luisterfunctie toe aan de modeDropdown zodat de ModeDropdownValueChanged() functie wordt gerunt wanneer deze wordt verandert
        dataToShowSelector.modeDropdown.onValueChanged.AddListener(delegate {
            ModeDropdownValueChanged(dataToShowSelector.modeDropdown);
        });
    }

    //Getriggerd door delegate in start()
    public void ModeDropdownValueChanged(Dropdown modeDropdown)
    {
        //Lees de modeDropdown
        string mode = modeDropdown.options[modeDropdown.value].text;

        //Wanneer de geselecteerde mode individueel is, instantieer accountDropdown
        if (mode == "Individueel" && accountDropdown == null)
        {
            accountDropdown = Instantiate(AccountDropdownPrefab, FindObjectOfType<DataToShowSelector>().transform);
        }
        else
        {
            //Verwijder anders accountDropdown
            if (mode != "Individueel")
            {
                Destroy(accountDropdown);
            }
        }
    }

    //Gerunt vanuit de Go! button
    public void UpdateScorebord()
    {
        //Lees wat er getoond moet worden
        dataToShowSelector = FindObjectOfType<DataToShowSelector>();
        dataToShow = dataToShowSelector.getInputData();
        mode = dataToShow[0];
        accountName = dataToShow[1];
        period = dataToShow[2];

        storageManagerScript = FindObjectOfType<StorageManager>();

        //Verwijder het paneel dat er al staat
        if (panel != null)
        {
            Destroy(panel);
        }

        //Bereken de startDatum van de geselecteerde periode
        periodStartDate = GetStartDate(period);

        //Bepaal de mode en dus welk paneel er geinstantieert moet worden
        if (mode == "Individueel")
        {
            panel = InstantiateIndividualPanel(individualPanelPrefab, accountName, periodStartDate);
        }

        if (mode == "Collectief")
        {
            panel = InstantiateCollectivePanel(collectivePanelPrefab, periodStartDate);
        }

        if (mode == "Competetief")
        {
            panel = InstantiateCompetitivePanel(competitivePanelPrefab, periodStartDate);
        }
    }
    
    //Instantieer individueel paneel
    GameObject InstantiateIndividualPanel(GameObject panelPrefab, string accountName, DateTime periodStartDate)
    {
        GameObject individualPanel = Instantiate(panelPrefab, transform);
        IndividualPanel individualPanelScript = individualPanel.GetComponent<IndividualPanel>();
        individualPanelScript.StartIndividualPanel(accountName, periodStartDate);

        return individualPanel;
    }

    //Instantieer Collectief paneel
    GameObject InstantiateCollectivePanel(GameObject panelPrefab, DateTime periodStartDate)
    {
        GameObject collectivePanel = Instantiate(panelPrefab, transform);
        CollectivePanel collectivePanelScript = collectivePanel.GetComponent<CollectivePanel>();
        collectivePanelScript.StartCollectivePanel(periodStartDate);

        return collectivePanel;
    }

    //Instantieer Competitief paneel
    GameObject InstantiateCompetitivePanel(GameObject panelPrefab,  DateTime periodStartDate)
    {
        GameObject competitivePanel = Instantiate(panelPrefab, transform);
        CompetitivePanel competitivePanelScript = competitivePanel.GetComponent<CompetitivePanel>();
        competitivePanelScript.StartCompetitivePanel(periodStartDate);

        return competitivePanel;
    }

    //Functie om startdatum te bepalen van de geselecteerde periode
    DateTime GetStartDate(string selectedOption)
    {
        DateTime periodStartDateInternal = DateTime.MinValue;

        if (selectedOption == "Wekelijks")
        {
            periodStartDateInternal = DateTime.Now.AddDays(-7);
        }
        if (selectedOption == "Maandelijks")
        {
            periodStartDateInternal = DateTime.Now.AddMonths(-1);
        }
        if (selectedOption == "Jaarlijks")
        {
            periodStartDateInternal = DateTime.Now.AddYears(-1);
        }
        if (selectedOption == "Alles")
        {
            periodStartDateInternal = DateTime.MinValue;
        }

        return periodStartDateInternal;
    }

    //------------------De functies hieronder worden getriggerd door alle panelen (of door andere functies hieronder) om informatie te verkrijgen over de geschiedenis-------------------------

    //Haal alle files op van een bepaald account (of van alle, wanneer accountName = Null) vanaf een bepaalde datum
    public List<FileInfo> GetAllSelectedEventFiles(string accountName, DateTime startDate)
    {
        List<FileInfo> eventFilesInternal = new List<FileInfo>();

        //Vraag alle files op in het eventpath
        string[] directories = Directory.GetFiles(storageManagerScript.eventPath, "*.*", SearchOption.AllDirectories);

        //Overloop al deze files
        foreach (string directory in directories)
        {
            //Lees de naam van het bestand
            FileInfo eventFileInfo = new FileInfo(directory);
            string[] eventFileInfoNameSplit = eventFileInfo.Name.Split(',');
            string dateTimeString = eventFileInfoNameSplit[2];

            //Maak een DateTime formaat van de datumstring in de naam
            DateTime dateTime = DateTime.ParseExact(dateTimeString, "MMddyyyy_HHmmss", CultureInfo.InvariantCulture);

            if (accountName == null)
            {
                //Wanneer de accountname Null is wordt hier niet naar gekeken en worden alle bestanden na de startdatum opgehaald
                if (dateTime > startDate)
                {
                    eventFilesInternal.Add(new FileInfo(directory));
                }
            }
            else
            {
                //Wanneer de accountname niet Null is worden alleen de files van een account na een bepaalde datum opgehaald
                if (eventFileInfo.Name.Contains(accountName) && dateTime > startDate)
                {
                    eventFilesInternal.Add(new FileInfo(directory));
                }
            }
        }
        return eventFilesInternal;
    }

    //Leest alle doorgegeven bestanden
    public List<List<float>> ReadAllEventFiles(List<FileInfo> selectedEventFilesInput)
    {
        List<List<float>> selectedEventFilesDataInternal = new List<List<float>>();

        //Overloop alle files die werden doorgegeven als argument en lees de inhoud hiervan
        foreach (FileInfo eventFile in selectedEventFilesInput)
        {
            List<float> eventFileData = storageManagerScript.Load(eventFile.FullName);
            selectedEventFilesDataInternal.Add(eventFileData);
        }
        return selectedEventFilesDataInternal;
    }

    //Bepaal het aantal sessies van een account door het aantal bestanden van het account te tellen
    public int GetNumberOfSessions(List<FileInfo> selectedEventFilesInput)
    {
        int numberOfSessionsInternal = selectedEventFilesInput.Count;
        return numberOfSessionsInternal;
    }

    //Bereken het totaal van een variabele, bijvoorbeeld tijd, dat een account heeft opgelopen in trainingen en races SAMEN. De indexInRace en indexInTraining zijn hier argumenten omdat de functie moet weten waar het moet kijken in het bestand van de sessie en deze zijn verschillend voor de bestanden van de race en de bestanden van de training
    public float GetTotalVar(List<List<float>> selectedEventFilesDataInput, int indexInRace, int indexInTraining)
    {
        float totalVar = 0;

        //Overloop alle bestanden
        foreach (List<float> eventFileData in selectedEventFilesDataInput)
        {
            //Check ofdat het een race of training is
            if (eventFileData[0] == 0) //0 dus race
            {
                //Lees dan de variabele aan de hand van de index in de race en tel deze op bij het totaal
                totalVar += eventFileData[indexInRace];
            }
            else //niet 0 dus training
            {
                //Lees dan de variabele aan de hand van de index in de training en tel deze op bij het totaal
                totalVar += eventFileData[indexInTraining];
            }
        }

        return totalVar;
    }


    //Bepaal aan hoeveel er werd gedaan in de bestanden in het argument
    public int GetRacesParticipated(List<FileInfo> selectedEventFilesInput)
    {
        int racesParticipated = 0;

        //Overloop alle bestanden en zoek naar het woord race in de naam, dit is dan 1 race
        foreach (FileInfo eventFileInfo in selectedEventFilesInput)
        {
            if (eventFileInfo.Name.Contains("race"))
            {
                racesParticipated += 1;
            }
        }
        return racesParticipated;
    }

    //Bepaal hoeveel races er werden gewonnen in de doorgegeven gelezen bestanden
    public int GetRacesWon(List<List<float>> selectedEventFilesDataInput)
    {
        int racesWon = 0;

        //Overloop alle gelezen bestanden
        foreach (List<float> eventFileData in selectedEventFilesDataInput)
        {
            //Wanneer er een race is (eventFileData[0] == 0) en deze werd gewonnen (eventFileData[1] == 1) tel dit op bij het totaal
            if (eventFileData[0] == 0 && eventFileData[1] == 1)
            {
                racesWon += 1;
            }
        }
        return racesWon;
    }

    //Bepaal de gemiddelde positie in een race in de doorgegeven gelezen bestganden
    public float GetAverageRankingInRaces(List<List<float>> selectedEventFilesDataInput)
    {
        float raceAverageRankingInternal = 0;
        float raceRankingsSum = 0;
        float numberOfRaces = 0;

        //Overloop alle gelezen bestanden
        foreach (List<float> eventFileData in selectedEventFilesDataInput)
        {
            if (eventFileData[0] == 0) //Wanneer het gelezen bestand de geschiedenis van een race is
            {
                raceRankingsSum += eventFileData[1]; //Tel de positie van de speler op bij het totaal
                numberOfRaces += 1; //1 race extra
            }
        }

        if (numberOfRaces != 0) //Wanneer er races waren
        {
            //Bepaal de gemiddelde positie door de som van de posities te delen door het aantal races
            raceAverageRankingInternal = raceRankingsSum / numberOfRaces;
        }
        else
        {
            //Wanneer er geen races werden gevonden return 42, dit kan toch nooit een echte uitkomst zijn en is the answer to life, the universe and everything dus leek mij wel gepast (Ik wist nog niet dat Default floats bestonden)
            raceAverageRankingInternal = 42;
        }
        return raceAverageRankingInternal;
    }
}
