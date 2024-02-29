using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UIEvent 
{
    /// <summary>
    /// UI-���³�������
    /// </summary>
    public class UIEvent_AddItemInHand
    {
        public int playerID;
        public ItemConfig itemConfig;
    }
    /// <summary>
    /// UI-���±�������
    /// </summary>
    public class UIEvent_UpdateItemInBag
    {
        public List<ItemConfig> itemConfigs;
    }

}
