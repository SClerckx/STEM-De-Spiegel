using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorativeRace : MonoBehaviour
{
    public TextAsset coordinatenLijstText;

    [Header("Spoor")]
    public float lapLength;
    public float lapsToFinish;
    //public float totalDistance;

    public List<List<float>> puntenLijst = new List<List<float>>(); //[[relx,rely,dnextrel,dtrel],...]
    //public List<Vector2> spoorCoordinaten = new List<Vector2>(); //(x,y)
    public Vector2 scale = new Vector2();

    public List<List<float>> VraagCoordinatenLijst()
    {
        List<List<float>> puntenLijst = new List<List<float>>();

        string text = coordinatenLijstText.text;  //this is the content as string
        string[] dataLines = text.Split('/');      //"512,1230

        foreach (string dataLine in dataLines)
        {
            List<float> dataLijst = new List<float>();
            string[] dataPoints = dataLine.Split(',');

            foreach (string dataPoint in dataPoints)
            {
                dataLijst.Add(float.Parse(dataPoint));
            }
            puntenLijst.Add(dataLijst);
        }

        return puntenLijst;
    }

    /*
    public List<Vector2> GetTrack()
    {
        puntenLijst = VraagCoordinatenLijst();

        int i = 0;
        foreach (List<float> dataLijst in puntenLijst)
        {
            float x = dataLijst[0];
            float y = dataLijst[1];
            Vector2 punt = new Vector2(x, y);
            i += 1;
        }

        return puntenLijst;
    }*/
}
