using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class TileUI_SunPiece : TileUI
{
    [SerializeField, Header("格子面板")]
    private Transform transform_Panel;
    [SerializeField, Header("格子列表")]
    private UI_GridCell gridCell_Food;
    [SerializeField, Header("信息")]
    private LocalizeStringEvent localizeStringEvent_Info ; 
    [SerializeField, Header("确定")]
    private Button btn_Sure;
    private BuildingObj_SunPiece buildingObj_Bind;

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

    public void BindBuilding(BuildingObj_SunPiece buildingObj)
    {
        buildingObj_Bind = buildingObj;
        buildingObj_Bind.OpenOrCloseAwakeUI(true);
        btn_Sure.onClick.AddListener(ClickSure);
        DrawInfo();
        DrawCell();
        CheckCell();
    }
    public void BindAllCell()
    {
        gridCell_Food.BindGrid(new ItemPath(ItemFrom.Default, 0), PutIn, PutOut, null, null);
    }
    public void DrawInfo()
    {
        localizeStringEvent_Info.StringReference.SetReference("BuildingInfo_String", "SunPieceInfo_" + buildingObj_Bind.info_Level.ToString());
    }
    public void DrawCell()
    {
        gridCell_Food.UpdateData(buildingObj_Bind.info_ItemData);
    }
    public void CheckCell()
    {
        btn_Sure.gameObject.SetActive(false);
        Debug.Log(buildingObj_Bind.info_ItemData.Item_ID);
        Debug.Log(buildingObj_Bind.info_Level);
        switch (buildingObj_Bind.info_Level)
        {
            case 0:
                if (buildingObj_Bind.info_ItemData.Item_ID == 1000)
                {
                    btn_Sure.gameObject.SetActive(true);
                }
                break;
            case 1:
                break;
            case 2:
                break;
        }
    }
    public void ClickSure()
    {
        switch (buildingObj_Bind.info_Level)
        {
            case 0:
                if (buildingObj_Bind.info_ItemData.Item_ID == 1000)
                {
                    buildingObj_Bind.info_ItemData = buildingObj_Bind.State_GetItemData(2010, 1);
                    buildingObj_Bind.info_Level = 1;
                    buildingObj_Bind.WriteInfo();
                }
                break;
            case 1:
                break;
            case 2:
                break;
        }
    }
    public void PutIn(ItemData addData, ItemPath path)
    {
        buildingObj_Bind.info_ItemData = GameToolManager.Instance.CombineItem(buildingObj_Bind.info_ItemData, addData, out ItemData resData);
        if (resData.Item_ID > 0 && resData.Item_Count != 0)
        {
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryAddItemInBag()
            {
                itemData = resData,
            });
        }
        buildingObj_Bind.WriteInfo();
    }
    public ItemData PutOut(ItemData itemData_From, ItemData itemData_Out, ItemPath itemPath)
    {
        buildingObj_Bind.info_ItemData = GameToolManager.Instance.SplitItem(itemData_From, itemData_Out);
        buildingObj_Bind.WriteInfo();
        return itemData_Out;
    }
}
