using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PlayerData 
{
    [SerializeField]
    public Vector3Int Pos;
    [SerializeField]
    public int Hp;
    [SerializeField]
    public int MaxHp;
    [SerializeField]
    public List<ItemData> Bag = new List<ItemData>();
}
