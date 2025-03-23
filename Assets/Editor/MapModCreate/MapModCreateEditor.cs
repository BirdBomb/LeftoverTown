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
        if (GUILayout.Button("���õ�ǰ��ͼģ��"))
        {
            MapModCreate mapModCreate = (MapModCreate)target;
            if (mapModCreate != null)
            {
                mapModCreate.SetMapMod();
            }
        }
        if (GUILayout.Button("���浱ǰ��ͼģ��"))
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