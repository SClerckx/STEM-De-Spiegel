using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour {

    CommunicationManager communicationManager;

    [Header("SceneNames")]
    public string raceActivationScene = "SpelerSelectie";
    public string trainingActivationScene = "TrainingDev";
    public string hoofdMenuActivationScene = "Hoofdmenu";

    // Use this for initialization
    void Start () {
        DontDestroyOnLoad(gameObject);
        communicationManager = FindObjectOfType<CommunicationManager>();
    }

    public void LoadLevel(string Level)
    {
        SceneManager.LoadScene(Level);

        if (Level == raceActivationScene)
        {
            communicationManager.SendRaceActivation();
        }

        if (Level == trainingActivationScene)
        {
            communicationManager.SendTrainingActivation();
        }

        if (Level == hoofdMenuActivationScene)
        {
            communicationManager.SendMenuActivation();
        }
    }
}
