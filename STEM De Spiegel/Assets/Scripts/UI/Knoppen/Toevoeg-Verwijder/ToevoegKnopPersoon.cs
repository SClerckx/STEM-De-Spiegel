using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToevoegKnopPersoon : MonoBehaviour {

    public GameObject PersoonDropdown;
    GameObject nieuwePersoonDropdown;

    GameObject Knoppen; //Parent van deze toevoegknop

    RectTransform rectTransform;
    Vector3 localPos;

    public int offset = 50;
    int buttonPresses;
    public int maxSpelers = 6;

    // Use this for initialization
    void Start () {
        Knoppen = transform.parent.gameObject;
        rectTransform = gameObject.GetComponent<RectTransform>();
    }
	
	// Update is called once per frame
	void Update () {

    }

    public void ButtonPress()
    {
        buttonPresses += 1;

        if (buttonPresses <= maxSpelers - 2)
        {
            localPos = rectTransform.localPosition;

            nieuwePersoonDropdown = Instantiate(PersoonDropdown, localPos, Quaternion.identity, Knoppen.transform);
            nieuwePersoonDropdown.transform.localPosition = localPos;

            rectTransform.localPosition = new Vector3(localPos.x, localPos.y - offset, localPos.z);

            if (buttonPresses == maxSpelers - 2)
            {
                Destroy(gameObject);
            }
        }
    }
}
