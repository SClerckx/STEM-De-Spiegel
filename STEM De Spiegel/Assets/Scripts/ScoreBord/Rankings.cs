using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rankings : MonoBehaviour
{
    ScoreBord scoreBordScript;
    PhysicsManager physicsManager;

    public void ApplyRankings(List<AccountHistory> accountHistories, int dataToDisplay)
    {
        physicsManager = FindObjectOfType<PhysicsManager>();
        scoreBordScript = FindObjectOfType<ScoreBord>();

        int t = 0;
        Ranking[] rankingsArray = GetComponentsInChildren<Ranking>();
        foreach (Ranking ranking in rankingsArray)
        {
            ranking.number.text = (t + 1).ToString();

            if (t < accountHistories.Count) //zijn er genoeg accounts om het leaderboard te vullen tot 10?
            {
                // Geeft eerst rauwe data aan ranking en bewerkt nadien in speciale gevallen.
                ranking.name.text = accountHistories[t].accountName;
                ranking.value.text = accountHistories[t].historyData[dataToDisplay].ToString();

                //Speciale gevallen
                if (dataToDisplay == 1) //Tijd
                {
                    ranking.value.text = physicsManager.FormatSeconds(accountHistories[t].historyData[dataToDisplay]);
                }

                if (dataToDisplay == 2) //Afstand
                {
                    ranking.value.text = physicsManager.FormatDistance(accountHistories[t].historyData[dataToDisplay]);
                }

                if (dataToDisplay == 4) //Gemiddelde ranking
                {
                    //Aan races meegedaan, laat zien met 2 kommagetallen
                    ranking.value.text = accountHistories[t].historyData[4].ToString("F2");
                    //Niet aan races meegedaan, delen door nul dus speciaal geval nodig
                    if (accountHistories[t].historyData[4] == 42)
                    {
                        ranking.value.text = "Nog geen  ."; //Twee spaties en punt om rare textbug te voorkomen (Bug komt alleen voor op sommige PC's, op mijn thuispc bijvoorbeeld ziet het er wel ok uit en is die "  ." maar raar)
                    }
                }
            }
            else
            {
                ranking.name.text = "-----";
                ranking.value.text = "-----";
            }
            t += 1;
        }
    }
}
