using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class RaceResultsManager : MonoBehaviour
{
    GameObject storageManager;
    StorageManager storageManagerScript;

    SubDisplays subDisplayScript;
    List<GameObject> subDisplays = new List<GameObject>();

    public GameObject subDisplayPrefab;

    GameObject newSubDisplay;

    List<Player> players = new List<Player>();
    List<Player> finishedPlayers = new List<Player>();
    List<Player> nonFinishedPlayers = new List<Player>();
    List<Player> orderedPlayers = new List<Player>();

    void Start()
    {
        storageManager = GameObject.FindGameObjectWithTag("StorageManager");
        storageManagerScript = storageManager.GetComponent<StorageManager>();

        //lees de lijst van spelers die is meegenomen door de StorageManager 
        players = storageManagerScript.players;

        //Subdisplays
        subDisplayScript = GetComponent<SubDisplays>();
        subDisplays = subDisplayScript.subDisplays;

        //Overloop elke speler en maak onderscheid of hij was gefinisht of niet
        foreach (Player playerObject in players)
        {
            if (playerObject.finished == true)
            {
                finishedPlayers.Add(playerObject);
            }
            else
            {
                nonFinishedPlayers.Add(playerObject);
            }
        }

        //Sorteer spelers die zijn gefinisht op tijd en spelers die niet waren gefinisht op afstand (tijd en afstand stoppen met optellen na finish)
        finishedPlayers.Sort(SortByTime);
        nonFinishedPlayers.Sort(SortByDistance);

        //Voeg deze twee lijsten dan toe aan de lijst met geordende spelers
        orderedPlayers.AddRange(finishedPlayers);
        orderedPlayers.AddRange(nonFinishedPlayers);

        //Overloop nu elke speler in volgorde
        int i = 1;
        foreach (Player orderedPlayer in orderedPlayers)
        {
            //Elke speler krijgt een SubDisplay en een nieuw Player script op die SubDisplay waarop al zijn informatie wordt overgedragen (een component kan niet van object wisselen)
            newSubDisplay = Instantiate(subDisplayPrefab, Vector3.zero, Quaternion.identity, transform);
            newSubDisplay.AddComponent<Player>();
            Player newPlayer = newSubDisplay.GetComponent<Player>();
            orderedPlayer.ranking = i;
            newPlayer.ranking = i;
            newPlayer.naam = orderedPlayer.naam;
            newPlayer.afgeledeAfstand = orderedPlayer.afgeledeAfstand;
            newPlayer.verstrekenTijd = orderedPlayer.verstrekenTijd;
            newPlayer.opgewekteEnergie = orderedPlayer.opgewekteEnergie;

            subDisplays.Add(newSubDisplay);

            subDisplayScript.UpdatePersoondisplays();

            i += 1;
        }

        //Geen idee waarom die niet gewoon bijstaat in de loop hierboven
        foreach (Player orderedPlayer in orderedPlayers)
        {
            //Sla de race op en verwijder de speler uit de actieve spelers
            storageManagerScript.activePlayers.Remove(orderedPlayer.naam);
            storageManagerScript.SaveRace(orderedPlayer.naam, orderedPlayer.ranking / orderedPlayers.Count, orderedPlayer.afgeledeAfstand, orderedPlayer.verstrekenTijd, orderedPlayer.opgewekteEnergie);
        }
    }

    static int SortByTime(Player p1, Player p2)
    {
        return p1.verstrekenTijd.CompareTo(p2.verstrekenTijd); //verstrekentijd p1 groter dan p1
    }
    
    static int SortByDistance(Player p1, Player p2)
    {
        return p2.afgeledeAfstand.CompareTo(p1.afgeledeAfstand); //Afgelegdeafstand p2 groter dan p1
    }
}
