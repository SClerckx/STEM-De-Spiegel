using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LeesSpoorCoordinaten : MonoBehaviour
{
    public TextAsset coordinatenLijstText;
    StorageManager storageManager;

    public List<List<float>> VraagCoordinatenLijst()
    {
        List<List<float>> puntenLijst = new List<List<float>>();
        storageManager = FindObjectOfType<StorageManager>();

        //Vraag spoorcoordinaten .txt op van StorageManager
        coordinatenLijstText = storageManager.mapData.spoorCoordinaten;

        string text = coordinatenLijstText.text; 
        
        //Split de coordinaten van elkaar x,y,aa,ra/x,y,aa,ra --> [x,y,aa,ra,x,y,aa,ra]
        string[] dataLines = text.Split('/');     

        //Overloop alle coordinaten
        foreach (string dataLine in dataLines)
        {
            List<float> dataLijst = new List<float>();

            //Split de inividuele coordinaatgetallen van een coordinaat
            string[] dataPoints = dataLine.Split(',');

            //Overloop al deze getallen
            foreach(string dataPoint in dataPoints)
            {
                //Zet om van string naar floats en voeg ze toe aan een lijst van floats
                dataLijst.Add(float.Parse(dataPoint));
            }
            puntenLijst.Add(dataLijst);
        }

        return puntenLijst;
    }
}
