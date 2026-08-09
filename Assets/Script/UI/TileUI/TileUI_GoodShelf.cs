using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class TileUI_GoodShelf : TileUI
{
    [Header("¸ñ×ÓÃæ°å")]
    public Transform transform_Panel;
    public List<TileUI_GoodShelf_GoodCell> list_GoodCells = new List<TileUI_GoodShelf_GoodCell>();
    protected BuildingObj_GoodShelf buildingObj_Bind;

    private void Awake()
    {
        BindAllCell();
    }
    public override void Show()
    {
        transform_Panel.DOKill();
        transform_Panel.localScale = Vector3.one;
        transform_Panel.DOPunchScale(new Vector3(0.1f, -0.1f, 0), 0.1f);
        base.Show();
    }
    public override void Hide()
    {
        buildingObj_Bind.OpenOrCloseAwakeUI(false);
        base.Hide();
    }
    public void BindBuilding(BuildingObj_GoodShelf buildingObj)
    {
        buildingObj_Bind = buildingObj;
        buildingObj_Bind.OpenOrCloseAwakeUI(true);
    }
    public virtual void BindAllCell()
    {
        for (int i = 0; i < list_GoodCells.Count; i++)
        {
            int index = i;
            list_GoodCells[i].BindGrid(new ItemPath(ItemFrom.Default, index), null, null, null, null);
            list_GoodCells[i].BindUI(this);
        }
    }
    public void DrawEveryCell()
    {
        buildingObj_Bind.buildingData_GoodShelf.ReadItemDataList(out var itemDatas_GoodShelf);
        for (int i = 0; i < list_GoodCells.Count; i++)
        {
            if (i < itemDatas_GoodShelf.Count)
            {
                list_GoodCells[i].Init(itemDatas_GoodShelf[i]);
                list_GoodCells[i].gameObject.SetActive(true); ;
            }
            else
            {
                list_GoodCells[i].Clean();
                list_GoodCells[i].gameObject.SetActive(false);
            }
        }
    }
    public void GoodsPutIn(ItemData itemData, ItemPath path)
    {
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemBag_Add()
        {
            itemData = itemData,
            itemFrom = ItemFrom.OutSide
        });
    }
    public ItemData GoodsPutOut(ItemData itemData_From, ItemData itemData_Out, ItemPath itemPath)
    {
        return new ItemData();
    }
    public void GoodPutOut(ItemData itemData)
    {
        buildingObj_Bind.buildingData_GoodShelf.ReadItemDataList(out var oldList);
        buildingObj_Bind.buildingData_GoodShelf.WriteItemDataList(GameToolManager.Instance.PutOutItemList(oldList, itemData));
        buildingObj_Bind.All_TryToPush();
    }
}
