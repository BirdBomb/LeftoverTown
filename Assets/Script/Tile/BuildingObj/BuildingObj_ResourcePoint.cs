using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UniRx;
using UnityEngine;
/// <summary>
/// 资源点
/// </summary>
public class BuildingObj_ResourcePoint : BuildingObj_Manmade
{
    #region 序列化字段
    [Header("基础翻找时间")]
    public int int_BaseLootTime = 2;
    [Header("每次翻找增加的时间")]
    public int int_IncrementLootTime = 2;
    #endregion
    protected GameObject obj_SingalUI_FwithBar;
    protected GameObject obj_HighlightUI;
    protected Transform tran_BarFillUI;
    private float float_totalLootTime => int_BaseLootTime + int_IncrementLootTime * int_LootTime;
    private int int_LootTime;//翻找次数
    private Coroutine lootCoroutine;
    private System.Random random = new System.Random();
    public override void Start()
    {
        MessageBroker.Default.Receive<GameEvent.GameEvent_All_UpdateHour>().Subscribe(All_OnHourUpdate).AddTo(this);
    }
    #region 时间更新
    public virtual void All_OnHourUpdate(GameEvent.GameEvent_All_UpdateHour eventData)
    {
        if (eventData.hour == 0)
        {
            int_LootTime = 0;
        }
    }
    #endregion
    #region 瓦片交互
    public override void Local_ActorInputKeycode(ActorManager actor, KeyCode code)
    {
        if (code == KeyCode.F) 
        {
            if (lootCoroutine != null) StopCoroutine(lootCoroutine);
            lootCoroutine = StartCoroutine(LootLife(actor, float_totalLootTime));
        }
        base.Local_ActorInputKeycode(actor, code);
    }
    public override void Local_PlayerHighlight(bool on)
    {
        OpenOrCloseHighlightUI(on);
        base.Local_PlayerHighlight(on);
    }
    public override void Local_PlayerFaraway()
    {
        LootReset();
        base.Local_PlayerFaraway();
    }
    public override void OpenOrCloseHighlightUI(bool open)
    {
        if (open)
        {
            obj_SingalUI_FwithBar = obj_SingalUI_FwithBar ? obj_SingalUI_FwithBar : PoolManager.Instance.GetObject("UI/TileUI/SignalUI_FwithBar");
            obj_SingalUI_FwithBar.transform.position = transform.position + All_GetTileGenter();
            obj_SingalUI_FwithBar.transform.localScale = Vector3.one;
            obj_SingalUI_FwithBar.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
            obj_HighlightUI = obj_HighlightUI ? obj_HighlightUI : PoolManager.Instance.GetObject("UI/TileUI/" + All_GetTileSize());
            obj_HighlightUI.transform.position = transform.position;
            obj_HighlightUI.transform.localScale = Vector3.one;
            obj_HighlightUI.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
            tran_BarFillUI = obj_SingalUI_FwithBar.transform.Find("Fill").transform;
            tran_BarFillUI.localScale = new Vector3(0, 1, 1);
        }
        else
        {
            PoolManager.Instance.ReleaseObject("UI/TileUI/SignalUI_FwithBar", obj_SingalUI_FwithBar);
            obj_SingalUI_FwithBar = null;
            PoolManager.Instance.ReleaseObject("UI/TileUI/" + All_GetTileSize(), obj_HighlightUI);
            obj_HighlightUI = null;
        }
    }
    public override bool CanHighlight()
    {
        return true;
    }
    #endregion
    #region 翻找
    /// <summary>
    /// 完成翻找
    /// </summary>
    private void LootComplete()
    {
        if (tran_BarFillUI != null) tran_BarFillUI.localScale = new Vector3(0, 1, 1);
        int_LootTime += 1;
        var data = Tool_GetRandomItem(LootItemConfigData.GetLootRandomConfig(buildingTile.tileID).Loot_List,random);
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemBag_Add()
        {
            itemData = data,
            itemFrom = ItemFrom.OutSide
        });
    }
    /// <summary>
    /// 终止翻找
    /// </summary>
    private void LootReset()
    {
        if (tran_BarFillUI != null) tran_BarFillUI.localScale = new Vector3(0, 1, 1);
        if (lootCoroutine != null) StopCoroutine(lootCoroutine);
    }
    private IEnumerator LootLife(ActorManager actorManager, float duration)
    {
        LootReset();

        if (tran_BarFillUI)
        {
            tran_BarFillUI.DOKill();
            tran_BarFillUI.localScale = new Vector3(0, 1, 1);
            tran_BarFillUI.DOScaleX(1, float_totalLootTime - 0.1f).SetEase(Ease.Linear);
        }
        while (duration > 0)
        {
            actorManager.actorNetManager.RPC_Local_SetHandAction((short)HandActionType.Work, 1);
            actorManager.actorNetManager.RPC_Local_SetHeadAction((short)HeadActionType.Work, 1);
            float time = Mathf.Min(duration, 1);
            duration -= time;
            yield return new WaitForSeconds(time);
        }
        LootComplete();
    }
    #endregion
}
