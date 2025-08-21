using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// �������
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
        "������ͨ��",
        "����ִ����",
        "����Ȩ��",
        "����",
        "����",
    };
}
[Serializable]
public struct StatusConfig
{
    /// <summary>
    /// ��ݱ��
    /// </summary>
    [SerializeField]
    public short Status_ID;
    /// <summary>
    /// ������
    /// </summary>
    [SerializeField]
    public StatusType Status_Type;
    /// <summary>
    /// �·�
    /// </summary>
    [SerializeField]
    public short Status_Clothes;
    /// <summary>
    /// ñ��
    /// </summary>
    [SerializeField]
    public short Status_Hat;
}
/// <summary>
/// ������
/// </summary>
public enum StatusType
{
    /// <summary>
    /// ������ͨ��
    /// </summary>
    Human_Common,
    /// <summary>
    /// ����ִ����
    /// </summary>
    Human_Lawman,
    /// <summary>
    /// ����Ȩ��
    /// </summary>
    Human_Bigwigs,
    /// <summary>
    /// ����
    /// </summary>
    Monster_Common,
    /// <summary>
    /// ����
    /// </summary>
    Animal_Common,
    /// <summary>
    /// �����ﷸ
    /// </summary>
    Human_Offender,
}