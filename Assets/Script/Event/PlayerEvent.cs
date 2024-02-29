using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEvent 
{
    public class PlayerEvent_AddItemInHand
    {
        public int playerID;
        public ItemConfig itemConfig;
    }
    public class PlayerEvent_AddItemInBag
    {
        public int playerID;
        public ItemConfig itemConfig;
    }
}
