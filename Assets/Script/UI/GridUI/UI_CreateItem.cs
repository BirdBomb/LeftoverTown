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
using TMPro;
using static Fusion.Allocator;
using UnityEditor;

public class UI_CreateItem : UI_Grid
{
    [SerializeField, Header("�ϳ��嵥")]
    private RectTransform panel_CreateList;
    [SerializeField, Header("�ϳ��嵥_��������Ʒ�б�")]
    private List<UI_ItemCell> itemCells_CreateList = new List<UI_ItemCell>();
    [SerializeField, Header("�ϳ��嵥_��������Ʒ�ȼ�")]
    private TextMeshProUGUI text_CreateLevel;
    [SerializeField, Header("�ϳ��嵥_��һ�ѶȰ�ť")]
    private Button btn_LastCreateLevel;
    [SerializeField, Header("�ϳ��嵥_��һ�ѶȰ�ť")]
    private Button btn_NextCreateLevel;
    [SerializeField, Header("�ϳ��嵥_��һҳ��ť")]
    private Button btn_LastCreateListPage;
    [SerializeField, Header("�ϳ��嵥_��һҳ��ť")]
    private Button btn_NextCreateListPage;
    [SerializeField, Header("�ϳ�Ԥ��")]
    private RectTransform panel_CreateShow;
    [SerializeField, Header("�ϳ�Ԥ��_��������")]
    private TextMeshProUGUI text_TargetItemName;
    [SerializeField, Header("�ϳ�Ԥ��_��������")]
    private TextMeshProUGUI text_TargetItemDesc;
    [SerializeField, Header("�ϳ�Ԥ��_�ϳ�Ŀ������")]
    private UI_ItemCell itemCell_TargetItem;
    [SerializeField, Header("�ϳ�Ԥ��_�ϳ�����Ʒ�б�")]
    private List<UI_ItemCell> itemCells_TargetItemRawList = new List<UI_ItemCell>();
    [SerializeField, Header("�ϳ�Ԥ��_���찴ť")]
    private Button btn_Create;
    [SerializeField, Header("����̨����")]
    private List<UI_GridCell> gridCells_WorkbenchPool = new List<UI_GridCell>();
    [SerializeField, Header("����̨����_��Ʒ")]
    private UI_GridCell gridCell_TargetItem;
    [SerializeField, Header("����̨����_���������")]
    private Image bar_CreateProgressBar;
    [Header("��Ʒͼ��ͼ��")]
    public SpriteAtlas spriteAtlas_ItemIcon;
    [Header("��Ʒͼ��ͼ��")]
    public SpriteAtlas spriteAtlas_ItemBG;
    private TileObj bindTileObj;
    private List<ItemData> itemDatas_WorkbenchPool = new List<ItemData>();
    private ItemData itemData_TargetItem;
    private List<CreateConfig> createConfigs_TempList = new List<CreateConfig>();
    private CreateConfig createConfig_TargetCreateItem;

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
                createConfigs_TempList = CreateConfigData.createConfigs.FindAll((x) => { return x.Create_Level == value; });
                curLevel = value;
                CurPage = 0;
                UpdateCreateListUI();
            }
        }
    }

    private void Start()
    {
        btn_Create.onClick.AddListener(ClickCreateBtn);
        btn_LastCreateLevel.onClick.AddListener(ClickLastLevelBtn);
        btn_NextCreateLevel.onClick.AddListener(ClickNextLevelBtn);
        btn_LastCreateListPage.onClick.AddListener(ClickLastPageBtn);
        btn_NextCreateListPage.onClick.AddListener(ClickNextPageBtn);
        createConfigs_TempList = CreateConfigData.createConfigs.FindAll((x) => { return x.Create_Level == 0; });
        UpdateCreateListUI();
    }
    #region//�򿪹ر�
    public override void Open(TileObj tileObj)
    {
        bindTileObj = tileObj;
        base.Open(tileObj);
    }

    #endregion
    #region//UI����
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
        if (CurPage * itemCells_CreateList.Count < createConfigs_TempList.Count)
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
    /// ��һҳ��ť���
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
    /// Ԥ����ť���
    /// </summary>
    /// <param name="buildingConfig"></param>
    private void ClickItemShowBtn(CreateConfig createConfig)
    {
        createConfig_TargetCreateItem = createConfig;
        UpdateCreatePanel(createConfig_TargetCreateItem);
    }
    /// <summary>
    /// ���찴ť���
    /// </summary>
    private void ClickCreateBtn()
    {
        if(itemData_TargetItem.Item_ID == 0)
        {
            bar_CreateProgressBar.DOKill();
            bar_CreateProgressBar.transform.DOScaleX(1, 1).OnComplete(() =>
            {
                CreateItem(createConfig_TargetCreateItem);
            });
        }
    }
    #endregion
    #region//UI����
    /// <summary>
    /// ��������UI
    /// </summary>
    private void ResetCreateItemUI()
    {
        text_TargetItemName.text = "";
        text_TargetItemDesc.text = "";
        bar_CreateProgressBar.transform.localScale = new Vector3(0, 1, 1);
        itemCell_TargetItem.Clean();
        for (int i = 0; i < itemCells_TargetItemRawList.Count; i++)
        {
            itemCells_TargetItemRawList[i].Clean();
        }
    }
    /// <summary>
    /// ���������б�
    /// </summary>
    private void UpdateCreateListUI()
    {
        for (int i = 0; i < itemCells_CreateList.Count; i++)
        {
            itemCells_CreateList[i].btn_ItemCell.onClick.RemoveAllListeners();
            if (i + CurPage * itemCells_CreateList.Count < createConfigs_TempList.Count)
            {
                CreateConfig createConfig = createConfigs_TempList[i + CurPage * itemCells_CreateList.Count];
                ItemConfig itemConfig = ItemConfigData.GetItemConfig(createConfig.Create_ID);
                itemCells_CreateList[i].btn_ItemCell.onClick.AddListener(() => { ClickItemShowBtn(createConfig); });
                itemCells_CreateList[i].Draw
                    (spriteAtlas_ItemIcon.GetSprite("Item_" + itemConfig.Item_ID.ToString()), 
                    spriteAtlas_ItemBG.GetSprite("ItemBG_" + itemConfig.ItemRarity),
                    itemConfig.ItemRarity,
                    "", 
                    itemConfig.Item_Name,
                    itemConfig.Item_Desc);
            }
            else
            {
                itemCells_CreateList[i].Clean();
            }
        }
        text_CreateLevel.text = "Lv" + CurLevel;
    }
    /// <summary>
    /// ��������Ԥ��
    /// </summary>
    private void UpdateCreatePanel(CreateConfig config)
    {
        ResetCreateItemUI();
        DrawCreateRaw(config);
        DrawCreatePanel(config);
        CheckRawList();
    }
    /// <summary>
    /// ��������Ԥ��
    /// </summary>
    /// <param name="config"></param>
    private void DrawCreatePanel(CreateConfig config)
    {
        if (config.Create_ID > 0)
        {
            ItemConfig itemConfig = ItemConfigData.GetItemConfig(config.Create_ID);
            itemCell_TargetItem.Draw
                (spriteAtlas_ItemIcon.GetSprite("Item_" + itemConfig.Item_ID.ToString()), 
                spriteAtlas_ItemBG.GetSprite("ItemBG_" + itemConfig.ItemRarity),
                itemConfig.ItemRarity,
                "", 
                itemConfig.Item_Name,
                itemConfig.Item_Desc);
            text_TargetItemName.text = ItemConfigData.GetItemConfig(config.Create_ID).Item_Name;
            text_TargetItemDesc.text = ItemConfigData.GetItemConfig(config.Create_ID).Item_Desc;
        }
        else
        {
            itemCell_TargetItem.Clean();
            text_TargetItemName.text = "";
            text_TargetItemDesc.text = "";
        }
    }
    /// <summary>
    /// ����������Ͻ���
    /// </summary>
    private void DrawCreateRaw(CreateConfig config)
    {
        if (config.Create_ID == 0)
        {
            for (int i = 0; i < itemCells_TargetItemRawList.Count; i++)
            {
                itemCells_TargetItemRawList[i].Clean();
            }
        }
        else
        {
            for (int i = 0; i < config.Create_Raw.Count; i++)
            {
                ItemConfig itemConfig = ItemConfigData.GetItemConfig(config.Create_Raw[i].ID);
                string info="";
                int tempIndex = itemDatas_WorkbenchPool.FindIndex((x) => { return x.Item_ID == config.Create_Raw[i].ID; });

                if (tempIndex >= 0)
                {
                    info = itemDatas_WorkbenchPool[tempIndex].Item_Count.ToString() + "/" + config.Create_Raw[i].Count.ToString();
                }
                else
                {
                    info = "0/" + config.Create_Raw[i].Count.ToString();
                }

                itemCells_TargetItemRawList[i].Draw
                    (spriteAtlas_ItemIcon.GetSprite("Item_" + itemConfig.Item_ID.ToString()), 
                    spriteAtlas_ItemBG.GetSprite("ItemBG_" + itemConfig.ItemRarity),
                    itemConfig.ItemRarity,
                    info, 
                    itemConfig.Item_Name,
                    itemConfig.Item_Desc);
            }
        }
    }
    /// <summary>
    /// ���ƹ���̨����
    /// </summary>
    private void DrawCreatePool()
    {
        if (itemData_TargetItem.Item_ID != 0)
        {
            DrawCell(itemData_TargetItem, gridCell_TargetItem);
        }
        else 
        {
            ResetCell(gridCell_TargetItem);
        }
        for (int i = 0; i < gridCells_WorkbenchPool.Count; i++)
        {
            if (i < itemDatas_WorkbenchPool.Count && itemDatas_WorkbenchPool[i].Item_ID != 0)
            {
                DrawCell(itemDatas_WorkbenchPool[i], gridCells_WorkbenchPool[i]);
            }
            else
            {
                ResetCell(gridCells_WorkbenchPool[i]);
            }
        }
    }
    /// <summary>
    /// ����һ������
    /// </summary>
    /// <param name="cell"></param>
    private void ResetCell(UI_GridCell cell)
    {
        cell.ResetGridCell();
    }
    /// <summary>
    /// ����һ������
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
    #region//�ó�����
    public override void PutOut(ItemData before, out ItemData after)
    {
        if (itemData_TargetItem.Equals(before))
        {
            itemData_TargetItem = new ItemData();
        }
        itemDatas_WorkbenchPool.Remove(before);
        after = before;
        ChangeInfoToTile();
    }
    public override void PutIn(ItemData before, out ItemData after)
    {
        ItemConfig config = ItemConfigData.GetItemConfig(before.Item_ID);
        ItemData resData = before;
        if (config.Item_Size == ItemSize.AsGroup)
        {
            for (int i = 0; i < gridCells_WorkbenchPool.Count; i++)
            {
                if (itemDatas_WorkbenchPool.Count > i)
                {
                    if (itemDatas_WorkbenchPool[i].Item_ID == resData.Item_ID)
                    {
                        Type type = Type.GetType("Item_" + itemDatas_WorkbenchPool[i].Item_ID.ToString());
                        ((ItemBase)Activator.CreateInstance(type)).StaticAction_PileUp(itemDatas_WorkbenchPool[i], resData, config.Item_MaxCount, out ItemData newData, out resData);
                        itemDatas_WorkbenchPool[i] = newData;
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
                        itemDatas_WorkbenchPool.Add(newData);
                    }
                }
            }
        }
        else
        {
            if (itemDatas_WorkbenchPool.Count < gridCells_WorkbenchPool.Count)
            {
                ItemData emptyData = resData;
                emptyData.Item_Count = 0;
                Type type = Type.GetType("Item_" + resData.Item_ID.ToString());
                ((ItemBase)Activator.CreateInstance(type)).StaticAction_PileUp(emptyData, resData, config.Item_MaxCount, out ItemData newData, out resData);
                itemDatas_WorkbenchPool.Add(newData);
            }
        }
        after = resData;
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
        itemDatas_WorkbenchPool.Clear();
        itemData_TargetItem = new ItemData();
        string[] strings = info.Split("/*I*/");
        for (int i = 0; i < strings.Length; i++)
        {
            if (i == 0)
            {
                /*��һλ�Ǵ�������id*/
                if (strings[i] != "")
                {
                    itemData_TargetItem = JsonUtility.FromJson<ItemData>(strings[i]);
                }
            }
            else if (strings[i] != "")
            {
                ItemData data = JsonUtility.FromJson<ItemData>(strings[i]);
                if(data.Item_ID != 0)
                {
                    itemDatas_WorkbenchPool.Add(data);
                }
            }
        }
        if (createConfig_TargetCreateItem.Create_ID != 0)
        {
            UpdateCreatePanel(createConfig_TargetCreateItem);
        }
        DrawCreatePool();
    }
    /// <summary>
    /// ������Ϣ
    /// </summary>
    public void ChangeInfoToTile()
    {
        StringBuilder builder = new StringBuilder();
        builder.Append(JsonUtility.ToJson(itemData_TargetItem));
        for (int i = 0; i < itemDatas_WorkbenchPool.Count; i++)
        {
            builder.Append("/*I*/" + JsonUtility.ToJson(itemDatas_WorkbenchPool[i]));
        }
        bindTileObj.TryToChangeInfo(builder.ToString());
    }
    #endregion
    #region//����UI
    /// <summary>
    /// ���itemData����
    /// </summary>
    private void CheckRawList()
    {
        btn_Create.gameObject.SetActive(false);
        if (createConfig_TargetCreateItem.Create_ID != 0 && itemData_TargetItem.Item_ID == 0)
        {
            for (int i = 0; i < createConfig_TargetCreateItem.Create_Raw.Count; i++)
            {
                int index = itemDatas_WorkbenchPool.FindIndex((x) => { return x.Item_ID == createConfig_TargetCreateItem.Create_Raw[i].ID; });
                if (index >= 0)
                {
                    if (itemDatas_WorkbenchPool[index].Item_Count < createConfig_TargetCreateItem.Create_Raw[i].Count)
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
            btn_Create.gameObject.SetActive(true);
        }
    }
    /// <summary>
    /// ��������
    /// </summary>
    private void CreateItem(CreateConfig createConfig)
    {
        for (int i = 0; i < createConfig.Create_Raw.Count; i++)
        {
            CreateRaw raw = createConfig.Create_Raw[i];
            for(int j = 0; j < itemDatas_WorkbenchPool.Count; j++)
            {
                if (itemDatas_WorkbenchPool[j].Item_ID == raw.ID)
                {
                    ItemData temp = itemDatas_WorkbenchPool[j];
                    if (itemDatas_WorkbenchPool[j].Item_Count > raw.Count)
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
                        itemDatas_WorkbenchPool[j] = new ItemData();
                    }
                    else
                    {
                        itemDatas_WorkbenchPool[j] = temp;
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
        itemData_TargetItem = initData;
        ChangeInfoToTile();
    }
    #endregion

}
