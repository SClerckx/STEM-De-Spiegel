using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpoorManager3D : MonoBehaviour
{
    StorageManager storageManager;

    [Header("Spoor")]
    public float lapLength;
    public float lapsToFinish;

    public List<List<float>> puntenLijst = new List<List<float>>(); //[[relx,rely,dnextrel,dtrel],...]
    public Vector2 scale = new Vector2();

    public void PrepareTrack()
    {
        storageManager = FindObjectOfType<StorageManager>();
        //Vraag de lengte van een ronde en aantal rondes aan StorageManager
        lapLength = storageManager.mapData.lapLength;
        lapsToFinish = storageManager.mapData.assignedLaps;

        //Vraag gelezen gegevens uit de .txt aan de LeesSpoorCoorDinaten
        puntenLijst = gameObject.GetComponent<LeesSpoorCoordinaten>().VraagCoordinatenLijst();

        int i = 0;
        //Overloop elk coordinaat uit de LeesSpoorCoordinaten
        foreach (List<float> dataLijst in puntenLijst)
        {
            //Maak een lijst met alleen x en y: [x,y,...]
            float x = dataLijst[0];
            float y = dataLijst[1];
            Vector2 punt = new Vector2(x, y);
            i += 1;
        }
    }
}
