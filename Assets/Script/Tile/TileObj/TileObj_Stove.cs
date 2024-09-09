using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.U2D;

public class TileObj_Stove : TileObj
{
    [SerializeField, Header("燃料UI")]
    private UI_Grid_Fuel uI_Grid_Fuel;
    [SerializeField, Header("烹饪UI")]
    private UI_Grid_CookPanel uI_Grid_CookPanel;
    [SerializeField, Header("SingalF")]
    private GameObject obj_singalF;
    [SerializeField, Header("SingalE")]
    private GameObject obj_singalE;
    [SerializeField, Header("Fuel")]
    private GameObject obj_fuel;
    [SerializeField, Header("CookPanel")]
    private GameObject obj_cookPanel;
    [SerializeField, Header("Item")]
    private SpriteRenderer sprite_Item;
    [SerializeField, Header("Fire")]
    private GameObject obj_fire;

    private SpriteAtlas itemAtlas;
    private short lastItemID;
    private ItemData fuelItem;
    private short fuelVal;
    private List<ItemData> cookRawDataList = new List<ItemData>();
    private ItemData cookLocalResultData = new ItemData();
    private ItemData cookResultData = new ItemData();
    private short cookVal;
    private short cookMax;

    private short fuelMax = 100;

    private void Awake()
    {
        itemAtlas = Resources.Load<SpriteAtlas>("Atlas/ItemSprite");
    }
    private void Start()
    {
        InvokeRepeating("AddSecond", 1, 1);
    }
    private void AddSecond()
    {
        if (fuelVal > 0)
        {
            Burn();
        }
    }

    #region//玩家交互
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
            OpenOrCloseSingal(obj_cookPanel.activeSelf);
            OpenOrCloseBarbecue(!obj_cookPanel.activeSelf);
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
            obj_cookPanel.transform.localScale = Vector3.one;
            obj_cookPanel.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
            obj_cookPanel.SetActive(true);
            uI_Grid_CookPanel.Open(this);
            uI_Grid_CookPanel.BindAction(BindAddRawlAction, BindCookAction, BindPutOutFood);
            uI_Grid_CookPanel.UpdateInfoFromTile(cookRawDataList, cookResultData, cookVal, cookMax);
        }
        else
        {
            obj_cookPanel.transform.DOScale(Vector3.zero, 0.1f).OnComplete(() =>
            {
                obj_cookPanel.SetActive(false);
            });
            uI_Grid_CookPanel.Close(this);
        }
    }
    public override bool PlayerNearby(PlayerController player)
    {
        /*靠近是我自己*/
        if (player.thisPlayerIsMe)
        {
            OpenOrCloseSingal(true);
            return true;
        }
        return false;
    }
    public override bool PlayerFaraway(PlayerController player)
    {
        /*离开是我自己*/
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
    #region//事件绑定
    /// <summary>
    /// 添加食材
    /// </summary>
    /// <param name="cookRaw"></param>
    public void BindAddRawlAction(List<ItemData> cookRaw)
    {
        cookRawDataList.Clear();
        for (int i = 0; i < cookRaw.Count; i++)
        {
            cookRawDataList.Add(cookRaw[i]);
        }
    }
    /// <summary>
    /// 开始烹饪
    /// </summary>
    /// <param name="cookResult"></param>
    /// <param name="cookTime"></param>
    /// <param name="cookMaxTime"></param>
    public void BindCookAction(ItemData cookResult, short cookTime, short cookMaxTime)
    {
        cookVal = cookTime;
        cookMax = cookMaxTime;
        cookLocalResultData = cookResult;
    }
    /// <summary>
    /// 取出成菜
    /// </summary>
    public void BindPutOutFood()
    {
        cookResultData = new ItemData();
    }
    /// <summary>
    /// 添加燃料
    /// </summary>
    /// <param name="data"></param>
    public void BindAddFuelAction(ItemData data)
    {
        fuelItem = data;
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
    #endregion

    #region//信息更新与上传
    /// <summary>
    /// 信息更新
    /// </summary>
    /// <param name="info"></param>
    public override void TryToUpdateInfo(string info)
    {
        cookRawDataList.Clear();
        string[] strings = info.Split("/*I*/");
        for (int i = 0; i < strings.Length; i++)
        {
            if (i == 0)
            {
                /*第一位是燃料id*/
                if (strings[i] != "")
                {
                    fuelItem = JsonUtility.FromJson<ItemData>(strings[i]);
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
                /*第三位是食材id*/
                if (strings[i] != "")
                {
                    cookRawDataList.Add(JsonUtility.FromJson<ItemData>(strings[i]));
                }
            }
            else if (i == 3)
            {
                /*第四位是食材id*/
                if (strings[i] != "")
                {
                    cookRawDataList.Add(JsonUtility.FromJson<ItemData>(strings[i]));
                }
            }
            else if (i == 4)
            {
                /*第五位是食材id*/
                if (strings[i] != "")
                {
                    cookRawDataList.Add(JsonUtility.FromJson<ItemData>(strings[i]));
                }
            }
            else if (i == 5)
            {
                /*第六位是食材id*/
                if (strings[i] != "")
                {
                    cookRawDataList.Add(JsonUtility.FromJson<ItemData>(strings[i]));
                }
            }
            else if (i == 6)
            {
                /*第七位是烹饪剩余时间*/
                if (strings[i] != "")
                {
                    cookVal = (short.Parse(strings[i]));
                }
            }
            else if (i == 7)
            {
                /*第八位是烹饪所需时间*/
                if (strings[i] != "")
                {
                    cookMax = (short.Parse(strings[i]));
                }
            }
            else if (i == 8)
            {
                /*第九位是成菜id*/
                if (strings[i] != "")
                {
                    cookResultData = JsonUtility.FromJson<ItemData>(strings[i]);
                }
            }
        }
        uI_Grid_Fuel.UpdateInfoFromTile(fuelVal, fuelMax, fuelItem);
        uI_Grid_CookPanel.UpdateInfoFromTile(cookRawDataList, cookResultData, cookVal, cookMax);
        UpdateStove();
        base.TryToUpdateInfo(info);
    }
    /// <summary>
    /// 信息上传
    /// </summary>
    /// <param name="info"></param>
    public override void TryToChangeInfo(string info)
    {
        StringBuilder builder = new StringBuilder();
        builder.Append(JsonUtility.ToJson(fuelItem));
        builder.Append("/*I*/" + fuelVal);
        for(int i = 0; i < 4; i++)
        {
            if (cookRawDataList.Count > i)
            {
                builder.Append("/*I*/" + JsonUtility.ToJson(cookRawDataList[i]));
            }
            else
            {
                builder.Append("/*I*/" + JsonUtility.ToJson(new ItemData()));
            }
        }
        builder.Append("/*I*/" + cookVal);
        builder.Append("/*I*/" + cookMax);
        builder.Append("/*I*/" + JsonUtility.ToJson(cookResultData));
        base.TryToChangeInfo(builder.ToString());
    }
    #endregion
    #region//锅炉
    private void UpdateStove()
    {
        if (fuelVal > 0) { obj_fire.SetActive(true); }
        else { obj_fire.SetActive(false); }
        if (cookResultData.Item_ID != lastItemID)
        {
            lastItemID = cookResultData.Item_ID;
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
            cookVal++;
            if (cookVal == cookMax)
            {
                Cook();
            }
            TryToChangeInfo("");
        }
        uI_Grid_Fuel.UpdateInfoFromTile(fuelVal, fuelMax, fuelItem);
        uI_Grid_CookPanel.UpdateInfoFromTile(cookRawDataList, cookResultData, cookVal, cookMax);
    }
    private void Cook()
    {
        cookRawDataList.Clear();
        cookResultData = cookLocalResultData;
        cookLocalResultData = new ItemData();
    }
    #endregion
}
