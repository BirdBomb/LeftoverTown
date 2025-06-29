using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class TileUI_Dictionary : TileUI
{
    public Transform transform_Panel;
    public List<UI_ItemCell> itemCell_List = new List<UI_ItemCell>();
    public UI_GridCell gridCell_Check;
    public Button btn_BuildingLeftBtn;
    public Button btn_BuildingRightBtn;
    public Image image_BuildingIcon;
    public TextMeshProUGUI text_BuildingName;
    public SpriteAtlas spriteAtlas_BuildingIcon;
    public SpriteAtlas spriteAtlas_ItemIcon;
    public SpriteAtlas spriteAtlas_ItemBG;
    private ItemData itemData_Check;
    private int index_Machine = 0;
    private int id_Machine = 0;
    private List<int> itemIDs_List = new List<int>();
    private void Awake()
    {
        BindAllCell();
    }
    public void Init()
    {
        transform_Panel.gameObject.SetActive(true);
        transform_Panel.localScale = Vector3.one;
        transform_Panel.DOPunchScale(new Vector3(0.1f, -0.1f, 0), 0.1f);
        UpdateMachine();
    }
    private void BindAllCell()
    {
        gridCell_Check.BindGrid(new ItemPath(ItemFrom.Default, 0), CheckPutIn, CheckPutOut, null, null);
        btn_BuildingLeftBtn.onClick.AddListener(ClickLeftBtn);
        btn_BuildingRightBtn.onClick.AddListener(ClickRightBtn);
    }
    #region//更改原料
    public void CheckPutIn(ItemData itemData, ItemPath path)
    {
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryAddItemInBag()
        {
            itemData = itemData,
        });
        itemData_Check = itemData;
        DrawSellCell();
        GetAllItem();
    }
    public ItemData CheckPutOut(ItemData itemData_From, ItemData itemData, ItemPath itemPath)
    {
        DrawSellCell();
        return new ItemData();
    }
    private void DrawSellCell()
    {
        if (itemData_Check.Item_ID > 0)
        {
            gridCell_Check.UpdateData(itemData_Check);
        }
        else
        {
            gridCell_Check.CleanItemBase();
        }
    }

    #endregion
    #region//更改机器
    private void ClickLeftBtn()
    {
        if(index_Machine == 0)
        {
            index_Machine =  CreateListConfigData.createListConfigs.Count - 1;
        }
        else
        {
            index_Machine--;
        }
        UpdateMachine();
    }
    private void ClickRightBtn()
    {
        if(index_Machine == CreateListConfigData.createListConfigs.Count - 1)
        {
            index_Machine = 0;
        }
        else
        {
            index_Machine++;
        }
        UpdateMachine();
    }
    private void UpdateMachine()
    {
        id_Machine = CreateListConfigData.createListConfigs[index_Machine].ID;
        image_BuildingIcon.sprite = spriteAtlas_BuildingIcon.GetSprite(id_Machine.ToString());
        text_BuildingName.text = LocalizationManager.Instance.GetLocalization("Building_String", id_Machine + "_Name");
        GetAllItem();
    }
    #endregion
    #region//绘制所有合成物体
    private void GetAllItem()
    {
        itemIDs_List.Clear();
        List<int> itemList = CreateListConfigData.GetCreateListConfig(id_Machine).List;
        Debug.Log(itemList.Count);
        for (int i = 0; i < itemList.Count; i++)
        {
            int temp = itemList[i];
            Debug.Log(temp);
            CreateRawConfig rawConfig = CreateRawConfigData.GetCreateRawConfig(temp);
            if (rawConfig.Create_RawList.Find((x) => { return x.ID == itemData_Check.Item_ID; }).ID != 0)
            {
                itemIDs_List.Add(rawConfig.Create_TargetID);
            }
        }
        DrawAllItem();
    }
    private void DrawAllItem()
    {
        for(int i = 0; i < itemCell_List.Count; i++)
        {
            itemCell_List[i].CleanCell();
        }
        for(int i = 0; i < itemIDs_List.Count; i++)
        {
            if(i<= itemCell_List.Count)
            {
                ItemConfig itemConfig = ItemConfigData.GetItemConfig(itemIDs_List[i]);
                Sprite icon = spriteAtlas_ItemIcon.GetSprite("Item_" + itemConfig.Item_ID.ToString());
                Sprite bg = spriteAtlas_ItemBG.GetSprite("ItemBG_" + (int)itemConfig.Item_Rarity);
                string itemName = LocalizationManager.Instance.GetLocalization("Item_String", itemConfig.Item_ID + "_Name");
                string itemDesc = LocalizationManager.Instance.GetLocalization("Item_String", itemConfig.Item_ID + "_Desc");
                itemCell_List[i].DrawCellIcon(icon, Color.gray, bg, Color.gray);
                itemCell_List[i].DrawCellInfo("", itemName, itemDesc, itemConfig.Item_Rarity);
            }
        }
    }
    #endregion
}
