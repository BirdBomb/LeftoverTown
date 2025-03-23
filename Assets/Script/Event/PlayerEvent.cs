using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEvent 
{
    /// <summary>
    /// �������:���Ը��޸�����
    /// </summary>
    public class PlayerEvent_Local_TryChangeItemInBag
    {
        public ItemData oldItem;
        public ItemData newItem;
    }

    /// <summary>
    /// �������:���Դӱ����Ƴ�����
    /// </summary>
    public class PlayerEvent_Local_TrySubItemInBag
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
    /// �������:���Դ��ֲ��Ƴ�����
    /// </summary>
    public class PlayerEvent_Local_TrySubItemOnHand
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
    public class PlayerEvent_Local_TrySubItemOnHead
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
    public class PlayerEvent_Local_TrySubItemOnBody
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
        public AreaSize size;
        public bool force = false; 
        public int id;
    }
    /// <summary>
    /// �������:����һ������
    /// </summary>
    public class PlayerEvent_Local_Emoji
    {
        public int id;
        public Emoji emoji;
    }

    /// <summary>
    /// �������:�󶨼���
    /// </summary>
    public class PlayerEvent_Local_BindSkill
    {
        public List<short> skillIDs = new List<short>();
    }
}
