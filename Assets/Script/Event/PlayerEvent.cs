using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEvent 
{
    #region//��ұ���
    /// <summary>
    /// �������:���ı�������
    /// </summary>
    public class PlayerEvent_Local_TryExpendItemInBag
    {
        public int itemID;
        public int itemCount;
    }
    /// <summary>
    /// �������:��ӱ�������
    /// </summary>
    public class PlayerEvent_Local_TryAddItemInBag
    {
        public int index;
        public ItemData itemData;
    }
    /// <summary>
    /// �������:���ı�������
    /// </summary>
    public class PlayerEvent_Local_TryChangeItemInBag
    {
        public ItemData itemData;
        public int index;
    }
    /// <summary>
    /// �������:ʹ�ñ�������
    /// </summary>
    public class PlayerEvent_Local_TryUseItemInBag
    {
        public int index;
    }
    #endregion
    #region//����ֳ�
    /// <summary>
    /// �������:����ֲ�����
    /// </summary>
    public class PlayerEvent_Local_TryAddItemOnHand
    {
        public ItemData item;
        public Action<ItemData> itemResidueBack;
    }
    /// <summary>
    /// �������:�Ƴ��ֲ�����
    /// </summary>
    public class PlayerEvent_Local_TrySubItemOnHand
    {
        public ItemData item;
    }
    /// <summary>
    /// �������:�����ֲ�����
    /// </summary>
    public class PlayerEvent_Local_TryChangeItemOnHand
    {
        public ItemData oldItem;
        public ItemData newItem;
    }
    /// <summary>
    /// �������:�滻�������嵽����
    /// </summary>
    public class PlayerEvent_Local_TrySwitchItemBetweenHandAndBag 
    {
        public int index;
    }
    /// <summary>
    /// �������:�����ֳ�����
    /// </summary>
    public class PlayerEvent_Local_TryPutAwayItemOnHand
    {
        
    }

    #endregion
    #region//���ͷ��
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
    /// �������:����ͷ������
    /// </summary>
    public class PlayerEvent_Local_TryChangeItemOnHead
    {
        public ItemData oldItem;
        public ItemData newItem;
    }
    /// <summary>
    /// �滻�������嵽ͷ��
    /// </summary>
    public class PlayerEvent_Local_TrySwitchItemBetweenHeadAndBag
    {
        public int index;
    }
    /// <summary>
    /// �������:����ͷ������
    /// </summary>
    public class PlayerEvent_Local_TryPutAwayItemOnHead
    {

    }

    #endregion
    #region//�������
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
    /// �������:������������
    /// </summary>
    public class PlayerEvent_Local_TryChangeItemOnBody
    {
        public ItemData oldItem;
        public ItemData newItem;
    }
    /// <summary>
    /// �滻�������嵽����
    /// </summary>
    public class PlayerEvent_Local_TrySwitchItemBetweenBodyAndBag
    {
        public int index;
    }
    /// <summary>
    /// �������:������������
    /// </summary>
    public class PlayerEvent_Local_TryPutAwayItemOnBody
    {

    }
    #endregion




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
    public class PlayerEvent_Local_SendEmoji
    {
        public int id;
        public Emoji emoji;
    }
    /// <summary>
    /// �������:����һ�λ�
    /// </summary>
    public class PlayerEvent_Local_SendText
    {
        public string text;
    }

    /// <summary>
    /// �������:�󶨼���
    /// </summary>
    public class PlayerEvent_Local_BindSkill
    {
        public List<short> skillIDs = new List<short>();
    }
}
