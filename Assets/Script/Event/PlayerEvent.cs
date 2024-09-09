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
    }
    /// <summary>
    /// �������:���Ը������޸�����
    /// </summary>
    public class PlayerEvent_Local_TryChangeItemInBag
    {
        public ItemData oldItem;
        public ItemData newItem;
    }

    /// <summary>
    /// �������:���Դ��ֲ��Ƴ�����
    /// </summary>
    public class PlayerEvent_Local_TryRemoveItemOnHand
    {
        public ItemData item;
    }
    /// <summary>
    /// �������:���Ը��ֲ��������
    /// </summary>
    public class PlayerEvent_Local_TryAddItemOnHand
    {
        public ItemData item;
        public Action<ItemData> itemResidueBack;
    }



    /// <summary>
    /// �������:���Դ�ͷ���Ƴ�����
    /// </summary>
    public class PlayerEvent_Local_TryRemoveItemOnHead
    {
        public ItemData item;
    }
    /// <summary>
    /// �������:���Ը�ͷ���������
    /// </summary>
    public class PlayerEvent_Local_TryAddItemOnHead
    {
        public ItemData item;
        public Action<ItemData> itemResidueBack;
    }

    /// <summary>
    /// �������:���Դ������Ƴ�����
    /// </summary>
    public class PlayerEvent_Local_TryRemoveItemOnBody
    {
        public ItemData item;
    }
    /// <summary>
    /// �������:���Ը������������
    /// </summary>
    public class PlayerEvent_Local_TryAddItemOnBody
    {
        public ItemData item;
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
    /// <summary>
    /// �������:���콨��
    /// </summary>
    public class PlayerEvent_Local_TryBuildBuilding 
    {
        public int id;
    }
    /// <summary>
    /// �������:���콨��
    /// </summary>
    public class PlayerEvent_Local_TryBuildFloor
    {
        public int id;
    }

    /// <summary>
    /// �������:����һ������
    /// </summary>
    public class PlayerEvent_Local_Emoji
    {
        public int id;
    }

    /// <summary>
    /// �������:�󶨼���
    /// </summary>
    public class PlayerEvent_Local_BindSkill
    {
        public List<short> skillIDs = new List<short>();
    }
}
