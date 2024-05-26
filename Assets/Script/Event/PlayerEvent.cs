using System;
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
        public Action<ItemData> itemResidueBack;
    }
    /// <summary>
    /// �������:���Ը������޸�����
    /// </summary>
    public class PlayerEvent_Local_TryChangeItemInBag
    {
        public ItemData oldItem;
        public ItemData newItem;
        public Action<ItemData> itemResidueBack;
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
