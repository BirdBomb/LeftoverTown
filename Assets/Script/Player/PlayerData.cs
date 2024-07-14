using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PlayerData 
{
    [SerializeField]
    public Vector3Int Pos;
    [SerializeField]
    public int Speed;
    [SerializeField]
    public int MaxSpeed;
    [SerializeField]
    public int En;
    [SerializeField]
    public int Hp;
    [SerializeField]
    public int MaxHp;
    [SerializeField]
    public int Food;
    [SerializeField]
    public int MaxFood;
    [SerializeField]
    public int San;
    [SerializeField]
    public int MaxSan;
    [SerializeField]
    public int Point_Strength;
    [SerializeField]
    public int Point_Intelligence;
    [SerializeField]
    public int Point_Focus;
    [SerializeField]
    public int Point_Agility;

    [SerializeField]
    public List<ItemData> BagItems = new List<ItemData>();
    public ItemData HandItem;
    public ItemData HeadItem;

    [SerializeField]
    public List<int> Skills_Know = new List<int>();
    [SerializeField]
    public List<int> Skills_Use = new List<int>();

}
