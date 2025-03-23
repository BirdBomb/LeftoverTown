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

public class UI_Grid_CreateItem : UI_Grid
{
    [SerializeField, Header("�ϳ��嵥")]
    private RectTransform tran_CreateListPanel;
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
    private TextMeshProUGUI text_CreateItemName;
    [SerializeField, Header("�ϳ�Ԥ��_��������")]
    private TextMeshProUGUI text_CreateItemDesc;
    [SerializeField, Header("�ϳ�Ԥ��_�ϳ�����Ʒ�б�")]
    private List<UI_ItemCell> itemCells_RawItemList = new List<UI_ItemCell>();
    [SerializeField, Header("�ϳ�Ԥ��_�ϳ�Ŀ������")]
    private UI_ItemCell itemCell_CreateItem;
    [SerializeField, Header("�ϳ�Ԥ��_���찴ť")]
    private Button btn_Create;
    [SerializeField, Header("����̨����")]
    private List<UI_GridCell> gridCells_RawItemList = new List<UI_GridCell>();
    [SerializeField, Header("����̨����_��Ʒ")]
    private UI_GridCell gridCell_CreateItem;
    [SerializeField, Header("����̨����_���������")]
    private Image bar_CreateProgressBar;
    [Header("��Ʒͼ��ͼ��")]
    public SpriteAtlas spriteAtlas_ItemIcon;
    [Header("��Ʒͼ��ͼ��")]
    public SpriteAtlas spriteAtlas_ItemBG;
    private List<ItemData> itemDatas_Raw = new List<ItemData>();
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
        BindAllCell();
    }
    private void BindAllCell()
    {
        for (int i = 0; i < gridCells_RawItemList.Count; i++)
        {
            gridCells_RawItemList[i].BindAction(PutInRaw,PutOutRaw, null, null);
        }
        gridCell_CreateItem.BindAction(PutInTarget, PutOutTarget, null, null);
    }
    #region//UI����
    /// <summary>
    /// ��һ�ѶȰ�ť���
    /// </summary>
    private void ClickNextLevelBtn()
    {
        if (curLevel < itemCells_CreateList.Count)
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
            CurLevel = itemCells_CreateList.Count;
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
        text_CreateItemName.text = "";
        text_CreateItemDesc.text = "";
        bar_CreateProgressBar.transform.localScale = new Vector3(0, 1, 1);
        itemCell_CreateItem.Clean();
        for (int i = 0; i < itemCells_RawItemList.Count; i++)
        {
            itemCells_RawItemList[i].Clean();
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
            itemCell_CreateItem.Draw
                (spriteAtlas_ItemIcon.GetSprite("Item_" + itemConfig.Item_ID.ToString()), 
                spriteAtlas_ItemBG.GetSprite("ItemBG_" + itemConfig.ItemRarity),
                itemConfig.ItemRarity,
                "", 
                itemConfig.Item_Name,
                itemConfig.Item_Desc);
            text_CreateItemName.text = ItemConfigData.GetItemConfig(config.Create_ID).Item_Name;
            text_CreateItemDesc.text = ItemConfigData.GetItemConfig(config.Create_ID).Item_Desc;
        }
        else
        {
            itemCell_CreateItem.Clean();
            text_CreateItemName.text = "";
            text_CreateItemDesc.text = "";
        }
    }
    /// <summary>
    /// ����������Ͻ���
    /// </summary>
    private void DrawCreateRaw(CreateConfig config)
    {
        if (config.Create_ID == 0)
        {
            for (int i = 0; i < itemCells_RawItemList.Count; i++)
            {
                itemCells_RawItemList[i].Clean();
            }
        }
        else
        {
            for (int i = 0; i < config.Create_Raw.Count; i++)
            {
                ItemConfig itemConfig = ItemConfigData.GetItemConfig(config.Create_Raw[i].ID);
                string info="";
                int tempIndex = itemDatas_Raw.FindIndex((x) => { return x.Item_ID == config.Create_Raw[i].ID; });

                if (tempIndex >= 0)
                {
                    info = itemDatas_Raw[tempIndex].Item_Count.ToString() + "/" + config.Create_Raw[i].Count.ToString();
                }
                else
                {
                    info = "0/" + config.Create_Raw[i].Count.ToString();
                }

                itemCells_RawItemList[i].Draw
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
            gridCell_CreateItem.UpdateData(itemData_TargetItem);
        }
        else 
        {
            gridCell_CreateItem.CleanData();
        }
        for (int i = 0; i < gridCells_RawItemList.Count; i++)
        {
            if (i < itemDatas_Raw.Count && itemDatas_Raw[i].Item_ID != 0)
            {
                gridCells_RawItemList[i].UpdateData(itemDatas_Raw[i]);
            }
            else
            {
                gridCells_RawItemList[i].CleanData();
            }
        }
    }
    #endregion
    #region//��Ϣ�ϴ������
    /// <summary>
    /// ������Ϣ
    /// </summary>
    /// <param name="info"></param>
    public void UpdateInfo(string info)
    {
        itemDatas_Raw.Clear();
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
                    itemDatas_Raw.Add(data);
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
    public void ChangeInfo()
    {
        StringBuilder builder = new StringBuilder();
        builder.Append(JsonUtility.ToJson(itemData_TargetItem));
        for (int i = 0; i < itemDatas_Raw.Count; i++)
        {
            builder.Append("/*I*/" + JsonUtility.ToJson(itemDatas_Raw[i]));
        }
        if (action_ChangeInfo != null)
        {
            action_ChangeInfo.Invoke(builder.ToString());
        }
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
                int index = itemDatas_Raw.FindIndex((x) => { return x.Item_ID == createConfig_TargetCreateItem.Create_Raw[i].ID; });
                if (index >= 0)
                {
                    if (itemDatas_Raw[index].Item_Count < createConfig_TargetCreateItem.Create_Raw[i].Count)
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
            for(int j = 0; j < itemDatas_Raw.Count; j++)
            {
                if (itemDatas_Raw[j].Item_ID == raw.ID)
                {
                    ItemData temp = itemDatas_Raw[j];
                    if (itemDatas_Raw[j].Item_Count > raw.Count)
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
                        itemDatas_Raw[j] = new ItemData();
                    }
                    else
                    {
                        itemDatas_Raw[j] = temp;
                    }
                }
            }
        }
        Type type = Type.GetType("Item_" + createConfig.Create_ID.ToString());
        ((ItemBase)Activator.CreateInstance(type)).StaticAction_InitData(createConfig.Create_ID, out ItemData initData);
        itemData_TargetItem = initData;
        ChangeInfo();
    }
    #endregion
    public void PutInRaw(ItemData data)
    {
        itemDatas_Raw = GameToolManager.Instance.PutInItemList(itemDatas_Raw, gridCells_RawItemList.Count, data, out data);
        if (data.Item_Count != 0)
        {
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryAddItemInBag()
            {
                item = data,
            });
        }
        ChangeInfo();
    }
    public ItemData PutOutRaw(ItemData data)
    {
        itemDatas_Raw = GameToolManager.Instance.PutOutItemList(itemDatas_Raw, data);
        ChangeInfo();
        return data;
    }
    public void PutInTarget(ItemData data)
    {
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryAddItemInBag()
        {
            item = data,
        });
    }
    public ItemData PutOutTarget(ItemData data)
    {
        itemData_TargetItem = GameToolManager.Instance.PutOutItemSingle(itemData_TargetItem, data);
        ChangeInfo();
        return data;
    }
}
