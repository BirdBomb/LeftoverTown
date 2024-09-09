using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.U2D;
using static UnityEditor.Progress;

public class TileObj_Bonfire : TileObj
{
    [SerializeField, Header("ȼ��UI")]
    private UI_Grid_Fuel uI_Grid_Fuel;
    [SerializeField, Header("�տ�UI")]
    private UI_Grid_Barbecue uI_Grid_Barbecue;
    [SerializeField, Header("SingalF")]
    private GameObject obj_singalF;
    [SerializeField, Header("SingalE")]
    private GameObject obj_singalE;
    [SerializeField, Header("Fuel")]
    private GameObject obj_fuel;
    [SerializeField, Header("Barbecue")]
    private GameObject obj_barbecue;
    [SerializeField, Header("Fire")]
    private GameObject obj_fire;
    [SerializeField, Header("Item")]
    private SpriteRenderer sprite_Item;

    private SpriteAtlas itemAtlas;
    private short lastItemID;
    /// <summary>
    /// ȼ����Ʒ
    /// </summary>
    private ItemData fuelItem;
    /// <summary>
    /// ȼ��ֵ
    /// </summary>
    private short fuelVal;
    /// <summary>
    /// ȼ�������ֵ
    /// </summary>
    [Header("ȼ�����ֵ")]
    public short fuelMax;
    /// <summary>
    /// ������Ʒ
    /// </summary>
    private ItemData cookItem;
    /// <summary>
    /// ���Ƚ���
    /// </summary>
    private short cookVal;
    /// <summary>
    /// �������ֵ
    /// </summary>
    private short cookMax;

    private void Awake()
    {
        itemAtlas = Resources.Load<SpriteAtlas>("Atlas/ItemSprite");
    }
    private void Start()
    {
        InvokeRepeating("AddSecond",1,1);
    }
    private void AddSecond()
    {
        if (fuelVal > 0)
        {
            Burn();
        }
    }
    #region//��ҽ���
    public override void Invoke(PlayerController player, KeyCode code)
    {
        if (code == KeyCode.F)
        {
            OpenOrCloseSingal(obj_fuel.activeSelf);
            OpenOrCloseFuel(!obj_fuel.activeSelf);
            OpenOrCloseBarbecue(false);
        }
        if (code == KeyCode.E)
        {
            OpenOrCloseSingal(obj_barbecue.activeSelf);
            OpenOrCloseBarbecue(!obj_barbecue.activeSelf);
            OpenOrCloseFuel(false);
        }
        base.Invoke(player, code);
    }
    private void OpenOrCloseSingal(bool open)
    {
        obj_singalF.transform.DOKill();
        obj_singalE.transform.DOKill();
        if (open)
        {
            obj_singalF.SetActive(true);
            obj_singalF.transform.localScale = Vector3.one;
            obj_singalF.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
            obj_singalE.SetActive(true);
            obj_singalE.transform.localScale = Vector3.one;
            obj_singalE.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
        }
        else
        {
            obj_singalF.transform.DOScale(Vector3.zero, 0.1f).OnComplete(() =>
            {
                obj_singalF.SetActive(false);
            });
            obj_singalE.transform.DOScale(Vector3.zero, 0.1f).OnComplete(() =>
            {
                obj_singalE.SetActive(false);
            });
        }
    }
    private void OpenOrCloseFuel(bool open)
    {
        if (open)
        {
            obj_fuel.transform.localScale = Vector3.one;
            obj_fuel.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
            obj_fuel.SetActive(true);
            uI_Grid_Fuel.Open(this);
            uI_Grid_Fuel.BindAction(BindAddFuelAction, BindIgniteFuelAction);
            uI_Grid_Fuel.UpdateInfoFromTile(fuelVal, fuelMax, fuelItem);
        }
        else
        {
            obj_fuel.transform.DOScale(Vector3.zero, 0.1f).OnComplete(() =>
            {
                obj_fuel.SetActive(false);
            });
            uI_Grid_Fuel.Close(this);
        }
    }
    private void OpenOrCloseBarbecue(bool open)
    {
        if (open)
        {
            obj_barbecue.transform.localScale = Vector3.one;
            obj_barbecue.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
            obj_barbecue.SetActive(true);
            uI_Grid_Barbecue.Open(this);
            uI_Grid_Barbecue.BindAction(BindAddBarbecue);
            uI_Grid_Barbecue.UpdateInfoFromTile(cookVal, cookMax, cookItem);
        }
        else
        {
            obj_barbecue.transform.DOScale(Vector3.zero, 0.1f).OnComplete(() =>
            {
                obj_barbecue.SetActive(false);
            });
            uI_Grid_Barbecue.Close(this);
        }
    }

    public override bool PlayerNearby(PlayerController player)
    {
        /*���������Լ�*/
        if (player.thisPlayerIsMe)
        {
            OpenOrCloseSingal(true);
            return true;
        }
        return false;
    }
    public override bool PlayerFaraway(PlayerController player)
    {
        /*�뿪�����Լ�*/
        if (player.thisPlayerIsMe)
        {
            OpenOrCloseSingal(false);
            OpenOrCloseFuel(false);
            OpenOrCloseBarbecue(false);
            return true;
        }
        return true;
    }
    #endregion
    #region//�¼���
    /// <summary>
    /// ���ȼ��
    /// </summary>
    /// <param name="data"></param>
    public void BindAddFuelAction(ItemData data)
    {
        fuelItem = data;
    }
    /// <summary>
    /// ����ȼ��ֵ
    /// </summary>
    /// <param name="val"></param>
    public void BindIgniteFuelAction(short val)
    {
        fuelVal += val;
        if (fuelVal > fuelMax) { fuelVal = fuelMax; }
    }
    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="data"></param>
    /// <param name="max"></param>
    public void BindAddBarbecue(ItemData data, short max)
    {
        cookItem = data; 
        cookVal = 0; 
        cookMax = max;
    }
    #endregion
    #region//��Ϣ�������ϴ�
    public override void TryToUpdateInfo(string info)
    {
        string[] strings = info.Split("/*I*/");
        for (int i = 0; i < strings.Length; i++)
        {
            if (i == 0)
            {
                /*��һλ��ȼ��id*/
                if (strings[i] != "")
                {
                    fuelItem = JsonUtility.FromJson<ItemData>(strings[i]);
                }
            }
            else if (i == 1)
            {
                /*�ڶ�λ��ȼ��ʣ��*/
                if (strings[i] != "")
                {
                    fuelVal = (short.Parse(strings[i]));
                }
            }

            else if (i == 2)
            {
                /*����λ�����id*/
                if (strings[i] != "")
                {
                    cookItem = JsonUtility.FromJson<ItemData>(strings[i]);
                }
            }

            else if (i == 3)
            {
                /*����λ�����ʣ��*/
                if (strings[i] != "")
                {
                    cookVal = (short.Parse(strings[i]));
                }
            }
            else if (i == 4)
            {
                /*����λ�����ʣ��*/
                if (strings[i] != "")
                {
                    cookMax = (short.Parse(strings[i]));
                }
            }
        }
        uI_Grid_Fuel.UpdateInfoFromTile(fuelVal, fuelMax, fuelItem);
        uI_Grid_Barbecue.UpdateInfoFromTile(cookVal, cookMax, cookItem);
        UpdateBonfire();
        base.TryToUpdateInfo(info);
    }
    public override void TryToChangeInfo(string info)
    {
        StringBuilder builder = new StringBuilder();
        builder.Append(JsonUtility.ToJson(fuelItem));
        builder.Append("/*I*/" + fuelVal);
        builder.Append("/*I*/" + JsonUtility.ToJson(cookItem));
        builder.Append("/*I*/" + cookVal);
        builder.Append("/*I*/" + cookMax);
        base.TryToChangeInfo(builder.ToString());
    }
    #endregion
    #region//����
    private void UpdateBonfire()
    {
        if (fuelVal > 0) { obj_fire.SetActive(true); }
        else { obj_fire.SetActive(false); }
        if (cookItem.Item_ID != lastItemID)
        {
            lastItemID = cookItem.Item_ID;
            if (lastItemID > 0)
            {
                sprite_Item.sprite = itemAtlas.GetSprite("Item_" + lastItemID.ToString());
            }
            else
            {
                sprite_Item.sprite = null;
            }
        }
    }
    private void Burn()
    {
        fuelVal--;
        if (fuelVal == 0)
        {
            TryToChangeInfo("");
        }
        if (cookVal < cookMax)
        {
            Debug.Log(cookVal + "/" + cookMax);
            cookVal++;
            if (cookVal == cookMax)
            {
                Barbecue();
            }
            TryToChangeInfo("");
        }
        uI_Grid_Fuel.UpdateInfoFromTile(fuelVal, fuelMax, fuelItem);
        uI_Grid_Barbecue.UpdateInfoFromTile(cookVal, cookMax, cookItem);
    }
    private void Barbecue()
    {
        short id = BarbecueConfigData.GetBarbecueConfig(cookItem.Item_ID).BarbecueToID;
        Type type = Type.GetType("Item_" + id.ToString());
        ItemData itemData = new ItemData
            (id,
             MapManager.Instance.mapSeed + (int)(System.DateTime.Now.Ticks * 1000),
             1,
             0,
             0,
             0,
             new ContentData());
        ((ItemBase)Activator.CreateInstance(type)).StaticAction_InitData(itemData, out ItemData initData);
        cookItem = initData;
    }
    #endregion
}
