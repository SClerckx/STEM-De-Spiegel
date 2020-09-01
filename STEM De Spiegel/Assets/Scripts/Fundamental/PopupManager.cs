using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PopupManager : MonoBehaviour
{
    public UnityEvent triggerEvent;

    public GameObject warningPopup;
    public GameObject feedbakcPopup;
    public GameObject confirmPopup;

    public Popup CreateCustomPopup(GameObject popupPrefab, Transform popupParentTransform)
    {
        GameObject popupClone = Instantiate(popupPrefab, popupParentTransform);
        Popup popupScript = popupClone.GetComponent<Popup>();
        return popupScript;
    }

    public Popup CreateConfirmPopup(Transform popupParentTransform, string text)
    {
        GameObject popupClone = Instantiate(confirmPopup, popupParentTransform);
        Popup popupScript = popupClone.GetComponent<Popup>();
        popupScript.gameObject.GetComponentInChildren<Text>().text = text;
        return popupScript;
    }

    public void CreateWarningPopup(Transform popupParentTransform, string text)
    {
        GameObject popupClone = Instantiate(warningPopup, popupParentTransform);
        Popup popupScript = popupClone.GetComponent<Popup>();
        popupScript.gameObject.GetComponentInChildren<Text>().text = text;
    }

    public void CreateFeedbackPopup(Transform popupParentTransform, string text)
    {
        GameObject popupClone = Instantiate(feedbakcPopup, popupParentTransform);
        Popup popupScript = popupClone.GetComponent<Popup>();
        popupScript.gameObject.GetComponentInChildren<Text>().text = text;
    }
}
