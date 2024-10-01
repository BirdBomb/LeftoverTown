using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.U2D;
using static Fusion.Allocator;
using UniRx;
using System.Text;
using UnityEngine.EventSystems;
using System;
using TMPro;
using DG.Tweening;

public class UI_BuildingBuilder : UI_Grid
{
    #region//建筑预览
    [SerializeField, Header("建筑预览")]
    private RectTransform panel_buildPanel;
    [SerializeField, Header("建筑名字")]
    private Text text_buildName;
    [SerializeField, Header("建筑图片")]
    private Image image_buildSprite;
    [SerializeField, Header("合成栏图片")]
    private List<Image> imageList_buildRawSprite = new List<Image>();
    [SerializeField, Header("合成栏数量")]
    private List<Text> textList_buildRawText = new List<Text>();
    [SerializeField, Header("准备建造按钮")]
    private Button btn_readyBuild;
    [SerializeField, Header("开始建造按钮")]
    private Button btn_startBuild;
    #endregion
    #region//建筑列表
    [SerializeField, Header("建筑列表")]
    private RectTransform panel_buildList;
    [SerializeField, Header("建筑等级")]
    private Text text_buildingLevel;
    [SerializeField, Header("建筑按钮列表")]
    private List<Button> btns_buildingNameBtn = new List<Button>();
    [SerializeField, Header("上一难度按钮")]
    private Button btn_LastLevel;
    [SerializeField, Header("下一难度按钮")]
    private Button btn_NextLevel;
    [SerializeField, Header("上一页按钮")]
    private Button btn_LastPage;
    [SerializeField, Header("下一页按钮")]
    private Button btn_NextPage;
    #endregion
    /// <summary>
    /// 建筑列表
    /// </summary>
    private List<BuildingConfig> buildingConfigs = new List<BuildingConfig>();
    /// <summary>
    /// 选中的建筑
    /// </summary>
    private BuildingConfig targetBuilding;
    private List<ItemData> itemDataList = new List<ItemData>();
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
                buildingConfigs = BuildingConfigData.buildConfigs.FindAll((x) => { return x.Building_RawLevel == value; });
                curLevel = value;
                CurPage = 0;
                UpdateBuildingListUI();
            }
        }
    }

    private TileObj bindTileObj;
    private SpriteAtlas buildingAtlas;
    private SpriteAtlas itemAtlas;
    private void Awake()
    {
        buildingAtlas = Resources.Load<SpriteAtlas>("Atlas/TileSprite");
        itemAtlas = Resources.Load<SpriteAtlas>("Atlas/ItemSprite");
    }
    private void Start()
    {
        btn_startBuild.onClick.AddListener(ClickStartBuildBtn);
        btn_LastLevel.onClick.AddListener(ClickLastLevelBtn);
        btn_NextLevel.onClick.AddListener(ClickNextLevelBtn);
        btn_LastPage.onClick.AddListener(ClickLastPageBtn);
        btn_NextPage.onClick.AddListener(ClickNextPageBtn);
        buildingConfigs = BuildingConfigData.buildConfigs.FindAll((x) => { return x.Building_RawLevel == 0; });
        UpdateBuildingListUI();
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
        if (CurPage * 10 < buildingConfigs.Count)
        {
            CurPage++;
        }
        else
        {
            CurPage = 0;
        }
        UpdateBuildingListUI();
    }
    /// <summary>
    /// 下一页按钮点击
    /// </summary>
    private void ClickLastPageBtn()
    {
        if (CurPage > 0)
        {
            CurPage--;
            UpdateBuildingListUI();
        }
    }
    /// <summary>
    /// 预览建筑按钮点击
    /// </summary>
    /// <param name="buildingConfig"></param>
    private void ClickBuildingNameBtn(BuildingConfig buildingConfig)
    {
        if (targetBuilding.Building_ID == 0)
        {
            DrawBuildingPanel(buildingConfig);
            DrawBuildingRaw(buildingConfig);
            btn_readyBuild.gameObject.SetActive(true);
            btn_readyBuild.onClick.RemoveAllListeners();
            btn_readyBuild.onClick.AddListener(() => { ClickReadyBuildBtn(buildingConfig); });
        }
    }
    /// <summary>
    /// 准备建造按钮点击
    /// </summary>
    /// <param name="buildingConfig"></param>
    private void ClickReadyBuildBtn(BuildingConfig buildingConfig)
    {
        targetBuilding = buildingConfig;
        ChangeInfoToTile();
    }
    /// <summary>
    /// 开始建造按钮
    /// </summary>
    private void ClickStartBuildBtn()
    {
        MessageBroker.Default.Publish(new MapEvent.MapEvent_LocalTile_ChangeBuilding()
        {
            buildingID = targetBuilding.Building_ID,
            buildingPos = bindTileObj.bindTile._posInCell
        });
    }

    #endregion
    #region//UI绘制
    /// <summary>
    /// 重置建筑UI
    /// </summary>
    private void ResetBuildUI()
    {
        btn_readyBuild.gameObject.SetActive(false);
        btn_startBuild.gameObject.SetActive(false);
        text_buildName.text = "";
        image_buildSprite.gameObject.SetActive(false);
        for (int i = 0; i < imageList_buildRawSprite.Count; i++)
        {
            imageList_buildRawSprite[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < textList_buildRawText.Count; i++)
        {
            textList_buildRawText[i].text = "";
        }
    }
    /// <summary>
    /// 更新建筑列表
    /// </summary>
    private void UpdateBuildingListUI()
    {
        for (int i = 0; i < btns_buildingNameBtn.Count; i++)
        {
            btns_buildingNameBtn[i].onClick.RemoveAllListeners();
            if (i + CurPage * 10 < buildingConfigs.Count)
            {
                BuildingConfig buildingConfig = buildingConfigs[i + CurPage * 10];
                btns_buildingNameBtn[i].gameObject.SetActive(true);
                btns_buildingNameBtn[i].onClick.AddListener(() => { ClickBuildingNameBtn(buildingConfig); });
                btns_buildingNameBtn[i].transform.Find("Text").GetComponent<TextMeshProUGUI>().text = buildingConfig.Building_Name;
            }
            else
            {
                btns_buildingNameBtn[i].gameObject.SetActive(false);
                btns_buildingNameBtn[i].transform.Find("Text").GetComponent<TextMeshProUGUI>().text = "";
            }
        }
        text_buildingLevel.text = "难度等级" + CurLevel;
    }
    private void HideBuildingListUI()
    {
        panel_buildList.DOLocalMoveX(0, 0.2f);
        panel_buildPanel.DOLocalMoveX(0, 0.2f);

        btn_NextLevel.gameObject.SetActive(false);
        btn_LastLevel.gameObject.SetActive(false);
        btn_NextPage.gameObject.SetActive(false);
        btn_LastPage.gameObject.SetActive(false);
    }
    /// <summary>
    /// 绘制建筑预览界面
    /// </summary>
    private void DrawBuildingPanel(BuildingConfig config)
    {
        if (config.Building_ID > 0)
        {
            image_buildSprite.gameObject.SetActive(true);
            image_buildSprite.sprite = buildingAtlas.GetSprite(config.Building_SpriteName);
            text_buildName.text = config.Building_Name;
        }
        else
        {
            image_buildSprite.gameObject.SetActive(false);
            text_buildName.text = "";
        }
    }
    /// <summary>
    /// 绘制建筑材料界面
    /// </summary>
    private void DrawBuildingRaw(BuildingConfig config)
    {
        for (int i = 0; i < imageList_buildRawSprite.Count; i++)
        {
            imageList_buildRawSprite[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < textList_buildRawText.Count; i++)
        {
            textList_buildRawText[i].text = "";
        }

        if (config.Building_ID == 0)
        {
            for (int i = 0; i < config.Building_Raw.Count; i++)
            {
                imageList_buildRawSprite[i].gameObject.SetActive(true);
                imageList_buildRawSprite[i].sprite = itemAtlas.GetSprite("Item_" + config.Building_Raw[i].ID.ToString());
                textList_buildRawText[i].text = "0/" + config.Building_Raw[i].Count.ToString();
            }
        }
        else
        {
            for (int i = 0; i < config.Building_Raw.Count; i++)
            {
                imageList_buildRawSprite[i].gameObject.SetActive(true);
                imageList_buildRawSprite[i].sprite = itemAtlas.GetSprite("Item_" + config.Building_Raw[i].ID.ToString());

                int tempIndex = itemDataList.FindIndex((x) => { return x.Item_ID == config.Building_Raw[i].ID; });

                if (tempIndex >= 0)
                {
                    textList_buildRawText[i].text = itemDataList[tempIndex].Item_Count.ToString() + "/" + config.Building_Raw[i].Count.ToString();
                }
                else
                {
                    textList_buildRawText[i].text = "0/" + config.Building_Raw[i].Count.ToString();
                }
            }

        }
    }

    #endregion
    #region//拿出放入
    public override void PutOut(ItemData before, out ItemData after)
    {
        itemDataList.Remove(before);
        after = before;
        ChangeInfoToTile();
    }
    public override void PutIn(ItemData before, out ItemData after)
    {
        if (targetBuilding.Building_ID > 0)
        {
            ItemData resData = before;

            for (int i = 0; i < targetBuilding.Building_Raw.Count; i++)
            {
                if (targetBuilding.Building_Raw[i].ID == before.Item_ID)
                {
                    /*需要这个作为材料*/
                    int index = itemDataList.FindIndex((x) => { return x.Item_ID == before.Item_ID; });
                    if (index >= 0)
                    {
                        /*已经有这个材料*/
                        Type type = Type.GetType("Item_" + before.Item_ID.ToString());
                        ((ItemBase)Activator.CreateInstance(type)).StaticAction_PileUp(itemDataList[index], resData, targetBuilding.Building_Raw[i].Count, out ItemData newData, out resData);
                        itemDataList[index] = newData;
                    }
                    else
                    {
                        /*还没有这个材料*/
                        ItemData itemData = before;
                        itemData.Item_Count = 0;
                        Type type = Type.GetType("Item_" + before.Item_ID.ToString());
                        ((ItemBase)Activator.CreateInstance(type)).StaticAction_PileUp(itemData, resData, targetBuilding.Building_Raw[i].Count, out ItemData newData, out resData);
                        itemDataList.Add(newData);
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
        itemDataList.Clear();
        string[] strings = info.Split("/*I*/");
        for (int i = 0; i < strings.Length; i++)
        {
            if (i == 0)
            {
                /*第一位是待建建筑id*/
                if (strings[i] != "")
                {
                    targetBuilding = BuildingConfigData.GetItemConfig(int.Parse(strings[i]));
                }
            }
            else if (strings[i] != "")
            {
                /*后位位是已经添加的材料*/
                ItemData data = JsonUtility.FromJson<ItemData>(strings[i]);
                itemDataList.Add(data);
            }
        }
        ResetBuildUI();
        if (targetBuilding.Building_ID != 0)
        {
            DrawBuildingPanel(targetBuilding);
            DrawBuildingRaw(targetBuilding);
            HideBuildingListUI();
            CheckRawList();
        }
    }
    /// <summary>
    /// 更改信息
    /// </summary>
    public void ChangeInfoToTile()
    {
        StringBuilder builder = new StringBuilder();
        builder.Append(targetBuilding.Building_ID.ToString());
        for (int i = 0; i < itemDataList.Count; i++)
        {
            builder.Append("/*I*/" + JsonUtility.ToJson(itemDataList[i]));
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
        if (targetBuilding.Building_ID != 0)
        {
            for (int i = 0; i < targetBuilding.Building_Raw.Count; i++)
            {
                int index = itemDataList.FindIndex((x) => { return x.Item_ID == targetBuilding.Building_Raw[i].ID; });
                if (index >= 0)
                {
                    if (itemDataList[index].Item_Count < targetBuilding.Building_Raw[i].Count)
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
            btn_startBuild.gameObject.SetActive(true);
        }
    }
    #endregion
}
