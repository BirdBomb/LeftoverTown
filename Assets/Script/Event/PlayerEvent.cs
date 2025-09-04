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
    public class PlayerEvent_Local_ItemBag_Expend
    {
        public int itemID;
        public int itemCount;
    }
    /// <summary>
    /// �������:��ӱ�������
    /// </summary>
    public class PlayerEvent_Local_ItemBag_Add
    {
        public int index;
        public ItemData itemData;
    }
    /// <summary>
    /// �������:���ı�������
    /// </summary>
    public class PlayerEvent_Local_ItemBag_Change
    {
        public ItemData itemData;
        public int index;
    }
    /// <summary>
    /// �������:ʹ�ñ�������
    /// </summary>
    public class PlayerEvent_Local_ItemBag_Use
    {
        public int index;
    }
    #endregion
    #region//����ֳ�
    /// <summary>
    /// �������:����ֲ�����
    /// </summary>
    public class PlayerEvent_Local_ItemHand_Add
    {
        public ItemData item;
        public Action<ItemData> itemResidueBack;
    }
    /// <summary>
    /// �������:�Ƴ��ֲ�����
    /// </summary>
    public class PlayerEvent_Local_ItemHand_Sub
    {
        public ItemData item;
    }
    /// <summary>
    /// �������:�����ֲ�����
    /// </summary>
    public class PlayerEvent_Local_ItemHand_Change
    {
        public ItemData oldItem;
        public ItemData newItem;
    }
    /// <summary>
    /// �������:�л��ֳ�����
    /// </summary>
    public class PlayerEvent_Local_ItemHand_Switch 
    {
        public int index;
    }
    /// <summary>
    /// �������:�����ֳ�����
    /// </summary>
    public class PlayerEvent_Local_ItemHand_PutAway
    {
        
    }

    #endregion
    #region//���ͷ��
    /// <summary>
    /// �������:�Ƴ�ͷ������
    /// </summary>
    public class PlayerEvent_Local_ItemHead_Sub
    {
        public ItemData item;
    }
    /// <summary>
    /// �������:���ͷ������
    /// </summary>
    public class PlayerEvent_Local_ItemHead_Add
    {
        public ItemData item;
        public Action<ItemData> itemResidueBack;
    }
    /// <summary>
    /// �������:����ͷ������
    /// </summary>
    public class PlayerEvent_Local_ItemHead_Change
    {
        public ItemData oldItem;
        public ItemData newItem;
    }
    /// <summary>
    /// �������:�л�ͷ������
    /// </summary>
    public class PlayerEvent_Local_ItemHead_Switch
    {
        public int index;
    }
    /// <summary>
    /// �������:����ͷ������
    /// </summary>
    public class PlayerEvent_Local_ItemHead_PutAway
    {

    }

    #endregion
    #region//�������
    /// <summary>
    /// �������:�Ƴ���������
    /// </summary>
    public class PlayerEvent_Local_ItemBody_Sub
    {
        public ItemData item;
    }
    /// <summary>
    /// �������:�����������
    /// </summary>
    public class PlayerEvent_Local_ItemBody_Add
    {
        public ItemData item;
        public Action<ItemData> itemResidueBack;
    }
    /// <summary>
    /// �������:������������
    /// </summary>
    public class PlayerEvent_Local_ItemBody_Change
    {
        public ItemData oldItem;
        public ItemData newItem;
    }
    /// <summary>
    /// �������:�滻��������
    /// </summary>
    public class PlayerEvent_Local_ItemBody_Switch
    {
        public int index;
    }
    /// <summary>
    /// �������:������������
    /// </summary>
    public class PlayerEvent_Local_ItemBody_PutAway
    {

    }
    #endregion
    #region//�����Ʒ
    /// <summary>
    /// �������:�Ƴ���Ʒ
    /// </summary>
    public class PlayerEvent_Local_ItemAccessory_Sub
    {
        public ItemData item;
    }
    /// <summary>
    /// �������:�����Ʒ
    /// </summary>
    public class PlayerEvent_Local_ItemAccessory_Add
    {
        public ItemData item;
        public Action<ItemData> itemResidueBack;
    }
    /// <summary>
    /// �������:������Ʒ
    /// </summary>
    public class PlayerEvent_Local_ItemAccessory_Change
    {
        public ItemData oldItem;
        public ItemData newItem;
    }
    /// <summary>
    /// �������:�滻��Ʒ
    /// </summary>
    public class PlayerEvent_Local_ItemAccessory_Switch
    {
        public int index;
    }
    /// <summary>
    /// �������:������Ʒ
    /// </summary>
    public class PlayerEvent_Local_ItemAccessory_PutAway
    {

    }

    #endregion
    #region//��ҺĲ�
    /// <summary>
    /// �������:�Ƴ��Ĳ�
    /// </summary>
    public class PlayerEvent_Local_ItemConsumables_Sub
    {
        public ItemData item;
    }
    /// <summary>
    /// �������:��ӺĲ�
    /// </summary>
    public class PlayerEvent_Local_ItemConsumables_Add
    {
        public ItemData item;
        public Action<ItemData> itemResidueBack;
    }
    /// <summary>
    /// �������:���ĺĲ�
    /// </summary>
    public class PlayerEvent_Local_ItemConsumables_Change
    {
        public ItemData oldItem;
        public ItemData newItem;
    }
    /// <summary>
    /// �������:�滻�Ĳ�
    /// </summary>
    public class PlayerEvent_Local_ItemConsumables_Switch
    {
        public int index;
    }
    /// <summary>
    /// �������:����Ĳ�
    /// </summary>
    public class PlayerEvent_Local_ItemConsumables_PutAway
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
