using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapData
{
    public SerializedTransform[] blockSquare;
    public SerializedTransform[] cubeSquare;
    public SerializedTransform playerBlue;
    public SerializedTransform playerRed;
    public SerializedTransform keyBlue;
    public SerializedTransform keyRed;
    public SerializedTransform doorBlue;
    public SerializedTransform doorRed;

    public MapGO[] listMapGO1;
    public MapGO[] listMapGO2;
    public MapGO[] listMapGO3;
    public MapGO[] listMapGO4;
}

[System.Serializable]
public class MapGO
{
    public SerializedTransform[] item;
}

[System.Serializable]
public class SerializedTransform
{
    public SerializedVector3 position;
    public SerializedVector3 rotation;
    public SerializedVector3 scale;
}
[System.Serializable]
public class SerializedVector3
{
    public float x;
    public float y;
    public float z;
}
