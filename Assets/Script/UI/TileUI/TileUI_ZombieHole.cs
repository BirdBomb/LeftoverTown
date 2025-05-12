using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class TileUI_ZombieHole : TileUI
{
    [SerializeField, Header("格子面板")]
    private Transform transform_Panel;
    [SerializeField, Header("格子列表")]
    private UI_GridCell gridCell_Food;
    [SerializeField, Header("信息")]
    private Text text_Info;
    private BuildingObj_ZombieHole buildingObj_Bind;

    private void Awake()
    {
        BindAllCell();
    }
    public void BindBuilding(BuildingObj_ZombieHole buildingObj)
    {
        buildingObj_Bind = buildingObj;
        transform_Panel.DOKill();
        transform_Panel.localScale = Vector3.one;
        transform_Panel.DOPunchScale(new Vector3(0.1f, -0.1f, 0), 0.1f);
        DrawInfo();
    }
    private void BindAllCell()
    {
        gridCell_Food.BindGrid(new ItemPath(ItemFrom.Default, 0), PutIn, PutOut, null, null);
    }
    public void DrawInfo()
    {
        if (buildingObj_Bind.gameTime_CurTimeSign >= buildingObj_Bind.gameTime_UseableTime)
        {
            text_Info.text = "被封住的地穴,下面传来明显不是人类的呼吸声，或许一些气味浓郁的肉会吸引那个生物";
        }
        else
        {
            text_Info.text = "已经不再传出奇怪声音的地穴,取而代之的微弱的电流声?";
        }
    } 
    public void PutIn(ItemData addData, ItemPath path)
    {
        if (addData.Item_ID == buildingObj_Bind.itemData_ID && buildingObj_Bind.gameTime_CurTimeSign >= buildingObj_Bind.gameTime_UseableTime)
        {
            ItemData putIn = addData;
            putIn.Item_Count = 1;
            ItemData resData = GameToolManager.Instance.SplitItem(addData, putIn);
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryAddItemInBag()
            {
                itemData = resData,
            });
            MessageBroker.Default.Publish(new GameEvent.GameEvent_Local_SpawnActor()
            {
                name = "Actor/Zombie_Hook",
                pos = buildingObj_Bind.transform.position,
            });
            buildingObj_Bind.WriteInfo();
        }
        else
        {
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryAddItemInBag()
            {
                itemData = addData,
            });
        }
    }
    public ItemData PutOut(ItemData itemData_From, ItemData itemData_Out, ItemPath itemPath)
    {
        return itemData_Out;
    }
}
