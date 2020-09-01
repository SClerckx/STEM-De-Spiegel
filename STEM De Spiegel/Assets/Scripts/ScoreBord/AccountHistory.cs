using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccountHistory : MonoBehaviour
{
    public string accountName;
    public List<float> historyData = new List<float>();
    public float numberOfSessions;
    public float totalTime;
    public float totalDistance;
    public float numberOfRacesWon;
    public float averageRanking;

    public void UpdateDataList()
    {
        historyData.Add(numberOfSessions);
        historyData.Add(totalTime);
        historyData.Add(totalDistance);
        historyData.Add(numberOfRacesWon);
        historyData.Add(averageRanking);
    }
}
