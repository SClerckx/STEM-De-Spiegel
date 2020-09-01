using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class PersoonText : MonoBehaviour
{
    Player player;

    PhysicsManager physicsManager;

    public GameObject naam;
    Text naamText;

    public GameObject afgeledeAfstand;
    Text afgeledeAfstandText;

    public GameObject opgewekteEnergie;
    Text opgewekteEnergieText;

    public GameObject vermogen;
    Text vermogenText;

    public GameObject verstrekenTijd;
    Text verstrekenTijdText;

    public GameObject snelheid;
    Text snelheidText;

    public GameObject fietsNummer;
    Text fietsNummerText;

    // Start is called before the first frame update
    void Start()
    {
        player = transform.parent.GetComponent<Player>();
        physicsManager = FindObjectOfType<PhysicsManager>();

        naamText = naam.GetComponent<Text>();
        fietsNummerText = fietsNummer.GetComponent<Text>();
        afgeledeAfstandText = afgeledeAfstand.GetComponent<Text>();
        opgewekteEnergieText = opgewekteEnergie.GetComponent<Text>();
        vermogenText = vermogen.GetComponent<Text>();
        verstrekenTijdText = verstrekenTijd.GetComponent<Text>();
        snelheidText = snelheid.GetComponent<Text>();

        naamText.text = player.naam;
    }

    // Update is called once per frame
    void Update()
    {
        fietsNummerText.text = player.fietsnummer.ToString();
        afgeledeAfstandText.text = physicsManager.FormatDistance(player.afgeledeAfstand);
        opgewekteEnergieText.text = physicsManager.FormatEnergy(player.opgewekteEnergie);
        vermogenText.text = player.vermogen.ToString() + " w";
        verstrekenTijdText.text = physicsManager.FormatSeconds(player.verstrekenTijd);
        snelheidText.text = physicsManager.FormatSpeed(player.fietsSnelheid);
    }
}
