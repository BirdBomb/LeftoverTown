using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class TileUI_Blender : TileUI
{
    [SerializeField, Header("格子面板")]
    private Transform transform_Panel;
    [SerializeField, Header("加工前")]
    private UI_GridCell gridCell_From;
    [SerializeField, Header("加工后")]
    private UI_GridCell gridCell_To;
    [SerializeField, Header("加工按钮")]
    private Button btn_Blender;
    private BuildingObj_Machine_Blender buildingObj_Bind;
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
    public void BindBuilding(BuildingObj_Machine_Blender buildingObj)
    {
        buildingObj_Bind = buildingObj;
        buildingObj_Bind.OpenOrCloseAwakeUI(true);
    }

    public void BindAllCell()
    {
        gridCell_From.BindGrid(new ItemPath(ItemFrom.Default, 0), FromPutIn, FromPutOut, null, null);
        gridCell_To.BindGrid(new ItemPath(ItemFrom.Default, 0), ToPutIn, ToPutOut, null, null);
        btn_Blender.onClick.AddListener(ClickBlenderBtn);
    }
    public void DrawAllCell()
    {
        buildingObj_Bind.buildingData_Blender.ReadItemDataFrom(out ItemData itemData_From);
        buildingObj_Bind.buildingData_Blender.ReadItemDataTo(out ItemData itemData_To);
        gridCell_From.UpdateData(itemData_From);
        gridCell_To.UpdateData(itemData_To);
    }
    #region/取出放入
    public void FromPutIn(ItemData addData, ItemPath path)
    {
        buildingObj_Bind.buildingData_Blender.ReadItemDataFrom(out ItemData itemData_From);
        BlenderConfig blenderConfig = BlenderConfigData.GetBlenderConfig(addData.I);
        if (blenderConfig.blender_FromID != 0)
        {
            if (itemData_From.I == 0)
            {
                itemData_From = addData;
            }
            else if (addData.I == itemData_From.I)
            {
                itemData_From = GameToolManager.Instance.CombineItem(itemData_From, addData, out ItemData res);
                MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemBag_Add()
                {
                    itemData = res,
                    itemFrom = ItemFrom.OutSide
                });
            }
            else
            {
                MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemBag_Add()
                {
                    itemData = addData,
                    itemFrom = ItemFrom.OutSide
                });
            }
        }
        else
        {
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemBag_Add()
            {
                itemData = addData,
                itemFrom = ItemFrom.OutSide
            });
        }
        buildingObj_Bind.buildingData_Blender.WriteItemDataFrom(itemData_From);
        buildingObj_Bind.All_TryToPush();
    }
    public ItemData FromPutOut(ItemData itemData_From, ItemData itemData_Out, ItemPath itemPath)
    {
        ItemData itemData = GameToolManager.Instance.SplitItem(itemData_From, itemData_Out);
        buildingObj_Bind.buildingData_Blender.WriteItemDataFrom(itemData);
        buildingObj_Bind.All_TryToPush();
        return itemData_Out;
    }
    public void ToPutIn(ItemData addData, ItemPath path)
    {
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemBag_Add()
        {
            itemData = addData,
            itemFrom = ItemFrom.OutSide
        });
    }
    public ItemData ToPutOut(ItemData itemData_From, ItemData itemData_Out, ItemPath itemPath)
    {
        ItemData itemData = GameToolManager.Instance.SplitItem(itemData_From, itemData_Out);
        buildingObj_Bind.buildingData_Blender.WriteItemDataTo(itemData);
        buildingObj_Bind.All_TryToPush();
        return itemData_Out;
    }
    #endregion
    #region//加工
    private void ClickBlenderBtn()
    {
        buildingObj_Bind.Local_Blender();
    }
    #endregion
}
