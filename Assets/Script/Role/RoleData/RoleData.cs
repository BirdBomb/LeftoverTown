using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
[SerializeField]
public struct RoleData
{
    [Header("�ٶ�")]
    public float Data_Speed;
    [Header("����ֵ")]
    public int Data_Hp;
    [Header("�������ֵ")]
    public int Data_HpMax;
    [Header("����ֵ")]
    public int Data_Hunger;
    [Header("��󼢶�ֵ")]
    public int Data_HungerMax;

    [Header("����ֵ")]
    public int Temp_Alert;
    [Header("�ֲ�����")]
    public ItemConfig Holding_ByHand;
    [Header("��������")]
    public List<ItemConfig> Holding_BagList;

}
public enum RoleType
{
    
}
