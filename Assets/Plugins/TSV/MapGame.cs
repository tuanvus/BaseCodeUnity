using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGame : MonoBehaviour
{
    public static MapGame instance;

    private void Awake()
    {
        instance = this;
    }

    public bool rescue;
    public int idRescue;
    public GameObject playerBlue;
    public GameObject playerRed;
    public GameObject keyBlue;
    public GameObject keyRed;
    public GameObject doorBlue;
    public GameObject doorRed;
    public GameObject ironCot;
    public float xMin;
    public float xMax;
    public float yMin;
    public float yMax;
    public float zZoom = -30;

    public bool isDoorRedOpen = false;
    public bool isDoorBlueOpen = false;
}
