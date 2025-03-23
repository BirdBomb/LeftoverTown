using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class UI_Build : MonoBehaviour
{
    #region
    private List<BuildingConfig> buildingConfigs_TempList;
    private BuildingConfig buildingConfig_Target;
    private int CurPage
    {
        get { return curPage; }
        set
        {
            curPage = value;
        }
    }
    private int curPage;

    private int curTypeIndex = 0;
    private BuildingType CurType
    {
        get { return curType; }
        set
        {
            if (curType != value)
            {
                buildingConfigs_TempList = BuildingConfigData.buildConfigs.FindAll((x) =>
                {
                    return x.Building_Type == value && x.Building_ID <= 9999;
                });
                CurPage = 0;
                curType = value;
                textMeshProUGUI_Type.text = curType.ToString();
                UpdateBuildingListUI();
            }
        }
    }
    private BuildingType curType = BuildingType.Structure;


    [SerializeField, Header("上一页")]
    private Button button_LastPage;
    [SerializeField, Header("下一页")]
    private Button button_NextPage;
    [SerializeField, Header("页数")]
    private TextMeshProUGUI textMeshProUGUI_Page;
    [SerializeField, Header("上一类别")]
    private Button button_LastType;
    [SerializeField, Header("下一类别")]
    private Button button_NextType;
    [SerializeField, Header("类别")]
    private TextMeshProUGUI textMeshProUGUI_Type;
    private List<BuildingType> buildingTypes = 
        new List<BuildingType>() { BuildingType.Structure, BuildingType.Furniture, BuildingType.Machine };
    [SerializeField]
    private List<Button> buttons_Building;
    [SerializeField]
    private List<Image> image_Building;
    [SerializeField, Header("建筑预览_建筑面板")]
    private Transform transform_TargetBuildingPanel;
    [SerializeField, Header("建筑预览_建筑名字")]
    private TextMeshProUGUI text_TargetBuildingName;
    [SerializeField, Header("建筑预览_目标建筑原料")]
    private List<UI_ItemCell> itemCells_TargetBuildingRawList = new List<UI_ItemCell>();

    [SerializeField]
    private SpriteAtlas spriteAtlas_BuildingIcon;
    [SerializeField]
    private SpriteAtlas spriteAtlas_Item;
    [SerializeField]
    private SpriteAtlas spriteAtlas_ItemBG;
    public void Start()
    {
        button_LastPage.onClick.AddListener(ClickLastPageBtn);
        button_NextPage.onClick.AddListener(ClickNextPageBtn);

        button_LastType.onClick.AddListener(ClickLastTypeBtn);
        button_NextType.onClick.AddListener(ClickNextTypeBtn);
    }
    #region//UI交互
    /// <summary>
    /// 上一页按钮点击
    /// </summary>
    private void ClickNextPageBtn()
    {
        if (CurPage * buttons_Building.Count < buildingConfigs_TempList.Count)
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
    /// 上一类别按钮点击
    /// </summary>
    private void ClickNextTypeBtn()
    {
        if (curTypeIndex < buildingTypes.Count - 1)
        {
            curTypeIndex++;
        }
        else
        {
            curTypeIndex = 0;
        }
        CurType = buildingTypes[curTypeIndex];
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
            curTypeIndex = buildingTypes.Count - 1;
        }
        CurType = buildingTypes[curTypeIndex];
    }
    private void ClickBuildingBtn(BuildingConfig buildingConfig,int index)
    {
        buildingConfig_Target = buildingConfig;
        BuildingBuilderConfig buildingBuilderConfig = new BuildingBuilderConfig();
        buildingBuilderConfig.Config = buildingConfig;
        buildingBuilderConfig.Type = CurType;
        buildingBuilderConfig.Page = CurPage;
        DrawBuildingPanel(buildingConfig, index);
        MapPreviewManager.Instance.PreviewOpen(buildingConfig, MapPreviewManager.BuildState.TryBuildBuilding);
    }
    #endregion

    private void UpdateBuildingListUI()
    {
        textMeshProUGUI_Page.text = CurPage.ToString();
        for (int i = 0; i < buttons_Building.Count; i++)
        {
            buttons_Building[i].onClick.RemoveAllListeners();
            if (i + CurPage * buttons_Building.Count < buildingConfigs_TempList.Count)
            {
                BuildingConfig buildingConfig = buildingConfigs_TempList[i + CurPage * buttons_Building.Count];
                int index = i;
                buttons_Building[i].gameObject.SetActive(true);
                buttons_Building[i].onClick.AddListener(() => { ClickBuildingBtn(buildingConfig, index); });
                image_Building[i].sprite = spriteAtlas_BuildingIcon.GetSprite(buildingConfig.Building_ID.ToString());
            }
            else
            {
                buttons_Building[i].gameObject.SetActive(false);
                image_Building[i].sprite = null;
            }
        }
    }
    private void DrawBuildingPanel(BuildingConfig config,int index)
    {
        if (config.Building_ID > 0)
        {
            transform_TargetBuildingPanel.gameObject.SetActive(true);
            transform_TargetBuildingPanel.position = buttons_Building[index].transform.position;
            text_TargetBuildingName.text = config.Building_Name;
            CheckBuildingRaw(config);
        }
        else
        {
            transform_TargetBuildingPanel.gameObject.SetActive(false);
            text_TargetBuildingName.text = "";
            for (int i = 0; i < itemCells_TargetBuildingRawList.Count; i++)
            {
                itemCells_TargetBuildingRawList[i].Clean();
            }
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
            itemCells_TargetBuildingRawList[i].Draw(spriteAtlas_Item.GetSprite("Item_" + itemConfig.Item_ID.ToString()), spriteAtlas_ItemBG.GetSprite("ItemBG_" + itemConfig.ItemRarity), itemConfig.ItemRarity, info, itemConfig.Item_Name, itemConfig.Item_Desc);
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
