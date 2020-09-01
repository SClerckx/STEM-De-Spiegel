using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapManager : MonoBehaviour
{
    RaceManager raceManager;

    List<List<float>> puntenLijst = new List<List<float>>(); //[[x,y,d,dt,dp],[[x,y,d,dt,dp]]
    List<float> dataLijst = new List<float>(); //[x,y,d,dt,dp]
    public List<Vector2> spoorCoordinaten = new List<Vector2>(); //(x,y)

    RectTransform ownRectTransform;
    LineRenderer lineRenderer;

    List<Player> players = new List<Player>();
    public GameObject playerIconPrefab;

    Dictionary<GameObject, Player> playerDots = new Dictionary<GameObject, Player>();

    void Start()
    {
        raceManager = FindObjectOfType<RaceManager>();

        puntenLijst = FindObjectOfType<LeesSpoorCoordinaten>().VraagCoordinatenLijst();
        ownRectTransform = GetComponent<RectTransform>();

        int i = 0;
        foreach (List<float> dataLijst in puntenLijst)
        {
            float x = dataLijst[0];
            float y = dataLijst[1];
            Vector2 punt = new Vector2(x, y);
            spoorCoordinaten.Add(punt);
            i += 1;
        }

        //totalDistance = puntenLijst[i-1][3];

        lineRenderer = GetComponent<LineRenderer>();
        RenderLines(lineRenderer, spoorCoordinaten);
    }

    private void Update()
    {
        if (players.Count == 0)
        {
            players = raceManager.orderedPlayers;
            foreach (Player player in players)
            {
                GameObject playerDot = Instantiate(playerIconPrefab, transform);
                playerDots.Add(playerDot, player);
                playerDot.GetComponent<Image>().color = raceManager.playerColors[player.naam];
                Debug.Log(playerIconPrefab.GetComponent<Image>().color);
            }
        }

        foreach (KeyValuePair<GameObject, Player> playerDot in playerDots)
        {
            int i = playerDot.Value.GetComponent<SpelerBeweging3D>().i;
            playerDot.Key.GetComponent<RectTransform>().anchoredPosition = new Vector3(-1 * spoorCoordinaten[i].x * ownRectTransform.rect.width + playerDot.Key.GetComponent<RectTransform>().rect.width / 2, -1 * spoorCoordinaten[i].y * ownRectTransform.rect.height + playerDot.Key.GetComponent<RectTransform>().rect.height / 2, 0);
        }
    }

    void RenderLines(LineRenderer lineRenderer, List<Vector2> coordinatenLijst)
    {
        int aantalCoordinaten = coordinatenLijst.Count;
        lineRenderer.positionCount = aantalCoordinaten;
        for (int i = 0; i < aantalCoordinaten; i++)
        {
            lineRenderer.SetPosition(i, new Vector3(-1 * coordinatenLijst[i].x * ownRectTransform.rect.width, -1* coordinatenLijst[i].y * ownRectTransform.rect.height, 0));
        }
        lineRenderer.Simplify(0.5f);
    }
}
