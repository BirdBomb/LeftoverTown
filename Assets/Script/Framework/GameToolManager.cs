using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameToolManager : SingleTon<GameLocalManager>, ISingleTon
{
    public void Init()
    {
        
    }
    public void PutItemInToItemList(ItemData item, List<ItemData> items, out ItemData itemIn, out ItemData itemOut)
    {
        itemIn = new ItemData();
        itemOut = new ItemData();
    }
}
