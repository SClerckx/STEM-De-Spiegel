using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spelerBeweging : MonoBehaviour
{
    RectTransform rectTransform;

    public bool race;
    public bool raceStarted = false;

    [Header("Player")]
    Player player;
    int spelerNummer; //krijgt bij instantiate van Player
    public float vermogen; //vraagt van Player (Nu nog even van SpelerManager)
    float spelerMassa; //krijgt bij instantiate van Player
    float totaalMassa;
    float laps = 0;
    public float afgelegdeAfstand = 0;
    float vorigeAfgelegdeAfstand = 0;
    float verstrekenTijd;
    float opgewekteEnergie;
    bool finished;

    [Header("SpelerInternal")]
    float afstandInRonde;
    float afgelegdeWegPercentueel;
    float snelheid = 0;
    float vorigeSnelheid = 0;
    

    [Header("Wrijving")]
    public float CdA = 0.30f;
    public float luchtDichtheid = 1.225f;
    public float MazieConstante = 3.6f;
    float wrijving;

    void Start()
    {
        player = GetComponent<Player>();
    }

    void FixedUpdate()
    {
        //Vraag van Player
        spelerNummer = player.fietsnummer;
        spelerMassa = player.spelerMassa;
        totaalMassa = spelerMassa + 15;
        vermogen = player.vermogen;
        afgelegdeAfstand = VraagAfgelegdeAfstand(vermogen);

        verstrekenTijd += Time.deltaTime;

        //Energie
        opgewekteEnergie += vermogen * Time.fixedDeltaTime;

        //Zet in spelerfiets
        player.laps = laps;
        player.finished = finished;
        player.verstrekenTijd = verstrekenTijd;
        player.afgeledeAfstand = afgelegdeAfstand;
        player.opgewekteEnergie = opgewekteEnergie;
        player.fietsSnelheid = (afgelegdeAfstand - vorigeAfgelegdeAfstand) / Time.fixedDeltaTime;

        vorigeAfgelegdeAfstand = afgelegdeAfstand;
    }

    //Zie spelerBeweging3D 
    float VraagAfgelegdeAfstand(float huidigVermogen)
    {
        if (vorigeSnelheid > 0)
        {
            wrijving = 0.5f * luchtDichtheid * Mathf.Pow(vorigeSnelheid, 2) * CdA + MazieConstante;
        }
        else
        {
            wrijving = 0;
        }

        if (spelerMassa != 0 && vorigeSnelheid != 0)
        {
            snelheid = (vermogen / (totaalMassa * vorigeSnelheid) - (wrijving / totaalMassa)) * Time.fixedDeltaTime + vorigeSnelheid; // ve = (P/(m*v0) - Fw/m)   * dt + v0
        }
        else
        {
            snelheid = 0.1f;
        }

        if (snelheid < 0 || vermogen < 10)
        {
            snelheid = 0;
        }

        afgelegdeAfstand += snelheid * Time.fixedDeltaTime;
        vorigeSnelheid = snelheid;
        
        return afgelegdeAfstand;
    }
}
