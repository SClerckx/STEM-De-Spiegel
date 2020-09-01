using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using UnityEngine.UI;

public class Buttonlistner : MonoBehaviour {

    int recieved;
    public Text text;
    public Button button;
    public string printstring;
    LEDTrigger lEDTrigger;
    string[] portNames;

    // Use this for initialization
    void Start()
    {
        portNames = SerialPort.GetPortNames();
        foreach (string portName in portNames)
        {



            //.Log(portName);
        }
        
        lEDTrigger = button.GetComponent<LEDTrigger>();
    }

    // Update is called once per frame
    void Update()
    {
        recieved = lEDTrigger.recieved;
        if (recieved == 5)
        {
            printstring = "No button";
        }
        else
        {
            printstring = recieved.ToString();
        }
        text.text = printstring;
    }
}
