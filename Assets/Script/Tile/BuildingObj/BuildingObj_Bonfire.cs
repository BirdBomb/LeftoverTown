using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

public class BuildingObj_Bonfire : BuildingObj
{
    [SerializeField]
    private GameObject obj_Singal;
    [SerializeField]
    private GameObject obj_UI;
    [SerializeField]
    private GameObject obj_Fire;
    [SerializeField]
    private SpriteRenderer sprite_Item;

    [SerializeField, Header("燃料上限")]
    private short fuelMax;
    private short fuelVal;
    private ItemData itemData_Fuel;
    private short cookVal;
    private short cookMax;
    private ItemData itemData_Cook;


    public override void Start()
    {
        InvokeRepeating("Burn", 1, 1);
        base.Start();
    }
    public override void UpdateInfo(string info)
    {
        string[] strings = info.Split("/*I*/");
        for (int i = 0; i < strings.Length; i++)
        {
            if (i == 0)
            {
                /*第一位是燃料id*/
                if (strings[i] != "")
                {
                    itemData_Fuel = JsonUtility.FromJson<ItemData>(strings[i]);
                }
            }
            else if (i == 1)
            {
                /*第二位是燃料剩余*/
                if (strings[i] != "")
                {
                    fuelVal = (short.Parse(strings[i]));
                }
            }

            else if (i == 2)
            {
                /*第三位是填充id*/
                if (strings[i] != "")
                {
                    itemData_Cook = JsonUtility.FromJson<ItemData>(strings[i]);
                }
            }

            else if (i == 3)
            {
                /*第四位是烹饪进度*/
                if (strings[i] != "")
                {
                    cookVal = (short.Parse(strings[i]));
                }
            }
            else if (i == 4)
            {
                /*第四位是烹饪时间*/
                if (strings[i] != "")
                {
                    cookMax = (short.Parse(strings[i]));
                }
            }
        }
        UpdateBonfire();
        base.UpdateInfo(info);
    }
    public override void ChangeInfo(string info = "")
    {
        StringBuilder builder = new StringBuilder();
        builder.Append(JsonUtility.ToJson(itemData_Fuel));
        builder.Append("/*I*/" + fuelVal);
        builder.Append("/*I*/" + JsonUtility.ToJson(itemData_Cook));
        builder.Append("/*I*/" + cookVal);
        builder.Append("/*I*/" + cookMax);
        base.ChangeInfo(builder.ToString());
    }

    public override bool PlayerHolding(PlayerCoreLocal player)
    {
        /*靠近是我自己*/
        if (player.bool_Local)
        {
            OpenOrCloseSingal(true);
            return true;
        }
        return false;
    }
    public override bool PlayerRelease(PlayerCoreLocal player)
    {
        /*离开是我自己*/
        if (player.bool_Local)
        {
            OpenOrCloseSingal(false);
            return true;
        }
        return false;
    }
    public override void ActorInputKeycode(ActorManager actor, KeyCode code)
    {
        if (code == KeyCode.F)
        {
            OpenOrCloseSingal(obj_UI.activeSelf);
            OpenOrCloseUI(!obj_UI.activeSelf);
        }
        base.ActorInputKeycode(actor, code);
    }
    private void OpenOrCloseSingal(bool open)
    {
        obj_Singal.transform.DOKill();
        if (open)
        {
            obj_Singal.SetActive(true);
            obj_Singal.transform.localScale = Vector3.one;
            obj_Singal.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
        }
        else
        {
            obj_Singal.transform.DOScale(Vector3.zero, 0.1f).OnComplete(() =>
            {
                obj_Singal.SetActive(false);
            });
        }
    }
    private void OpenOrCloseUI(bool open)
    {
        if (open)
        {
            obj_UI.transform.localScale = Vector3.one;
            obj_UI.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
            obj_UI.SetActive(true);
        }
        else
        {
            obj_UI.transform.DOScale(Vector3.zero, 0.1f).OnComplete(() =>
            {
                obj_UI.SetActive(false);
            });
        }
    }
    private void UpdateBonfire()
    {
        if (fuelVal > 0) { obj_Fire.SetActive(true); }
        else { obj_Fire.SetActive(false); }
    }
    private void Burn()
    {
        if (fuelVal > 0)
        {
            fuelVal--;
            if (fuelVal <= 0)
            {
                ChangeInfo();
            }
            if (cookVal < cookMax)
            {
                cookVal++;
                if (cookVal == cookMax)
                {
                    Barbecue(itemData_Cook.Item_ID);
                }
                ChangeInfo();
            }
        }
    }
    private void Barbecue(int ID)
    {
        short id = BarbecueConfigData.GetBarbecueConfig(ID).BarbecueToID;
        Type type = Type.GetType("Item_" + id.ToString());
        ((ItemBase)Activator.CreateInstance(type)).StaticAction_InitData(id, out ItemData initData);
        itemData_Cook = initData;
    }
    #region//事件绑定
    /// <summary>
    /// 添加燃料
    /// </summary>
    /// <param name="data"></param>
    public void BindAddFuelAction(ItemData data)
    {
        itemData_Fuel = data;
    }
    /// <summary>
    /// 增加燃烧值
    /// </summary>
    /// <param name="val"></param>
    public void BindIgniteFuelAction(short val)
    {
        fuelVal += val;
        if (fuelVal > fuelMax) { fuelVal = fuelMax; }
    }
    /// <summary>
    /// 加热物体
    /// </summary>
    /// <param name="data"></param>
    /// <param name="max"></param>
    public void BindAddBarbecue(ItemData data, short max)
    {
        itemData_Cook = data;
        cookVal = 0;
        cookMax = max;
    }
    #endregion

}
