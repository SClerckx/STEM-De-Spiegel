using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.Threading;
using Microsoft.Win32;
using UnityEngine.UI;

public class CommunicationManager : MonoBehaviour
{
    [Header("Communicatie instellingen")]
    public string prevOntvangen;
    public string ontvangen;
    public string[] ontvangenGesplitst;
    public int ontvangenGetal;
    public int leesTimeout = 16;
    public int boudRate = 9600;
    public string serialPortNaam = "COM6";
    public string arduinoNaam = "CH";
    public Thread arduinoReadThread;

    string[] serialPorts;
    SerialPort serialPort = new SerialPort();

    public bool IsActive = true;

    //ONTVANGEN
    //commando,fietsnummer,getal/

    //Commando:
    //1 snelheid
    //2 vermogen

    //fietsnummer:
    //van 1 tot 6, niet van 0 tot 5!

    //getal mag alles zijn

    // / om bericht te sluiten

    //VERZONDEN
    //commando,fietsnummer,getal/

    //Commando:
    //1 activeer training
    //2 activeer race
    //3 activeer hoofdmenu
    //4 activeer of deactiveer fiets (1 als getal = activeer, 0 als getal = deactiveer)
    //5 pas vermogen aan
    //6 pas weerstand aan

    //fietsnummer:
    //van 1 tot 6, niet van 0 tot 5!

    //getal mag alles zijn

    // / om bericht te sluiten

    void Start()
    {
        //Zoek naar COM poort met Arduino met AutodetectArduinoPort(), als niet gevonden (null) probeer de voorag ingestelde poort 
        if (AutodetectArduinoPort() != null)
        {
            serialPortNaam = BewerkPoortNaam(AutodetectArduinoPort()); //Arduino poort gevonden, deze moet soms nog bewerkt worden naar leesbaar formaat met BewerkPoortNaam()
        }
        else
        {
            Debug.Log("NO ARDUINO FOUND: TRYING DEFAULT OPTION: " + serialPortNaam);
        }
        DontDestroyOnLoad(gameObject);

        //Maak nieuwe serialpoort met de gevonden naam van hierboven
        serialPort = new SerialPort(serialPortNaam, boudRate);
        serialPort.ReadTimeout = leesTimeout;

        //Maak en start nieuwe Thread met de LeesPoort functie
        arduinoReadThread = new Thread(new ThreadStart(leesPort));
        arduinoReadThread.IsBackground = true;
        arduinoReadThread.Start();
    }

    void Update()
    {
        //Als communicatie is geactiveerd (wordt manueel gedeactiveerd in inspector om programma te testen zonder communicatie en wordt gedeactiveerd voor het sluiten van het programma)
        if (IsActive)
        {
            //Als er iets is ontvangen
            if (ontvangen != null || ontvangen != "")
            {
                //Split ontvangen string op komma's, dit geeft een array van gesplitste strings. Voorbeeld van Ontvangen: 0,3,210 Voorbeeld van ontvangenGesplitst: [0,3,210]
                ontvangenGesplitst = ontvangen.Split(char.Parse(","));
            }
            else
            {
                //Debug.Log("Nothing recieved");
            }
        }
    }

    //Dit is de functie voor het lezen van de aangewezen poort. Deze wordt uitgevoerd op een aparte Thread
    public void leesPort()
    {
        //Als communicatie is geactiveerd (wordt manueel gedeactiveerd in inspector om programma te testen zonder communicatie en wordt gedeactiveerd voor het sluiten van het programma)
        while (IsActive)
        {
            //Probeer serialport te openen, wanneer niet succesvol probeer te sluiten en gooi Debug
            try
            {
                //Als serialport niet open is open
                if (!serialPort.IsOpen)
                {
                    serialPort.Open();
                }
            }
            catch
            {
                serialPort.Close();
                Debug.Log("CATCH OPEN Port Not open: " + serialPort.PortName);
            }

            //Probeer serialport te lezen, wanneer unsuccesvol (heel vaak) doe niets
            try
            {
                ontvangen = serialPort.ReadLine();
                Debug.Log(ontvangen);

                //houd ontvangen variabele altijd op het laatste zinnige ontvangen signaal, vaak komen lege of onleesbare signalen binnen 
                //en het doel is deze te negeren en dus niet in de ontvangen variabele te zetten
                //concreet: is ontvangen signaal zinnig? Zo ja, blijf gebruiken en sla op in prevOntvangen. Zo nee, gebruik vorig zinnig signaal (prevOntvangen)
                if (ontvangen != "" || ontvangen == null)
                {
                    prevOntvangen = ontvangen;
                    Debug.Log(ontvangen);
                }
                else
                {
                    ontvangen = prevOntvangen;
                }

                //Debug.Log("Ontvangen in thread: " + ontvangen);
            }
            catch
            {
                //Debug.Log("CATCH ONTVANGEN: " + serialPort.PortName);
                //ontvangen = null;
            }
        }
    }

    //Functie voor het schrijven naar de com poort
    public void SendPort(int commando, int fietsNummer, int toSend)
    {

        //Als communicatie is geactiveerd (wordt manueel gedeactiveerd in inspector om programma te testen zonder communicatie en wordt gedeactiveerd voor het sluiten van het programma)
        if (IsActive)
        {
            //Als serialport niet open is open (hier niet de try catch want LeesPort wordt voor deze functie uitgevoerd dus is de poort eigenlijk gegarandeerd open, 
            //wanneer dit niet zo is is alles toch al ferm in de soep gedraaid)
            if (!serialPort.IsOpen)
            {
                serialPort.Open();
            }

            string teVerzendenString;

            //voeg argumenten samen tot een formaat dat de Arduino kan lezen: Voorbeeld: 5,2,100/ Betekenis: pas vermogen aan van fiets 2 naar 100 watt
            teVerzendenString = commando.ToString() + "," + fietsNummer.ToString() + "," + toSend.ToString() + ',' + '/';
            Debug.Log("Verzonden:" + teVerzendenString);

            //verzend string
            serialPort.Write(teVerzendenString);
        }
    }

    //Functie voor het leesbaar maken van sommige poorten: Com poorten met getallen hoger dan 9 moeten een speciale opmaak krijgen, deze functie zorgt hiervoor
    public string BewerkPoortNaam(string serialPoortNaam)
    {
        serialPortNaam.Trim(); //Verwijder spaties
        string serialPoortNaamInternal;

        //Split poortnaam na de M en lees het nummer hierachter. Bijvoorbeeld: COM4 --> [COM,4] --> lees 4
        string[] serialPoortNaamGesplitst = serialPoortNaam.Split('M');
        int serialPoortNummer = int.Parse(serialPoortNaamGesplitst[1]);

        //Als het getal hoger is dan 9, voeg \\.\ voor de naam (buiten comments tellen twee \\ voor één 'echte' \)
        if (serialPoortNummer > 9)
        {
            serialPoortNaamInternal = "\\\\.\\" + serialPoortNaam;
            Debug.Log("Bewerkpoortnaam:" + serialPoortNaamInternal);
        }
        else
        {
            serialPoortNaamInternal = serialPoortNaam;
        }

        return serialPoortNaamInternal;
    }

    public void SendRaceActivation()
    {
        SendPort(2, 0, 0); //2,0,0,/ activeer race, 4,1,1,/ activeer fiets, 6,1,1,/ pas weerstand aan
    }

    public void SendTrainingActivation()
    {
        SendPort(1, 0, 0); //1,0,0,/ activeer training, 4,1,1,/ activeer fiets, 5,1,100,/ pas vermogen aan
    }

    public void SendMenuActivation()
    {
        SendPort(3, 0, 0); //3,0,0 activeer hoofdmenu
    }

    //Dit is de functie waarmee de poort van de Arduino wordt gezocht. Deze werkt door het registry van windows te doorzoeken op de naam van de arduino
    //Ik heb deze functie van het internet geplukt en gebruikt als een doosje magie waar ik gewoon input in steek en output uit krijg, zorg dat je goed weet wat je doet als je hierin gaat rommelen
    public string AutodetectArduinoPort()
    {
        List<string> comports = new List<string>();
        RegistryKey rk1 = Registry.LocalMachine;
        RegistryKey rk2 = rk1.OpenSubKey("SYSTEM\\CurrentControlSet\\Enum");
        string temp;
        foreach (string s3 in rk2.GetSubKeyNames())
        {
            RegistryKey rk3 = rk2.OpenSubKey(s3);
            foreach (string s in rk3.GetSubKeyNames())
            {
                if (s.Contains("VID") && s.Contains("PID"))
                {
                    RegistryKey rk4 = rk3.OpenSubKey(s);
                    foreach (string s2 in rk4.GetSubKeyNames())
                    {
                        RegistryKey rk5 = rk4.OpenSubKey(s2);
                        if ((temp = (string)rk5.GetValue("FriendlyName")) != null && temp.Contains(arduinoNaam)) //Hier is het enige wat ik heb veranderd, de term waar naar werd gezocht een variabele string gemaakt zodat die makkelijk aanpasbaar is
                        {
                            RegistryKey rk6 = rk5.OpenSubKey("Device Parameters");
                            if (rk6 != null && (temp = (string)rk6.GetValue("PortName")) != null)
                            {
                                comports.Add(temp);
                            }
                        }
                    }
                }
            }
        }

        if (comports.Count > 0)
        {
            foreach (string s in SerialPort.GetPortNames())
            {
                if (comports.Contains(s))
                    return s;
            }
        }

        return null;
    }

    //Deze functie wordt getriggerd bij het afsluiten van het programma
    private void OnApplicationQuit()
    {
        //Hier worden de serialport gesloten en zijn er twee maatregelen om proberen de Thread te sluiten
        serialPort.Close();

        //De IsActive wordt gedactiveerd om de while loop te onderbreken in de arduinoReadThread zodat .Abort() werkt
        IsActive = false; //Dit kan mooier met een tweede bool en een return of abort statement in de Thread zelf maar dit werkt ook.

        //Nu de Thread niets meer doet kan deze gesloten worden 
        arduinoReadThread.Abort();
    }
}
