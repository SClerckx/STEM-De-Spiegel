using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OkKnop : MonoBehaviour {

    public string levelToLoad;

    public void ButtonClick()
    {
        GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneManagerScript>().LoadLevel(levelToLoad);
    }
}
