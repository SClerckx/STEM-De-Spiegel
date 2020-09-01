using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using System.IO;
using System.Globalization;
using UnityEngine;

public class CollectivePanel : MonoBehaviour
{
    StorageManager storageManagerScript;
    ScoreBord scoreBordScript;
    PhysicsManager physicsManager;

    [Header("Texts")]
    public Text numberOfSessions;
    public Text totalTime;
    public Text totalDistance;
    public Text totalEnergy;
    public Text racesHeld;

    [Header("Data")]
    string accountName;
    DateTime periodStartDate;
    List<FileInfo> selectedEventFiles = new List<FileInfo>();
    List<List<float>> selectedEventFilesData = new List<List<float>>();

    public void StartCollectivePanel(DateTime periodStartDateInput)
    {
        periodStartDate = periodStartDateInput;
        storageManagerScript = FindObjectOfType<StorageManager>();
        scoreBordScript = FindObjectOfType<ScoreBord>();
        physicsManager = FindObjectOfType<PhysicsManager>();

        selectedEventFiles = scoreBordScript.GetAllSelectedEventFiles(null, periodStartDate);
        selectedEventFilesData = scoreBordScript.ReadAllEventFiles(selectedEventFiles);

        numberOfSessions.text = scoreBordScript.GetNumberOfSessions(selectedEventFiles).ToString();
        totalTime.text = physicsManager.FormatSeconds(scoreBordScript.GetTotalVar(selectedEventFilesData, 3, 2)); //Tijd heeft in racebestanden index 3 en in training index 2
        totalDistance.text = physicsManager.FormatDistance(scoreBordScript.GetTotalVar(selectedEventFilesData, 2, 1)); //Afstand heeft in racebestanden index 2 en in training index 1
        totalEnergy.text = physicsManager.FormatEnergy(scoreBordScript.GetTotalVar(selectedEventFilesData, 4, 3)); //Energie heeft in racebestanden index 4 em in training index 1
        racesHeld.text = scoreBordScript.GetRacesParticipated(selectedEventFiles).ToString();
    }
}
