using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerName3D : MonoBehaviour
{
    Player playerScript;
    TextMesh textMesh;
    RaceManager raceManagerScript;
    StorageManager storageManager;

    public Vector3 offset = new Vector3(1, 0, 0);
    public float scale;

    // Start is called before the first frame update
    void Start()
    {
        raceManagerScript = FindObjectOfType<RaceManager>();
        playerScript = transform.parent.GetComponent<Player>();
        textMesh = GetComponent<TextMesh>();
        textMesh.color = raceManagerScript.playerColors[playerScript.naam];
        scale = FindObjectOfType<StorageManager>().mapData.nameScale;
    }

    // Update is called once per frame
    void Update()
    {
        //Rotatie
        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);

        //positie
        transform.position = playerScript.gameObject.transform.position + offset;

        //Schaal
        transform.localScale = scale * new Vector3(1, 1, 1);

        textMesh.text = playerScript.naam;
    }
}
