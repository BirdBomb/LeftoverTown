using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MapModData : ScriptableObject
{
    public List<KeyValuePair<Vector2Int, short>> data_mapFloor = new List<KeyValuePair<Vector2Int, short>>();
    public List<KeyValuePair<Vector2Int, short>> data_mapBuilding = new List<KeyValuePair<Vector2Int, short>>();
}
[System.Serializable]
public class KeyValuePair<TKey, TValue>
{
    public TKey key;
    public TValue value;
}
