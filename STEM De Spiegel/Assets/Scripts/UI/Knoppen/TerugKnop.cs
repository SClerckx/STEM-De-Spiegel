using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerugKnop : MonoBehaviour {
    public string levelToLoad = "Hoofdmenu";

    public void ButtonClick()
    {
        GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneManagerScript>().LoadLevel(levelToLoad);
    }
}