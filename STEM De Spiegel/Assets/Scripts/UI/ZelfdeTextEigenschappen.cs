using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZelfdeTextEigenschappen : MonoBehaviour
{
    public GameObject objectOmTeVolgen;

    [Header("Eigenschappen om te volgen")]
    //public bool positieX = false;
    public bool positieY = false;
    //public bool schaalX = false;
    //public bool schaalY = false;
    //public bool breedte = false;
    //public bool hoogte = false;

    RectTransform eigenRectTransform;
    RectTransform andereRectTransform;

    void Start()
    {
        eigenRectTransform = GetComponent<RectTransform>();
        andereRectTransform = objectOmTeVolgen.GetComponent<RectTransform>();

        if (positieY)
        {
            eigenRectTransform.position = new Vector3(eigenRectTransform.position.x, andereRectTransform.position.y, eigenRectTransform.position.z);
        }

        /*
        if (positieX)
        {
            eigenRectTransform.localPosition = new Vector3(andereRectTransform.position.x, 0, 0);
        }

        if (schaalX)
        {
            eigenRectTransform.localScale = new Vector3(andereRectTransform.localScale.x, 0, 0);
        }

        if (schaalY)
        {
            eigenRectTransform.localScale = new Vector3(0, andereRectTransform.localScale.y, 0);
        }

        if (breedte)
        {
            eigenRectTransform.sizeDelta = new Vector2(andereRectTransform.rect.width, 0);
        }

        if (hoogte)
        {
            eigenRectTransform.sizeDelta = new Vector2(0, andereRectTransform.rect.height);
        } 
        */
    }
}
