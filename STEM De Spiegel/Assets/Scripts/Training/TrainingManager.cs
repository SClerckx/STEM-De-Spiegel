using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingManager : MonoBehaviour
{
    SubDisplays subDisplayScript;

    void Start()
    {
        subDisplayScript = FindObjectOfType<SubDisplays>();
    }

    void Update()
    {
        if (subDisplayScript.subDisplays.Count <= 0) //Wanneer niemand meer traint automatisch naar hoofdmenu
        {
            GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneManagerScript>().LoadLevel("Hoofdmenu");
        }
    }
}
