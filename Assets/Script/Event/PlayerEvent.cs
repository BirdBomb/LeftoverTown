using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEvent 
{
    /// <summary>
    /// �������:���Դӱ����Ƴ�����
    /// </summary>
    public class PlayerEvent_Local_TryRemoveItemFromBag
    {
        public ItemData item;
    }
    /// <summary>
    /// �������:���Ը������������
    /// </summary>
    public class PlayerEvent_Local_TryAddItemInBag
    {
        public ItemData item;
    }

    /// <summary>
    /// �������:����һ������
    /// </summary>
    public class PlayerEvent_Local_TryDropItem
    {
        public ItemData item;
    }
    /// <summary>
    /// �������:�ٻ�һ������
    /// </summary>
    public class PlayerEvent_Local_TrySpawnActor
    {
        public string name;
    }

}
