using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Grid_CookPanel : UI_Grid
{
    [SerializeField, Header("原料格子")]
    private List<UI_GridCell> gridCells_Ingredient = new List<UI_GridCell>();
    [SerializeField, Header("成品格子")]
    private UI_GridCell gridCell_Food;
    [SerializeField, Header("最大容量")]
    private int int_IngredientCapacity;
    [SerializeField, Header("烹饪进度条")]
    private Image image_CookBar;
    [SerializeField, Header("烹饪结果描述")]
    private TextMeshProUGUI text_CookDesc;
    [SerializeField, Header("烹饪按钮")]
    private Button btn_CookStart;
    [SerializeField, Header("烹饪技能描述")]
    private TextMeshProUGUI text_CookSkill;
    [SerializeField, Header("烹饪技能加成")]
    private int skillOffset;
    public Action<List<ItemData>> action_PutInIngredient;
    public Action action_PutOutFood;
    public Action<ItemData, short, short> action_Cook;
    private List<ItemData> itemDatas_Ingredient = new List<ItemData>();
    private ItemData itemData_Food;
    public void Start()
    {
        BindAllCell();
    }
    private void BindAllCell()
    {
        for (int i = 0; i < gridCells_Ingredient.Count; i++)
        {
            gridCells_Ingredient[i].BindAction(PutInIngredient, PutOutIngredient, null, null);
        }
        gridCell_Food.BindAction(PutInFood, PutOutFood, null, null);
    }
    public void BindAction_Cook(Action<List<ItemData>> putInIngredient, Action<ItemData,short,short> cook,Action putOutFood)
    {
        action_PutInIngredient = putInIngredient;
        action_Cook = cook;
        action_PutOutFood = putOutFood;
    }
    #region//上传更新
    public void UpdateInfo(List<ItemData> rawList, ItemData result, short time, short maxTime)
    {
        itemData_Food = result;
        itemDatas_Ingredient.Clear();
        for (int i = 0; i < rawList.Count; i++)
        {
            if (rawList[i].Item_ID != 0)
            {
                itemDatas_Ingredient.Add(rawList[i]);
            }
        }
        if (maxTime != 0)
        {
            image_CookBar.transform.DOKill();
            image_CookBar.transform.DOScaleX(((float)time / maxTime), 0.1f);
            if (time == maxTime)
            {
                gridCell_Food.DOKill();
                gridCell_Food.transform.localScale = Vector3.one;
                gridCell_Food.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
            }
        }
        else
        {
            image_CookBar.transform.localScale = new Vector3(0, 1f, 1f);
        }
        DrawAllCell();
    }
    public void ChangeInfo()
    {
        if (action_ChangeInfo != null)
        {
            action_ChangeInfo.Invoke("");
        }
    }
    #endregion
    #region//UI绘制
    /// <summary>
    /// 绘制所有格子
    /// </summary>
    private void DrawAllCell()
    {
        for (int i = 0; i < gridCells_Ingredient.Count; i++)
        {
            if (itemDatas_Ingredient.Count > i && itemDatas_Ingredient[i].Item_ID > 0)
            {
                gridCells_Ingredient[i].UpdateData(itemDatas_Ingredient[i]);
            }
            else
            {
                gridCells_Ingredient[i].CleanData();
            }
        }
        if (itemData_Food.Item_ID > 0)
        {
            gridCell_Food.UpdateData(itemData_Food);
        }
        else
        {
            gridCell_Food.CleanData();
        }
        CheckRaw();
    }
    #endregion
    #region//UI管理
    private void CheckRaw()
    {
        text_CookSkill.text = "技能加成" + skillOffset.ToString();
        if (itemDatas_Ingredient.Count > 1)
        {
            CookConfig cookResult;
            FindCookResult(itemDatas_Ingredient[0].Item_ID, itemDatas_Ingredient[1].Item_ID, out cookResult);
            if (cookResult.Cook_ID != 0)
            {
                CheckCookResult(cookResult);
                btn_CookStart.gameObject.SetActive(true);
            }
        }
        else
        {
            btn_CookStart.gameObject.SetActive(false);
            text_CookDesc.text = "";
        }
    }
    private void FindCookResult(short raw_0, short raw_1, out CookConfig config)
    {
        config = CookConfigData.cookConfigs.Find((x) => 
        {
            if (x.CooK_Raw_Main.Contains(raw_0) && x.CooK_Raw_Add.Contains(raw_1))
            {
                return true;
            }
            else
            {
                return false;
            }
        });
        if (config.Cook_ID == 0)
        {
            config = CookConfigData.cookConfigs.Find((x) =>
            {
                if (x.CooK_Raw_Main.Contains(raw_1) && x.CooK_Raw_Add.Contains(raw_0))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            });
        }
        if(config.Cook_ID == 0)
        {
            config = CookConfigData.GetCookConfig(4100);
        }
    }
    private void CheckCookResult(CookConfig config)
    {
        int val = config.Cook_Level - skillOffset;
        int succesPro = 100;
        if (val > 0)
        {
            for (int i = 0; i < val; i++)
            {
                succesPro /= 2;
            }
            text_CookDesc.text = ItemConfigData.GetItemConfig(config.Cook_ID).Item_Name + ":成功几率" + succesPro + "%";
        }
        else
        {
            text_CookDesc.text = ItemConfigData.GetItemConfig(config.Cook_ID).Item_Name + ":成功几率" + succesPro + "%";
        }
        btn_CookStart.onClick.RemoveAllListeners();
        UnityEngine.Random.InitState(System.DateTime.Now.Second);
        if (UnityEngine.Random.Range(0, 100) < succesPro)
        {
            /*成功*/
            btn_CookStart.onClick.AddListener(() =>
            {
                Type type = Type.GetType("Item_" + config.Cook_ID.ToString());
                ((ItemBase)Activator.CreateInstance(type)).StaticAction_InitData(config.Cook_ID, out ItemData initData);
                ClickCookBtn(initData, 0, config.Cook_Time);
            });
        }
        else
        {
            /*失败*/
            btn_CookStart.onClick.AddListener(() =>
            {
                Type type = Type.GetType("Item_" + 4100.ToString());
                ((ItemBase)Activator.CreateInstance(type)).StaticAction_InitData(4100, out ItemData initData);
                ClickCookBtn(initData, 0, config.Cook_Time);
            });
        }
    }
    private void ClickCookBtn(ItemData item, short val, short max)
    {
        action_Cook(item, val, max);
        ChangeInfo();
    }
    #endregion
    public void PutInIngredient(ItemData addData)
    {
        ItemConfig itemConfig = ItemConfigData.GetItemConfig(addData.Item_ID);
        if((itemConfig.Item_Type == ItemType.Ingredient || itemConfig.Item_Type == ItemType.Food) && itemDatas_Ingredient.Count < int_IngredientCapacity)
        {
            ItemData resData = addData;
            addData.Item_Count = 1;
            resData.Item_Count -= 1;
            if (resData.Item_ID > 0 && resData.Item_Count != 0)
            {
                MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryAddItemInBag()
                {
                    item = resData,
                });
            }
            itemDatas_Ingredient.Add(addData);
            action_Cook.Invoke(new ItemData(), 0, 0);
            action_PutInIngredient.Invoke(itemDatas_Ingredient);
            ChangeInfo();
        }
        else
        {
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryAddItemInBag()
            {
                item = addData,
            });
        }
    }
    public ItemData PutOutIngredient(ItemData subData)
    {
        itemDatas_Ingredient = GameToolManager.Instance.PutOutItemList(itemDatas_Ingredient, subData);
        action_Cook.Invoke(new ItemData(), 0, 0);
        action_PutInIngredient.Invoke(itemDatas_Ingredient);
        ChangeInfo();
        return subData;
    }
    public void PutInFood(ItemData addData)
    {
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryAddItemInBag()
        {
            item = addData,
        });
    }
    public ItemData PutOutFood(ItemData subData)
    {
        itemData_Food = GameToolManager.Instance.PutOutItemSingle(itemData_Food, subData) ;
        action_PutOutFood.Invoke();
        action_Cook.Invoke(new ItemData(), 0, 0);
        ChangeInfo();
        return subData;
    }
}
