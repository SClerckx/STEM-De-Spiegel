using UnityEngine;
using System.IO.Ports;
using UnityEngine.UI;


public class LEDTrigger : MonoBehaviour {
    string stringToSend;
    public Text text;
    public int recieved;

    SerialPort serialPort = new SerialPort("COM4", 9600);

    // Use this for initialization
    void Start () {
        serialPort.ReadTimeout = 16;
    }

    public void Update()
    {
        if (!serialPort.IsOpen)
        {
            serialPort.Open();
            Debug.Log("Port opened");
        }

        try
        {
            recieved = serialPort.ReadByte();
            Debug.Log("Recieved");
        }
        catch (System.Exception)
        {
            throw;
        }
    }

    // Update is called once per frame
    public void ButtonClick()
    {
        stringToSend = text.text;
        Debug.Log("Buttonclick");
        if (!serialPort.IsOpen)
        {
            serialPort.Open();
            Debug.Log("Port opened");
        }
        serialPort.Write(stringToSend);
        Debug.Log("Sent test signal");
    }
}
