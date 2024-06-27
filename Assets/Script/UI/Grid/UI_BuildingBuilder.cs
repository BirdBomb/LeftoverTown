using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.U2D;
using static Fusion.Allocator;
using UniRx;
using System.Text;
using UnityEngine.EventSystems;

public class UI_BuildingBuilder : UI_Grid
{
    [SerializeField, Header("名字")]
    private Text buildName;
    [SerializeField, Header("图片")]
    private Image buildSprite;
    [SerializeField, Header("合成栏图片")]
    private List<Image> buildRawSpriteList = new List<Image>();
    [SerializeField, Header("合成栏数量")]
    private List<Text> buildRawTextList = new List<Text>();
    [SerializeField, Header("左滚")]
    private Button leftBtn;
    [SerializeField, Header("右滚")]
    private Button rightBtn;
    [SerializeField, Header("准备建造")]
    private Button readyBuildBtn;
    [SerializeField, Header("开始建造")]
    private Button startBuildBtn;

    /// <summary>
    /// 是否准备开始建造
    /// </summary>
    private bool isReadyBuilding;
    /// <summary>
    /// 准备开始建造的建筑
    /// </summary>
    private BuildingConfig targetBuilding;

    private TileObj bindTileObj;
    private SpriteAtlas buildingAtlas;
    private SpriteAtlas itemAtlas;
    private int index = 0;
    private List<ItemData> rawConfigList = new List<ItemData>();
    private List<ItemData> itemDataList = new List<ItemData>();

    private void Awake()
    {
        buildingAtlas = Resources.Load<SpriteAtlas>("Atlas/TileSprite");
        itemAtlas = Resources.Load<SpriteAtlas>("Atlas/ItemSprite");
    }
    private void Start()
    {
        leftBtn.onClick.AddListener(ClickLeftBtn);
        rightBtn.onClick.AddListener(ClickRightBtn);
        readyBuildBtn.onClick.AddListener(ClickReadyBuildBtn);
        startBuildBtn.onClick.AddListener(ClickStartBuildBtn);
        targetBuilding = BuildingConfigData.GetItemConfig(1001);
        gameObject.SetActive(false);
    }
    public override void Open(TileObj tileObj)
    {
        bindTileObj = tileObj;
        base.Open(tileObj);
    }
    public override void Close(TileObj tileObj)
    {
        base.Close(tileObj);
    }
    public void ClickCellLeft(ItemData itemData)
    {

    }
    public void ClickCellRight(ItemData itemData)
    {

    }
    public override void CellDragBegin(UI_GridCell gridCell, ItemData itemData, PointerEventData pointerEventData)
    {

    }
    public override void CellDragIn(UI_GridCell gridCell, ItemData itemData, PointerEventData pointerEventData)
    {
        RectTransformUtility.ScreenPointToWorldPointInRectangle(gridCell.image_Icon.rectTransform, Input.mousePosition, Camera.main, out Vector3 pos);
        gridCell.image_Icon.transform.position = pos;
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
                    PutOut(itemData);
                    grid.ListenDragOn(this, gridCell, itemData);
                    return;
                }
            }
        }
        else
        {
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryDropItem()
            {
                item = itemData
            });
            PutOut(itemData);
        }

    }
    public override void ListenDragOn<T>(T grid, UI_GridCell cell, ItemData itemData)
    {
        Calculate(itemData, out ItemData back);
        if (back.Item_Count != itemData.Item_Count)
        {
            PutIn(itemData);
        }
        if (back.Item_Count != 0)
        {
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryAddItemInBag()
            {
                item = back,
            });
        }
        base.ListenDragOn<T>(grid, cell, itemData);
    }

    public override void PutIn(ItemData itemData)
    {
        int index = -1;
        int add = 0;
        for (int i = 0; i < itemDataList.Count; i++)
        {
            if (itemDataList[i].Item_ID == itemData.Item_ID)
            {
                /*背包里面有同名物体*/
                ItemConfig item = ItemConfigData.GetItemConfig(itemData.Item_ID);
                int maxCount = item.Item_MaxCount;
                if (itemData.Item_Count + itemDataList[i].Item_Count <= maxCount)
                {
                    /*背包里面有同名物体,且可以叠加*/
                    index = i;
                    add = itemData.Item_Count;
                }
                else
                {
                    /*背包里面有同名物体,不可叠加*/
                    index = i;
                    add = maxCount - itemDataList[i].Item_Count;
                }
            }
        }
        if (index == -1)
        {
            /*背包里面没有同名物体,查看是否需要*/
            int temp_b = -1;
            temp_b = rawConfigList.FindIndex((x) => { return x.Item_ID == itemData.Item_ID; });
            if (temp_b >= 0)/*我需要这个物体*/
            {
                ItemData rawItemData = rawConfigList[temp_b];
                int maxCount = rawItemData.Item_Count;
                if (itemData.Item_Count <= maxCount)
                {
                    /*可以全部放入*/
                    add = itemData.Item_Count;
                    itemData.Item_Count -= add;
                }
                else
                {
                    /*可以部分放入*/
                    add = maxCount;
                    itemData.Item_Count -= add;
                }
                ItemData newItemData = new ItemData();
                newItemData.Item_ID = itemData.Item_ID;
                newItemData.Item_Seed = itemData.Item_Seed;
                newItemData.Item_Count = add;
                itemDataList.Add(newItemData);
            }
            else/*我不需要这个物体*/
            {

            }
        }
        else
        {
            ItemData targetItem = itemDataList[index];
            targetItem.Item_Count += add;
            itemDataList[index] = targetItem;
        }
        ChangeInfoToTile();

        base.PutIn(itemData);
    }
    public override void PutOut(ItemData data)
    {
        itemDataList.Remove(data);
        ChangeInfoToTile();
        base.PutOut(data);
    }
    public override void Calculate(ItemData before, out ItemData after)
    {
        if (isReadyBuilding)
        {
            int temp_a = -1;
            int add = 0;

            for (int i = 0; i < itemDataList.Count; i++)
            {
                if (itemDataList[i].Item_ID == before.Item_ID)
                {
                    /*存放列表里面有同名物体,查询需要的物体上限*/
                    ItemData rawItemData = rawConfigList.Find((x) => { return x.Item_ID == before.Item_ID; });
                    int maxCount = rawItemData.Item_Count;
                    if (before.Item_Count + itemDataList[i].Item_Count <= maxCount)
                    {
                        /*背包里面有同名物体,且未达上限*/
                        temp_a = i;
                        add = before.Item_Count;
                    }
                    else
                    {
                        /*背包里面有同名物体,不可再叠加*/
                        temp_a = i;
                        add = maxCount - itemDataList[i].Item_Count;
                    }
                }
            }

            if (temp_a == -1)
            {
                /*背包里面没有同名物体,查看是否需要*/
                int temp_b = -1;
                temp_b = rawConfigList.FindIndex((x) => { return x.Item_ID == before.Item_ID; });
                if (temp_b >= 0)/*我需要这个物体*/
                {
                    ItemData rawItemData = rawConfigList[temp_b];
                    int maxCount = rawItemData.Item_Count;
                    if (before.Item_Count <= maxCount)
                    {
                        /*可以全部放入*/
                        add = before.Item_Count;
                        before.Item_Count -= add;
                    }
                    else
                    {
                        /*可以部分放入*/
                        add = maxCount;
                        before.Item_Count -= add;
                    }
                }
                else/*我不需要这个物体*/
                {

                }
            }
            else
            {
                before.Item_Count -= add;
            }
            after = before;
        }
        else
        {
            base.Calculate(before, out after);
        }
    }
    public void UpdateInfoFromTile(string info)
    {
        itemDataList.Clear();
        isReadyBuilding = false;
        string[] strings = info.Split("/*I*/");
        for (int i = 0; i < strings.Length; i++)
        {
            if (i == 0)
            {
                /*第一位是待建建筑id*/
                if(strings[i] != "")
                {
                    isReadyBuilding = true;
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
        if(targetBuilding.Building_ID == 0) { targetBuilding = BuildingConfigData.GetItemConfig(1001); }
        ResetBuildUI();
        UpdateBuildUI();
        CheckRawList();
    }
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
    private void CheckRawList()
    {
        if (isReadyBuilding)
        {
            for (int i = 0; i < rawConfigList.Count; i++)
            {
                int temp = -1;
                temp = itemDataList.FindIndex((x) => { return x.Item_ID == rawConfigList[i].Item_ID; });
                if (temp >= 0)
                {
                    if (itemDataList[temp].Item_Count >= rawConfigList[i].Item_Count)
                    {

                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    return;
                }
            }
            startBuildBtn.gameObject.SetActive(true);
        }
    }
    private void ResetBuildUI()
    {
        for (int i = 0; i < buildRawSpriteList.Count; i++)
        {
            buildRawSpriteList[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < buildRawTextList.Count; i++)
        {
            buildRawTextList[i].text = "";
        }
    }
    private void UpdateBuildUI()
    {
        if (isReadyBuilding)
        {
            leftBtn.gameObject.SetActive(false);
            rightBtn.gameObject.SetActive(false);
            readyBuildBtn.gameObject.SetActive(false);

            DrawBuildingSprite(targetBuilding);
            UpdateBuildRawList(targetBuilding);
            DrawBuildingRawList();
        }
        else
        {
            leftBtn.gameObject.SetActive(true);
            rightBtn.gameObject.SetActive(true);
            readyBuildBtn.gameObject.SetActive(true);

            DrawBuildingSprite(targetBuilding);
            UpdateBuildRawList(targetBuilding);
            DrawBuildingRawList();
        }
    }
    /// <summary>
    /// 绘制建筑预览
    /// </summary>
    private void DrawBuildingSprite(BuildingConfig config)
    {
        if (config.Building_ID > 0)
        {
            //Debug.Log(config.Building_SpriteName);
            //if (buildingAtlas == null) 
            //{
            //    buildingAtlas = Resources.Load<SpriteAtlas>("Atlas/TileSprite");
            //}
            buildSprite.sprite = buildingAtlas.GetSprite(config.Building_SpriteName);
            buildName.text = config.Building_Name;
        }
    }
    /// <summary>
    /// 更新建筑合成列表
    /// </summary>
    private void UpdateBuildRawList(BuildingConfig config)
    {
        rawConfigList.Clear();
        for (int i = 0; i < config.Building_RawList.Count; i++)
        {
            int temp = rawConfigList.FindIndex((x) => { return x.Item_ID == config.Building_RawList[i]; });
            if (temp >= 0)
            {
                ItemData rawItemData = new ItemData();
                rawItemData.Item_ID = config.Building_RawList[i];
                rawItemData.Item_Count = rawConfigList[temp].Item_Count + 1;
                rawConfigList[temp] = rawItemData;

            }
            else
            {
                ItemData rawItemData = new ItemData();
                rawItemData.Item_ID = config.Building_RawList[i];
                rawItemData.Item_Count = 1;
                rawConfigList.Add(rawItemData);
            }
        }

    }
    /// <summary>
    /// 绘制建筑合成列表
    /// </summary>
    private void DrawBuildingRawList()
    {
        if (!isReadyBuilding)
        {
            for (int i = 0; i < rawConfigList.Count; i++)
            {
                buildRawSpriteList[i].gameObject.SetActive(true);
                buildRawSpriteList[i].sprite = itemAtlas.GetSprite("Item_" + rawConfigList[i].Item_ID.ToString());
                buildRawTextList[i].text = "0/" + rawConfigList[i].Item_Count.ToString();
            }
        }
        else
        {
            for (int i = 0; i < rawConfigList.Count; i++)
            {
                buildRawSpriteList[i].gameObject.SetActive(true);
                buildRawSpriteList[i].sprite = itemAtlas.GetSprite("Item_" + rawConfigList[i].Item_ID.ToString());

                int tempIndex = itemDataList.FindIndex((x) => { return x.Item_ID == rawConfigList[i].Item_ID; });

                if (tempIndex >= 0)
                {
                    buildRawTextList[i].text = itemDataList[tempIndex].Item_Count.ToString() + "/" + rawConfigList[i].Item_Count.ToString();
                }
                else
                {
                    buildRawTextList[i].text = "0/" + rawConfigList[i].Item_Count.ToString();
                }
            }

        }
    }
    /// <summary>
    /// 上一页
    /// </summary>
    private void ClickLeftBtn()
    {
        if (index == 0)
        {
            index = BuildingConfigData.buildConfigs.Count - 1;
        }
        else
        {
            index--;
        }
        targetBuilding = BuildingConfigData.buildConfigs[index];
        ResetBuildUI();
        DrawBuildingSprite(targetBuilding);
        UpdateBuildRawList(targetBuilding);
        DrawBuildingRawList();
    }
    /// <summary>
    /// 下一页
    /// </summary>
    private void ClickRightBtn()
    {
        if (index == BuildingConfigData.buildConfigs.Count - 1)
        {
            index = 0;
        }
        else
        {
            index++;
        }
        targetBuilding = BuildingConfigData.buildConfigs[index];
        ResetBuildUI();
        DrawBuildingSprite(targetBuilding);
        UpdateBuildRawList(targetBuilding);
        DrawBuildingRawList();
    }
    private void ClickReadyBuildBtn()
    {
        isReadyBuilding = true;
        ChangeInfoToTile();
    }
    private void ClickStartBuildBtn()
    {
        MessageBroker.Default.Publish(new MapEvent.MapEvent_LocalTile_ChangeBuilding()
        {
            buildingName = targetBuilding.Building_FileName,
            buildingPos = bindTileObj.bindTile._posInCell
        }) ;
    }

}
