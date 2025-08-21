using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 身份配置
/// </summary>
public class StatusConfigData
{
    public static StatusConfig GetStatusConfig(short ID)
    {
        return statusConfigs.Find((x) => { return x.Status_ID == ID; });
    }
    public readonly static List<StatusConfig> statusConfigs = new List<StatusConfig>()
    {
        new StatusConfig(){ Status_ID = 0,Status_Type = StatusType.Human_Common,Status_Clothes = 0,Status_Hat = 0,},
        new StatusConfig(){ Status_ID = 1,Status_Type = StatusType.Human_Lawman,Status_Clothes = 5001,Status_Hat = 5002,},
    };
    public readonly static List<string> statusName = new List<string>
    {
        "人类普通人",
        "人类执法者",
        "人类权贵",
        "怪物",
        "生物",
    };
}
[Serializable]
public struct StatusConfig
{
    /// <summary>
    /// 身份编号
    /// </summary>
    [SerializeField]
    public short Status_ID;
    /// <summary>
    /// 身份类别
    /// </summary>
    [SerializeField]
    public StatusType Status_Type;
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
/// <summary>
/// 身份类别
/// </summary>
public enum StatusType
{
    /// <summary>
    /// 人类普通人
    /// </summary>
    Human_Common,
    /// <summary>
    /// 人类执法者
    /// </summary>
    Human_Lawman,
    /// <summary>
    /// 人类权贵
    /// </summary>
    Human_Bigwigs,
    /// <summary>
    /// 怪物
    /// </summary>
    Monster_Common,
    /// <summary>
    /// 生物
    /// </summary>
    Animal_Common,
    /// <summary>
    /// 人类罪犯
    /// </summary>
    Human_Offender,
}