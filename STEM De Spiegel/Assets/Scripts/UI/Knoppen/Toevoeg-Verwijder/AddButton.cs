using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddButton: MonoBehaviour {

    GameObject display; //Parent van deze toevoegknop
    SubDisplays subDisplayScript;

    public void ButtonPress()
    {
        display = transform.parent.gameObject;
        subDisplayScript = display.GetComponent<SubDisplays>();
        subDisplayScript.InstantiateSubdisplay();
    }
}
