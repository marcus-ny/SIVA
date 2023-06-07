using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum EnemyClasses { SOLDIER, MECHANIC, DRONE, GENERATOR }
public class EnemyTile : Tile
{
    public GameObject enemy_prefab;
#if UNITY_EDITOR
    // The following is a helper that adds a menu item to create a RoadTile Asset
    [MenuItem("Assets/Create/2D/Tiles/EnemyTile")]
    public static void CreateRoadTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Enemy Tile", "New Enemy Tile", "Asset", "Save Enemy Tile", "Assets");
        if (path == "")
            return;
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<EnemyTile>(), path);
    }
#endif
}
