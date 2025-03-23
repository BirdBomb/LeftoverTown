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
using Fusion;

public class UI_Grid_BuildingBuilder : UI_Grid
{
    [SerializeField, Header("建筑尺寸")]
    private AreaSize areaSize_Size;
    [SerializeField, Header("建造清单")]
    private RectTransform tran_BuildListPanel;
    [SerializeField, Header("建造清单_可建造建筑按钮列表")]
    private List<Button> btns_BuildNameBtn = new List<Button>();
    [SerializeField, Header("建造清单_建筑等级")]
    private TextMeshProUGUI text_BuildLevel;
    [SerializeField, Header("建造清单_上一难度按钮")]
    private Button btn_LastBuildLevel;
    [SerializeField, Header("建造清单_下一难度按钮")]
    private Button btn_NextBuildLevel;
    [SerializeField, Header("建造清单_上一页按钮")]
    private Button btn_LastBuildPage;
    [SerializeField, Header("建造清单_下一页按钮")]
    private Button btn_NextBuildPage;
    [SerializeField, Header("建造清单_结构按钮")]
    private Button btn_BuildingStructure;
    [SerializeField, Header("建造清单_结构按钮高亮")]
    private Transform tran_BuildingStructure;
    [SerializeField, Header("建造清单_家具按钮")]
    private Button btn_BuildingFurniture;
    [SerializeField, Header("建造清单_家具按钮高亮")]
    private Transform tran_BuildingFurniture;
    [SerializeField, Header("建造清单_机械按钮")]
    private Button btn_BuildingMachine;
    [SerializeField, Header("建造清单_机械按钮高亮")]
    private Transform tran_BuildingMachine;

    [SerializeField, Header("建筑预览")]
    private RectTransform panel_BuildingShow;
    [SerializeField, Header("建筑预览_建筑名字")]
    private TextMeshProUGUI text_TargetBuildingName;
    [SerializeField, Header("建筑预览_建筑图片")]
    private Image image_TargetBuildingImage;
    [SerializeField, Header("建筑预览_目标建筑原料")]
    private List<UI_ItemCell> itemCells_TargetBuildingRawList = new List<UI_ItemCell>();
    [SerializeField, Header("建筑预览_开始建造按钮")]
    private Button btn_TargetBuildingStart;
    [Header("建筑图标图集")]
    public SpriteAtlas spriteAtlas_BuildIcon;
    [Header("物品图标图集")]
    public SpriteAtlas spriteAtlas_ItemIcon;
    [Header("物品图标图集")]
    public SpriteAtlas spriteAtlas_ItemBG;
    private List<BuildingConfig> buildingConfigs_TempList = new List<BuildingConfig>();
    private BuildingConfig buildingConfig_TargetBuilding = new BuildingConfig();
    private int CurPage
    {
        get { return curPage; }
        set
        {
            curPage = value;
        }
    }
    private int curPage;
    private int CurLevel
    {
        get { return curLevel; }
        set
        {
            if (curLevel != value)
            {
                buildingConfigs_TempList = BuildingConfigData.buildConfigs.FindAll((x) => 
                {
                    return x.Building_Size == areaSize_Size && x.Building_Type == CurType && x.Building_RawLevel == value;
                });
                curLevel = value;
                CurPage = 0;
                UpdateBuildingListUI();
            }
        }
    }
    private int curLevel;
    private BuildingType CurType 
    {
        get { return curType; }
        set
        {
            if (curType != value)
            {
                buildingConfigs_TempList = BuildingConfigData.buildConfigs.FindAll((x) =>
                {
                    return x.Building_Size == areaSize_Size && x.Building_Type == value && x.Building_RawLevel == curLevel;
                });
                curType = value;
                CurPage = 0;
                UpdateBuildingListUI();
            }
        }
    }
    private BuildingType curType = BuildingType.Structure;


    private void Start()
    {
        btn_TargetBuildingStart.onClick.AddListener(ClickStartBuildBtn);
        btn_LastBuildLevel.onClick.AddListener(ClickLastLevelBtn);
        btn_NextBuildLevel.onClick.AddListener(ClickNextLevelBtn);
        btn_LastBuildPage.onClick.AddListener(ClickLastPageBtn);
        btn_NextBuildPage.onClick.AddListener(ClickNextPageBtn);
        btn_BuildingFurniture.onClick.AddListener(() =>
        {
            ClickBuildingTypeBtn(BuildingType.Furniture);
            tran_BuildingFurniture.gameObject.SetActive(true);
        });
        btn_BuildingMachine.onClick.AddListener(() =>
        {
            ClickBuildingTypeBtn(BuildingType.Machine);
            tran_BuildingMachine.gameObject.SetActive(true);
        });
        btn_BuildingStructure.onClick.AddListener(() =>
        {
            ClickBuildingTypeBtn(BuildingType.Structure);
            tran_BuildingStructure.gameObject.SetActive(true);
        });
        buildingConfigs_TempList = BuildingConfigData.buildConfigs.FindAll((x) => 
        {
            return x.Building_Size == areaSize_Size && x.Building_Type == CurType && x.Building_RawLevel == CurLevel;
        });
        UpdateBuildingListUI();
    }
    public override void Open()
    {
        UpdateBuildingListUI();
        base.Open();
    }
    #region//UI交互
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
        if (CurPage * btns_BuildNameBtn.Count < buildingConfigs_TempList.Count)
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
        buildingConfig_TargetBuilding = buildingConfig;
        BuildingBuilderConfig buildingBuilderConfig = new BuildingBuilderConfig();
        buildingBuilderConfig.Config = buildingConfig_TargetBuilding;
        buildingBuilderConfig.Type = CurType;
        buildingBuilderConfig.Level = CurLevel;
        buildingBuilderConfig.Page = CurPage;
        DrawBuildingPanel(buildingConfig);
        DrawBuildingRaw(buildingConfig);
    }
    private void ClickBuildingTypeBtn(BuildingType buildingType)
    {
        tran_BuildingFurniture.gameObject.SetActive(false);
        tran_BuildingMachine.gameObject.SetActive(false);
        tran_BuildingStructure.gameObject.SetActive(false);
        CurType = buildingType;
    }
    /// <summary>
    /// 开始建造按钮
    /// </summary>
    private void ClickStartBuildBtn()
    {
        if (CheckBuildingRaw(buildingConfig_TargetBuilding))
        {
            ExpendBuildingRaw(buildingConfig_TargetBuilding);
            MessageBroker.Default.Publish(new MapEvent.MapEvent_LocalTile_ChangeBuildingArea()
            {
                buildingID = buildingConfig_TargetBuilding.Building_ID,
                areaSize = areaSize_Size
            });
        }
    }

    #endregion
    #region//UI绘制
    /// <summary>
    /// 重置建筑UI
    /// </summary>
    private void ResetBuildUI()
    {
        btn_TargetBuildingStart.gameObject.SetActive(false);
        text_TargetBuildingName.text = "";
        image_TargetBuildingImage.gameObject.SetActive(false);
        for (int i = 0; i < itemCells_TargetBuildingRawList.Count; i++)
        {
            itemCells_TargetBuildingRawList[i].Clean() ;
        }
    }
    /// <summary>
    /// 更新建筑列表
    /// </summary>
    private void UpdateBuildingListUI()
    {
        for (int i = 0; i < btns_BuildNameBtn.Count; i++)
        {
            btns_BuildNameBtn[i].onClick.RemoveAllListeners();
            if (i + CurPage * btns_BuildNameBtn.Count < buildingConfigs_TempList.Count)
            {
                BuildingConfig buildingConfig = buildingConfigs_TempList[i + CurPage * btns_BuildNameBtn.Count];
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
    /// <summary>
    /// 绘制建筑预览界面
    /// </summary>
    private void DrawBuildingPanel(BuildingConfig config)
    {
        if (config.Building_ID > 0)
        {
            image_TargetBuildingImage.gameObject.SetActive(true);
            image_TargetBuildingImage.sprite = spriteAtlas_BuildIcon.GetSprite(config.Building_ID.ToString());
            text_TargetBuildingName.text = config.Building_Name;
        }
        else
        {
            image_TargetBuildingImage.gameObject.SetActive(false);
            text_TargetBuildingName.text = "";
        }
    }
    /// <summary>
    /// 绘制建筑材料界面
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
            CheckBuildingRaw(config);
        }
    }
    /// <summary>
    /// 检查建筑原材料
    /// </summary>
    /// <param name="config"></param>
    /// <returns></returns>
    private bool CheckBuildingRaw(BuildingConfig config)
    {
        bool temp = true;
        List<ItemData> data = new List<ItemData>(GameLocalManager.Instance.playerCoreLocal.actorManager_Bind.actorNetManager.Net_ItemsInBag);
        for (int i = 0; i < config.Building_Raw.Count; i++)
        {
            ItemConfig itemConfig = ItemConfigData.GetItemConfig(config.Building_Raw[i].ID);
            int itemCount = 0;
            for (int j = 0; j < data.Count; j++)
            {
                if (data[j].Item_ID == config.Building_Raw[i].ID)
                {
                    itemCount += data[j].Item_Count;
                }
            }
            string info = itemCount.ToString() + "/" + config.Building_Raw[i].Count.ToString();
            if (itemCount < config.Building_Raw[i].Count)
            {
                temp = false;
            }
            itemCells_TargetBuildingRawList[i].Draw(spriteAtlas_ItemIcon.GetSprite("Item_" + itemConfig.Item_ID.ToString()), spriteAtlas_ItemBG.GetSprite("ItemBG_" + itemConfig.ItemRarity), itemConfig.ItemRarity, info, itemConfig.Item_Name, itemConfig.Item_Desc);
        }
        return temp;
    }
    /// <summary>
    /// 消耗建筑原材料
    /// </summary>
    /// <param name="config"></param>
    private void ExpendBuildingRaw(BuildingConfig config)
    {
        List<ItemData> data = new List<ItemData>(GameLocalManager.Instance.playerCoreLocal.actorManager_Bind.actorNetManager.Net_ItemsInBag);
        for (int i = 0; i < config.Building_Raw.Count; i++)
        {
            ItemConfig itemConfig = ItemConfigData.GetItemConfig(config.Building_Raw[i].ID);
            ItemData itemData = new ItemData(config.Building_Raw[i].ID);
            itemData.Item_Count = config.Building_Raw[i].Count;
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TrySubItemInBag()
            {
                item = itemData
            });
        }
    }
    #endregion
}
public struct BuildingBuilderConfig
{
    public BuildingType Type;
    public BuildingConfig Config;
    public int Level;
    public int Page;
}