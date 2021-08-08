using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferenceController : MonoBehaviour
{
    public static ReferenceController instance;

    private void Awake()
    {
        instance = this;
    }

    public GameObject keyBlue;
    public GameObject keyRed;
    public GameObject doorBlue;
    public GameObject doorRed;
    public GameObject IronCot;

    public GameObject blockMap1;
    public GameObject cubeMap;

    public GameObject[] map1GO;
    public GameObject[] map2GO;
    public GameObject[] map3GO;
    public GameObject[] map4GO;
}
