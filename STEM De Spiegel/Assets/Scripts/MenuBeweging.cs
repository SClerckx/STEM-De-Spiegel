using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBeweging : MonoBehaviour
{
    public int i;

    [Header("Spoor/spelermanager")]
    GameObject spoor; //parent object, bevat spelermanager
    DecorativeRace decorativeRace; //in spoor
    float lapLength = 1000; //in spoorManager
    public Vector2 scale; // in spoorManager
    public List<List<float>> puntenLijst = new List<List<float>>();
    Vector3 nextCoordinate = new Vector3();

    [Header("Player")]
    Player player;
    public float vermogen; //vraagt van Player (Nu nog even van SpelerManager)
    float spelerMassa = 75; //krijgt bij instantiate van Player
    public float afgelegdeAfstand = 0;

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
        decorativeRace = FindObjectOfType<DecorativeRace>();
        player = GetComponent<Player>();

        lapLength = decorativeRace.lapLength;
        puntenLijst = decorativeRace.VraagCoordinatenLijst();
        scale = decorativeRace.scale;

        spelerMassa = Random.Range(50, 100);
        vermogen = Random.Range(50, 400);
    }

    void FixedUpdate()
    { 
        afgelegdeAfstand = VraagAfgelegdeAfstand(vermogen);

        //Afstandinronde om laps mogelijk te maken
        afstandInRonde = afgelegdeAfstand;

        while (afstandInRonde > lapLength)
        {
            afstandInRonde = afstandInRonde - lapLength;
        }

        afgelegdeWegPercentueel = afstandInRonde / lapLength;

        i = 0;
        while (puntenLijst[i][3] < afgelegdeWegPercentueel && i < puntenLijst.Count - 2)
        {
            i += 1;
        }

        //Rotatie
        if (i + 1 < puntenLijst.Count)
        {
            nextCoordinate = new Vector3(puntenLijst[i + 1][0], 0, puntenLijst[i + 1][1]);
            transform.rotation = Quaternion.LookRotation(Vector3.up, nextCoordinate - new Vector3(puntenLijst[i][0], 0, puntenLijst[i][1]));
        }

        Vector3 perpindicularVector = Vector3.Cross(transform.forward, transform.up);
        transform.localPosition = new Vector3(puntenLijst[i][0] * scale.x, 0, puntenLijst[i][1] * scale.y) + perpindicularVector * (player.fietsnummer - 3);
        
        //Snelheid
        fietsSnelheid = (afgelegdeAfstand - vorigeAfgelegdeAgstand) / Time.deltaTime;

        player.fietsSnelheid = fietsSnelheid;

        vorigeAfgelegdeAgstand = afgelegdeAfstand;
    }

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
            snelheid = (vermogen / (spelerMassa * vorigeSnelheid) - (wrijving / spelerMassa)) * Time.fixedDeltaTime + vorigeSnelheid; // ve = (P/(m*v0) - Fw/m)   * dt + v0
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
