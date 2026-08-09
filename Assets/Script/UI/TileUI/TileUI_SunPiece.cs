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
    [Header("格子面板")]
    public Transform transform_Panel;
    [Header("格子列表")]
    public UI_GridCell gridCell_Food;
    public Text text_SunRange;
    [Header("信息")]
    public LocalizeStringEvent localizeStringEvent_Info ;
    public List<GameObject> gameObjects_LightCube = new List<GameObject>();
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
        DrawCell();
        CheckCell();
    }
    public void BindAllCell()
    {
        gridCell_Food.BindGrid(new ItemPath(ItemFrom.Default, 0), PutIn, PutOut, null, null);
    }
    public void DrawCell()
    {
        gridCell_Food.UpdateData(buildingObj_Bind.buildingData_SunPiece.ReadItemData());
    }
    public void CheckCell()
    {
        buildingObj_Bind.All_CheckSun(out short level, out short range);
        text_SunRange.text = range.ToString();
        localizeStringEvent_Info.StringReference.SetReference("BuildingInfo_String", $"SunPieceInfo_{level}");
        CubeLightOn(level);
    }
    public void CubeLightOn(int count)
    {
        for(int i = 0; i < gameObjects_LightCube.Count; i++)
        {
            gameObjects_LightCube[i].gameObject.SetActive((i < count));
        }
    }
    public void PutIn(ItemData addData, ItemPath path)
    {
        if (addData.I == 1016)
        {
            ItemData itemData = buildingObj_Bind.buildingData_SunPiece.ReadItemData();
            itemData = GameToolManager.Instance.CombineItem(itemData, addData, out ItemData resData);
            if (resData.I > 0 && resData.C != 0)
            {
                MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemBag_Add()
                {
                    itemData = resData,
                    itemFrom = ItemFrom.OutSide
                });

            }
            buildingObj_Bind.buildingData_SunPiece.WriteItemData(itemData);
            buildingObj_Bind.TryToPush();
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
    public ItemData PutOut(ItemData itemData_From, ItemData itemData_Out, ItemPath itemPath)
    {
        return new ItemData();
    }
}
