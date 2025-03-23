using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapModConfigData 
{
    public static MapModConfig GetMapModConfig(int ID)
    {
        return mapModConfigs.Find((x) => { return x.MapMod_ID == ID; });
    }
    public readonly static List<MapModConfig> mapModConfigs = new List<MapModConfig>()
    {
        new MapModConfig(){ MapMod_ID = 0,MapMod_Name = "��ʼ̨��"},
        new MapModConfig(){ MapMod_ID = 1,MapMod_Name = "С��ó��վ"},
    };
}
[Serializable]
public struct MapModConfig
{
    /// <summary>
    /// ��ݱ��
    /// </summary>
    [SerializeField]/*���*/
    public int MapMod_ID;
    /// <summary>
    /// �������
    /// </summary>
    [SerializeField]
    public string MapMod_Name;
}
