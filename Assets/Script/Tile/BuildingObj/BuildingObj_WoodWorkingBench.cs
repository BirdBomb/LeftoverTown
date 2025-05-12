using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingObj_WoodWorkingBench : BuildingObj
{
    [SerializeField]
    private GameObject obj_Singal;
    [SerializeField]
    private GameObject prefab_UI;
    private TileUI tileUI_Bind = null;
    private bool bool_OpenUI = false;
    #region//瓦片交互
    public override void All_ActorInputKeycode(ActorManager actor, KeyCode code)
    {
        if (code == KeyCode.F)
        {
            OpenOrCloseSingal(bool_OpenUI);
            OpenOrCloseCreateUI(!bool_OpenUI);
        }
        base.All_ActorInputKeycode(actor, code);
    }
    private void OpenOrCloseSingal(bool open)
    {
        obj_Singal.transform.DOKill();
        if (open)
        {
            obj_Singal.SetActive(true);
            obj_Singal.transform.localScale = Vector3.one;
            obj_Singal.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
        }
        else
        {
            obj_Singal.transform.DOScale(Vector3.zero, 0.1f).OnComplete(() =>
            {
                obj_Singal.SetActive(false);
            });
        }
    }
    private void OpenOrCloseCreateUI(bool open)
    {
        if (open)
        {
            bool_OpenUI = true;
            UIManager.Instance.ShowTileUI(prefab_UI, out tileUI_Bind);
            CreateListConfig createListConfig = CreateListConfigData.GetCreateListConfig(1000);
            List<CreateRawConfig> createRawConfigs = new List<CreateRawConfig>();
            for(int i = 0; i < createListConfig.List.Count; i++)
            {
                createRawConfigs.Add(CreateRawConfigData.GetCreateRawConfig(createListConfig.List[i]));
            }
            tileUI_Bind.GetComponent<TileUI_CreateItem>().InitPool(createRawConfigs, createListConfig.Name);
        }
        else
        {
            bool_OpenUI = false;
            if (tileUI_Bind) UIManager.Instance.HideTileUI(tileUI_Bind);
        }
    }
    public override bool All_PlayerHolding(PlayerCoreLocal player)
    {
        /*靠近是我自己*/
        if (player.bool_Local)
        {
            OpenOrCloseSingal(true);
            return true;
        }
        return false;
    }
    public override bool All_PlayerRelease(PlayerCoreLocal player)
    {
        /*离开是我自己*/
        if (player.bool_Local)
        {
            OpenOrCloseSingal(false);
            OpenOrCloseCreateUI(false);
            return true;
        }
        return true;
    }
    #endregion
}
