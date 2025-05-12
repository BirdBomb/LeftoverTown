using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapModConfigData
{
    public static MapModConfig GetMapModConfig(int ID)
    {
        return mapModConfigs.Find((x) => { return x.MapMod_ID == ID; });
    }
    public readonly static List<MapModConfig> mapModConfigs = new List<MapModConfig>()
    {
        new MapModConfig(){ MapMod_ID = 0,MapMod_Base = -1,MapMod_Name = "̫����̳"},
        new MapModConfig(){ MapMod_ID = 100,MapMod_Base = -1,MapMod_Name = "��ʬ��"},
        new MapModConfig(){ MapMod_ID = 10010,MapMod_Base = 1001,MapMod_Name = "����С��"},
        new MapModConfig(){ MapMod_ID = 10010,MapMod_Base = 1000,MapMod_Name = "����С��"},
    };
}
[Serializable]
public struct MapModConfig
{
    /// <summary>
    /// ��ͼԤ����
    /// </summary>
    [SerializeField]/*���*/
    public int MapMod_ID;
    /// <summary>
    /// ��ͼԤ������
    /// </summary>
    [SerializeField]
    public string MapMod_Name;
    /// <summary>
    /// ��ͼԤ��ػ�����
    /// </summary>
    [SerializeField]
    public short MapMod_Base;
}
