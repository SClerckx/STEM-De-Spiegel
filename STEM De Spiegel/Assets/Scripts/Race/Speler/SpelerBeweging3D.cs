using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpelerBeweging3D : MonoBehaviour
{
    public bool raceStarted = false;
    public int i;
    public bool manualMode = false;

    [Header("Spoor/spelermanager")]
    GameObject spoor; //parent object, bevat spelermanager
    RaceManager raceManager; //spelermanager, in spoor
    SpoorManager3D spoorManager; //in spoor
    float lapLength; //in spoorManager
    float lapsToFinish; // in spoorManager
    public Vector2 scale; // in spoorManager
    public List<List<float>> puntenLijst = new List<List<float>>();
    Vector3 nextCoordinate = new Vector3();

    [Header("Player")]
    Player player;
    int spelerNummer; //krijgt bij instantiate van Player
    public float vermogen; //vraagt van Player (Nu nog even van SpelerManager)
    float spelerMassa = 75; //krijgt bij instantiate van Player
    float totaalMassa;
    float laps = 0;
    public float afgelegdeAfstand = 0;
    float verstrekenTijd;
    float opgewekteEnergie;
    bool finished;

    [Header("SpelerInternal")]
    float afstandInRonde;
    float afgelegdeWegPercentueel;
    float snelheid = 0;
    float vorigeSnelheid = 0;
    float fietsSnelheid;
    float vorigeAfgelegdeAgstand;

    [Header("Wrijving")]
    public float CdA = 0.30f;
    public float luchtDichtheid = 1.225f;
    public float MazieConstante = 3.6f;
    float wrijving;

    void Start()
    {
        raceManager = FindObjectOfType<RaceManager>();
        spoorManager = FindObjectOfType<SpoorManager3D>();
        player = GetComponent<Player>();

        //Vraag spoorinformatie aan Spoormanager
        lapLength = spoorManager.lapLength;
        lapsToFinish = spoorManager.lapsToFinish;
        puntenLijst = spoorManager.puntenLijst;
        scale = spoorManager.scale;
    }

    void FixedUpdate()
    {
        //Vraag spelerinformaatie aan Speler (kan ook in Start)
        spelerNummer = player.fietsnummer;
        spelerMassa = player.spelerMassa;
        totaalMassa = spelerMassa + 15;

        //Check of de race is gestart
        if (raceStarted)
        {
            //Check of de race in manualmode staat (debug mode om te testen zonder Arduino)
            if (!manualMode)
            {
                //Wanneer in normale mode, neem gewoon het vermogen van de speler
                vermogen = player.vermogen;
            }
            else
            {
                //Wanneer niet in manualmode, geef spelers een vermogen gebaseerd op hun fietsnummer
                vermogen = player.fietsnummer * 100;
            }
        }
        else
        {
           vermogen = 0;
        }

        afgelegdeAfstand = VraagAfgelegdeAfstand(vermogen);

        //Afstandinronde om laps mogelijk te maken
        afstandInRonde = afgelegdeAfstand;

        //Bereken het aantal rondes dat de speler heeft gemaakt
        laps = Mathf.Floor(afgelegdeAfstand / lapLength);

        //Bereken nu de afstand in de laatste ronde door de afstand van een ronde van de totale afstand van de speler te blijven aftrekken totdat de afstandInRonde kleiner is dan de afstand van een ronde
        //Bijvoorbeeld:
        //Afstand die de speler heeft afgelegd: 2432 m
        //Afstand van een ronde: 1000m
        //Trek twee keer 1000m af van 2432m om de afstand die de speler nu heeft in de 3e ronde te bekomen: 432m
        while (afstandInRonde > lapLength)
        {
            afstandInRonde = afstandInRonde - lapLength;
        }

        //Bereken nu de relatieve weg die de speler heeft afgelegd tenopzichte van het einde van de ronde
        //Dit gaat dus altijd van 0 tot 1 gaan: als de speler de helft van de ronde heeft afgelegd is deze waarde 0.5
        afgelegdeWegPercentueel = afstandInRonde / lapLength;

        i = 0;
        //Overloop de coordinatenlijst van Python totdat een grotere relatieve afstand wordt bereikt en onthoud het nummer van het punt ervoor
        while (puntenLijst[i][3] < afgelegdeWegPercentueel && i < puntenLijst.Count - 2)
        {
            i += 1;
        }

        //Check ofdat de speler gefinisht is
        if (afgelegdeAfstand >= lapLength * lapsToFinish)
        {
            finished = true;
        }
        else
        {
            //Zolang de speler niet gefinisht is loopt zijn tijd op
            verstrekenTijd += Time.fixedDeltaTime;
        }

        //Draai de speler in de richting van het punt voor hem (zolang dit punt nog binnen het bereik van de puntenlijst valt)
        if (i + 1 < puntenLijst.Count)
        {
            //Bereken het punt voor de speler
            nextCoordinate = new Vector3(puntenLijst[i + 1][0], 0, puntenLijst[i + 1][1]);

            //Draai de speler naar dit punt
            transform.rotation = Quaternion.LookRotation(Vector3.up, nextCoordinate - new Vector3(puntenLijst[i][0], 0, puntenLijst[i][1]));
        }

        //Bereken de positie van de speler door:

        //1) De vector te bepalen die loodrecht staat op de speler
        Vector3 perpindicularVector = Vector3.Cross(transform.forward, transform.up);

        //2) De positie van het coordinaat op het spoor te nemen en hierbij die loodrechte vector, vermenigvuldigt door het spelernummer van de fiets (min het aantal spelers gedeeld door 2), op te tellen
        //Die loodrechte vector is nodig om de spelers te verdelen over het spoor:
        //    spoor                                                                                             ~+
        //      |                                                                                                        *       +
        //  * * * * * <-- spelers ----- <-- loodrechte vector                                                       '                  | 
        //      |                                                                                               ()    .-.,="``"=.    - o -    
        // Die 'min het aantal spelers gedeeld door 2' zodat het niet dit is:                                         '=/_       \     |     
        //      |                                                                                                  *   |  '=._    |
        //      * * * * * <-- spelers ----- <-- loodrechte vector                                                       \     `=./`,        '   
        //      |                                                                                                    .   '=.__.=' `='      *
        // Dit allemaal om onderstaande te voorkomen:                                                         +                         +  
        //      |                                                                                                  O      *        '       .
        //      *     <-- meerdere spelers op een hoopje                                                        
        //      |
        transform.localPosition = new Vector3(puntenLijst[i][0] * scale.x, 0, puntenLijst[i][1] * scale.y) + perpindicularVector * (player.fietsnummer - raceManager.orderedPlayers.Count /2);

        //Bereken de opgewekte energie door het vermogen te vermenigvuldigen met de tijdsduur van een frame
        opgewekteEnergie += vermogen * Time.fixedDeltaTime;

        //Bereken de snelheid
        fietsSnelheid = (afgelegdeAfstand - vorigeAfgelegdeAgstand) / Time.deltaTime;

        //Zet al de berekende waardes in het Player script
        player.finished = finished;
        if (!finished)
        {
            player.laps = laps;
            player.verstrekenTijd = verstrekenTijd;
            player.afgeledeAfstand = afgelegdeAfstand;
            player.opgewekteEnergie = opgewekteEnergie;
            player.fietsSnelheid = fietsSnelheid;
        }

        vorigeAfgelegdeAgstand = afgelegdeAfstand;
    }

    //Bereken de afgelegde afstand in een frame aan de hand van het vermogen en tel dit op bij de oude afstand
    float VraagAfgelegdeAfstand(float huidigVermogen)
    {
        if (vorigeSnelheid > 0)
        {
            //Formule voor wrijving: 0.5 * rho * v^2 * Cd * A (luchtwrijving)
            //MazieConstante = bepaalde constante voor rolwrijving
            wrijving = 0.5f * luchtDichtheid * Mathf.Pow(vorigeSnelheid, 2) * CdA + MazieConstante;
        }
        else
        {
            wrijving = 0;
        }

        if (spelerMassa != 0 && vorigeSnelheid != 0) //Anders delen door nul
        {
            snelheid = (vermogen / (totaalMassa * vorigeSnelheid) - (wrijving / totaalMassa)) * Time.fixedDeltaTime + vorigeSnelheid; // ve = (P/(m*v0) - Fw/m)   * dt + v0
        }
        else
        {
            snelheid = 0.1f;
        }

        if (snelheid < 0 || vermogen < 10) //High fidelity simulatie van een speler die omvalt
        {
            snelheid = 0;
        }

        afgelegdeAfstand += snelheid * Time.fixedDeltaTime;
        vorigeSnelheid = snelheid;

        return afgelegdeAfstand;
    }
}
