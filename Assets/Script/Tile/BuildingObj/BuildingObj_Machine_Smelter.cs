using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UniRx;
using UnityEngine;

public class BuildingObj_Machine_Smelter : BuildingObj_Manmade
{
    public GameObject obj_SingalFUI;
    public GameObject obj_SingalAwakeUI;
    public GameObject obj_HightlightUI;
    [SerializeField]
    private GameObject obj_Fire;
    [SerializeField]
    private GameObject prefab_UI;
    private TileUI_Smelter tileUI_Bind;
    public ItemData itemData_RefiningBefore = new ItemData();
    public ItemData itemData_RefiningAfter = new ItemData();
    public ItemData itemData_Fuel = new ItemData();
    public RefiningConfig config_Refining;
    public FuelConfig config_Fuel;
    /// <summary>
    /// 下次完成燃烧时间
    /// </summary>
    public int gameTime_NextFuelSign = 0;
    /// <summary>
    /// 下次完成炼制时间
    /// </summary>
    public int gameTime_NextRefiningSign = 0;
    /// <summary>
    /// 记录时间
    /// </summary>
    public int gameTime_LastTimeSign;
    /// <summary>
    /// 当前时间
    /// </summary>
    public int gameTime_CurTimeSign;
    public override void Start()
    {
        MessageBroker.Default.Receive<GameEvent.GameEvent_All_UpdateSecond>().Subscribe(_ =>
        {
            gameTime_CurTimeSign = _.second + _.hour * 60 + _.day * 600;
            UpdateTime();
            gameTime_LastTimeSign = gameTime_CurTimeSign;
        }).AddTo(this);
    }
    #region//燃烧计算
    /// <summary>
    /// 提供的燃烧时间
    /// </summary>
    int fuelOffer = 0;
    /// <summary>
    /// 是否更改并上传信息
    /// </summary>
    bool uploadInfo = false;
    private void UpdateTime()
    {
        uploadInfo = false;
        if (gameTime_NextFuelSign > gameTime_CurTimeSign)
        {
            /*还有剩余燃烧时间*/
            fuelOffer = gameTime_CurTimeSign - gameTime_LastTimeSign;
            Fire(true);
        }
        else
        {
            /*没有剩余燃烧时间*/
            if (itemData_Fuel.Item_ID != 0 && itemData_Fuel.Item_Count > 0)
            {
                /*有剩余燃料*/
                int count = (gameTime_CurTimeSign - gameTime_NextFuelSign) / config_Fuel.FuelSecond + 1;
                fuelOffer = ExpendFuel(count);
                uploadInfo = true;
                Fire(true);
            }
            else
            {
                /*无剩余燃料*/
                fuelOffer = gameTime_NextFuelSign - gameTime_LastTimeSign;
                if (fuelOffer < 0) 
                {
                    Fire(false);
                    fuelOffer = 0; 
                }
                else
                {
                    Fire(true);
                }
            }
        }
        if (itemData_RefiningBefore.Item_ID != 0 && itemData_RefiningBefore.Item_Count != 0)
        {
            /*有原料*/
            if (fuelOffer > 0)
            {
                /*火炉在燃烧*/
                int RefiningAfter = config_Refining.RefiningAfterID;
                if (itemData_RefiningAfter.Item_ID == RefiningAfter || itemData_RefiningAfter.Item_ID == 0)
                {
                    /*合成通顺*/
                    if (fuelOffer + gameTime_LastTimeSign > gameTime_NextRefiningSign)
                    {
                        /*炼制完成*/
                        int offset = fuelOffer + gameTime_LastTimeSign - gameTime_NextRefiningSign;
                        int count = offset / config_Refining.RefiningSecond + 1;
                        fuelOffer = ExpendRefining(fuelOffer, count);
                        uploadInfo = true;
                    }
                    else
                    {
                        /*炼制未完成*/
                    }
                }
                else
                {
                    /*合成阻塞*/
                    gameTime_NextRefiningSign += (gameTime_CurTimeSign - gameTime_LastTimeSign);
                }
            }
            else
            {
                /*火炉不燃烧*/
                gameTime_NextRefiningSign += (gameTime_CurTimeSign - gameTime_LastTimeSign);
            }
        }
        else
        {
            /*没有原料*/
            gameTime_NextRefiningSign = int.MaxValue;
        }
        /*回收过剩燃烧时间*/
        //gameTime_NextFuelSign += fuelOffer;
        if (tileUI_Bind)
        {
            tileUI_Bind.DrawBar();
        }
        if (uploadInfo && MapManager.Instance.mapNetManager.Object.HasStateAuthority)
        {
            WriteInfo();
        }
    }
    /// <summary>
    /// 消耗燃料
    /// </summary>
    /// <param name="curTime"></param>
    /// <param name="expendCount"></param>
    /// <returns>提供的燃烧时间</returns>
    private int ExpendFuel(int expendCount)
    {
        int fuelTime;
        if (expendCount > itemData_Fuel.Item_Count)
        {
            /*原料不足*/
            expendCount = itemData_Fuel.Item_Count;
            fuelTime = expendCount * config_Fuel.FuelSecond + (gameTime_NextFuelSign - gameTime_LastTimeSign);
        }
        else
        {
            /*原料充足*/
            fuelTime = gameTime_CurTimeSign - gameTime_LastTimeSign;
        }

        ItemData itemData_Expend = itemData_Fuel;
        itemData_Expend.Item_Count = (short)expendCount;
        itemData_Fuel = GameToolManager.Instance.SplitItem(itemData_Fuel, itemData_Expend);
        gameTime_NextFuelSign += config_Fuel.FuelSecond * expendCount;

        return fuelTime;
    }
    /// <summary>
    /// 精炼消耗
    /// </summary>
    /// <param name="expendCount"></param>
    /// <returns>剩余的燃烧时间</returns>
    private int ExpendRefining(int fuel, int expendCount)
    {
        int fuelResTime;
        if (expendCount > itemData_RefiningBefore.Item_Count)
        {
            /*原料不足*/
            expendCount = itemData_RefiningBefore.Item_Count;
            fuelResTime = (gameTime_LastTimeSign + fuel) - (gameTime_NextRefiningSign + config_Refining.RefiningSecond * expendCount);
            gameTime_NextRefiningSign = int.MaxValue;
        }
        else
        {
            /*原料充足*/
            gameTime_NextRefiningSign += config_Refining.RefiningSecond * expendCount;
            fuelResTime = 0;
        }
        ItemData itemData_Expend = itemData_RefiningBefore;
        itemData_Expend.Item_Count = (short)expendCount;
        itemData_RefiningBefore = GameToolManager.Instance.SplitItem(itemData_RefiningBefore, itemData_Expend);
        CreateRefining(config_Refining.RefiningAfterID, expendCount);
        return fuelResTime;
    }
    /// <summary>
    /// 精炼产出
    /// </summary>
    /// <param name="id"></param>
    /// <param name="count"></param>
    private void CreateRefining(int id, int count)
    {
        Type type = Type.GetType("Item_" + id.ToString());
        ((ItemBase)Activator.CreateInstance(type)).StaticAction_InitData((short)id, out ItemData initData);
        initData.Item_Count = (short)count;
        itemData_RefiningAfter = GameToolManager.Instance.CombineItem(itemData_RefiningAfter, initData, out ItemData itemData_Res);
        if (MapManager.Instance.mapNetManager.Object.HasStateAuthority && itemData_Res.Item_ID != 0 && itemData_Res.Item_Count > 0)
        {
            State_CreateLootItem(new List<ItemData>() { itemData_Res });
        }
    }

    #endregion
    #region//信息更新与上传
    public override void All_UpdateInfo(string info)
    {
        ReadInfo(info);
        base.All_UpdateInfo(info);
    }
    public void ReadInfo(string info)
    {
        string[] strings = info.Split("/*I*/");
        for (int i = 0; i < strings.Length; i++)
        {
            if (i == 0 && strings[i] != "")
            {
                itemData_RefiningBefore = JsonUtility.FromJson<ItemData>(strings[i]);
                config_Refining = RefiningConfigData.GetRefiningConfig(itemData_RefiningBefore.Item_ID);
            }
            else if (i == 1 && strings[i] != "")
            {
                itemData_RefiningAfter = JsonUtility.FromJson<ItemData>(strings[i]);
            }
            else if (i == 2 && strings[i] != "")
            {
                itemData_Fuel = JsonUtility.FromJson<ItemData>(strings[i]);
                config_Fuel = FuelConfigData.GetFuelConfig(itemData_Fuel.Item_ID);
            }
            else if (i == 3 && strings[i] != "")
            {
                gameTime_NextFuelSign = int.Parse(strings[i]);
            }
            else if (i == 4 && strings[i] != "")
            {
                gameTime_NextRefiningSign = int.Parse(strings[i]);
            }
            else if (i == 5 && strings[i] != "")
            {
                gameTime_LastTimeSign = int.Parse(strings[i]);
            }
        }
        if (tileUI_Bind) 
        { 
            tileUI_Bind.DrawEveryCell(); 
            tileUI_Bind.DrawBar(); 
        }
    }
    public void WriteInfo()
    {
        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < 5; i++)
        {
            if (i == 0)
            {
                builder.Append(JsonUtility.ToJson(itemData_RefiningBefore));
            }
            else if (i == 1)
            {
                builder.Append("/*I*/" + JsonUtility.ToJson(itemData_RefiningAfter));
            }
            else if (i == 2)
            {
                builder.Append("/*I*/" + JsonUtility.ToJson(itemData_Fuel));
            }
            else if (i == 3)
            {
                builder.Append("/*I*/" + gameTime_NextFuelSign);
            }
            else if (i == 4)
            {
                builder.Append("/*I*/" + gameTime_NextRefiningSign);
            }
            else if (i == 5)
            {
                builder.Append("/*I*/" + gameTime_LastTimeSign);
            }
        }
        Local_ChangeInfo(builder.ToString());
    }
    #endregion
    #region//瓦片交互
    public override void All_ActorInputKeycode(ActorManager actor, KeyCode code)
    {
        if (code == KeyCode.F)
        {
            OpenOrCloseUI(tileUI_Bind == null);
        }
        base.All_ActorInputKeycode(actor, code);
    }
    public override void All_PlayerHighlight(bool on)
    {
        OpenOrCloseHighlightUI(on);
        base.All_PlayerHighlight(on);
    }
    public override void All_PlayerFaraway()
    {
        OpenOrCloseUI(false);
        base.All_PlayerFaraway();
    }
    public override void OpenOrCloseHighlightUI(bool open)
    {
        obj_SingalFUI.transform.DOKill();
        if (open)
        {
            obj_SingalFUI.SetActive(true);
            obj_SingalFUI.transform.localScale = Vector3.one;
            obj_SingalFUI.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
        }
        else
        {
            obj_SingalFUI.transform.DOScale(Vector3.zero, 0.1f).OnComplete(() =>
            {
                obj_SingalFUI.SetActive(false);
            });
        }
        obj_HightlightUI.transform.DOKill();
        if (open)
        {
            obj_HightlightUI.SetActive(true);
            obj_HightlightUI.transform.localScale = Vector3.one;
            obj_HightlightUI.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
        }
        else
        {
            obj_HightlightUI.transform.DOScale(Vector3.zero, 0.1f).OnComplete(() =>
            {
                obj_HightlightUI.SetActive(false);
            });
        }
    }
    public override void OpenOrCloseAwakeUI(bool open)
    {
        obj_SingalAwakeUI.transform.DOKill();
        if (open)
        {
            obj_SingalAwakeUI.SetActive(true);
            obj_SingalAwakeUI.transform.localScale = Vector3.one;
            obj_SingalAwakeUI.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
        }
        else
        {
            obj_SingalAwakeUI.transform.DOScale(Vector3.zero, 0.1f).OnComplete(() =>
            {
                obj_SingalAwakeUI.SetActive(false);
            });
        }
    }
    public override void OpenOrCloseUI(bool open)
    {
        if (open)
        {
            UIManager.Instance.ShowTileUI(prefab_UI, out TileUI tileUI);
            tileUI_Bind = tileUI.GetComponent<TileUI_Smelter>();
            tileUI_Bind.BindBuilding(this);
            tileUI_Bind.DrawEveryCell();
        }
        else
        {
            if (tileUI_Bind) UIManager.Instance.HideTileUI(tileUI_Bind);
            if (tileUI_Bind) tileUI_Bind = null;
        }

    }
    public override bool CanHighlight()
    {
        return true;
    }

    #endregion
    private void Fire(bool on)
    {
        if (obj_Fire.activeSelf != on)
        {
            obj_Fire.SetActive(on);
        }
    }
}
