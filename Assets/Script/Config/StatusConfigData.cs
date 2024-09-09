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
        new StatusConfig(){ Status_ID = 0,Status_Name = "��������",Status_Desc = "",Status_Clothes = -1,Status_Hat = -1,},
        new StatusConfig(){ Status_ID = 1001,Status_Name = "��©��",Status_Desc = "",Status_Clothes = 0,Status_Hat = -1,},
        new StatusConfig(){ Status_ID = 1002,Status_Name = "�񾭲�",Status_Desc = "",Status_Clothes = -1,Status_Hat = -1,},
        new StatusConfig(){ Status_ID = 1003,Status_Name = "��ͨ��",Status_Desc = "",Status_Clothes = -1,Status_Hat = -1,},
        new StatusConfig(){ Status_ID = 1004,Status_Name = "ҹѲ�ӳ�",Status_Desc = "",Status_Clothes = 5001,Status_Hat = 5002,},
    };


}
[Serializable]
public struct StatusConfig
{
    /// <summary>
    /// ��ݱ��
    /// </summary>
    [SerializeField]/*���*/
    public short Status_ID;
    /// <summary>
    /// �������
    /// </summary>
    [SerializeField]
    public string Status_Name;
    /// <summary>
    /// �������
    /// </summary>
    [SerializeField]
    public string Status_Desc;
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
