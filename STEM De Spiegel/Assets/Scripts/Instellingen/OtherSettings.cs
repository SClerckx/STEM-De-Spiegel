using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class OtherSettings : MonoBehaviour
{
    GameObject storageManager;
    StorageManager storageManagerScript;
    PopupManager popupManagerScript;

    void Start()
    {
        storageManager = GameObject.FindGameObjectWithTag("StorageManager");
        storageManagerScript = storageManager.GetComponent<StorageManager>();
        popupManagerScript = FindObjectOfType<PopupManager>();
    }

    public void RemoveButtonClick()
    {
        Popup popup = popupManagerScript.CreateConfirmPopup(transform, "Zeker dat u alle geschiedenis wilt verwijderen?");
        popup.yesEvent.AddListener(CreateAnotherPopup);
    }

    void CreateAnotherPopup()
    {
        Popup popup = popupManagerScript.CreateConfirmPopup(transform, "Helemaal zeker dat u alle geschiedenis wilt verwijderen?");
        popup.yesEvent.AddListener(RemoveAllStorage);
    }

    public void RemoveAllStorage() //Beter naar StorageManager
    {
        string[] allFileDirectories = Directory.GetFiles(storageManagerScript.eventPath, "*", SearchOption.AllDirectories);

        //Overloop alle files en verwijder ze
        foreach(string fileDirectory in allFileDirectories)
        {
            File.Delete(fileDirectory);
        }

        popupManagerScript.CreateFeedbackPopup(transform, "Alle geschiedenis verwijderd!");
    }

    /*
    public void RemoveAllAccounts()
    {
        DirectoryInfo dir = new DirectoryInfo(storageManagerScript.accountPath);
        FileInfo[] fileInfos = dir.GetFiles("*.*");
        foreach (FileInfo fileInfo in fileInfos)
        {
            File.Delete(fileInfo.FullName);
        }
    }*/
}
