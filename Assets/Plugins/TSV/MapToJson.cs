#if Unity_Editor
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MapToJson : EditorWindow
{
    Object obj;
    public ReferenceController referenceController;
    [MenuItem("Window/SaveMapData")]
    public static void ShowWindow()
    {
        GetWindow<MapToJson>("SaveMapData");
    }
    Transform grid;
    private void OnGUI()
    {
        obj = EditorGUILayout.ObjectField(referenceController, typeof(ReferenceController), true);
        referenceController = obj as ReferenceController;

        if (GUILayout.Button("SaveMap"))
        {
            GameObject[] list = Selection.gameObjects;
            foreach (var item in list)
            {
                MapGame currentMap = item.GetComponent<MapGame>();
                SaveIntoJson(currentMap);
            }
        }
    }

    public void SaveIntoJson(MapGame currentMap)
    {
        MapData _MapData = new MapData
        {
            playerBlue = SetupObjectData(currentMap.playerBlue.transform),
            playerRed = SetupObjectData(currentMap.playerRed.transform),
            keyBlue = SetupObjectData(currentMap.keyBlue.transform),
            keyRed = SetupObjectData(currentMap.keyRed.transform),
            doorBlue = SetupObjectData(currentMap.doorBlue.transform),
            doorRed = SetupObjectData(currentMap.doorRed.transform),          
        };

        SaveGrid(currentMap, _MapData);
        SaveDeco(currentMap, _MapData, 1);
        SaveDeco(currentMap, _MapData, 2);
        SaveDeco(currentMap, _MapData, 3);

        string mapData = JsonUtility.ToJson(_MapData, true);
        System.IO.File.WriteAllText("Assets/Resources/LevelJson/" + currentMap.name + ".json", mapData);
        Debug.Log("Save Complete");
    }

    public SerializedTransform SetupObjectData(Transform ObjGame)
    {
        SerializedTransform temp = new SerializedTransform
        {
            position = new SerializedVector3
            {
                x = ObjGame.transform.position.x,
                y = ObjGame.transform.position.y,
                z = ObjGame.transform.position.z,
            },
            rotation = new SerializedVector3
            {
                x = ObjGame.transform.eulerAngles.x,
                y = ObjGame.transform.eulerAngles.y,
                z = ObjGame.transform.eulerAngles.z,
            },
            scale = new SerializedVector3
            {
                x = ObjGame.transform.localScale.x,
                y = ObjGame.transform.localScale.y,
                z = ObjGame.transform.localScale.z,
            }
        };
        return temp;
    }

    public void SaveGrid(MapGame currentMap, MapData _MapData)
    {
        if (currentMap.transform.GetChild(0).transform.GetChild(0).name.StartsWith("Grid"))
        {
            grid = currentMap.transform.GetChild(0).transform.GetChild(0);
        }
        else if (currentMap.transform.GetChild(0).transform.GetChild(1).name.StartsWith("Grid"))
        {
            grid = currentMap.transform.GetChild(0).transform.GetChild(1);
        }
        int numBlock = 0;
        int numCube = 0;
        for (int i = 0; i < grid.childCount; i++)
        {
            if (grid.GetChild(i).name.StartsWith("Block") && grid.GetChild(i).gameObject.activeSelf)
            {
                numBlock += 1;
            }
            if ((grid.GetChild(i).name.StartsWith("Cube ") || grid.GetChild(i).name == "Cube") && grid.GetChild(i).gameObject.activeSelf)
            {
                numCube += 1;
            }
        }
        _MapData.blockSquare = new SerializedTransform[numBlock];
        _MapData.cubeSquare = new SerializedTransform[numCube];
        int blockId = 0;
        int cubeId = 0;
        for (int i = 0; i < grid.childCount; i++)
        {
            if (grid.GetChild(i).name.StartsWith("Block") && grid.GetChild(i).gameObject.activeSelf)
            {
                _MapData.blockSquare[blockId] = SetupObjectData(grid.GetChild(i).transform);
                blockId += 1;
            }
            else if ((grid.GetChild(i).name.StartsWith("Cube ") || grid.GetChild(i).name == "Cube") && grid.GetChild(i).gameObject.activeSelf)
            {
                _MapData.cubeSquare[cubeId] = SetupObjectData(grid.GetChild(i).transform);
                cubeId += 1;
            }
        }
    }

    public void SaveDeco(MapGame currentMap, MapData _MapData, int id)
    {
        grid = currentMap.transform.GetChild(0).transform.GetChild(1);
        if (id == 1)
        {
            _MapData.listMapGO1 = new MapGO[referenceController.map1GO.Length];
            for (int i = 0; i < referenceController.map1GO.Length; i++)
            {
                int idGO = 0;
                int numIdGO = 0;
                for (int j = 0; j < grid.childCount; j++)
                {
                    if (grid.GetChild(j).name.StartsWith(referenceController.map1GO[i].name) && grid.GetChild(i).gameObject.activeSelf)
                    {
                        numIdGO += 1;
                    }
                }
                _MapData.listMapGO1[i] = new MapGO();
                _MapData.listMapGO1[i].item = new SerializedTransform[numIdGO];
                for (int j = 0; j < grid.childCount; j++)
                {
                    if (grid.GetChild(j).name.StartsWith(referenceController.map1GO[i].name) && grid.GetChild(i).gameObject.activeSelf)
                    {
                        _MapData.listMapGO1[i].item[idGO] = SetupObjectData(grid.GetChild(j).transform);
                        idGO += 1;
                    }
                }
            }
        }
        else if (id == 2)
        {
            _MapData.listMapGO2 = new MapGO[referenceController.map2GO.Length];
            for (int i = 0; i < referenceController.map2GO.Length; i++)
            {
                int idGO = 0;
                int numIdGO = 0;
                for (int j = 0; j < grid.childCount; j++)
                {
                    if (grid.GetChild(j).name.StartsWith(referenceController.map2GO[i].name) && grid.GetChild(i).gameObject.activeSelf)
                    {
                        numIdGO += 1;
                    }
                }
                _MapData.listMapGO2[i] = new MapGO();
                _MapData.listMapGO2[i].item = new SerializedTransform[numIdGO];
                for (int j = 0; j < grid.childCount; j++)
                {
                    if (grid.GetChild(j).name.StartsWith(referenceController.map2GO[i].name) && grid.GetChild(i).gameObject.activeSelf)
                    {
                        _MapData.listMapGO2[i].item[idGO] = SetupObjectData(grid.GetChild(j).transform);
                        idGO += 1;
                    }
                }
            }
        }
        else if (id == 3)
        {
            _MapData.listMapGO3 = new MapGO[referenceController.map3GO.Length];
            for (int i = 0; i < referenceController.map3GO.Length; i++)
            {
                int idGO = 0;
                int numIdGO = 0;
                for (int j = 0; j < grid.childCount; j++)
                {
                    if (grid.GetChild(j).name.StartsWith(referenceController.map3GO[i].name) && grid.GetChild(i).gameObject.activeSelf)
                    {
                        numIdGO += 1;
                    }
                }
                _MapData.listMapGO3[i] = new MapGO();
                _MapData.listMapGO3[i].item = new SerializedTransform[numIdGO];
                for (int j = 0; j < grid.childCount; j++)
                {
                    if (grid.GetChild(j).name.StartsWith(referenceController.map3GO[i].name) && grid.GetChild(i).gameObject.activeSelf)
                    {
                        _MapData.listMapGO3[i].item[idGO] = SetupObjectData(grid.GetChild(j).transform);
                        idGO += 1;
                    }
                }
            }
        }
    }
}
#endif
