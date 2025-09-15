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
        gridCell_From.UpdateData(buildingObj_Bind.itemData_From);
        gridCell_To.UpdateData(buildingObj_Bind.itemData_To);
    }
    #region/取出放入
    public void FromPutIn(ItemData addData, ItemPath path)
    {
        BlenderConfig blenderConfig = BlenderConfigData.GetBlenderConfig(addData.Item_ID);
        if (blenderConfig.blender_FromID != 0)
        {
            if (buildingObj_Bind.itemData_From.Item_ID == 0)
            {
                buildingObj_Bind.itemData_From = addData;
            }
            else if (addData.Item_ID == buildingObj_Bind.itemData_From.Item_ID)
            {
                buildingObj_Bind.itemData_From = GameToolManager.Instance.CombineItem(buildingObj_Bind.itemData_From, addData, out ItemData res);
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
        buildingObj_Bind.WriteInfo();
    }
    public ItemData FromPutOut(ItemData itemData_From, ItemData itemData_Out, ItemPath itemPath)
    {
        buildingObj_Bind.itemData_From = GameToolManager.Instance.SplitItem(itemData_From, itemData_Out);
        buildingObj_Bind.WriteInfo();
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
        buildingObj_Bind.itemData_To = GameToolManager.Instance.SplitItem(itemData_From, itemData_Out);
        buildingObj_Bind.WriteInfo();
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
