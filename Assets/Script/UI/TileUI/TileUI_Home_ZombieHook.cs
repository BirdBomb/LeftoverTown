using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class TileUI_Home_ZombieHook : TileUI
{
    [SerializeField, Header("�������")]
    private Transform transform_Panel;
    [SerializeField, Header("�����б�")]
    private UI_GridCell gridCell_Food;
    [SerializeField, Header("��Ϣ")]
    private Text text_Info;
    private BuildingObj_Home_ZombieHook buildingObj_Bind;

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
    public void BindBuilding(BuildingObj_Home_ZombieHook buildingObj)
    {
        buildingObj_Bind = buildingObj;
        buildingObj_Bind.OpenOrCloseAwakeUI(true);
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
            text_Info.text = "����ס�ĵ�Ѩ,���洫�����Բ�������ĺ�����������һЩ��ζŨ������������Ǹ�����";
        }
        else
        {
            text_Info.text = "�Ѿ����ٴ�����������ĵ�Ѩ,ȡ����֮��΢���ĵ�����?";
        }
    } 
    public void PutIn(ItemData addData, ItemPath path)
    {
        if (addData.Item_ID == buildingObj_Bind.itemData_ID && buildingObj_Bind.gameTime_CurTimeSign >= buildingObj_Bind.gameTime_UseableTime)
        {
            ItemData putIn = addData;
            putIn.Item_Count = 1;
            ItemData resData = GameToolManager.Instance.SplitItem(addData, putIn);
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemBag_Add()
            {
                itemData = resData,
            });
            MessageBroker.Default.Publish(new GameEvent.GameEvent_Local_SpawnActor()
            {
                name = "Actor/Zombie_Hook",
                pos = buildingObj_Bind.transform.position + Vector3.up * 2,
            });
            buildingObj_Bind.WriteInfo();
        }
        else
        {
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemBag_Add()
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
