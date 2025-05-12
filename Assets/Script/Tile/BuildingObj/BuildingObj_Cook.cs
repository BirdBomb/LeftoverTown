using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UniRx;
using UnityEngine;

public class BuildingObj_Cook : BuildingObj
{
    [SerializeField]
    private GameObject obj_Singal;
    [SerializeField]
    private GameObject obj_Fire;
    [SerializeField]
    private GameObject prefab_UI;
    private bool bool_OpenUI = false;
    [HideInInspector]
    public TileUI_Cook tileUI_Bind;
    [HideInInspector]
    public ItemData itemData_Raw0;
    [HideInInspector]
    public ItemData itemData_Raw1;
    [HideInInspector]
    public ItemData itemData_Raw2;
    [HideInInspector]
    public ItemData itemData_Food;
    [HideInInspector]
    public int gameTime_CookFinishTime;
    [HideInInspector]
    public int gameTime_CurTimeSign;
    [SerializeField, Header("烹饪用时")]
    private int config_CookDuration;
    private bool getAllRaw = false;
    public override void Start()
    {
        MessageBroker.Default.Receive<GameEvent.GameEvent_All_UpdateSecond>().Subscribe(_ =>
        {
            gameTime_CurTimeSign = _.second + _.hour * 60 + _.day * 600;
            UpdateTime();
        }).AddTo(this);
    }
    private void UpdateTime()
    {
        if (getAllRaw)
        {
            if (gameTime_CookFinishTime < gameTime_CurTimeSign)
            {
                if (MapManager.Instance.mapNetManager.Object.HasStateAuthority)
                {
                    if (itemData_Food.Item_ID == 0)
                    {
                        Cook();
                    }
                    else
                    {
                        /*堵塞*/
                    }
                }
                Fire(false);
            }
            else
            {
                int val = gameTime_CookFinishTime - gameTime_CurTimeSign;
                if (tileUI_Bind != null)
                {
                    tileUI_Bind.ShakeRaw();
                    tileUI_Bind.DrawBar(1 - val / (float)config_CookDuration);
                }
                Fire(true);
            }
        }
        else
        {
            Fire(false);
        }
    }
    #region//信息更新与上传
    public override void All_UpdateInfo(string info)
    {
        ReadInfo(info);
        if (itemData_Raw0.Item_ID != 0 && itemData_Raw1.Item_ID != 0 && itemData_Raw2.Item_ID != 0)
        {
            getAllRaw = true;
        }
        else
        {
            getAllRaw = false;
        }
        base.All_UpdateInfo(info);
    }
    public void ReadInfo(string info)
    {
        itemData_Raw0 = new ItemData();
        itemData_Raw1 = new ItemData();
        itemData_Raw2 = new ItemData();
        itemData_Food = new ItemData();
        string[] strings = info.Split("/*I*/");
        for (int i = 0; i < strings.Length; i++)
        {
            if (strings[i] != "")
            {
                if (i == 0)
                {
                    itemData_Raw0 = JsonUtility.FromJson<ItemData>(strings[i]);
                }
                else if (i == 1)
                {
                    itemData_Raw1 = JsonUtility.FromJson<ItemData>(strings[i]);
                }
                else if (i == 2)
                {
                    itemData_Raw2 = JsonUtility.FromJson<ItemData>(strings[i]);
                }
                else if (i == 3)
                {
                    itemData_Food = JsonUtility.FromJson<ItemData>(strings[i]);
                }
                else if (i == 4)
                {
                    gameTime_CookFinishTime = int.Parse(strings[i]);
                }
            }
        }
        if (tileUI_Bind)
        {
            tileUI_Bind.DrawAllCell();
        }
    }
    public void WriteInfo()
    {
        StringBuilder builder = new StringBuilder();
        builder.Append(JsonUtility.ToJson(itemData_Raw0));
        builder.Append("/*I*/" + JsonUtility.ToJson(itemData_Raw1));
        builder.Append("/*I*/" + JsonUtility.ToJson(itemData_Raw2));
        builder.Append("/*I*/" + JsonUtility.ToJson(itemData_Food));
        builder.Append("/*I*/" + (gameTime_CurTimeSign + config_CookDuration));
        Local_ChangeInfo(builder.ToString());
    }
    #endregion
    #region//烹任
    List<short> rawList = new List<short>() { };

    private void Cook()
    {
        rawList.Clear();
        rawList.Add(itemData_Raw0.Item_ID);
        rawList.Add(itemData_Raw1.Item_ID);
        rawList.Add(itemData_Raw2.Item_ID);

        int foodID = 4100;
        for (int i = 0; i < CookConfigData.cookConfigs.Count; i++)
        {
            List<short> temp = new List<short>(rawList);
            if (CookConfigData.cookConfigs[i].Cook_Raw_0 != null)
            {
                bool hasRaw = false;
                for (int j = 0; j < CookConfigData.cookConfigs[i].Cook_Raw_0.Count; j++)
                {
                    if (temp.Contains(CookConfigData.cookConfigs[i].Cook_Raw_0[j]))
                    {
                        temp.Remove(CookConfigData.cookConfigs[i].Cook_Raw_0[j]);
                        hasRaw = true;
                        break;
                    }
                }
                if (!hasRaw)
                {
                    continue;
                }
            }
            if (CookConfigData.cookConfigs[i].Cook_Raw_1 != null)
            {
                bool hasRaw = false;
                for (int j = 0; j < CookConfigData.cookConfigs[i].Cook_Raw_1.Count; j++)
                {
                    if (temp.Contains(CookConfigData.cookConfigs[i].Cook_Raw_1[j]))
                    {
                        temp.Remove(CookConfigData.cookConfigs[i].Cook_Raw_1[j]);
                        hasRaw = true;
                        break;
                    }
                }
                if (!hasRaw)
                {
                    continue;
                }
            }
            if (CookConfigData.cookConfigs[i].Cook_Raw_2 != null)
            {
                bool hasRaw = false;
                for (int j = 0; j < CookConfigData.cookConfigs[i].Cook_Raw_2.Count; j++)
                {
                    if (temp.Contains(CookConfigData.cookConfigs[i].Cook_Raw_2[j]))
                    {
                        temp.Remove(CookConfigData.cookConfigs[i].Cook_Raw_2[j]);
                        hasRaw = true;
                        break;
                    }
                }
                if (!hasRaw)
                {
                    continue;
                }
            }


            
            /*所有材料都齐全*/
            foodID = CookConfigData.cookConfigs[i].Cook_ID;
            break;
        }
        itemData_Raw0 = new ItemData();
        itemData_Raw1 = new ItemData();
        itemData_Raw2 = new ItemData();
        Type type = Type.GetType("Item_" + foodID.ToString());
        ((ItemBase)Activator.CreateInstance(type)).StaticAction_InitData((short)foodID, out ItemData item);
        itemData_Food = item;
        WriteInfo();
    }
    private void Fire(bool on)
    {
        if (obj_Fire.activeSelf != on)
        {
            obj_Fire.SetActive(on);
        }
    }

    #endregion
    #region//瓦片交互
    public override void All_ActorInputKeycode(ActorManager actor, KeyCode code)
    {
        if (code == KeyCode.F)
        {
            OpenOrCloseSingal(bool_OpenUI);
            OpenOrCloseCreateUI(!bool_OpenUI);
        }
        base.All_ActorInputKeycode(actor, code);
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
    private void OpenOrCloseCreateUI(bool open)
    {
        if (open)
        {
            bool_OpenUI = true;
            UIManager.Instance.ShowTileUI(prefab_UI, out TileUI tileUI);
            tileUI_Bind = tileUI.GetComponent<TileUI_Cook>();
            tileUI_Bind.BindBuilding(this);
            ReadInfo(info);
        }
        else
        {
            bool_OpenUI = false;
            if (tileUI_Bind) UIManager.Instance.HideTileUI(tileUI_Bind);
            if (tileUI_Bind) tileUI_Bind = null;
        }
    }
    public override bool All_PlayerHolding(PlayerCoreLocal player)
    {
        /*靠近是我自己*/
        if (player.bool_Local)
        {
            OpenOrCloseSingal(true);
            return true;
        }
        return false;
    }
    public override bool All_PlayerRelease(PlayerCoreLocal player)
    {
        /*离开是我自己*/
        if (player.bool_Local)
        {
            OpenOrCloseSingal(false);
            OpenOrCloseCreateUI(false);
            return true;
        }
        return true;
    }
    #endregion

}
