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
using static UnityEditor.Progress;
using static UnityEditor.Timeline.Actions.MenuPriority;

public class UI_Grid_CookPanel : UI_Grid
{
    [SerializeField, Header("原料格子")]
    private List<UI_GridCell> cell_IngredientList = new List<UI_GridCell>();
    [SerializeField, Header("成品格子")]
    private UI_GridCell cell_Food;
    [SerializeField, Header("最大容量")]
    private int max_FoodSlot;
    [SerializeField, Header("烹饪进度条")]
    private Image image_CookBar;
    [SerializeField, Header("烹饪结果描述")]
    private TextMeshProUGUI text_CookDesc;
    [SerializeField, Header("烹饪按钮")]
    private Button btn_Cook;
    [SerializeField, Header("烹饪技能描述")]
    private TextMeshProUGUI text_CookBar;
    [SerializeField, Header("烹饪技能加成")]
    private int skillOffset;
    public Action<List<ItemData>> bindAddRawAction;
    public Action bindPutOutFoodAction;
    public Action<ItemData, short, short> bindCookAction;
    private List<ItemData> cookRawDataList = new List<ItemData>();
    private ItemData cookResultData;
    private short cookTime = 0;
    private short cookMaxTime = 0;
    private TileObj bindTileObj;
    public override void Open(TileObj tileObj)
    {
        bindTileObj = tileObj;
        base.Open(tileObj);
    }
    public override void Close(TileObj tileObj)
    {
        bindTileObj = tileObj;
        base.Close(tileObj);
    }
    public void BindAction(Action<List<ItemData>> addRawAction,Action<ItemData,short,short> cookAction,Action putOutAction)
    {
        bindAddRawAction = addRawAction;
        bindCookAction = cookAction;
        bindPutOutFoodAction = putOutAction;
    }

    #region//UI操作
    public override void CellDragBegin(UI_GridCell gridCell, ItemData itemData, PointerEventData pointerEventData)
    {

    }
    public override void CellDragIn(UI_GridCell gridCell, ItemData itemData, PointerEventData pointerEventData)
    {
        RectTransformUtility.ScreenPointToWorldPointInRectangle(gridCell.image_MainIcon.rectTransform, Input.mousePosition, Camera.main, out Vector3 pos);
        gridCell.image_MainIcon.transform.position = pos;
    }
    public override void CellDragEnd(UI_GridCell gridCell, ItemData itemData, PointerEventData pointerEventData)
    {
        pointerEventData.position = Input.mousePosition;
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResults);
        if (raycastResults.Count > 0)
        {
            foreach (RaycastResult result in raycastResults)
            {
                if (result.gameObject.TryGetComponent(out UI_Grid grid))
                {
                    PutOut(itemData, out ItemData afterData);
                    grid.ListenDragOn(this, gridCell, afterData);
                    return;
                }
            }
        }
        else
        {
            PutOut(itemData, out ItemData afterData);
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryDropItem()
            {
                item = afterData
            });
        }

    }
    public override void ListenDragOn<T>(T grid, UI_GridCell cell, ItemData itemData)
    {
        PutIn(itemData, out ItemData back);
        if (back.Item_Count != 0)
        {
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryAddItemInBag()
            {
                item = back,
            });
        }
        base.ListenDragOn<T>(grid, cell, itemData);
    }
    public void ClickCookBtn(ItemData item, short val, short max)
    {
        bindCookAction(item, val, max);
        ChangeInfoToTile();
    }
    #endregion
    #region//UI管理
    /// <summary>
    /// 绘制所有格子
    /// </summary>
    private void DrawAllCell()
    {
        for(int i = 0; i< cell_IngredientList.Count; i++)
        {
            ResetCell(cell_IngredientList[i]);
            if (cookRawDataList.Count > i && cookRawDataList[i].Item_ID != 0)
            {
                DrawCell(cookRawDataList[i], cell_IngredientList[i]);
            }
        }
        ResetCell(cell_Food);
        DrawCell(cookResultData, cell_Food);
        CheckRaw();
    }

    /// <summary>
    /// 重置一个格子
    /// </summary>
    /// <param name="cell"></param>
    private void ResetCell(UI_GridCell cell)
    {
        cell.ResetGridCell();
    }
    /// <summary>
    /// 绘制一个格子
    /// </summary>
    /// <param name="cell"></param>
    /// <param name="config"></param>
    private void DrawCell(ItemData data, UI_GridCell cell)
    {
        cell.UpdateGridCell(data);
        cell.BindDragAction(CellDragBegin, CellDragIn, CellDragEnd);
    }
    private void CheckRaw()
    {
        text_CookBar.text = skillOffset.ToString();
        if (cookRawDataList.Count > 1)
        {
            CookConfig cookResult;
            int id_0 = cookRawDataList[0].Item_ID;
            int id_1 = cookRawDataList[1].Item_ID;
            if (id_0 == id_1)
            {
                FindCookResulr(id_0, out cookResult);
            }
            else
            {
                if (id_0 > id_1)
                {
                    FindCookResulr(id_1 * 10000 + id_0,out cookResult);
                }
                else
                {
                    FindCookResulr(id_0 * 10000 + id_1, out cookResult);
                }
            }
            if (cookResult.Cook_ID != 0)
            {
                CheckCookResult(cookResult);
                btn_Cook.gameObject.SetActive(true);
            }
        }
        else
        {
            btn_Cook.gameObject.SetActive(false);
            text_CookDesc.text = "";
        }
    }
    private void FindCookResulr(int raw,out CookConfig config)
    {
        config = CookConfigData.cookConfigs.Find((x) => { return x.Cook_Raw.Contains(raw); });
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
        btn_Cook.onClick.RemoveAllListeners();
        UnityEngine.Random.InitState(System.DateTime.Now.Second);
        if (UnityEngine.Random.Range(0, 100) < succesPro)
        {
            /*成功*/
            btn_Cook.onClick.AddListener(() =>
            {
                Type type = Type.GetType("Item_" + config.Cook_ID.ToString());
                ItemData itemData = new ItemData
                    (config.Cook_ID,
                     MapManager.Instance.mapSeed + (int)(System.DateTime.Now.Ticks * 1000),
                     1,
                     0,
                     0,
                     0,
                     new ContentData());
                ((ItemBase)Activator.CreateInstance(type)).StaticAction_InitData(itemData, out ItemData initData);
                ClickCookBtn(initData, 0, config.Cook_Time);
            });
        }
        else
        {
            /*失败*/
            btn_Cook.onClick.AddListener(() =>
            {
                Type type = Type.GetType("Item_" + 4000
                    .ToString());
                ItemData itemData = new ItemData
                    (4000,
                     MapManager.Instance.mapSeed + (int)(System.DateTime.Now.Ticks * 1000),
                     1,
                     0,
                     0,
                     0,
                     new ContentData());
                ((ItemBase)Activator.CreateInstance(type)).StaticAction_InitData(itemData, out ItemData initData);
                ClickCookBtn(initData, 0, config.Cook_Time);
            });
        }
    }
    #endregion
    #region//放入取出
    public override void PutIn(ItemData before, out ItemData after)
    {
        ItemData resData = before;
        ItemConfig itemConfig = ItemConfigData.GetItemConfig(before.Item_ID);
        if ((itemConfig.Item_Type == ItemType.Ingredient || itemConfig.Item_Type == ItemType.Food) && cookRawDataList.Count < max_FoodSlot)
        {
            ItemData itemData = before;
            itemData.Item_Count = 0;
            Type type = Type.GetType("Item_" + before.Item_ID.ToString());
            ((ItemBase)Activator.CreateInstance(type)).StaticAction_PileUp(itemData, resData, 1, out ItemData newData, out resData);
            after = resData;

            cookRawDataList.Add(newData);
            bindCookAction.Invoke(new ItemData(), 0, 0);
            bindAddRawAction.Invoke(cookRawDataList);
            ChangeInfoToTile();
        }
        else
        {
            base.PutIn(before, out after);
        }
    }
    public override void PutOut(ItemData before, out ItemData after)
    {
        if (before.Equals(cookResultData))
        {
            cookResultData = new ItemData();
            bindPutOutFoodAction.Invoke();
        }
        else
        {
            cookRawDataList.Remove(before);
        }
        after = before;
        bindCookAction.Invoke(new ItemData(), 0, 0);
        bindAddRawAction.Invoke(cookRawDataList);
        ChangeInfoToTile();
    }
    #endregion
    #region//上传更新
    public void UpdateInfoFromTile(List<ItemData> rawList, ItemData result, short time, short maxTime)
    {
        cookResultData = result;
        cookRawDataList.Clear();
        for (int i = 0; i < rawList.Count; i++)
        {
            if (rawList[i].Item_ID != 0)
            {
                cookRawDataList.Add(rawList[i]);
            }
        }
        if (maxTime != 0)
        {
            image_CookBar.transform.DOKill();
            image_CookBar.transform.DOScaleX(((float)time / maxTime), 0.1f);
            if (time == maxTime)
            {
                cell_Food.DOKill();
                cell_Food.transform.localScale = Vector3.one;
                cell_Food.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
            }
        }
        else
        {
            image_CookBar.transform.localScale = new Vector3(0, 1f, 1f);
        }
        DrawAllCell();
    }
    public void ChangeInfoToTile()
    {
        bindTileObj.TryToChangeInfo("");
    }
    #endregion

}
