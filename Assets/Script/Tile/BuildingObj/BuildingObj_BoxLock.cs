using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using WebSocketSharp;

public class BuildingObj_BoxLock : BuildingObj_Box
{
    #region 序列化字段
    public Transform trans_Root;
    public SpriteRenderer spriteRenderer_Top;
    public SpriteRenderer spriteRenderer_Lock;
    public Sprite sprite_TopClose;
    public Sprite sprite_TopOpen;
    public int int_UnLockPro;
    #endregion
    private Coroutine coroutine_Unlock;
    private System.Random random = new System.Random();
    #region 信息交互
    public override void All_OnRawDataUpdate()
    {
        base.All_OnRawDataUpdate();
        spriteRenderer_Lock.gameObject.SetActive(buildingData_Box.ReadLock());
    }
    #endregion
    #region 箱子表现
    public override void All_ChangeBoxState(BoxState state)
    {
        if (state == boxState) return;
        boxState = state;
        trans_Root.DOKill();
        trans_Root.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
        switch (boxState)
        {
            case BoxState.Close:
                {
                    AudioManager.Instance?.Play3DEffect(3007, transform.position);
                    spriteRenderer_Top.sprite = sprite_TopClose;
                }
                break;
            case BoxState.Open:
                {
                    AudioManager.Instance?.Play3DEffect(3006, transform.position);
                    spriteRenderer_Top.sprite = sprite_TopOpen;
                }
                break;
        }
    }
    #endregion
    #region 箱子计算
    public int All_CalculateUnlockPro()
    {
        return int_UnLockPro;
    }
    #endregion
    #region 箱子开关
    public void All_TryToOpenBox(ActorManager actorManager)
    {
        if (buildingData_Box.ReadLock()) { All_TryToUnlockBox(actorManager); }
        else OpenOrCloseUI(true);
    }
    public void All_TryToCloseBox()
    {
        OpenOrCloseUI(false);
    }

    public void All_TryToUnlockBox(ActorManager actorManager)
    {
        if (coroutine_Unlock != null) StopCoroutine(coroutine_Unlock);
        coroutine_Unlock = StartCoroutine(All_TryToUnlockBox_Coroutine(actorManager, 2));
    }
    public IEnumerator All_TryToUnlockBox_Coroutine(ActorManager actorManager, float duration)
    {
        if (obj_SingalUI_F.TryGetComponent(out SignalUI_Unlock signalUI_Unlock))
        {
            signalUI_Unlock.PlayUnlock(duration);
            while (duration > 0)
            {
                actorManager.actorNetManager.RPC_Local_SetHandAction((short)HandActionType.Work, 1);
                actorManager.actorNetManager.RPC_Local_SetHeadAction((short)HeadActionType.Work, 1);
                float time = Mathf.Min(duration, 1);
                duration -= time;
                yield return new WaitForSeconds(time);
            }



            buildingData_Box.WriteLock(random.Next(0, 99) >= All_CalculateUnlockPro());
            buildingData_Box.WriteItemDataList(Tool_GetRandomItemList(LootItemConfigData.GetLootRandomConfig(2908).Loot_List, new System.Random().Next(3, 6)));
            All_TryToPush();

            OpenOrCloseUI(!buildingData_Box.ReadLock());
            signalUI_Unlock.PlayUnlockResult(!buildingData_Box.ReadLock());
            AudioManager.Instance?.Play2DEffect(!buildingData_Box.ReadLock() ? 1000 : 1001);
        }
        coroutine_Unlock = null;
    }

    #endregion
    #region 交互
    public override void Local_ActorInputKeycode(ActorManager actor, KeyCode code)
    {
        switch (code)
        {
            case KeyCode.F: 
                {
                    if (tileUI_Bind == null) All_TryToOpenBox(actor);
                    else All_TryToCloseBox();
                } break;
        }
    }

    public override void OpenOrCloseHighlightUI(bool open)
    {
        if (open)
        {
            obj_SingalUI_F = obj_SingalUI_F ? obj_SingalUI_F : PoolManager.Instance.GetObject("UI/TileUI/SignalUI_F_Unlock");
            obj_SingalUI_F.transform.position = transform.position + All_GetTileGenter();
            obj_SingalUI_F.transform.localScale = Vector3.one;
            obj_SingalUI_F.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
            obj_SingalUI_F.GetComponent<SignalUI_Unlock>().UpdateState(!buildingData_Box.ReadLock(), All_CalculateUnlockPro());
            obj_HighlightUI = obj_HighlightUI ? obj_HighlightUI : PoolManager.Instance.GetObject("UI/TileUI/" + All_GetTileSize());
            obj_HighlightUI.transform.position = transform.position;
            obj_HighlightUI.transform.localScale = Vector3.one;
            obj_HighlightUI.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
        }
        else
        {
            PoolManager.Instance.ReleaseObject("UI/TileUI/SignalUI_F_Unlock", obj_SingalUI_F);
            obj_SingalUI_F = null;
            PoolManager.Instance.ReleaseObject("UI/TileUI/" + All_GetTileSize(), obj_HighlightUI);
            obj_HighlightUI = null;
        }
    }
    public override void OpenOrCloseAwakeUI(bool open)
    {
        if (open)
        {
            if (obj_SingalUI_F)
            {
                PoolManager.Instance.ReleaseObject("UI/TileUI/SignalUI_F_Unlock", obj_SingalUI_F);
                obj_SingalUI_F = null;
            }
            obj_SingalUI_Awake = obj_SingalUI_Awake ? obj_SingalUI_Awake : PoolManager.Instance.GetObject("UI/TileUI/SignalUI_Awake");
            obj_SingalUI_Awake.transform.position = transform.position + All_GetTileGenter();
            obj_SingalUI_Awake.transform.localScale = Vector3.one;
            obj_SingalUI_Awake.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
        }
        else
        {
            PoolManager.Instance.ReleaseObject("UI/TileUI/SignalUI_Awake", obj_SingalUI_Awake);
            obj_SingalUI_Awake = null;
        }
    }

    #endregion

}
