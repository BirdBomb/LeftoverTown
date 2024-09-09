using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.U2D;
using UnityEngine.UI;
using static Fusion.Allocator;

public class UI_CreateItem : UI_Grid
{
    #region//合成预览
    [SerializeField, Header("合成预览")]
    private RectTransform panel_createPanel;
    [SerializeField, Header("物体名字")]
    private Text text_itemName;
    [SerializeField, Header("物体描述")]
    private TextMeshProUGUI text_itemDesc;
    [SerializeField, Header("物体图片")]
    private Image image_itemSprite;
    [SerializeField, Header("合成栏图片")]
    private List<Image> imageList_itemRawSprite = new List<Image>();
    [SerializeField, Header("合成栏数量")]
    private List<Text> textList_itemRawText = new List<Text>();
    [SerializeField, Header("制造按钮")]
    private Button btn_Create;
    [SerializeField, Header("制造进度条")]
    private Image bar_Create;
    private SpriteAtlas itemAtlas;
    #endregion
    #region//合成列表
    [SerializeField, Header("合成列表")]
    private RectTransform panel_createList;
    [SerializeField, Header("建筑等级")]
    private Text text_buildingLevel;
    [SerializeField, Header("建筑按钮列表")]
    private List<Button> btns_itemNameBtn = new List<Button>();
    [SerializeField, Header("上一难度按钮")]
    private Button btn_LastLevel;
    [SerializeField, Header("下一难度按钮")]
    private Button btn_NextLevel;
    [SerializeField, Header("上一页按钮")]
    private Button btn_LastPage;
    [SerializeField, Header("下一页按钮")]
    private Button btn_NextPage;
    /// <summary>
    /// 物品列表
    /// </summary>
    private List<CreateConfig> createItemConfigs = new List<CreateConfig>();
    /// <summary>
    /// 选中的物品
    /// </summary>
    private CreateConfig targetCreateItem;
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
                createItemConfigs = CreateConfigData.createConfigs.FindAll((x) => { return x.Create_Level == value; });
                curLevel = value;
                CurPage = 0;
                UpdateCreateListUI();
            }
        }
    }

    #endregion
    #region//合成池
    [SerializeField, Header("材料格子")]
    private List<UI_GridCell> cell_RawList = new List<UI_GridCell>();
    [SerializeField, Header("成品格子")]
    private UI_GridCell cell_Item;
    private List<ItemData> rawDataList = new List<ItemData>();
    private ItemData itemData;
    #endregion

    private TileObj bindTileObj;
    private void Awake()
    {
        itemAtlas = Resources.Load<SpriteAtlas>("Atlas/ItemSprite");
    }
    private void Start()
    {
        btn_Create.onClick.AddListener(ClickCreateBtn);
        btn_LastLevel.onClick.AddListener(ClickLastLevelBtn);
        btn_NextLevel.onClick.AddListener(ClickNextLevelBtn);
        btn_LastPage.onClick.AddListener(ClickLastPageBtn);
        btn_NextPage.onClick.AddListener(ClickNextPageBtn);
        createItemConfigs = CreateConfigData.createConfigs.FindAll((x) => { return x.Create_Level == 0; });
        UpdateCreateListUI();
    }
    public override void Open(TileObj tileObj)
    {
        bindTileObj = tileObj;
        base.Open(tileObj);
    }
    #region//UI操作
    public void ClickCellLeft(UI_GridCell gridCell)
    {

    }
    public void ClickCellRight(UI_GridCell gridCell)
    {

    }
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
        if (CurPage * btns_itemNameBtn.Count < createItemConfigs.Count)
        {
            CurPage++;
        }
        else
        {
            CurPage = 0;
        }
        UpdateCreateListUI();
    }
    /// <summary>
    /// 下一页按钮点击
    /// </summary>
    private void ClickLastPageBtn()
    {
        if (CurPage > 0)
        {
            CurPage--;
        }
        UpdateCreateListUI();
    }
    /// <summary>
    /// 预览按钮点击
    /// </summary>
    /// <param name="buildingConfig"></param>
    private void ClickItemShowBtn(CreateConfig createConfig)
    {
        targetCreateItem = createConfig;
        UpdateCreatePanel(targetCreateItem);
    }
    /// <summary>
    /// 制造按钮点击
    /// </summary>
    private void ClickCreateBtn()
    {
        if(itemData.Item_ID == 0)
        {
            bar_Create.DOKill();
            bar_Create.transform.DOScaleX(1, 1).OnComplete(() =>
            {
                CreateItem(targetCreateItem);
            });
        }
    }
    #endregion
    #region//UI管理
    /// <summary>
    /// 重置制造UI
    /// </summary>
    private void ResetCreateItemUI()
    {
        text_itemName.text = "";
        text_itemDesc.text = "";
        image_itemSprite.gameObject.SetActive(false);
        bar_Create.transform.localScale = new Vector3(0, 1, 1);
        for (int i = 0; i < imageList_itemRawSprite.Count; i++)
        {
            imageList_itemRawSprite[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < textList_itemRawText.Count; i++)
        {
            textList_itemRawText[i].text = "";
        }
    }
    /// <summary>
    /// 更新制造列表
    /// </summary>
    private void UpdateCreateListUI()
    {
        for (int i = 0; i < btns_itemNameBtn.Count; i++)
        {
            btns_itemNameBtn[i].onClick.RemoveAllListeners();
            if (i + CurPage * btns_itemNameBtn.Count < createItemConfigs.Count)
            {
                CreateConfig createConfig = createItemConfigs[i + CurPage * btns_itemNameBtn.Count];
                btns_itemNameBtn[i].gameObject.SetActive(true);
                btns_itemNameBtn[i].onClick.AddListener(() => { ClickItemShowBtn(createConfig); });
                btns_itemNameBtn[i].transform.Find("Text").GetComponent<TextMeshProUGUI>().text = ItemConfigData.GetItemConfig(createConfig.Create_ID).Item_Name;
            }
            else
            {
                btns_itemNameBtn[i].gameObject.SetActive(false);
                btns_itemNameBtn[i].transform.Find("Text").GetComponent<TextMeshProUGUI>().text = "";
            }
        }
        text_buildingLevel.text = "难度等级" + CurLevel;
    }
    /// <summary>
    /// 更新制造预览
    /// </summary>
    private void UpdateCreatePanel(CreateConfig config)
    {
        ResetCreateItemUI();
        DrawCreateRaw(config);
        DrawCreatePanel(config);
        CheckRawList();
    }
    /// <summary>
    /// 绘制制造预览
    /// </summary>
    /// <param name="config"></param>
    private void DrawCreatePanel(CreateConfig config)
    {
        if (config.Create_ID > 0)
        {
            image_itemSprite.gameObject.SetActive(true);
            image_itemSprite.sprite = itemAtlas.GetSprite("Item_" + config.Create_ID);
            text_itemName.text = ItemConfigData.GetItemConfig(config.Create_ID).Item_Name;
            text_itemDesc.text = ItemConfigData.GetItemConfig(config.Create_ID).Item_Desc;
        }
        else
        {
            image_itemSprite.gameObject.SetActive(false);
            text_itemName.text = "";
            text_itemDesc.text = "";
        }

    }
    /// <summary>
    /// 绘制制造材料界面
    /// </summary>
    private void DrawCreateRaw(CreateConfig config)
    {
        for (int i = 0; i < imageList_itemRawSprite.Count; i++)
        {
            imageList_itemRawSprite[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < textList_itemRawText.Count; i++)
        {
            textList_itemRawText[i].text = "";
        }

        if (config.Create_ID == 0)
        {
            for (int i = 0; i < config.Create_Raw.Count; i++)
            {
                imageList_itemRawSprite[i].gameObject.SetActive(true);
                imageList_itemRawSprite[i].sprite = itemAtlas.GetSprite("Item_" + config.Create_Raw[i].ID.ToString());
                textList_itemRawText[i].text = "0/" + config.Create_Raw[i].Count.ToString();
            }
        }
        else
        {
            for (int i = 0; i < config.Create_Raw.Count; i++)
            {
                imageList_itemRawSprite[i].gameObject.SetActive(true);
                imageList_itemRawSprite[i].sprite = itemAtlas.GetSprite("Item_" + config.Create_Raw[i].ID.ToString());

                int tempIndex = rawDataList.FindIndex((x) => { return x.Item_ID == config.Create_Raw[i].ID; });

                if (tempIndex >= 0)
                {
                    textList_itemRawText[i].text = rawDataList[tempIndex].Item_Count.ToString() + "/" + config.Create_Raw[i].Count.ToString();
                }
                else
                {
                    textList_itemRawText[i].text = "0/" + config.Create_Raw[i].Count.ToString();
                }
            }
        }
    }
    /// <summary>
    /// 绘制工具台池子
    /// </summary>
    private void DrawCreatePool()
    {
        ResetCell(cell_Item);
        DrawCell(itemData, cell_Item);
        for (int i = 0; i < cell_RawList.Count; i++)
        {
            if (i < rawDataList.Count)
            {
                DrawCell(rawDataList[i], cell_RawList[i]);
            }
            else
            {
                ResetCell(cell_RawList[i]);
            }
        }
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
        cell.BindClickAction(ClickCellLeft, ClickCellRight);
        cell.BindDragAction(CellDragBegin, CellDragIn, CellDragEnd);
    }

    #endregion
    #region//拿出放入
    public override void PutOut(ItemData before, out ItemData after)
    {
        if (itemData.Equals(before))
        {
            itemData = new ItemData();
        }
        else
        {
            rawDataList.Remove(before);
        }
        after = before;
        ChangeInfoToTile();
    }
    public override void PutIn(ItemData before, out ItemData after)
    {
        ItemConfig config = ItemConfigData.GetItemConfig(before.Item_ID);
        ItemData resData = before;
        if (config.Item_Size == ItemSize.AsGroup)
        {
            for (int i = 0; i < cell_RawList.Count; i++)
            {
                if (rawDataList.Count > i)
                {
                    if (rawDataList[i].Item_ID == resData.Item_ID)
                    {
                        Type type = Type.GetType("Item_" + rawDataList[i].Item_ID.ToString());
                        ((ItemBase)Activator.CreateInstance(type)).StaticAction_PileUp(rawDataList[i], resData, config.Item_MaxCount, out ItemData newData, out resData);
                        rawDataList[i] = newData;
                    }
                }
                else
                {
                    if (resData.Item_Count > 0)
                    {
                        ItemData emptyData = resData;
                        emptyData.Item_Count = 0;
                        Type type = Type.GetType("Item_" + resData.Item_ID.ToString());
                        ((ItemBase)Activator.CreateInstance(type)).StaticAction_PileUp(emptyData, resData, config.Item_MaxCount, out ItemData newData, out resData);
                        rawDataList.Add(newData);
                    }
                }
            }
        }
        else
        {
            if (rawDataList.Count < cell_RawList.Count)
            {
                ItemData emptyData = resData;
                emptyData.Item_Count = 0;
                Type type = Type.GetType("Item_" + resData.Item_ID.ToString());
                ((ItemBase)Activator.CreateInstance(type)).StaticAction_PileUp(emptyData, resData, config.Item_MaxCount, out ItemData newData, out resData);
                rawDataList.Add(newData);
            }
        }
        after = resData;
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
        rawDataList.Clear();
        itemData = new ItemData();
        string[] strings = info.Split("/*I*/");
        for (int i = 0; i < strings.Length; i++)
        {
            if (i == 0)
            {
                /*第一位是待建建筑id*/
                if (strings[i] != "")
                {
                    itemData = JsonUtility.FromJson<ItemData>(strings[i]);
                }
            }
            else if (strings[i] != "")
            {
                ItemData data = JsonUtility.FromJson<ItemData>(strings[i]);
                rawDataList.Add(data);
            }
        }
        if (targetCreateItem.Create_ID != 0)
        {
            UpdateCreatePanel(targetCreateItem);
        }
        DrawCreatePool();
    }
    /// <summary>
    /// 更改信息
    /// </summary>
    public void ChangeInfoToTile()
    {
        StringBuilder builder = new StringBuilder();
        builder.Append(JsonUtility.ToJson(itemData));
        for (int i = 0; i < rawDataList.Count; i++)
        {
            builder.Append("/*I*/" + JsonUtility.ToJson(rawDataList[i]));
        }
        bindTileObj.TryToChangeInfo(builder.ToString());
    }
    #endregion
    #region//制造UI
    /// <summary>
    /// 检查itemData材料
    /// </summary>
    private void CheckRawList()
    {
        btn_Create.gameObject.SetActive(false);
        if (targetCreateItem.Create_ID != 0 && itemData.Item_ID == 0)
        {
            for (int i = 0; i < targetCreateItem.Create_Raw.Count; i++)
            {
                int index = rawDataList.FindIndex((x) => { return x.Item_ID == targetCreateItem.Create_Raw[i].ID; });
                if (index >= 0)
                {
                    if (rawDataList[index].Item_Count < targetCreateItem.Create_Raw[i].Count)
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
            btn_Create.gameObject.SetActive(true);
        }
    }
    /// <summary>
    /// 创建物体
    /// </summary>
    private void CreateItem(CreateConfig createConfig)
    {
        for (int i = 0; i < createConfig.Create_Raw.Count; i++)
        {
            CreateRaw raw = createConfig.Create_Raw[i];
            for(int j = 0; j < rawDataList.Count; j++)
            {
                if (rawDataList[j].Item_ID == raw.ID)
                {
                    ItemData temp = rawDataList[j];
                    if (rawDataList[j].Item_Count > raw.Count)
                    {
                        temp.Item_Count -= raw.Count;
                        raw.Count = 0;
                    }
                    else
                    {
                        raw.Count -= temp.Item_Count;
                        temp.Item_Count = 0;
                    }
                    if (temp.Item_Count == 0)
                    {
                        rawDataList[j] = new ItemData();
                    }
                    else
                    {
                        rawDataList[j] = temp;
                    }
                }
            }
        }
        Type type = Type.GetType("Item_" + createConfig.Create_ID.ToString());
        ItemData tempData = new ItemData
            (createConfig.Create_ID,
             MapManager.Instance.mapSeed + (int)(System.DateTime.Now.Ticks * 1000),
             1,
             0,
             0,
             0,
             new ContentData());
        ((ItemBase)Activator.CreateInstance(type)).StaticAction_InitData(tempData, out ItemData initData);
        itemData = initData;
        ChangeInfoToTile();
    }
    #endregion

}
