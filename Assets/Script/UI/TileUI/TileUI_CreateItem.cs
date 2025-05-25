using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;
using static Fusion.Allocator;

public class TileUI_CreateItem : TileUI
{
    [Header("物品图标图集")]
    public SpriteAtlas spriteAtlas_ItemIcon;
    [Header("物品图标图集")]
    public SpriteAtlas spriteAtlas_ItemBG;
    [Header("上一页")]
    public Button button_LastPage;
    [Header("下一页")]
    public Button button_NextPage;
    [Header("工具台名称")]
    public TextMeshProUGUI textMeshPro_Name;
    public Transform transform_Panel;
    [SerializeField]
    private List<UI_ItemCell> itemCells_PoolIcon = new List<UI_ItemCell>();
    [SerializeField]
    private List<UI_ItemCell> itemCells_RawIcon = new List<UI_ItemCell>();
    /// <summary>
    /// 合成池
    /// </summary>
    private List<CreateRawConfig> createConfigs_Pool = new List<CreateRawConfig>();
    /// <summary>
    /// 合成原材料
    /// </summary>
    private List<ItemConfig> itemConfigs_Raw = new List<ItemConfig>();
    private int curPage;
    private int CurPage
    {
        get { return curPage; }
        set
        {
            curPage = value;
        }
    }
    private CreateRawConfig createRawConfig_Temp;
    private void Awake()
    {
        button_LastPage.onClick.AddListener(ClickLastPageBtn);
        button_NextPage.onClick.AddListener(ClickNextPageBtn);
    }
    private void Start()
    {
        MessageBroker.Default.Receive<UIEvent.UIEvent_UpdateItemInBag>().Subscribe(_ =>
        {
            DrawPoolPanel();
            DrawRawPanel();
        }).AddTo(this);
    }
    public void InitPool(List<CreateRawConfig> configs,string str)
    {
        textMeshPro_Name.text = str;
        createConfigs_Pool.Clear();
        itemConfigs_Raw.Clear();
        for (int i = 0; i < configs.Count; i++)
        {
            createConfigs_Pool.Add(configs[i]);
        }
        DrawPoolPanel();
        transform_Panel.transform.DOKill();
        transform_Panel.transform.DOPunchScale(new Vector3(0.1f, -0.1f, 0), 0.1f);
    }
    #region//绘制
    private void DrawPoolPanel()
    {
        for (int i = 0; i < itemCells_PoolIcon.Count; i++)
        {
            if (i + CurPage * itemCells_PoolIcon.Count < createConfigs_Pool.Count)
            {
                CreateRawConfig createRawConfig = createConfigs_Pool[i + CurPage * itemCells_PoolIcon.Count];
                ItemConfig itemConfig = ItemConfigData.GetItemConfig(createRawConfig.Create_TargetID);
                Sprite icon = spriteAtlas_ItemIcon.GetSprite("Item_" + itemConfig.Item_ID.ToString());
                Sprite bg = spriteAtlas_ItemBG.GetSprite("ItemBG_" + (int)itemConfig.Item_Rarity);
                string itemName = LocalizationManager.Instance.GetLocalization("Item_String", itemConfig.Item_ID + "_Name");
                string itemDesc = LocalizationManager.Instance.GetLocalization("Item_String", itemConfig.Item_ID + "_Desc");
                itemCells_PoolIcon[i].DrawCellInfo(createRawConfig.Create_TargetCount.ToString(), itemName, itemDesc, itemConfig.Item_Rarity);
                if (CheckRaw(createRawConfig.Create_RawList))
                {
                    itemCells_PoolIcon[i].DrawCellIcon(icon, Color.white, bg, Color.white);
                    itemCells_PoolIcon[i].BindAction
                        (
                            () =>
                            {
                                ClickShowRawBtn(createRawConfig);
                            },
                            () =>
                            {
                                PressCreateBtn(createRawConfig);
                            }
                        );
                }
                else
                {
                    itemCells_PoolIcon[i].DrawCellIcon(icon, Color.red, bg, Color.red);
                    itemCells_PoolIcon[i].BindAction
                        (
                            () =>
                            {
                                ClickShowRawBtn(createRawConfig);
                            },
                            null
                        );
                }
            }
            else
            {
                itemCells_PoolIcon[i].CleanCell();
                itemCells_PoolIcon[i].CleanAction();
            }
        }
    }
    private void DrawRawPanel()
    {
        if (createRawConfig_Temp.Create_TargetID != 0)
        {
            for (int i = 0; i < itemCells_RawIcon.Count; i++)
            {
                if (i < createRawConfig_Temp.Create_RawList.Count)
                {
                    ItemConfig itemConfig = ItemConfigData.GetItemConfig(createRawConfig_Temp.Create_RawList[i].ID);
                    Sprite icon = spriteAtlas_ItemIcon.GetSprite("Item_" + itemConfig.Item_ID.ToString());
                    Sprite bg = spriteAtlas_ItemBG.GetSprite("ItemBG_" + (int)itemConfig.Item_Rarity);
                    List<ItemData> itemDatas = GameLocalManager.Instance.playerCoreLocal.actorManager_Bind.actorNetManager.Local_GetBagItem();
                    int itemCount = 0;
                    for (int k = 0; k < itemDatas.Count; k++)
                    {
                        if (itemDatas[k].Item_ID == createRawConfig_Temp.Create_RawList[i].ID)
                        {
                            itemCount += itemDatas[k].Item_Count;
                        }
                    }
                    string info_temp = itemCount.ToString() + "/" + createRawConfig_Temp.Create_RawList[i].Count.ToString();
                    string itemName = LocalizationManager.Instance.GetLocalization("Item_String", itemConfig.Item_ID + "_Name");
                    string itemDesc = LocalizationManager.Instance.GetLocalization("Item_String", itemConfig.Item_ID + "_Desc");
                    itemCells_RawIcon[i].DrawCellInfo(info_temp, itemName, itemDesc, itemConfig.Item_Rarity);
                    if (itemCount < createRawConfig_Temp.Create_RawList[i].Count)
                    {
                        itemCells_RawIcon[i].DrawCellIcon(icon, Color.red, bg, Color.red);
                    }
                    else
                    {
                        itemCells_RawIcon[i].DrawCellIcon(icon, Color.white, bg, Color.white);
                    }
                }
                else
                {
                    itemCells_RawIcon[i].CleanCell();
                }
            }
        }
    }
    #endregion
    #region//交互
    /// <summary>
    /// 下一页按钮点击
    /// </summary>
    private void ClickNextPageBtn()
    {
        if (CurPage * itemCells_PoolIcon.Count < createConfigs_Pool.Count)
        {
            CurPage++;
        }
        else
        {
            CurPage = 0;
        }
        DrawPoolPanel();
    }
    /// <summary>
    /// 上一页按钮点击
    /// </summary>
    private void ClickLastPageBtn()
    {
        if (CurPage > 0)
        {
            CurPage--;
        }
        DrawPoolPanel();
    }
    /// <summary>
    /// 预览材料
    /// </summary>
    /// <param name="createRawConfig"></param>
    private void ClickShowRawBtn(CreateRawConfig createRawConfig)
    {
        createRawConfig_Temp = createRawConfig;
        DrawRawPanel();
    }
    /// <summary>
    /// 长按建造
    /// </summary>
    /// <param name="createRawConfig"></param>
    private void PressCreateBtn(CreateRawConfig createRawConfig)
    {
        if (CheckRaw(createRawConfig.Create_RawList))
        {
            ExpendRaw(createRawConfig.Create_RawList);
            Type type = Type.GetType("Item_" + createRawConfig.Create_TargetID.ToString());
            ((ItemBase)Activator.CreateInstance(type)).StaticAction_InitData((short)createRawConfig.Create_TargetID, out ItemData initData);
            initData.Item_Count = (short)createRawConfig.Create_TargetCount;
            AudioManager.Instance.Play2DEffect(1000);
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryAddItemInBag()
            {
                itemData = initData,
            });
        }
        else
        {
            AudioManager.Instance.Play2DEffect(1001);
            Debug.Log("原料不足");
        }
    }
    /// <summary>
    /// 检查原料
    /// </summary>
    /// <param name="raws"></param>
    /// <returns></returns>
    private bool CheckRaw(List<ItemRaw> raws)
    {
        if (raws == null) { return false; }
        bool temp = true;
        List<ItemData> itemDatas = GameLocalManager.Instance.playerCoreLocal.actorManager_Bind.actorNetManager.Local_GetBagItem();
        for (int i = 0; i < raws.Count; i++)
        {
            ItemConfig itemConfig = ItemConfigData.GetItemConfig(raws[i].ID);
            int itemCount = 0;
            for (int j = 0; j < itemDatas.Count; j++)
            {
                if (itemDatas[j].Item_ID == raws[i].ID)
                {
                    itemCount += itemDatas[j].Item_Count;
                }
            }
            if (itemCount < raws[i].Count)
            {
                temp = false;
            }
        }
        return temp;
    }
    /// <summary>
    /// 消耗原料
    /// </summary>
    /// <param name="raws"></param>
    private void ExpendRaw(List<ItemRaw> raws)
    {
        for (int i = 0; i < raws.Count; i++)
        {
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryExpendItemInBag()
            {
                itemID = raws[i].ID,
                itemCount = raws[i].Count,
            });
        }
    }
    #endregion
}
