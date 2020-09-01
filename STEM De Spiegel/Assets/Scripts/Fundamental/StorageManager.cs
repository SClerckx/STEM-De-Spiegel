using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;

public class StorageManager : MonoBehaviour
{
    [Header("DataToCarryBetweenScenes")]
    List<string> playerOptions;
    public Dictionary<string, int> activePlayers = new Dictionary<string, int>();
    public List<Player> players;
    public MapData mapData;

    [Header("StorageStructure")]
    List<string> dataPaths = new List<string>();
    public string accountPath;
    public string eventPath;
    public string racePath;
    public string trainingPath;
    
    private void Start()
    {
        //Genereer alle locaties van de bestanden
        dataPaths = GetDataPaths();

        //Kijk welke playeraccounts er al bestasan
        GetPlayerOptions();
    }

    //Genereer alle locaties van de bestanden
    //Deze functie wordt ook getriggerd uit de Initialiser
    public List<string> GetDataPaths()
    {
        //Application.persistentDataPath is een pad dat eigen is aan het programma en nooit verandert, daarom is het dus handig dit te gebruiken als opslag
        //Onder Application.persistentDataPath wordt een map save gemaakt met twee mappen, accounts en events, de events map krijgt ook nog twee mappen, race en training
        accountPath = Application.persistentDataPath + "/save/accounts";
        eventPath = Application.persistentDataPath + "/save/events";
        racePath = eventPath + "/race";
        trainingPath = eventPath + "/training";
        dataPaths.Add(accountPath); dataPaths.Add(eventPath); dataPaths.Add(racePath);dataPaths.Add(trainingPath);
        return dataPaths;
    }

    //Sla een lijst van floats op in een gegeven locatie, als er al een bestand is in deze locatie wordt er gewoon overheen geschreven
    public void SaveAndReplace(string file_location, List<float> dataList)
    {
        using (FileStream fs = File.Create(file_location))
        {
            string file_string = "";

            //De floats uit de dataList worden achter elkaar geplaatst in een string met slashes ertussen.
            foreach (float data_point in dataList)
            {
                file_string = file_string + "/" + data_point.ToString();
            }

            //Schrijf deze string naar het bestand en sluit het daarna
            //Elk teken in de string wordt omgezet naar een byte aan de hand van de UTF8Encoding zodat het naar het tekstbestand kan worden geschreven in die vorm
            byte[] contentBytes = new UTF8Encoding(true).GetBytes(file_string);

            //Schrijf nu deze Array van bytes naar het bestand, beginnend op de eerste plaats (offset 0)
            fs.Write(contentBytes, 0, contentBytes.Length);
            fs.Close();
        }
    }

    //Voeg een lijst van floats toe aan een al bestaand bestand
    public void SaveAndAdd(string file_location, List<float> dataList)
    {
        //Lees de data die er al in het bestand staat
        List<float> priorData = new List<float>();
        priorData = Load(file_location);

        //Voeg oude en nieuwe data samen
        List<float> joinedData = new List<float>();
        joinedData.AddRange(priorData); joinedData.AddRange(dataList);

        //Sla deze data op op de 'normale' manier met vervanging
        SaveAndReplace(file_location, joinedData);
    }

    //Lees een bestand
    public List<float> Load(string file_location)
    {

        string text = "";  

        //Lees elke byte en zet deze terug om in een string
        using (FileStream fs = File.OpenRead(file_location))
        {

            byte[] b = new byte[1024];
            UTF8Encoding temp = new UTF8Encoding(true);
            while (fs.Read(b, 0, b.Length) > 0)
            {
                text = text + temp.GetString(b);
            }

        }

        //Zet de strings terug om in een float
        
        List<float> dataList = new List<float>();
        //Elke nieuwe lijn wordt in het tekstbestand aangeduid met een /, deze worden eeruit gehaald aan de hand van de split functie
        string[] dataLines = text.Split('/');      //"512,1230
        foreach (string dataLine in dataLines)
        {
            //Zet de string om in een float en voeg deze toe aan de lijst met floats
            try
            {
                dataList.Add(float.Parse(dataLine));
            }
            catch { }
            /*
            List<float> dataLijst = new List<float>();
            string[] dataPoints = dataLine.Split(',');

            foreach (string dataPoint in dataPoints)
            {
                dataLijst.Add(float.Parse(dataPoint));
            }
            puntenLijst.Add(dataLijst);
            */

        }
        return (dataList);
    }

    //Creer een nieuw account of pas een oud aan
    public void CreateOrEditAccount(string accountName, float weight, int preferredResistance)
    {
        //Maak de locatie van het account
        string dataPath = Application.persistentDataPath + "/save/accounts/" + accountName + ".txt";

        //Voeg de argumenten samen tot een list van floats
        List<float> toSaveFloats = new List<float>();
        toSaveFloats.Add(weight); toSaveFloats.Add((float)preferredResistance);

        //Sla deze list van floats op op de locatie van het account (een al bestaand account wordt overschreven met deze nieuwe gegevens)
        SaveAndReplace(dataPath, toSaveFloats);
    }

    //Verwijder een account (en de daaraan gekoppelde geschiedenis)
    public void DeleteAccount(string accountName) //itereer hele save folder om elk geraleteerd bestand te verwijderen
    { 
        //Overloop elke map
        foreach (string dataPath in dataPaths)
        {
            //Vraag elk vestand op in deze map
            DirectoryInfo dir = new DirectoryInfo(dataPath);
            FileInfo[] fileInfos = dir.GetFiles("*.*");

            //Doorzoek elk bestand in deze map
            foreach (FileInfo fileInfo in fileInfos)
            {
                //Split de naam van elk bestand zodat de acountnaam gelezen kan worden
                string[] splitInfoNames = fileInfo.Name.Split('.'); //example: "Arnout,race,dateTime.txt" --> ["Arnout,race,dateTime","txt"]
                string[] splitSplitInfoNames = splitInfoNames[0].Split(','); //example: "Arnout,race,dateTime" --> ["Arnout","race","dateTime"]

                //Check ofdat de accountnaam van het bestand overeenkomt met de naam die we zoeken, zo ja, verwijder het bestand
                //Deze foreach loop is niet echt nodig, deze kan verwijderd worden en er kan gewoon dit staan (want accountnaam staat altijd van voor):
                /*
                if (splitSplitInfoName[0] == accountName)
                {
                    File.Delete(fileInfo.FullName);
                }
                */
                foreach (string splitSplitInfoName in splitSplitInfoNames)
                {
                    if (splitSplitInfoName == accountName)
                    {
                        File.Delete(fileInfo.FullName);
                    }
                }
            }
        }
    }

    //Lees de data van een account
    public List<float> GetAccountData(string accountName)
    {
        List<float> accountData = new List<float>();
        accountData = Load(accountPath + "/" + accountName + ".txt");
        return accountData;
    }

    //Zoek welke accounts er bestaan
    public List<string> GetPlayerOptions()
    {
        List<string> playerOptions = new List<string>();
        playerOptions.Add(""); //(Dit is om het lege vakje in de dropdowns te maken)

        //Zoek alle bestanden in de accountmap
        DirectoryInfo dir = new DirectoryInfo(accountPath);
        FileInfo[] fileInfos = dir.GetFiles("*.*");

        //Doorzoek elk bestand in deze map
        foreach (FileInfo fileInfo in fileInfos)
        {
            //Voeg de naam van dit bestand en dus ook van het account toe aan de lijst van bestaande accounts (zonder de .txt natuurlijk)
            string[] splitInfoName = fileInfo.Name.Split('.');
            playerOptions.Add(splitInfoName[0]);
        }
        return playerOptions;
    }

    //Sla een training op
    public void SaveTraining(string playerNaam, float afgelegdeAfstand, float verstrekenTijd, float opgewekteEnergie)
    {
        //Creer de datum en tijd in het "MMddyyyy_HHmmss" formaat
        string dateTime = DateTime.Now.ToString("MMddyyyy_HHmmss");

        //Maak een lijst van floats van alle argumenten
        List<float> toSaveFloats = new List<float>();
        toSaveFloats.Add(1); toSaveFloats.Add(afgelegdeAfstand); toSaveFloats.Add(verstrekenTijd); toSaveFloats.Add(opgewekteEnergie);
        //1(training), afgelegdeAfstand, verstrekentijd, opgewekteEnergie

        //Maak de locatie waar dit bestand zal worden opgeslagen
        string saveLocation = trainingPath + "/" + playerNaam + "," + "training" + "," + dateTime + "," + ".txt";

        //Sla het bestand hier op
        SaveAndReplace(saveLocation, toSaveFloats);
    }

    //Sla een race op
    public void SaveRace(string playerNaam, float averageRanking, float afgelegdeAfstand, float verstrekenTijd, float opgewekteEnergie)
    {
        //Creer de datum en tijd in het "MMddyyyy_HHmmss" formaat
        string dateTime = DateTime.Now.ToString("MMddyyyy_HHmmss");

        //Maak een lijst van floats van alle argumenten
        List<float> toSaveFloats = new List<float>();
        toSaveFloats.Add(0); toSaveFloats.Add(averageRanking); toSaveFloats.Add(afgelegdeAfstand); toSaveFloats.Add(verstrekenTijd); toSaveFloats.Add(opgewekteEnergie);
        //0(race), gemiddeldeRanking, afgelegdeafstand, verstrekenTijd, opgewekteEnergie

        //Maak de locatie waar dit bestand zal worden opgeslagen
        string saveLocation = racePath + "/" + playerNaam + "," + "race" + "," + dateTime + "," + ".txt";
        //racepath/naam,race,dateTime,.txt

        //Sla het bestand hier op
        SaveAndReplace(saveLocation, toSaveFloats);
    }
}
