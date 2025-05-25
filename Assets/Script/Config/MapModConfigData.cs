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
        /*̫����̳*/new MapModConfig(){ MapMod_ID = 0,MapMod_Base = -1},
        /*������ʬ*/new MapModConfig(){ MapMod_ID = 100,MapMod_Base = -1},
        /*����С��*/new MapModConfig(){ MapMod_ID = 10010,MapMod_Base = 1001},
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
    /// ��ͼԤ��ػ�����
    /// </summary>
    [SerializeField]
    public short MapMod_Base;
}
