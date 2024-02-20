using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEvent 
{
    public class PlayerEvent_AddItemInHand
    {
        public ItemConfig itemConfig;
    }
    public class PlayerEvent_AddItemInSlot
    {
        public ItemConfig itemConfig;
    }
    public class PlayerEvent_AddItemInBag
    {
        public ItemConfig itemConfig;
    }
    public class PlayerEvent_UI_AddItemInHand
    {
        public ItemConfig itemConfig;
    }
    public class PlayerEvent_UI_AddItemInSlot
    {
        public ItemConfig itemConfig;
    }
    public class PlayerEvent_UI_AddItemInBag
    {
        public ItemConfig itemConfig;
    }
    /// <summary>
    /// ���UI-���±�������
    /// </summary>
    public class PlayerEvent_UI_UpdateItemInBag
    {
        public List<ItemConfig> itemConfigs;
    }
}
