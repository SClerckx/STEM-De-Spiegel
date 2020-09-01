using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveButton : MonoBehaviour
{
    GameObject selector; //parent
    GameObject display; //parent of parent

    // Update is called once per frame
    public void ButtonPress()
    {
        selector = transform.parent.gameObject;
        display = selector.transform.parent.gameObject;
        display.GetComponent<SubDisplays>().RemoveSubDisplay(selector);
    }
}
