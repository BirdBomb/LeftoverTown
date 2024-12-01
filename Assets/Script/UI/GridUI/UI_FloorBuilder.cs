using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.U2D;
using UniRx;
using System.Text;
using UnityEngine.EventSystems;
using System;
using DG.Tweening;
using TMPro;

public class UI_FloorBuilder : UI_Grid
{
    [SerializeField, Header("地块清单")]
    private RectTransform panel_FloorList;
    [SerializeField, Header("地块清单_地块按钮列表")]
    private List<Button> btns_FloorNameBtn = new List<Button>();
    [SerializeField, Header("地块清单_地块等级")]
    private TextMeshProUGUI text_FloorLevel;
    [SerializeField, Header("地块清单_上一难度按钮")]
    private Button btn_LastFloorLevel;
    [SerializeField, Header("地块清单_下一难度按钮")]
    private Button btn_NextFloorLevel;
    [SerializeField, Header("地块清单_上一页按钮")]
    private Button btn_LastFloorPage;
    [SerializeField, Header("地块清单_下一页按钮")]
    private Button btn_NextFloorPage;
    [SerializeField, Header("地块预览")]
    private RectTransform panel_FloorShow;
    [SerializeField, Header("地块预览_地块名字")]
    private TextMeshProUGUI text_TargetFloorName;
    [SerializeField, Header("地块预览_地块图片")]
    private Image image_TargetFloorImage;
    [SerializeField, Header("地块预览_目标建筑原料")]
    private List<UI_ItemCell> itemCells_TargetFloorRawList = new List<UI_ItemCell>();
    [SerializeField, Header("地块预览_准备建造按钮")]
    private Button btn_TargetFloorReady;
    [SerializeField, Header("地块预览_开始建造按钮")]
    private Button btn_TargetFloorStart;
    [Header("建筑图标图集")]
    public SpriteAtlas spriteAtlas_FloorIcon;
    [Header("物品图标图集")]
    public SpriteAtlas spriteAtlas_ItemIcon;
    [Header("物品图标图集")]
    public SpriteAtlas spriteAtlas_ItemBG;
    private TileObj bindTileObj;
    private List<FloorConfig> floorConfigs_TempList = new List<FloorConfig>();
    private FloorConfig floorConfig__TargetFloor;
    private List<ItemData> itemDatas_Pool = new List<ItemData>();
    private int curPage;
    private int CurPage
    {
        get { return curPage; }
        set
        {
            curPage = value;
        }
    }

    private int curLevel;
    private int CurLevel
    {
        get { return curLevel; }
        set
        {
            if (curLevel != value)
            {
                floorConfigs_TempList = FloorConfigData.floorConfigs.FindAll((x) => { return x.Floor_RawLevel == value; });
                curLevel = value;
                CurPage = 0;
                UpdateFloorListUI();
            }
        }
    }

    private void Start()
    {
        btn_TargetFloorStart.onClick.AddListener(ClickStartBuildBtn);
        btn_LastFloorLevel.onClick.AddListener(ClickLastLevelBtn);
        btn_NextFloorLevel.onClick.AddListener(ClickNextLevelBtn);
        btn_LastFloorPage.onClick.AddListener(ClickLastPageBtn);
        btn_NextFloorPage.onClick.AddListener(ClickNextPageBtn);
        floorConfigs_TempList = FloorConfigData.floorConfigs.FindAll((x) => { return x.Floor_RawLevel == 0; });
        UpdateFloorListUI();
    }
    #region//打开关闭
    public override void Open(TileObj tileObj)
    {
        bindTileObj = tileObj;
        base.Open(tileObj);
    }
    #endregion
    #region//UI交互
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
    /// <summary>
    /// 上一难度按钮点击
    /// </summary>
    private void ClickNextLevelBtn()
    {
        if (curLevel < 10)
        {
            CurLevel++;
        }
        else
        {
            CurLevel = 0;
        }
    }
    /// <summary>
    /// 下一难度按钮点击
    /// </summary>
    private void ClickLastLevelBtn()
    {
        if (curLevel > 0)
        {
            CurLevel--;
        }
        else
        {
            CurLevel = 10;
        }
    }
    /// <summary>
    /// 上一页按钮点击
    /// </summary>
    private void ClickNextPageBtn()
    {
        if (CurPage * 10 < floorConfigs_TempList.Count)
        {
            CurPage++;
        }
        else
        {
            CurPage = 0;
        }
        UpdateFloorListUI();
    }
    /// <summary>
    /// 下一页按钮点击
    /// </summary>
    private void ClickLastPageBtn()
    {
        if (CurPage > 0)
        {
            CurPage--;
            UpdateFloorListUI();
        }
    }
    /// <summary>
    /// 预览建筑按钮点击
    /// </summary>
    /// <param name="buildingConfig"></param>
    private void ClickBuildingNameBtn(FloorConfig floorConfig)
    {
        if (floorConfig__TargetFloor.Floor_ID == 0)
        {
            DrawFloorPanel(floorConfig);
            DrawFloorRaw(floorConfig);
            btn_TargetFloorReady.gameObject.SetActive(true);
            btn_TargetFloorReady.onClick.RemoveAllListeners();
            btn_TargetFloorReady.onClick.AddListener(() => { ClickReadyBuildBtn(floorConfig); });
        }
    }
    /// <summary>
    /// 准备建造按钮点击
    /// </summary>
    /// <param name="buildingConfig"></param>
    private void ClickReadyBuildBtn(FloorConfig floorConfig)
    {
        floorConfig__TargetFloor = floorConfig;
        ChangeInfoToTile();
    }
    /// <summary>
    /// 开始建造按钮
    /// </summary>
    private void ClickStartBuildBtn()
    {
        MessageBroker.Default.Publish(new MapEvent.MapEvent_LocalTile_ChangeBuilding()
        {
            buildingID = 0,
            buildingPos = bindTileObj.bindTile._posInCell
        });
        MessageBroker.Default.Publish(new MapEvent.MapEvent_LocalTile_ChangeFloor()
        {
            floorID = floorConfig__TargetFloor.Floor_ID,
            floorPos = bindTileObj.bindTile._posInCell
        });
    }

    #endregion
    #region//UI绘制
    /// <summary>
    /// 重置地板UI
    /// </summary>
    private void ResetFloorUI()
    {
        btn_TargetFloorReady.gameObject.SetActive(false);
        btn_TargetFloorStart.gameObject.SetActive(false);
        text_TargetFloorName.text = "";
        image_TargetFloorImage.gameObject.SetActive(false);
        for (int i = 0; i < itemCells_TargetFloorRawList.Count; i++)
        {
            itemCells_TargetFloorRawList[i].Clean();
        }
    }
    /// <summary>
    /// 更新地板列表
    /// </summary>
    private void UpdateFloorListUI()
    {
        for (int i = 0; i < btns_FloorNameBtn.Count; i++)
        {
            btns_FloorNameBtn[i].onClick.RemoveAllListeners();
            if (i + CurPage * 10 < floorConfigs_TempList.Count)
            {
                FloorConfig floorConfig = floorConfigs_TempList[i + CurPage * 10];
                btns_FloorNameBtn[i].gameObject.SetActive(true);
                btns_FloorNameBtn[i].onClick.AddListener(() => { ClickBuildingNameBtn(floorConfig); });
                btns_FloorNameBtn[i].transform.Find("Text").GetComponent<TextMeshProUGUI>().text = floorConfig.Floor_Name;
            }
            else
            {
                btns_FloorNameBtn[i].gameObject.SetActive(false);
                btns_FloorNameBtn[i].transform.Find("Text").GetComponent<TextMeshProUGUI>().text = "";
            }
        }
        text_FloorLevel.text = "难度等级" + CurLevel;
    }
    private void HideFloorListUI()
    {
        panel_FloorList.DOLocalMoveX(0, 0.2f);
        panel_FloorShow.DOLocalMoveX(0, 0.2f);

        btn_NextFloorLevel.gameObject.SetActive(false);
        btn_LastFloorLevel.gameObject.SetActive(false);
        btn_NextFloorPage.gameObject.SetActive(false);
        btn_LastFloorPage.gameObject.SetActive(false);
    }
    /// <summary>
    /// 绘制地板预览界面
    /// </summary>
    private void DrawFloorPanel(FloorConfig config)
    {
        if (config.Floor_ID > 0)
        {
            image_TargetFloorImage.gameObject.SetActive(true);
            image_TargetFloorImage.sprite = spriteAtlas_FloorIcon.GetSprite(config.Floor_SpriteName);
            text_TargetFloorName.text = config.Floor_Name;
        }
        else
        {
            image_TargetFloorImage.gameObject.SetActive(false);
            text_TargetFloorName.text = "";
        }
    }
    /// <summary>
    /// 绘制地板材料界面
    /// </summary>
    private void DrawFloorRaw(FloorConfig config)
    {
        if (config.Floor_ID == 0)
        {
            for (int i = 0; i < itemCells_TargetFloorRawList.Count; i++)
            {
                itemCells_TargetFloorRawList[i].Clean();
            }
        }
        else
        {
            for (int i = 0; i < config.Floor_Raw.Count; i++)
            {
                ItemConfig itemConfig = ItemConfigData.GetItemConfig(config.Floor_Raw[i].ID);
                string info = "";
                int tempIndex = itemDatas_Pool.FindIndex((x) => { return x.Item_ID == config.Floor_Raw[i].ID; });

                if (tempIndex >= 0)
                {
                    info = itemDatas_Pool[tempIndex].Item_Count.ToString() + "/" + config.Floor_Raw[i].Count.ToString();
                }
                else
                {
                    info = "0/" + config.Floor_Raw[i].Count.ToString();
                }

                itemCells_TargetFloorRawList[i].Draw
                    (spriteAtlas_ItemIcon.GetSprite("Item_" + itemConfig.Item_ID.ToString()),
                    spriteAtlas_ItemBG.GetSprite("ItemBG_" + itemConfig.ItemRarity),
                    itemConfig.ItemRarity,
                    info,
                    itemConfig.Item_Name,
                    itemConfig.Item_Desc);
            }
        }
    }

    #endregion
    #region//拿出放入
    public override void PutOut(ItemData before, out ItemData after)
    {
        itemDatas_Pool.Remove(before);
        after = before;
        ChangeInfoToTile();
    }
    public override void PutIn(ItemData before, out ItemData after)
    {
        if (floorConfig__TargetFloor.Floor_ID > 0)
        {
            ItemData resData = before;

            for (int i = 0; i < floorConfig__TargetFloor.Floor_Raw.Count; i++)
            {
                if (floorConfig__TargetFloor.Floor_Raw[i].ID == before.Item_ID)
                {
                    /*需要这个作为材料*/
                    int index = itemDatas_Pool.FindIndex((x) => { return x.Item_ID == before.Item_ID; });
                    if (index >= 0)
                    {
                        /*已经有这个材料*/
                        Type type = Type.GetType("Item_" + before.Item_ID.ToString());
                        ((ItemBase)Activator.CreateInstance(type)).StaticAction_PileUp(itemDatas_Pool[index], resData, floorConfig__TargetFloor.Floor_Raw[i].Count, out ItemData newData, out resData);
                        itemDatas_Pool[index] = newData;
                    }
                    else
                    {
                        /*还没有这个材料*/
                        ItemData itemData = before;
                        itemData.Item_Count = 0;
                        Type type = Type.GetType("Item_" + before.Item_ID.ToString());
                        ((ItemBase)Activator.CreateInstance(type)).StaticAction_PileUp(itemData, resData, floorConfig__TargetFloor.Floor_Raw[i].Count, out ItemData newData, out resData);
                        itemDatas_Pool.Add(newData);
                    }
                }
            }
            after = resData;
        }
        else
        {
            base.PutIn(before, out after);
        }
        ChangeInfoToTile();
    }
    #endregion
    #region//信息上传与更新
    /// <summary>
    /// 更新信息
    /// </summary>
    /// <param name="info"></param>
    public void UpdateInfoFromTile(string info)
    {
        itemDatas_Pool.Clear();
        string[] strings = info.Split("/*I*/");
        for (int i = 0; i < strings.Length; i++)
        {
            if (i == 0)
            {
                /*第一位是待建建筑id*/
                if (strings[i] != "")
                {
                    floorConfig__TargetFloor = FloorConfigData.GetItemConfig(int.Parse(strings[i]));
                }
            }
            else if (strings[i] != "")
            {
                /*后位位是已经添加的材料*/
                ItemData data = JsonUtility.FromJson<ItemData>(strings[i]);
                itemDatas_Pool.Add(data);
            }
        }
        ResetFloorUI();
        if (floorConfig__TargetFloor.Floor_ID != 0)
        {
            DrawFloorPanel(floorConfig__TargetFloor);
            DrawFloorRaw(floorConfig__TargetFloor);
            HideFloorListUI();
            CheckRawList();
        }
    }
    /// <summary>
    /// 更改信息
    /// </summary>
    public void ChangeInfoToTile()
    {
        StringBuilder builder = new StringBuilder();
        builder.Append(floorConfig__TargetFloor.Floor_ID.ToString());
        for (int i = 0; i < itemDatas_Pool.Count; i++)
        {
            builder.Append("/*I*/" + JsonUtility.ToJson(itemDatas_Pool[i]));
        }
        bindTileObj.TryToChangeInfo(builder.ToString());
    }
    #endregion
    #region//建造UI
    /// <summary>
    /// 检查建筑材料
    /// </summary>
    private void CheckRawList()
    {
        if (floorConfig__TargetFloor.Floor_ID != 0)
        {
            for (int i = 0; i < floorConfig__TargetFloor.Floor_Raw.Count; i++)
            {
                int index = itemDatas_Pool.FindIndex((x) => { return x.Item_ID == floorConfig__TargetFloor.Floor_Raw[i].ID; });
                if (index >= 0)
                {
                    if (itemDatas_Pool[index].Item_Count < floorConfig__TargetFloor.Floor_Raw[i].Count)
                    {
                        /*这个材料数量不足*/
                        return;
                    }
                }
                else
                {
                    /*没有这个材料*/
                    return;
                }
            }
            btn_TargetFloorStart.gameObject.SetActive(true);
        }
    }
    #endregion
}
