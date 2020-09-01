using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderBoard : MonoBehaviour
{
    SubDisplays subDisplayScript;
    RaceManager raceManagerScript;

    bool firstFrame = true;

    public GameObject leaderBoardItemPrefab;
    List<GameObject> subDisplays = new List<GameObject>();

    private void Start()
    {
        subDisplayScript = GetComponent<SubDisplays>();
        raceManagerScript = FindObjectOfType<RaceManager>();
    }

    public void InstantiateSubdisplay()
    {
        GameObject newSubDisplay = Instantiate(leaderBoardItemPrefab, transform);
        newSubDisplay.transform.SetParent(transform, false);
        RectTransform subDisplayRectTransform = newSubDisplay.GetComponent<RectTransform>();
        subDisplays.Add(newSubDisplay);

        if (subDisplays.Count * subDisplayRectTransform.rect.height < GetComponent<RectTransform>().rect.height)
        {
            subDisplayRectTransform.anchoredPosition = new Vector3(0, -1 * (subDisplays.Count - 1) * subDisplayRectTransform.rect.height, 0);
            subDisplayRectTransform.sizeDelta = new Vector3(GetComponent<RectTransform>().rect.width, subDisplayRectTransform.rect.height, 0);
        }
        else
        {
            int item = 0;
            foreach (GameObject subDisplay in subDisplays)
            {
                RectTransform rectTrans = subDisplay.GetComponent<RectTransform>();

                float wantedHeight = GetComponent<RectTransform>().rect.height / subDisplays.Count;

                rectTrans.anchoredPosition = new Vector3(0, -1 * item * wantedHeight, 0);
                rectTrans.sizeDelta = new Vector3(GetComponent<RectTransform>().rect.width, wantedHeight, 0);

                item += 1;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (raceManagerScript.raceStarted || firstFrame)
        {
            firstFrame = false;
            for (int i = 0; i < raceManagerScript.orderedPlayers.Count; i++)
            {
                string toDisplay;
                if (!raceManagerScript.orderedPlayers[i].finished)
                {
                    toDisplay = (i + 1).ToString() + ". " + raceManagerScript.orderedPlayers[i].naam + " " + raceManagerScript.orderedPlayers[i].rondesString;
                }
                else
                {
                    toDisplay = (i + 1).ToString() + ". " + raceManagerScript.orderedPlayers[i].naam + " AANGEKOMEN!";
                }
                subDisplays[i].GetComponent<Text>().text = toDisplay;
                subDisplays[i].GetComponent<Text>().color = raceManagerScript.playerColors[raceManagerScript.orderedPlayers[i].naam];
            }
        }
    }
}

