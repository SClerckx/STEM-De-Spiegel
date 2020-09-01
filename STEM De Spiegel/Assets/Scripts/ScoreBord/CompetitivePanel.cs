using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.Globalization;
using UnityEngine;

public class CompetitivePanel : MonoBehaviour
{
    StorageManager storageManagerScript;
    ScoreBord scoreBordScript;

    List<Rankings> rankingsList = new List<Rankings>();
    List<string> playerOptions = new List<string>();

    List<AccountHistory> accountHistories = new List<AccountHistory>();

    List<AccountHistory> accountsSortedByNumberOfSessions = new List<AccountHistory>();
    List<AccountHistory> accountsSortedByTotalTime = new List<AccountHistory>();
    List<AccountHistory> accountsSortedByTotalDistance = new List<AccountHistory>();
    List<AccountHistory> accountsSortedByNumberOfRacesWon = new List<AccountHistory>();
    List<AccountHistory> accountsSortedByAverageRanking = new List<AccountHistory>();
    List<List<AccountHistory>> sortedAccounts = new List<List<AccountHistory>>();

    public Rankings numberOfSessionsRanking;
    public Rankings totalTImeRanking;
    public Rankings totalDistanceRanking;
    public Rankings numberOfRacesWonRanking;
    public Rankings averageRankingRanking;

    //veel moed hiermee, als het teveel wordt is The Expanse kijken ook altijd een optie
    public void StartCompetitivePanel(DateTime periodStartDate)
    {
        storageManagerScript = FindObjectOfType<StorageManager>();
        scoreBordScript = FindObjectOfType<ScoreBord>();
        playerOptions = storageManagerScript.GetPlayerOptions();
        playerOptions.Remove("");

        rankingsList.Add(numberOfSessionsRanking); rankingsList.Add(totalTImeRanking); rankingsList.Add(totalDistanceRanking); rankingsList.Add(numberOfRacesWonRanking); rankingsList.Add(averageRankingRanking);

        foreach (string player in playerOptions) //Geeft iedere speler een AcountHistory met de data van deze speler.
        {
            //Haal alle bestanden op van de speler vanaf een bepaalde datum
            List<FileInfo> selectedEventFiles = scoreBordScript.GetAllSelectedEventFiles(player, periodStartDate);

            //Lees deze bestanden
            List<List<float>> selectedEventFilesData = scoreBordScript.ReadAllEventFiles(selectedEventFiles);

            //Creer een Accounthistory en prop deze vol met de informatie over de speler
            AccountHistory accountHistory = gameObject.AddComponent<AccountHistory>();
            accountHistory.accountName = player;
            accountHistory.numberOfSessions = scoreBordScript.GetNumberOfSessions(selectedEventFiles);
            accountHistory.totalTime = scoreBordScript.GetTotalVar(selectedEventFilesData, 3, 2); //Tijd heeft in racebestanden index 3 en in training index 2
            accountHistory.totalDistance = scoreBordScript.GetTotalVar(selectedEventFilesData, 2, 1); //Afstand heeft in racebestanden index 2 en in training index 1
            accountHistory.numberOfRacesWon = scoreBordScript.GetRacesWon(selectedEventFilesData);
            accountHistory.averageRanking = scoreBordScript.GetAverageRankingInRaces(selectedEventFilesData);
            accountHistory.UpdateDataList();
            accountHistories.Add(accountHistory);
        }

        //Maakt nu verschillende lijsten waarin de gesorteerde AccountHistories worden geplaatst
        accountsSortedByNumberOfSessions = new List<AccountHistory>(accountHistories);
        //Sorteer de lijsten op de geselecteerde waarde
        accountsSortedByNumberOfSessions.Sort(SortByNumberOfSessions);
        //Voeg toe aan de lijst van de lijst van gesorteerde accounts
        sortedAccounts.Add(accountsSortedByNumberOfSessions);
        //Repeat
        accountsSortedByTotalTime = new List<AccountHistory>(accountHistories);
        accountsSortedByTotalTime.Sort(SortByTotalTime); sortedAccounts.Add(accountsSortedByTotalTime);
        accountsSortedByTotalDistance = new List<AccountHistory>(accountHistories);
        accountsSortedByTotalDistance.Sort(SortByTotalDistance); sortedAccounts.Add(accountsSortedByTotalDistance);
        accountsSortedByNumberOfRacesWon = new List<AccountHistory>(accountHistories);
        accountsSortedByNumberOfRacesWon.Sort(SortByNumberOfRacesWon); sortedAccounts.Add(accountsSortedByNumberOfRacesWon);
        accountsSortedByAverageRanking = new List<AccountHistory>(accountHistories);
        accountsSortedByAverageRanking.Sort(SortByAverageRanking); sortedAccounts.Add(accountsSortedByAverageRanking);

        int i = 0;
        foreach (Rankings rankings in rankingsList) //Iedere ranking krijgt nu een gesorteerde lijst toegewezen om te laten zien
        {
            rankings.ApplyRankings(sortedAccounts[i], i);
            i += 1;
        }
    }

    static int SortByNumberOfSessions(AccountHistory a1, AccountHistory a2)
    {
        return a2.numberOfSessions.CompareTo(a1.numberOfSessions); //numberofsessions a1 groter dan a1
    }

    static int SortByTotalTime(AccountHistory a1, AccountHistory a2)
    {
        return a2.totalTime.CompareTo(a1.totalTime);
    }

    static int SortByTotalDistance(AccountHistory a1, AccountHistory a2)
    {
        return a2.totalDistance.CompareTo(a1.totalDistance);
    }

    static int SortByNumberOfRacesWon(AccountHistory a1, AccountHistory a2)
    {
        return a2.numberOfRacesWon.CompareTo(a1.numberOfRacesWon); 
    }

    static int SortByAverageRanking(AccountHistory a1, AccountHistory a2)
    {
        return a1.averageRanking.CompareTo(a2.averageRanking);
    }
}
