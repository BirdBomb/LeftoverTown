using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapModCreate : MonoBehaviour
{
#if UNITY_EDITOR
    public MapMod mapMod_Bind;
    public Tilemap tilemap_Floor;
    public Tilemap tilemap_Building;

    public void SetMapMod()
    {
        mapMod_Bind.dic_mapFloorData.Clear();
        mapMod_Bind.dic_mapBuildingData.Clear();

        tilemap_Floor = mapMod_Bind.transform.Find("Floor/Tilemap").GetComponent<Tilemap>();
        tilemap_Building = mapMod_Bind.transform.Find("Building/Tilemap").GetComponent<Tilemap>();
        Debug.Log(tilemap_Floor.size.x + "/" + tilemap_Floor.size.y);
        Debug.Log(tilemap_Building.size.x + "/" + tilemap_Building.size.y);
        for (int x = -tilemap_Floor.size.x; x < tilemap_Floor.size.x; x++)
        {
            for (int y = -tilemap_Floor.size.y; y < tilemap_Floor.size.y; y++)
            {
                // 获取当前位置的Tile
                Vector3Int tilePosition = new Vector3Int(x, y, 0);
                GroundTile tile = tilemap_Floor.GetTile<GroundTile>(tilePosition);

                // 输出Tile信息，可以根据需要进行相应的操作
                if (tile != null)
                {
                    mapMod_Bind.dic_mapFloorData.Add(new Vector2Int(x, y), short.Parse(tile.name));
                }
            }
        }
        for (int x = -tilemap_Building.size.x; x < tilemap_Building.size.x; x++)
        {
            for (int y = -tilemap_Building.size.y; y < tilemap_Building.size.y; y++)
            {
                // 获取当前位置的Tile
                Vector3Int tilePosition = new Vector3Int(x, y, 0);
                BuildingTile tile = tilemap_Building.GetTile<BuildingTile>(tilePosition);

                // 输出Tile信息，可以根据需要进行相应的操作
                if (tile != null)
                {
                    mapMod_Bind.dic_mapBuildingData.Add(new Vector2Int(x, y), short.Parse(tile.name));
                }
            }
        }
        Debug.Log(mapMod_Bind.dic_mapBuildingData.Keys.Count);
    }
    public void WriteMapMod()
    {
        MapModData mapModData = ScriptableObject.CreateInstance<MapModData>();
        foreach (Vector2Int vector2Int in mapMod_Bind.dic_mapFloorData.Keys)
        {
            KeyValuePair<Vector2Int, short> temp = new KeyValuePair<Vector2Int, short>() { key = vector2Int, value = mapMod_Bind.dic_mapFloorData[vector2Int] };
            mapModData.data_mapFloor.Add(temp);
        }
        foreach (Vector2Int vector2Int in mapMod_Bind.dic_mapBuildingData.Keys)
        {
            KeyValuePair<Vector2Int, short> temp = new KeyValuePair<Vector2Int, short>() { key = vector2Int, value = mapMod_Bind.dic_mapBuildingData[vector2Int] };
            mapModData.data_mapBuilding.Add(temp);
        }
        Debug.Log(mapModData.data_mapBuilding.Count);
        AssetDatabase.DeleteAsset($"Assets/Resources/MapModData/MapModData{mapMod_Bind.ID}.asset");
        AssetDatabase.CreateAsset(mapModData, $"Assets/Resources/MapModData/MapModData{mapMod_Bind.ID}.asset");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
#endif
}

