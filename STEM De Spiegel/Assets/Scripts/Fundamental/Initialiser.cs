using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Initialiser : MonoBehaviour
{
    [Header("Don'tDestroyOnload")]
    GameObject storageManager;
    StorageManager storageManagerScript;
    GameObject communicationManager;
    GameObject sceneManager;
    GameObject popupManager;
    GameObject physicsManager;

    [Header("Prefabs")]
    public GameObject storageManagerPrefab;
    public GameObject communicationManagerPrefab;
    public GameObject sceneManagerPrefab;
    public GameObject popupManagerPrefab;
    public GameObject physicsManagerPrefab;

    [Header("Storage")]
    public string accountPath;
    public string eventPath;
    List<string> directoryPaths = new List<string>();
    List<string> playerOptions = new List<string>();
    
    void Start()
    {
        //Probeer alle fundamentele scripts te vinden en gooi geen error wanneer ze niet gevonden worden
        try
        {
            storageManager = FindObjectOfType<StorageManager>().gameObject;
            communicationManager = FindObjectOfType<CommunicationManager>().gameObject;
            sceneManager = FindObjectOfType<SceneManagerScript>().gameObject;
            popupManager = FindObjectOfType<PopupManager>().gameObject;
            physicsManager = FindObjectOfType<PhysicsManager>().gameObject;
        }
        catch { }

        //Wanneer een script niet gevonden werd, creer een nieuwe versie hiervan
        if (storageManager == null)
        {
            storageManager = Instantiate(storageManagerPrefab);
            DontDestroyOnLoad(storageManager);
        }
        if (communicationManager == null)
        {
            communicationManager = Instantiate(communicationManagerPrefab);
            DontDestroyOnLoad(communicationManager);
        }
        if (sceneManager == null)
        {
            sceneManager = Instantiate(sceneManagerPrefab);
            DontDestroyOnLoad(sceneManager);
        }
        if (popupManager == null)
        {
            popupManager = Instantiate(popupManagerPrefab);
            DontDestroyOnLoad(popupManager);
        }
        if (physicsManager == null)
        {
            physicsManager = Instantiate(physicsManagerPrefab);
            DontDestroyOnLoad(physicsManager);
        }

        //Vraag alle locaties op van de mappen die zouden moeten bestaan
        storageManagerScript = storageManager.GetComponent<StorageManager>();
        storageManagerScript.activePlayers = new Dictionary<string, int>();
        directoryPaths = storageManagerScript.GetDataPaths();

        //Als deze niet bestaan, creer ze
        foreach (string directoryPath in directoryPaths)
        {
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }

        Debug.Log(Application.persistentDataPath);
    }
}
