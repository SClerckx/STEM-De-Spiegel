using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ResultsText : MonoBehaviour
{
    Player player;

    PhysicsManager physicsManager;

    public GameObject rankingNaam;
    Text rankingNaamText;

    public GameObject tijd;
    Text tijdText;

    public GameObject afstand;
    Text afstandText;

    public GameObject gemSnelheid;
    Text gemSnelheidText;

    public GameObject opgewekteEnergie;
    Text opgewekteEnergieText;

    private void Start()
    {
        UpdateText();
    }

    public void UpdateText()
    {
        player = GetComponent<Player>();
        physicsManager = FindObjectOfType<PhysicsManager>();

        rankingNaamText = rankingNaam.GetComponent<Text>();
        tijdText = tijd.GetComponent<Text>();
        afstandText = afstand.GetComponent<Text>();
        gemSnelheidText = gemSnelheid.GetComponent<Text>();
        opgewekteEnergieText = opgewekteEnergie.GetComponent<Text>();

        rankingNaamText.text = player.ranking.ToString() + ". " + player.naam.ToString();
        tijdText.text = physicsManager.FormatSeconds(player.verstrekenTijd);
        afstandText.text = physicsManager.FormatDistance(player.afgeledeAfstand);
        gemSnelheidText.text = physicsManager.FormatSpeed(player.afgeledeAfstand / player.verstrekenTijd);
        opgewekteEnergieText.text = physicsManager.FormatEnergy(player.opgewekteEnergie);
    }
}
