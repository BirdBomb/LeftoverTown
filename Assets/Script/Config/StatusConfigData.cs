using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusConfigData
{
    public static StatusConfig GetStatusConfig(short ID)
    {
        return statusConfigs.Find((x) => { return x.Status_ID == ID; });
    }
    public readonly static List<StatusConfig> statusConfigs = new List<StatusConfig>()
    {
        new StatusConfig(){ Status_ID = 0,Status_Name = "不明生物",Status_Desc = "",Status_Clothes = -1,Status_Hat = -1,},
        new StatusConfig(){ Status_ID = 1001,Status_Name = "裸漏狂",Status_Desc = "",Status_Clothes = 0,Status_Hat = -1,},
        new StatusConfig(){ Status_ID = 1002,Status_Name = "神经病",Status_Desc = "",Status_Clothes = -1,Status_Hat = -1,},
        new StatusConfig(){ Status_ID = 1003,Status_Name = "普通人",Status_Desc = "",Status_Clothes = -1,Status_Hat = -1,},
        new StatusConfig(){ Status_ID = 1004,Status_Name = "夜巡队长",Status_Desc = "",Status_Clothes = 5001,Status_Hat = 5002,},
    };


}
[Serializable]
public struct StatusConfig
{
    /// <summary>
    /// 身份编号
    /// </summary>
    [SerializeField]/*编号*/
    public short Status_ID;
    /// <summary>
    /// 身份名称
    /// </summary>
    [SerializeField]
    public string Status_Name;
    /// <summary>
    /// 身份描述
    /// </summary>
    [SerializeField]
    public string Status_Desc;
    /// <summary>
    /// 衣服
    /// </summary>
    [SerializeField]
    public short Status_Clothes;
    /// <summary>
    /// 帽子
    /// </summary>
    [SerializeField]
    public short Status_Hat;
}
