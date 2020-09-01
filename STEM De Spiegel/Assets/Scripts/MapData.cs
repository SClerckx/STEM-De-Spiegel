using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapData : MonoBehaviour
{
    [Header("Properties")]
    public string mapName;
    public string mapClass;
    public float lapLength;
    public int assignedLaps;
    public TextAsset spoorCoordinaten;
    public Vector2 absoluteScale;

    [Header("PlayerSettings")]
    public float nameScale;

    [Header("PropInstantiateValues")]
    public GameObject mapProp;
    public Vector3 propPosition;
    public Vector3 propRotation;
    public Vector3 propScale;

    [Header("MapInstantiateValues")]
    public GameObject mapPrefab;
    public Vector3 mapPosition;
    public Vector3 mapRotation;
    public Vector3 mapScale;

    [Header("PlaneValues")]
    public Vector3 planePosition;
    public Vector3 planeRotation;
    public Vector3 planeScale;

    [Header("CameraValues")]
    public Vector3 cameraPosition;
    public Vector3 cameraRotation;
    public float cameraFOV;

    [Header("DirectionalLight")]
    public Vector3 lightRotation;
}
