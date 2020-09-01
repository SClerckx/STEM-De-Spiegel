using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using System.IO;
using UnityEngine;
using System.Globalization;

public class IndividualPanel : MonoBehaviour
{
    StorageManager storageManagerScript;
    ScoreBord scoreBordScript;
    PhysicsManager physicsManager;

    [Header("Texts")]
    public Text numberOfSessions;
    public Text totalTime;
    public Text totalDistance;
    public Text totalEnergy;
    public Text racesParticipated;
    public Text racesWon;
    public Text racesAveragePosition;

    [Header("Data")]
    string accountName;
    DateTime periodStartDate;
    List<FileInfo> selectedEventFiles = new List<FileInfo>();
    List<List<float>> selectedEventFilesData = new List<List<float>>();

    public void StartIndividualPanel(string accountNameInput, DateTime periodStartDateInput)
    {
        accountName = accountNameInput;
        periodStartDate = periodStartDateInput;
        storageManagerScript = FindObjectOfType<StorageManager>();
        scoreBordScript = FindObjectOfType<ScoreBord>();
        physicsManager = FindObjectOfType<PhysicsManager>();

        selectedEventFiles = scoreBordScript.GetAllSelectedEventFiles(accountName, periodStartDate);
        selectedEventFilesData = scoreBordScript.ReadAllEventFiles(selectedEventFiles);

        numberOfSessions.text = scoreBordScript.GetNumberOfSessions(selectedEventFiles).ToString();
        totalTime.text = physicsManager.FormatSeconds(scoreBordScript.GetTotalVar(selectedEventFilesData, 3, 2)); //Tijd heeft in racebestanden index 3 en in training index 2
        totalDistance.text = physicsManager.FormatDistance(scoreBordScript.GetTotalVar(selectedEventFilesData, 2, 1)); //Afstand heeft in racebestanden index 2 en in training index 1
        totalEnergy.text = physicsManager.FormatEnergy(scoreBordScript.GetTotalVar(selectedEventFilesData, 4, 3)); //Energie heeft in racebestanden index 4 em in training index 3
        racesParticipated.text = scoreBordScript.GetRacesParticipated(selectedEventFiles).ToString();
        racesWon.text = scoreBordScript.GetRacesWon(selectedEventFilesData).ToString();
        if (scoreBordScript.GetAverageRankingInRaces(selectedEventFilesData) == 42)
        {
            racesAveragePosition.text = "Niet aan races deelgenomen";
        }
        else
        {
            racesAveragePosition.text = scoreBordScript.GetAverageRankingInRaces(selectedEventFilesData).ToString();
        }
    }
}
