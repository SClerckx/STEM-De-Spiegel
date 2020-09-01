using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapSelector : MonoBehaviour
{
    StorageManager storageManager;
    CommunicationManager communicationManager;
    PopupManager popupManager;
    PhysicsManager physicsManager;
    SceneManagerScript sceneManager;

    [Header("Maps")]
    MapData[] mapDatas;
    GameObject spawnedProp;
    int selectedMap;

    [Header("References")]
    public InputField aantalRondesInputField;
    public Text afstandPerRondeOutput;
    public Text totaleAfstandOutput;
    public Text kaartNaam;
    public Transform instellingenTransform;

    [HideInInspector]
    [Header("Selected")]
    float totalDistance;
    int numberOfLaps;

    void Start()
    {
        storageManager = FindObjectOfType<StorageManager>();
        communicationManager = FindObjectOfType<CommunicationManager>();
        popupManager = FindObjectOfType<PopupManager>();
        physicsManager = FindObjectOfType<PhysicsManager>();
        mapDatas = FindObjectsOfType<MapData>();
        sceneManager = FindObjectOfType<SceneManagerScript>();

        //Voeg luisterfunctie toe zodat wanneer er een nieuw aantal rondes wordt ingevoerd de NumberOfLapsSelected() functie wordt gerunt
        aantalRondesInputField.onValueChanged.AddListener(delegate
        {
            NumberOfLapsSelected();
        });
        //Selecteer als default optie 1 ronde
        aantalRondesInputField.text = 1.ToString();

        //Selecteer de eerste map in de lijst als default
        nextMap(0);
    }

    //Het gekozen aantal rondes is verandert
    public void NumberOfLapsSelected() //aantalRondesInputField.onValueChanged delegate
    {
        //Ga ervan uit dat de gegeven input juist is

        bool validInput = true;

        //Probeer de input te lezen en om te zetten in een cijfer
        try
        {
            numberOfLaps = int.Parse(aantalRondesInputField.text);
        }
        catch
        {
            validInput = false; //Wanneer dit niet lukt is de input niet juist 
        }

        //Ga na of het nummer binnen de toegelaten range is (1 tot 100)
        if (numberOfLaps < 1 || numberOfLaps > 100)
        {
            validInput = false;
        }

        //Is de input juist?
        if (validInput)
        {
            //Zo ja, pas de totale afstand die wordt getoond aan
            totalDistance = mapDatas[selectedMap].lapLength * numberOfLaps;
            totaleAfstandOutput.text = physicsManager.FormatDistance(totalDistance);
        }
        else
        {
            //Zo nee, gooi een waarschuwingspopup en zet terug naar de default 1 ronde
            popupManager.CreateWarningPopup(instellingenTransform.transform, "Geen geldig aantal rondes: 1-100 rondes");
            aantalRondesInputField.text = 1.ToString();
        }
    }

    //Selecteer de volgende map, wordt getriggerd door de Nextbutton
    public void nextMap(int step) 
    {
        //Ga een stap vooruit of achteruit (afhangend van de pijl die werd ingedrukt, pijl naar rechts heeft 1, pijl naar links -1)
        //Zorg dan dat de nieuwe waarde binnen de range van de list van de maps blijft liggen, doe anders van maximum naar minimum en omgekeerd
        selectedMap += step;
        if (selectedMap >= mapDatas.Length)
        {
            selectedMap = 0;
        }
        if (selectedMap < 0)
        {
            selectedMap = mapDatas.Length - 1;
        }

        //Instantieer de 3D representatie van de map
        //Als er al een was, verwijder deze
        if (spawnedProp != null)
        {
            Destroy(spawnedProp);
        }
        //Instantieer de nieuwe en pas rotatie, positie en schaal aan naar de waardes die staan opgeslagen in de mapdatas
        spawnedProp = Instantiate(mapDatas[selectedMap].mapProp, transform);
        spawnedProp.transform.localRotation = Quaternion.Euler(mapDatas[selectedMap].propRotation);
        spawnedProp.transform.localPosition = mapDatas[selectedMap].propPosition;
        spawnedProp.transform.localScale = mapDatas[selectedMap].propScale;

        //Pas de tekstelementen aan zodat alles juist is voor de nieuwe map
        kaartNaam.text = mapDatas[selectedMap].mapName; //Pas de kaartnaam aan
        afstandPerRondeOutput.text = physicsManager.FormatDistance(mapDatas[selectedMap].lapLength); //Pas de afstand per rondes aan (en vorm deze om naar een leesbaar vormaat aan de hand van de PhysicsManager)
        totaleAfstandOutput.text = physicsManager.FormatDistance(int.Parse(aantalRondesInputField.text) * mapDatas[selectedMap].lapLength); //Pas de totale afstand aan (en vorm deze om naar een leesbaar vormaat aan de hand van de PhysicsManager)
    }

    //Start de race door naar de volgende scene te gaan, getriggerd door de Okbutton
    public void StartRace() 
    {
        //Wijs het aantal geselecteerde rondes toe aan de geselecteerde mapdata en geef deze mapdata door aan de sceneManager om mee te nemen naar de volgende scene
        mapDatas[selectedMap].assignedLaps = numberOfLaps;
        storageManager.mapData = mapDatas[selectedMap];
        sceneManager.LoadLevel("Race3D");
    }
}

