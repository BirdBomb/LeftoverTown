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
    [SerializeField, Header("�����嵥")]
    private RectTransform panel_BuildList;
    [SerializeField, Header("�����嵥_�ɽ��콨����ť�б�")]
    private List<Button> btns_BuildNameBtn = new List<Button>();
    [SerializeField, Header("�����嵥_�����ȼ�")]
    private TextMeshProUGUI text_BuildLevel;
    [SerializeField, Header("�����嵥_��һ�ѶȰ�ť")]
    private Button btn_LastBuildLevel;
    [SerializeField, Header("�����嵥_��һ�ѶȰ�ť")]
    private Button btn_NextBuildLevel;
    [SerializeField, Header("�����嵥_��һҳ��ť")]
    private Button btn_LastBuildPage;
    [SerializeField, Header("�����嵥_��һҳ��ť")]
    private Button btn_NextBuildPage;

    [SerializeField, Header("����Ԥ��")]
    private RectTransform panel_BuildingShow;
    [SerializeField, Header("����Ԥ��_��������")]
    private TextMeshProUGUI text_TargetBuildingName;
    [SerializeField, Header("����Ԥ��_����ͼƬ")]
    private Image image_TargetBuildingImage;
    [SerializeField, Header("����Ԥ��_Ŀ�꽨��ԭ��")]
    private List<UI_ItemCell> itemCells_TargetBuildingRawList = new List<UI_ItemCell>();
    [SerializeField, Header("����Ԥ��_׼�����찴ť")]
    private Button btn_TargetBuildingReady;
    [SerializeField, Header("����Ԥ��_��ʼ���찴ť")]
    private Button btn_TargetBuildingStart;
    [Header("����ͼ��ͼ��")]
    public SpriteAtlas spriteAtlas_BuildIcon;
    [Header("��Ʒͼ��ͼ��")]
    public SpriteAtlas spriteAtlas_ItemIcon;
    [Header("��Ʒͼ��ͼ��")]
    public SpriteAtlas spriteAtlas_ItemBG;
    private TileObj bindTileObj;
    private List<BuildingConfig> buildingConfigs_TempList = new List<BuildingConfig>();
    private BuildingConfig buildingConfig_TargetBuilding;
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
                buildingConfigs_TempList = BuildingConfigData.buildConfigs.FindAll((x) => { return x.Building_RawLevel == value; });
                curLevel = value;
                CurPage = 0;
                UpdateBuildingListUI();
            }
        }
    }

    private void Start()
    {
        btn_TargetBuildingStart.onClick.AddListener(ClickStartBuildBtn);
        btn_LastBuildLevel.onClick.AddListener(ClickLastLevelBtn);
        btn_NextBuildLevel.onClick.AddListener(ClickNextLevelBtn);
        btn_LastBuildPage.onClick.AddListener(ClickLastPageBtn);
        btn_NextBuildPage.onClick.AddListener(ClickNextPageBtn);
        buildingConfigs_TempList = BuildingConfigData.buildConfigs.FindAll((x) => { return x.Building_RawLevel == 0; });
        UpdateBuildingListUI();
    }
    #region//�򿪹ر�
    public override void Open(TileObj tileObj)
    {
        bindTileObj = tileObj;
        base.Open(tileObj);
    }
    #endregion
    #region//UI����
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
    /// ��һ�ѶȰ�ť���
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
    /// ��һ�ѶȰ�ť���
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
    /// ��һҳ��ť���
    /// </summary>
    private void ClickNextPageBtn()
    {
        if (CurPage * 10 < buildingConfigs_TempList.Count)
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
    /// ��һҳ��ť���
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
    /// Ԥ��������ť���
    /// </summary>
    /// <param name="buildingConfig"></param>
    private void ClickBuildingNameBtn(BuildingConfig buildingConfig)
    {
        if (buildingConfig_TargetBuilding.Building_ID == 0)
        {
            DrawBuildingPanel(buildingConfig);
            DrawBuildingRaw(buildingConfig);
            btn_TargetBuildingReady.gameObject.SetActive(true);
            btn_TargetBuildingReady.onClick.RemoveAllListeners();
            btn_TargetBuildingReady.onClick.AddListener(() => { ClickReadyBuildBtn(buildingConfig); });
        }
    }
    /// <summary>
    /// ׼�����찴ť���
    /// </summary>
    /// <param name="buildingConfig"></param>
    private void ClickReadyBuildBtn(BuildingConfig buildingConfig)
    {
        buildingConfig_TargetBuilding = buildingConfig;
        ChangeInfoToTile();
    }
    /// <summary>
    /// ��ʼ���찴ť
    /// </summary>
    private void ClickStartBuildBtn()
    {
        MessageBroker.Default.Publish(new MapEvent.MapEvent_LocalTile_ChangeBuilding()
        {
            buildingID = buildingConfig_TargetBuilding.Building_ID,
            buildingPos = bindTileObj.bindTile._posInCell
        });
    }

    #endregion
    #region//UI����
    /// <summary>
    /// ���ý���UI
    /// </summary>
    private void ResetBuildUI()
    {
        btn_TargetBuildingReady.gameObject.SetActive(false);
        btn_TargetBuildingStart.gameObject.SetActive(false);
        text_TargetBuildingName.text = "";
        image_TargetBuildingImage.gameObject.SetActive(false);
        for (int i = 0; i < itemCells_TargetBuildingRawList.Count; i++)
        {
            itemCells_TargetBuildingRawList[i].Clean() ;
        }
    }
    /// <summary>
    /// ���½����б�
    /// </summary>
    private void UpdateBuildingListUI()
    {
        for (int i = 0; i < btns_BuildNameBtn.Count; i++)
        {
            btns_BuildNameBtn[i].onClick.RemoveAllListeners();
            if (i + CurPage * 10 < buildingConfigs_TempList.Count)
            {
                BuildingConfig buildingConfig = buildingConfigs_TempList[i + CurPage * 10];
                btns_BuildNameBtn[i].gameObject.SetActive(true);
                btns_BuildNameBtn[i].onClick.AddListener(() => { ClickBuildingNameBtn(buildingConfig); });
                btns_BuildNameBtn[i].transform.Find("Text").GetComponent<TextMeshProUGUI>().text = buildingConfig.Building_Name;
            }
            else
            {
                btns_BuildNameBtn[i].gameObject.SetActive(false);
                btns_BuildNameBtn[i].transform.Find("Text").GetComponent<TextMeshProUGUI>().text = "";
            }
        }
        text_BuildLevel.text = "Lv" + CurLevel;
    }
    private void HideBuildingListUI()
    {
        panel_BuildList.DOLocalMoveX(0, 0.2f);
        panel_BuildingShow.DOLocalMoveX(0, 0.2f);

        btn_NextBuildLevel.gameObject.SetActive(false);
        btn_LastBuildLevel.gameObject.SetActive(false);
        btn_NextBuildPage.gameObject.SetActive(false);
        btn_LastBuildPage.gameObject.SetActive(false);
    }
    /// <summary>
    /// ���ƽ���Ԥ������
    /// </summary>
    private void DrawBuildingPanel(BuildingConfig config)
    {
        if (config.Building_ID > 0)
        {
            image_TargetBuildingImage.gameObject.SetActive(true);
            image_TargetBuildingImage.sprite = spriteAtlas_BuildIcon.GetSprite(config.Building_SpriteName);
            text_TargetBuildingName.text = config.Building_Name;
        }
        else
        {
            image_TargetBuildingImage.gameObject.SetActive(false);
            text_TargetBuildingName.text = "";
        }
    }

    /// <summary>
    /// ���ƽ������Ͻ���
    /// </summary>
    private void DrawBuildingRaw(BuildingConfig config)
    {
        if (config.Building_ID == 0)
        {
            for (int i = 0; i < itemCells_TargetBuildingRawList.Count; i++)
            {
                itemCells_TargetBuildingRawList[i].Clean();
            }
        }
        else
        {
            for (int i = 0; i < config.Building_Raw.Count; i++)
            {
                ItemConfig itemConfig = ItemConfigData.GetItemConfig(config.Building_Raw[i].ID);
                string info = "";
                int tempIndex = itemDatas_Pool.FindIndex((x) => { return x.Item_ID == config.Building_Raw[i].ID; });

                if (tempIndex >= 0)
                {
                    info = itemDatas_Pool[tempIndex].Item_Count.ToString() + "/" + config.Building_Raw[i].Count.ToString();
                }
                else
                {
                    info = "0/" + config.Building_Raw[i].Count.ToString();
                }

                itemCells_TargetBuildingRawList[i].Draw
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
    #region//�ó�����
    public override void PutOut(ItemData before, out ItemData after)
    {
        itemDatas_Pool.Remove(before);
        after = before;
        ChangeInfoToTile();
    }
    public override void PutIn(ItemData before, out ItemData after)
    {
        if (buildingConfig_TargetBuilding.Building_ID > 0)
        {
            ItemData resData = before;

            for (int i = 0; i < buildingConfig_TargetBuilding.Building_Raw.Count; i++)
            {
                if (buildingConfig_TargetBuilding.Building_Raw[i].ID == before.Item_ID)
                {
                    /*��Ҫ�����Ϊ����*/
                    int index = itemDatas_Pool.FindIndex((x) => { return x.Item_ID == before.Item_ID; });
                    if (index >= 0)
                    {
                        /*�Ѿ����������*/
                        Type type = Type.GetType("Item_" + before.Item_ID.ToString());
                        ((ItemBase)Activator.CreateInstance(type)).StaticAction_PileUp(itemDatas_Pool[index], resData, buildingConfig_TargetBuilding.Building_Raw[i].Count, out ItemData newData, out resData);
                        itemDatas_Pool[index] = newData;
                    }
                    else
                    {
                        /*��û���������*/
                        ItemData itemData = before;
                        itemData.Item_Count = 0;
                        Type type = Type.GetType("Item_" + before.Item_ID.ToString());
                        ((ItemBase)Activator.CreateInstance(type)).StaticAction_PileUp(itemData, resData, buildingConfig_TargetBuilding.Building_Raw[i].Count, out ItemData newData, out resData);
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
    #region//��Ϣ�ϴ������
    /// <summary>
    /// ������Ϣ
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
                /*��һλ�Ǵ�������id*/
                if (strings[i] != "")
                {
                    buildingConfig_TargetBuilding = BuildingConfigData.GetItemConfig(int.Parse(strings[i]));
                }
            }
            else if (strings[i] != "")
            {
                /*��λλ���Ѿ���ӵĲ���*/
                ItemData data = JsonUtility.FromJson<ItemData>(strings[i]);
                itemDatas_Pool.Add(data);
            }
        }
        ResetBuildUI();
        if (buildingConfig_TargetBuilding.Building_ID != 0)
        {
            DrawBuildingPanel(buildingConfig_TargetBuilding);
            DrawBuildingRaw(buildingConfig_TargetBuilding);
            HideBuildingListUI();
            CheckRawList();
        }
    }
    /// <summary>
    /// ������Ϣ
    /// </summary>
    public void ChangeInfoToTile()
    {
        StringBuilder builder = new StringBuilder();
        builder.Append(buildingConfig_TargetBuilding.Building_ID.ToString());
        for (int i = 0; i < itemDatas_Pool.Count; i++)
        {
            builder.Append("/*I*/" + JsonUtility.ToJson(itemDatas_Pool[i]));
        }
        bindTileObj.TryToChangeInfo(builder.ToString());
    }
    #endregion
    #region//����UI
    /// <summary>
    /// ��齨������
    /// </summary>
    private void CheckRawList()
    {
        if (buildingConfig_TargetBuilding.Building_ID != 0)
        {
            for (int i = 0; i < buildingConfig_TargetBuilding.Building_Raw.Count; i++)
            {
                int index = itemDatas_Pool.FindIndex((x) => { return x.Item_ID == buildingConfig_TargetBuilding.Building_Raw[i].ID; });
                if (index >= 0)
                {
                    if (itemDatas_Pool[index].Item_Count < buildingConfig_TargetBuilding.Building_Raw[i].Count)
                    {
                        /*���������������*/
                        return;
                    }
                }
                else
                {
                    /*û���������*/
                    return;
                }
            }
            btn_TargetBuildingStart.gameObject.SetActive(true);
        }
    }
    #endregion
}
