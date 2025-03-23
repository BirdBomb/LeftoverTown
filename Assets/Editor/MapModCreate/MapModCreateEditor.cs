using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapModCreate))]
public class MapModCreateEditor : Editor
{
#if UNITY_EDITOR
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("设置当前地图模块"))
        {
            MapModCreate mapModCreate = (MapModCreate)target;
            if (mapModCreate != null)
            {
                mapModCreate.SetMapMod();
            }
        }
        if (GUILayout.Button("保存当前地图模块"))
        {
            MapModCreate mapModCreate = (MapModCreate)target;
            if (mapModCreate != null)
            {
                mapModCreate.WriteMapMod();
            }
        }

    }
#endif
}