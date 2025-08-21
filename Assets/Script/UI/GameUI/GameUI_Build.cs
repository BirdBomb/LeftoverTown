using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization.Components;
using UnityEngine.U2D;
using UnityEngine.UI;

public class GameUI_Build : MonoBehaviour
{
    private List<BuildingConfig> buildingConfigs_TempList = new List<BuildingConfig>();
    private List<GroundConfig> groundConfigs_TempList = new List<GroundConfig>();
    public Transform tran_Panel;
    public Transform tran_ShowBtn;
    public void Start()
    {
        MessageBroker.Default.Receive<UIEvent.UIEvent_UpdateItemInBag>().Subscribe(_ =>
        {
            DrawBuildingList();
            if (index >= 0)
            {
                if (CurType == BuildingType.Ground)
                {
                    DrawBuildingRaw(buildingConfig_Temp, index);
                }
                else
                {
                    DrawBuildingRaw(groundConfig_Temp, index);
                }
            }
        }).AddTo(this);

        button_LastPage.onClick.AddListener(ClickLastPageBtn);
        button_NextPage.onClick.AddListener(ClickNextPageBtn);

        button_LastType.onClick.AddListener(ClickLastTypeBtn);
        button_NextType.onClick.AddListener(ClickNextTypeBtn);

        button_LastAge.onClick.AddListener(ClickLastAgeBtn);
        button_NextAge.onClick.AddListener(ClickNextAgeBtn);

        UpdateBuildingList();
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0) && !EventSystem.current.IsPointerOverGameObject())
        {
            //DrawBuildingRaw(new BuildingConfig(), 0);
            //DrawBuildingRaw(new GroundConfig(), 0);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            if(tran_Panel.gameObject.activeSelf)
            {
                HidePanel();
            }
            else
            {
                ShowPanel();
            }
        }
    }
    private void ShowPanel()
    {
        tran_ShowBtn.gameObject.SetActive(false);
        tran_Panel.gameObject.SetActive(true);
        tran_Panel.DOKill();
        tran_Panel.localScale = Vector3.one;
        tran_Panel.DOPunchScale(new Vector3(0.1f, -0.1f, 0), 0.2f);
    }
    private void HidePanel()
    {
        tran_ShowBtn.gameObject.SetActive(true);
        tran_Panel.gameObject.SetActive(false);
        DrawBuildingRaw(new BuildingConfig(), 0);
        DrawBuildingRaw(new GroundConfig(), 0);
    }
    #region//建筑类别
    [Header("上一类别")]
    public Button button_LastType;
    [Header("下一类别")]
    public Button button_NextType;
    [Header("类别")]
    public LocalizeStringEvent localizeStringEvent_Type;

    private int curTypeIndex = 0;
    private List<BuildingType> buildingTypes_Config = new List<BuildingType>()
    {
        BuildingType.All,
        BuildingType.Ground,
        BuildingType.Structure,
        BuildingType.Furniture,
        BuildingType.Machine,
        BuildingType.Other,
    };
    private BuildingType CurType
    {
        get { return curType; }
        set
        {
            if (curType != value)
            {
                curType = value;
                UpdateBuildingList();
            }
        }
    }
    private BuildingType curType = BuildingType.All;
    /// <summary>
    /// 上一类别按钮点击
    /// </summary>
    private void ClickNextTypeBtn()
    {
        if (curTypeIndex < buildingTypes_Config.Count - 1)
        {
            curTypeIndex++;
        }
        else
        {
            curTypeIndex = 0;
        }
        CurType = buildingTypes_Config[curTypeIndex];
    }
    /// <summary>
    /// 下一类别按钮点击
    /// </summary>
    private void ClickLastTypeBtn()
    {
        if (curTypeIndex > 0)
        {
            curTypeIndex--;
        }
        else
        {
            curTypeIndex = buildingTypes_Config.Count - 1;
        }
        CurType = buildingTypes_Config[curTypeIndex];
    }

    #endregion
    #region//建筑时代
    [Header("上一时代")]
    public Button button_LastAge;
    [Header("下一时代")]
    public Button button_NextAge;
    [Header("时代")]
    public LocalizeStringEvent localizeStringEvent_Age;
    private int curAgeIndex = 0;
    private List<AgeGroup> buildingAges_Config = new List<AgeGroup>()
    {
        AgeGroup.StoneAge,
        AgeGroup.IronAge,
    };
    private AgeGroup CurAge
    {
        get { return curAge; }
        set
        {
            curAge = value;
            UpdateBuildingList();
        }
    }
    private AgeGroup curAge = AgeGroup.StoneAge;

    /// <summary>
    /// 上一时代按钮点击
    /// </summary>
    private void ClickNextAgeBtn()
    {
        if (curAgeIndex < buildingAges_Config.Count - 1)
        {
            curAgeIndex++;
        }
        else
        {
            curAgeIndex = 0;
        }
        curTypeIndex = 0;
        CurType = buildingTypes_Config[curTypeIndex];
        CurAge = buildingAges_Config[curAgeIndex];
    }
    /// <summary>
    /// 下一时代按钮点击
    /// </summary>
    private void ClickLastAgeBtn()
    {
        if (curAgeIndex > 0)
        {
            curAgeIndex--;
        }
        else
        {
            curAgeIndex = buildingAges_Config.Count - 1;
        }
        curTypeIndex = 0;
        CurType = buildingTypes_Config[curTypeIndex];
        CurAge = buildingAges_Config[curAgeIndex];
    }
    #endregion
    #region//列表页数
    [SerializeField, Header("上一页")]
    private Button button_LastPage;
    [SerializeField, Header("下一页")]
    private Button button_NextPage;
    [SerializeField, Header("页数")]
    private Text textMeshProUGUI_Page;
    private int CurPage
    {
        get { return curPage; }
        set
        {
            curPage = value;
        }
    }
    private int curPage;
    /// <summary>
    /// 上一页按钮点击
    /// </summary>
    private void ClickNextPageBtn()
    {
        if (CurPage * buttons_Icon.Count < buildingConfigs_TempList.Count)
        {
            CurPage++;
        }
        else
        {
            CurPage = 0;
        }
        DrawBuildingList();
    }
    /// <summary>
    /// 下一页按钮点击
    /// </summary>
    private void ClickLastPageBtn()
    {
        if (CurPage > 0)
        {
            CurPage--;
            DrawBuildingList();
        }
    }

    #endregion
    #region//列表UI
    [SerializeField]
    private List<Button> buttons_Icon;
    [SerializeField]
    private List<Image> image_Icon;
    [SerializeField]
    private SpriteAtlas spriteAtlas_BuildingIcon;
    [SerializeField]
    private SpriteAtlas spriteAtlas_GroundIcon;
    private void UpdateBuildingList()
    {
        localizeStringEvent_Type.StringReference.SetReference("Building_String", "BuildingType_" + (int)curType);
        localizeStringEvent_Age.StringReference.SetReference("Building_String", "BuildingAge_" + (int)curAge);
        groundConfigs_TempList = GroundConfigData.groundConfigs.FindAll((x) =>
        {
            return x.Ground_Age == CurAge;
        });
        if (CurType == BuildingType.All)
        {
            buildingConfigs_TempList = BuildingConfigData.buildConfigs.FindAll((x) =>
            {
                return x.Building_Age == CurAge && x.Building_ID <= 9999;
            });
        }
        else
        {
            buildingConfigs_TempList = BuildingConfigData.buildConfigs.FindAll((x) =>
            {
                return x.Building_Type == CurType && x.Building_Age == CurAge && x.Building_ID <= 9999;
            });
        }
        CurPage = 0;
        DrawBuildingList();
    }
    private void DrawBuildingList()
    {
        textMeshProUGUI_Page.text = CurPage.ToString();
        if (index >= 0)
        {
            if (CurType == BuildingType.Ground)
            {
                DrawBuildingRaw(buildingConfig_Temp, index);
            }
            else
            {
                DrawBuildingRaw(groundConfig_Temp, index);
            }
        }
        for (int i = 0; i < buttons_Icon.Count; i++)
        {
            buttons_Icon[i].onClick.RemoveAllListeners();
            if (CurType == BuildingType.Ground)
            {
                if (i + CurPage * buttons_Icon.Count < groundConfigs_TempList.Count)
                {
                    GroundConfig groundConfig = groundConfigs_TempList[i + CurPage * buttons_Icon.Count];
                    int index = i;
                    buttons_Icon[i].gameObject.SetActive(true);
                    buttons_Icon[i].onClick.AddListener(() => { ClickBuildingBtn(groundConfig, index); });
                    image_Icon[i].sprite = spriteAtlas_GroundIcon.GetSprite(groundConfig.Ground_ID.ToString());
                    if (CheckBuildingRaw(groundConfig))
                    {
                        image_Icon[i].color = Color.white;
                    }
                    else
                    {
                        image_Icon[i].color = Color.red;
                    }
                }
                else
                {
                    buttons_Icon[i].gameObject.SetActive(false);
                    image_Icon[i].sprite = null;
                }
            }
            else
            {
                if (i + CurPage * buttons_Icon.Count < buildingConfigs_TempList.Count)
                {
                    BuildingConfig buildingConfig = buildingConfigs_TempList[i + CurPage * buttons_Icon.Count];
                    int index = i;
                    buttons_Icon[i].gameObject.SetActive(true);
                    buttons_Icon[i].onClick.AddListener(() => { ClickBuildingBtn(buildingConfig, index); });
                    image_Icon[i].sprite = spriteAtlas_BuildingIcon.GetSprite(buildingConfig.Building_ID.ToString());
                    if (CheckBuildingRaw(buildingConfig))
                    {
                        image_Icon[i].color = Color.white;
                    }
                    else
                    {
                        image_Icon[i].color = Color.red;
                    }
                }
                else
                {
                    buttons_Icon[i].gameObject.SetActive(false);
                    image_Icon[i].sprite = null;
                }
            }
        }
    }
    private void ClickBuildingBtn(BuildingConfig buildingConfig, int index)
    {
        DrawBuildingRaw(buildingConfig, index);
        MapPreviewManager.Instance.Init(buildingConfig, MapPreviewManager.BuildState.TryBuildBuilding);
    }
    private void ClickBuildingBtn(GroundConfig groundConfig, int index)
    {
        DrawBuildingRaw(groundConfig, index);
        MapPreviewManager.Instance.Init(groundConfig, MapPreviewManager.BuildState.TryBuildFloor);
    }

    #endregion
    #region//原料UI
    [SerializeField, Header("原料面板信息")]
    private TextMeshProUGUI text_TargetName;
    [SerializeField, Header("原料列表")]
    private List<UI_ItemCell> itemCells_TargetRawList = new List<UI_ItemCell>();
    [SerializeField]
    private SpriteAtlas spriteAtlas_Item;
    [SerializeField]
    private SpriteAtlas spriteAtlas_ItemBG;
    private BuildingConfig buildingConfig_Temp;
    private GroundConfig groundConfig_Temp;
    private int index = -1;
    private void DrawBuildingRaw(BuildingConfig config, int index)
    {
        buildingConfig_Temp = config;
        this.index = index;
        for (int i = 0; i < itemCells_TargetRawList.Count; i++)
        {
            itemCells_TargetRawList[i].CleanCell();
        }
        if (config.Building_ID > 0 && buildingConfigs_TempList.Count > index && buildingConfigs_TempList[index + CurPage * buttons_Icon.Count].Equals(config))
        {
            text_TargetName.text = LocalizationManager.Instance.GetLocalization("Building_String", config.Building_ID + "_Name"); 
            CheckBuildingRaw(config);
        }
        else
        {
            text_TargetName.text = "";
            for (int i = 0; i < itemCells_TargetRawList.Count; i++)
            {
                itemCells_TargetRawList[i].CleanCell();
            }
        }
    }
    private bool CheckBuildingRaw(BuildingConfig config)
    {
        bool temp = true;
        List<ItemData> itemDatas;
        if (GameLocalManager.Instance.playerCoreLocal != null)
        {
            itemDatas = GameLocalManager.Instance.playerCoreLocal.actorManager_Bind.actorNetManager.Local_GetBagItem();
        }
        else
        {
            itemDatas = new List<ItemData>();
        }
        for (int i = 0; i < config.Building_Raw.Count; i++)
        {
            ItemConfig itemConfig = ItemConfigData.GetItemConfig(config.Building_Raw[i].ID);
            int itemCount = 0;
            for (int j = 0; j < itemDatas.Count; j++)
            {
                if (itemDatas[j].Item_ID == config.Building_Raw[i].ID)
                {
                    itemCount += itemDatas[j].Item_Count;
                }
            }
            string info = itemCount.ToString() + "/" + config.Building_Raw[i].Count.ToString();
            Sprite sprite_Icon = spriteAtlas_Item.GetSprite("Item_" + itemConfig.Item_ID.ToString());
            Sprite sprite_BG = spriteAtlas_ItemBG.GetSprite("ItemBG_" + (int)itemConfig.Item_Rarity);
            string itemName = LocalizationManager.Instance.GetLocalization("Item_String", itemConfig.Item_ID + "_Name");
            string itemDesc = LocalizationManager.Instance.GetLocalization("Item_String", itemConfig.Item_ID + "_Desc");
            itemCells_TargetRawList[i].DrawCellInfo(info, itemName, itemDesc, itemConfig.Item_Rarity);
            if (itemCount < config.Building_Raw[i].Count)
            {
                temp = false;
                itemCells_TargetRawList[i].DrawCellIcon(sprite_Icon, Color.red, sprite_BG, Color.red);
            }
            else
            {
                temp = true;
                itemCells_TargetRawList[i].DrawCellIcon(sprite_Icon, Color.white, sprite_BG, Color.white);
            }
        }
        return temp;
    }
    private void DrawBuildingRaw(GroundConfig config, int index)
    {
        Debug.Log(index + "/" + groundConfigs_TempList.Count + "//" + config.Ground_ID);
        groundConfig_Temp = config;
        this.index = index;
        for (int i = 0; i < itemCells_TargetRawList.Count; i++)
        {
            itemCells_TargetRawList[i].CleanCell();
        }
        if (config.Ground_ID > 0 && groundConfigs_TempList.Count > index && groundConfigs_TempList[index + CurPage * buttons_Icon.Count].Equals(config))
        {
            text_TargetName.text = LocalizationManager.Instance.GetLocalization("Ground_String", config.Ground_ID + "_Name");
            CheckBuildingRaw(config);
        }
        else
        {
            text_TargetName.text = "";
        }
    }
    private bool CheckBuildingRaw(GroundConfig config)
    {
        bool temp = true;
        List<ItemData> itemDatas = GameLocalManager.Instance.playerCoreLocal.actorManager_Bind.actorNetManager.Local_GetBagItem();
        for (int i = 0; i < config.Ground_Raw.Count; i++)
        {
            ItemConfig itemConfig = ItemConfigData.GetItemConfig(config.Ground_Raw[i].ID);
            int itemCount = 0;
            for (int j = 0; j < itemDatas.Count; j++)
            {
                if (itemDatas[j].Item_ID == config.Ground_Raw[i].ID)
                {
                    itemCount += itemDatas[j].Item_Count;
                }
            }
            string info = itemCount.ToString() + "/" + config.Ground_Raw[i].Count.ToString();
            Sprite sprite_Icon = spriteAtlas_Item.GetSprite("Item_" + itemConfig.Item_ID.ToString());
            Sprite sprite_BG = spriteAtlas_ItemBG.GetSprite("ItemBG_" + (int)itemConfig.Item_Rarity);
            if (itemCount < config.Ground_Raw[i].Count)
            {
                temp = false;
                string itemName = LocalizationManager.Instance.GetLocalization("Item_String", itemConfig.Item_ID + "_Name");
                string itemDesc = LocalizationManager.Instance.GetLocalization("Item_String", itemConfig.Item_ID + "_Desc");
                itemCells_TargetRawList[i].DrawCellIcon(sprite_Icon, Color.red, sprite_BG, Color.red);
                itemCells_TargetRawList[i].DrawCellInfo(info, itemName, itemDesc, itemConfig.Item_Rarity);
            }
            else
            {
                string itemName = LocalizationManager.Instance.GetLocalization("Item_String", itemConfig.Item_ID + "_Name");
                string itemDesc = LocalizationManager.Instance.GetLocalization("Item_String", itemConfig.Item_ID + "_Desc");
                itemCells_TargetRawList[i].DrawCellIcon(sprite_Icon, Color.white, sprite_BG, Color.white);
                itemCells_TargetRawList[i].DrawCellInfo(info, itemName, itemDesc, itemConfig.Item_Rarity);
            }
        }
        return temp;
    }
    #endregion
}
