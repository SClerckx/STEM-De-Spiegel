using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaceManager : MonoBehaviour
{
    CommunicationManager communicationManager;

    LeesSpoorCoordinaten leesSpoorCoordinaten;

    RectTransform rectTransform;
    SpoorManager3D spoorManager;

    GameObject storageManager;
    StorageManager storageManagerScript;

    PopupManager popupManager;

    OkButtonRace okButtonRace;

    public GameObject spoor;

    public bool raceFinished;

    [Header("Load")]
    public string levelToLoad;
    MapData mapData;

    [Header("Players")]
    public GameObject speler;
    public int aantalSpelers;
    public List<GameObject> playerObjects = new List<GameObject>();
    public List<string> playerNames = new List<string>();
    public List<Player> orderedPlayers = new List<Player>();

    public GameObject blackBikerPrefab;
    public GameObject greenBikerPrefab;
    public GameObject orangeBikerPrefab;
    public GameObject pinkBikerPrefab;
    public GameObject redBikerPrefab;
    public GameObject yellowBikerPrefab;
    public List<GameObject> orderedColorAnimationPrefabs = new List<GameObject>();
    public List<Color> colorOptions = new List<Color>(new Color[] {Color.yellow , Color.green, new Color(241, 90, 34, 255), Color.magenta, Color.red, Color.black});
    public Dictionary<string, Color> playerColors = new Dictionary<string, Color>();
    public Vector3 playerPosition = new Vector3(1/20f, 1/10f, 1/20f);
    public Vector3 playerRotation = new Vector3(270, 180, 0);
    public Vector3 playerScale = new Vector3(1/10f, 1/10f, 1/10f);

    [Header("Timer")]
    GameObject timer;
    RaceTimer timerScript;
    public GameObject timerPrefab;
    public float secondsToWait;
    float passedTime;
    public bool raceStarted = false;

    [Header("Leaderboard")]
    LeaderBoard leaderBoard;

    [Header("Objects")]
    Camera mainCamera;
    GameObject plain;
    GameObject map;
    GameObject light;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        spoorManager = GetComponent<SpoorManager3D>();
        okButtonRace = FindObjectOfType<OkButtonRace>();
        communicationManager = FindObjectOfType<CommunicationManager>();
        leaderBoard = FindObjectOfType<LeaderBoard>();

        storageManager = GameObject.FindGameObjectWithTag("StorageManager");
        storageManagerScript = storageManager.GetComponent<StorageManager>();

        //Vraag de naam van de spelers, het aantal spelers en de mapData aan de StorageManager
        playerNames = new List<string>(storageManagerScript.activePlayers.Keys); 
        aantalSpelers = playerNames.Count;
        mapData = storageManagerScript.mapData;

        //Instantieer de map en geef deze de juiste waardes
        map = Instantiate(mapData.mapPrefab);
        map.transform.position = mapData.mapPosition;
        map.transform.rotation = Quaternion.Euler(mapData.mapRotation);
        map.transform.localScale = mapData.mapScale;

        //Geef de juiste waardes aan de Camera
        mainCamera = Camera.main;
        mainCamera.transform.position = mapData.cameraPosition;
        mainCamera.transform.rotation = Quaternion.Euler(mapData.cameraRotation);
        mainCamera.fieldOfView = mapData.cameraFOV;

        //Geef de juiste waardes aan het plain
        plain = gameObject;
        plain.transform.position = mapData.planePosition;
        plain.transform.rotation = Quaternion.Euler(mapData.planeRotation);
        plain.transform.localScale = mapData.planeScale;

        //Geef de juiste waardes aan het spoor
        spoorManager.scale = mapData.absoluteScale;
        spoorManager.PrepareTrack();

        //Geef de juiste waardes aan het light
        light = FindObjectOfType<Light>().gameObject;
        light.transform.rotation = Quaternion.Euler(mapData.lightRotation);

        //Voeg de mogelijke spelerkleuren toe aan een lijst
        orderedColorAnimationPrefabs.Add(yellowBikerPrefab);
        orderedColorAnimationPrefabs.Add(greenBikerPrefab);
        orderedColorAnimationPrefabs.Add(orangeBikerPrefab);
        orderedColorAnimationPrefabs.Add(pinkBikerPrefab);
        orderedColorAnimationPrefabs.Add(redBikerPrefab);
        orderedColorAnimationPrefabs.Add(blackBikerPrefab);

        //Instantieer alle spelers
        for (int index = 0; index < aantalSpelers; index++)
        {
            //Instantieer de speler en geef zijn Player script de informatie over deze speler
            GameObject spelerClone = Instantiate(speler, spoor.transform);
            Player spelerCloneFiets = spelerClone.GetComponent<Player>();
            spelerCloneFiets.fietsnummer = index + 1;
            spelerCloneFiets.naam = playerNames[index];
            spelerCloneFiets.spelerMassa = 75;
            
            playerObjects.Add(spelerClone);
            orderedPlayers.Add(spelerCloneFiets);

            //Geef de speler zijn kleur, positie en schaal
            playerColors.Add(playerNames[index], colorOptions[index]);
            GameObject playerAnimation = Instantiate(orderedColorAnimationPrefabs[index], spelerClone.transform);
            playerAnimation.transform.localRotation = Quaternion.Euler(playerRotation);
            playerAnimation.transform.localPosition = playerPosition;
            playerAnimation.transform.localScale = playerScale;

            //Activeer de fiets in de Arduino en geef hem het geselecteerde vermogen
            communicationManager.SendPort(4, index + 1, 1);
            communicationManager.SendPort(6, index + 1, storageManagerScript.activePlayers[playerNames[index]]);

            //Creer een subdisplay voor de speler op het leaderboard
            leaderBoard.InstantiateSubdisplay();
        }

        //Instantieer de timer die aftelt naar het begin van de race
        timer = Instantiate(timerPrefab, FindObjectOfType<Canvas>().transform);
        timerScript = timer.GetComponent<RaceTimer>();                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                              
    }

    private void Update()
    {
        //Dit is de code voor het aftellen van de timer, geen idee waarom dit hier gebeurt en niet in het timerScript                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         "If you gaze long into an abyss, the abyss will gaze back into you"
        if (passedTime < secondsToWait)
        {
            int timeRemaining = (int)(secondsToWait - passedTime);
            timerScript.EditTimer(timeRemaining);
        }
        else
        {
            //Wanneer de teller is afgelopen start de race en wordt de timer vernietigd
            raceStarted = true;
            Destroy(timer);

            //De spelerBeweging3D van elke speler wordt geactiveerd
            foreach(GameObject player in playerObjects)
            {
                SpelerBeweging3D spelerBeweging = player.GetComponent<SpelerBeweging3D>();
                spelerBeweging.raceStarted = true;
            }
        }
        passedTime += Time.deltaTime;

        //Het leaderboard wordt georganiseerd
        //De lijst van de spelers wordt geordend op afgelegde afstand (deze lijst wordt gelezen door het leaderboard Script)
        orderedPlayers.Sort(SortByDistance);
        foreach (Player player in orderedPlayers)
        {
            //Elke string die moet getoond worden op het leaderboard wordt hiet al gemaakt
            player.rondesString = (player.laps + 1).ToString() + "/" + spoorManager.lapsToFinish.ToString();
        }

        //Hier wordt nagekeken of de race is afgelopen
        raceFinished = GetFinishStatus();
        if (raceFinished)
        {
            StopRace();
        }
    }

    //Is de race afgelopen?
    private bool GetFinishStatus()
    {
        bool raceFinishedInternal = false;

        //Ga ervan uit dat de race afgelopen is
        bool notFinishedDetected = false;

        //Overloop elke speler
        foreach (GameObject player in playerObjects)
        {
            //Kijk of deze speler niet (!) gefinisht is
            Player playerScript = player.GetComponent<Player>();
            if (playerScript.finished == false)
            {
                //Wanneer er een speler is die niet gefinisht is weten we dat de race niet gefinisht is
                notFinishedDetected = true;
            }
        }

        //Wanneer er een speler is die niet gefinisht is weten we dat de race niet gefinisht is
        if (notFinishedDetected)
        {
            //Dus race is nog niet afgelopen
            raceFinishedInternal = false;
        }
        else
        {
            //Iedereen is gefinisht? Dus race is afgelopen
            raceFinishedInternal = true;
        }

        return raceFinishedInternal;

        //Dit zou ook efficienter kunnen aangezien er toch al een lijst is van spelers geordend op afstand, hierin kan gewoon gekeken worden of de laatste speler gefinisht is
    }

    //OkButton om de race af te sluiten
    public void OkButtonClick()
    {
        //Gooi een bevestiginspopup
        popupManager = FindObjectOfType<PopupManager>();
        Popup popup = popupManager.CreateConfirmPopup(FindObjectOfType<Canvas>().transform, "Zeker dat u de race wilt stoppen?");
        popup.yesEvent.AddListener(StopRace);
    }

    //Getriggerd ofwel door afgelopen race ofwel door popup
    public void StopRace()
    {
        List<Player> players = new List<Player>();

        storageManager = GameObject.FindGameObjectWithTag("StorageManager");
        storageManagerScript = storageManager.GetComponent<StorageManager>();

        //Overloop alle spelers en neem hun Player object
        foreach (Player playerObject in FindObjectsOfType<Player>())
        {
            players.Add(playerObject);
        }
        
        //Geef deze lijst Player objecten door aan het StorageManager script om mee te nemen naar de volgende scene
        storageManagerScript.players = players;

        //laadt de volgende scene
        GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneManagerScript>().LoadLevel(levelToLoad);
    }

    static int SortByDistance(Player p1, Player p2)
    {
        return p2.afgeledeAfstand.CompareTo(p1.afgeledeAfstand); //Afgelegdeafstand p2 groter dan p1
    }
}
