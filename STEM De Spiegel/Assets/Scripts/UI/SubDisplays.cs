using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubDisplays : MonoBehaviour
{
    public GameObject subDisplayPrefab;

    public bool spawnOnStart = true;
    public int maxScreens = 6;
    public int minScreens = 1;

    public bool horizontal = true;

    GameObject storageManager;
    StorageManager storageManagerScript;

    public List<GameObject> subDisplays = new List<GameObject>();

    RectTransform displayRectTransform;

    private void Start()
    {
        //Wanneer er bij het laden van de scene SubDisplays moeten worden gemaakt, blijf dit doen totdat het minimale aantal is berijkt
        while (subDisplays.Count < minScreens && spawnOnStart)
        {
            InstantiateSubdisplay();
        }
    }

    //Pas grootte en plaats van persoondisplays aan aan hoeveel er zijn
    public void UpdatePersoondisplays() 
    {
        displayRectTransform = GetComponent<RectTransform>();
        
        int subDisplayCount = subDisplays.Count;
        int item = 0;

        //Horizontale verdeling of verticale verdiling?
        if (horizontal)
        {
            //Horizontale verdeling
            //Overloop elke SubDisplay
            foreach (GameObject subDisplay in subDisplays)
            {
                RectTransform rectTrans = subDisplay.GetComponent<RectTransform>();

                //Bereken de breedte die het moet krijgen (kan ook buiten de foreach loop want is overal hetzelfde)
                float wantedWidth = displayRectTransform.rect.width / subDisplayCount;

                //Verander de x positie naar i * de gewilde breedte
                rectTrans.anchoredPosition = new Vector3(item * wantedWidth, 0, 0);

                //Verander de grootte naar de gewilde breedte en de hoogte van de parent
                rectTrans.sizeDelta = new Vector3(wantedWidth, displayRectTransform.rect.height, 0);

                item += 1;
            }
        }
        else
        {
            //Verticale verdeling
            //Overloop elke SubDisplay
            foreach (GameObject subDisplay in subDisplays)
            {
                RectTransform rectTrans = subDisplay.GetComponent<RectTransform>();

                //Bereken de hooghte die het moet krijgen (kan ook buiten de foreach loop want is overal hetzelfde)
                float wantedHeight = displayRectTransform.rect.height / subDisplayCount;

                //Verander de y positie naar i * de gewilde hoogte
                rectTrans.anchoredPosition = new Vector3(0, item * wantedHeight, 0);

                //Verander de grootte naar de breedte van de parent en de gewilde hoogte
                rectTrans.sizeDelta = new Vector3(displayRectTransform.rect.width, wantedHeight, 0);

                item += 1;
            }
        }
    }

    //Maak een nieuwe SubDisplay
    public void InstantiateSubdisplay()
    {
        //Check of dat het maximum displays niet is bereikt
        if (subDisplays.Count <= maxScreens - 1)
        {
            //Maak een nieuw SubDisplay, geef het dit als parent en voeg het toe aan de lijst van Subdisplays
            GameObject newSubDisplay = Instantiate(subDisplayPrefab, transform);
            newSubDisplay.transform.SetParent(transform, false);
            subDisplays.Add(newSubDisplay);

            //Orden alle SubDisplays, rekening houdend met de nieuwkomer
            UpdatePersoondisplays();
        }
    }

    //Verwijder een SubDisplay
    public void RemoveSubDisplay(GameObject subDisplay)
    {
        //Haal uit de lijst van SubDisplays
        subDisplays.Remove(subDisplay);

        //Orden alle SubDisplays
        UpdatePersoondisplays();

        //Verwijder de SubDisplay
        Destroy(subDisplay);
    }
}
