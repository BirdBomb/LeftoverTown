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
    /// �´����ȼ��ʱ��
    /// </summary>
    public int gameTime_NextFuelSign = 0;
    /// <summary>
    /// �´��������ʱ��
    /// </summary>
    public int gameTime_NextRefiningSign = 0;
    /// <summary>
    /// ��¼ʱ��
    /// </summary>
    public int gameTime_LastTimeSign;
    /// <summary>
    /// ��ǰʱ��
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
    #region//ȼ�ռ���
    /// <summary>
    /// �ṩ��ȼ��ʱ��
    /// </summary>
    int fuelOffer = 0;
    /// <summary>
    /// �Ƿ���Ĳ��ϴ���Ϣ
    /// </summary>
    bool uploadInfo = false;
    private void UpdateTime()
    {
        uploadInfo = false;
        if (gameTime_NextFuelSign > gameTime_CurTimeSign)
        {
            /*����ʣ��ȼ��ʱ��*/
            fuelOffer = gameTime_CurTimeSign - gameTime_LastTimeSign;
            Fire(true);
        }
        else
        {
            /*û��ʣ��ȼ��ʱ��*/
            if (itemData_Fuel.Item_ID != 0 && itemData_Fuel.Item_Count > 0)
            {
                /*��ʣ��ȼ��*/
                int count = (gameTime_CurTimeSign - gameTime_NextFuelSign) / config_Fuel.FuelSecond + 1;
                fuelOffer = ExpendFuel(count);
                uploadInfo = true;
                Fire(true);
            }
            else
            {
                /*��ʣ��ȼ��*/
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
            /*��ԭ��*/
            if (fuelOffer > 0)
            {
                /*��¯��ȼ��*/
                int RefiningAfter = config_Refining.RefiningAfterID;
                if (itemData_RefiningAfter.Item_ID == RefiningAfter || itemData_RefiningAfter.Item_ID == 0)
                {
                    /*�ϳ�ͨ˳*/
                    if (fuelOffer + gameTime_LastTimeSign > gameTime_NextRefiningSign)
                    {
                        /*�������*/
                        int offset = fuelOffer + gameTime_LastTimeSign - gameTime_NextRefiningSign;
                        int count = offset / config_Refining.RefiningSecond + 1;
                        fuelOffer = ExpendRefining(fuelOffer, count);
                        uploadInfo = true;
                    }
                    else
                    {
                        /*����δ���*/
                    }
                }
                else
                {
                    /*�ϳ�����*/
                    gameTime_NextRefiningSign += (gameTime_CurTimeSign - gameTime_LastTimeSign);
                }
            }
            else
            {
                /*��¯��ȼ��*/
                gameTime_NextRefiningSign += (gameTime_CurTimeSign - gameTime_LastTimeSign);
            }
        }
        else
        {
            /*û��ԭ��*/
            gameTime_NextRefiningSign = int.MaxValue;
        }
        /*���չ�ʣȼ��ʱ��*/
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
    /// ����ȼ��
    /// </summary>
    /// <param name="curTime"></param>
    /// <param name="expendCount"></param>
    /// <returns>�ṩ��ȼ��ʱ��</returns>
    private int ExpendFuel(int expendCount)
    {
        int fuelTime;
        if (expendCount > itemData_Fuel.Item_Count)
        {
            /*ԭ�ϲ���*/
            expendCount = itemData_Fuel.Item_Count;
            fuelTime = expendCount * config_Fuel.FuelSecond + (gameTime_NextFuelSign - gameTime_LastTimeSign);
        }
        else
        {
            /*ԭ�ϳ���*/
            fuelTime = gameTime_CurTimeSign - gameTime_LastTimeSign;
        }

        ItemData itemData_Expend = itemData_Fuel;
        itemData_Expend.Item_Count = (short)expendCount;
        itemData_Fuel = GameToolManager.Instance.SplitItem(itemData_Fuel, itemData_Expend);
        gameTime_NextFuelSign += config_Fuel.FuelSecond * expendCount;

        return fuelTime;
    }
    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="expendCount"></param>
    /// <returns>ʣ���ȼ��ʱ��</returns>
    private int ExpendRefining(int fuel, int expendCount)
    {
        int fuelResTime;
        if (expendCount > itemData_RefiningBefore.Item_Count)
        {
            /*ԭ�ϲ���*/
            expendCount = itemData_RefiningBefore.Item_Count;
            fuelResTime = (gameTime_LastTimeSign + fuel) - (gameTime_NextRefiningSign + config_Refining.RefiningSecond * expendCount);
            gameTime_NextRefiningSign = int.MaxValue;
        }
        else
        {
            /*ԭ�ϳ���*/
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
    /// ��������
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
    #region//��Ϣ�������ϴ�
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
    #region//��Ƭ����
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
