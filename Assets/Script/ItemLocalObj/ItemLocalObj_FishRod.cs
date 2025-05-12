using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using Unity.VisualScripting;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.U2D;
using UniRx;
using static UnityEngine.UI.GridLayoutGroup;

public class ItemLocalObj_FishRod : ItemLocalObj
{
    public Transform tran_Rod;
    public SpriteRenderer spriteRenderer_Rod;
    public Transform tran_RightHand;
    public Transform tran_LeftHand;
    public Transform tran_LineStart_Common;
    public Transform tran_LineStart_Curl;
    [Header("鱼线起点")]
    public Transform tran_LineStart;
    [Header("鱼线终点")]
    public Transform tran_LineEnd;
    [Header("鱼")]
    public SpriteRenderer spriteRenderer_Fish;
    [Header("鱼线")]
    public LineRenderer lineRenderer_FishLine;
    [Header("水花")]
    public GameObject gameObject_Spary;
    [SerializeField, Header("进度条背景")]
    private GameObject gameObject_Bar;
    [SerializeField, Header("进度条")]
    private Transform tran_Bar;
    [Header("最小钓鱼时间")]
    public float float_MinFishTime;
    [Header("最大钓鱼时间")]
    public float float_MaxFishTime;
    [Header("直杆")]
    public Sprite sprite_Common;
    [Header("弯杆")]
    public Sprite sprite_Curl;
    public SpriteAtlas spriteAtlas_Item;
    [HideInInspector]
    public ActorManager actorManager_Owner;
    /// <summary>
    /// 鱼钩位置
    /// </summary>
    private Vector3 pos_FishHookCur = Vector3.zero;
    private Vector3 pos_Standing;
    /// <summary>
    /// 咬钩等待时间
    /// </summary>
    private float fishingTime = 0;
    /// <summary>
    /// 咬钩ID
    /// </summary>
    private short fishingID;
    private RodState rodState_Cur = RodState.Sleep;

    private const float config_readyTime = 0.5f;
    private InputData inputData = new InputData();
    private void FixedUpdate()
    {

        if (rodState_Cur == RodState.InWater)
        {
            //tran_LineEnd.position = pos_FishHookCur;
            //lineRenderer_FishLine.SetPosition(1, tran_LineEnd.position);
            if (Vector3.Distance(actorManager_Owner.transform.position, pos_Standing) > 0.5f)
            {
                EndReady(0.2f);
            }
        }
        if (rodState_Cur == RodState.Sleep || rodState_Cur == RodState.InReady)
        {
            pos_FishHookCur = Vector3.Lerp(pos_FishHookCur, tran_LineStart.position, 0.5f);
            //tran_LineEnd.position = tran_LineStart.position;
            //lineRenderer_FishLine.SetPosition(1, tran_LineEnd.position);
        }
        tran_LineEnd.position = pos_FishHookCur;
        lineRenderer_FishLine.SetPosition(0, tran_LineStart.transform.position);
        lineRenderer_FishLine.SetPosition(1, tran_LineEnd.position);
    }
    public override void HoldingByHand(ActorManager owner, BodyController_Human body, ItemData data)
    {
        itemData = data;
        actorManager = owner;

        transform.SetParent(body.transform_RightHand);
        body.gameObjects_ItemInHand.Add(gameObject);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;
        actorManager_Owner = owner;
        tran_RightHand.GetComponent<SpriteRenderer>().sprite = body.transform_RightHand.GetComponent<SpriteRenderer>().sprite;
        body.transform_RightHand.GetComponent<SpriteRenderer>().enabled = false;
        tran_LeftHand.GetComponent<SpriteRenderer>().sprite = body.transform_LeftHand.GetComponent<SpriteRenderer>().sprite;
        body.transform_LeftHand.GetComponent<SpriteRenderer>().enabled = false;
        base.HoldingByHand(owner, body, data);
    }
    public override bool PressRightMouse(float time, ActorAuthority actorAuthority)
    {
        if (time > 0)
        {
            if (inputData.rightPressTimer == 0)
            {
                StartReady(config_readyTime);
            }
            inputData.rightPressTimer = time;
            ChangeFishBar(true, Mathf.Lerp(0, 1, inputData.rightPressTimer / config_readyTime));
        }

        return base.PressRightMouse(time, actorAuthority);
    }
    public override void ReleaseRightMouse()
    {

        if (inputData.rightPressTimer > 0)
        {
            if (inputData.rightPressTimer > config_readyTime)
            {
                StartSwinging(actorManager.transform.position + inputData.mousePosition.normalized * 5, config_readyTime);
            }
            else
            {
                EndReady(config_readyTime);
            }
            inputData.rightPressTimer = 0;
        }

        base.ReleaseRightMouse();
    }
    public override bool PressLeftMouse(float time, ActorAuthority actorAuthority)
    {
        if (time > 0)
        {
            if (inputData.leftPressTimer == 0)
            {
                inputData.leftPressTimer = time;
                StartReaping(0.5f);
            }
        }

        return base.PressLeftMouse(time, actorAuthority);
    }
    public override void ReleaseLeftMouse()
    {
        inputData.leftPressTimer = 0;
        base.ReleaseLeftMouse();
    }
    public override void UpdateMousePos(Vector3 mouse)
    {
        inputData.mousePosition = mouse;
        base.UpdateMousePos(mouse);
    }
    /// <summary>
    /// 开始蓄力
    /// </summary>
    /// <param name="val"></param>
    public void StartReady(float val)
    {
        ChangeFishRodState(RodState.InReady);
        ChangeFishBar(true, 0);
        tran_Rod.DOKill();
        tran_Rod.DOLocalMove(new Vector3(-0.5f, 1f, 0f), val);
        tran_Rod.DOLocalRotate(new Vector3(0, 0, 90), val);
        //DOTween.To(() => pos_FishHookCur, x => pos_FishHookCur = x, tran_LineStart.position, val).OnComplete(() =>
        //{
           
        //});
    }
    /// <summary>
    /// 结束蓄力
    /// </summary>
    /// <param name="val"></param>
    public void EndReady(float val)
    {
        ChangeFishRodState(RodState.Sleep);
        ChangeFishBar(false, 0);
        tran_Rod.DOKill();
        tran_Rod.DOLocalMove(new Vector3(1f, 0.5f, 0f), val);
        tran_Rod.DOLocalRotate(new Vector3(0, 0, 0), val);
        //DOTween.To(() => pos_FishHookCur, x => pos_FishHookCur = x, tran_LineStart.position, val).OnComplete(() =>
        //{
            
        //});
    }
    /// <summary>
    /// 抛竿
    /// </summary>
    /// <param name="val"></param>
    public void StartSwinging(Vector3 hookTo, float val)
    {
        ChangeFishRodState(RodState.InWater);
        ChangeFishBar(false, 0);
        tran_Rod.DOKill();
        tran_Rod.DOLocalMove(new Vector3(1.25f, 0.75f, 0f), val * 0.5f);
        tran_Rod.DOLocalRotate(new Vector3(0, 0, 0), val * 0.5f);
        Vector3 hookStart = tran_LineStart.position;
        DOTween.To(setter: value => { pos_FishHookCur = Parabola(hookStart, hookTo, 3, value); }, startValue: 0, endValue: 1, duration: val).SetEase(Ease.Linear).OnComplete(() =>
        {
            Vector3Int vector3Int = MapManager.Instance.grid_Ground.WorldToCell(hookTo);
            MapManager.Instance.GetGround(vector3Int,out GroundTile groundTile);
            if (groundTile.tileID == 1007)
            {
                GameObject muzzleFire101 = PoolManager.Instance.GetObject("Effect/Effect_WaterTick");
                muzzleFire101.transform.SetParent(tran_LineEnd);
                muzzleFire101.transform.localScale = Vector3.one;
                muzzleFire101.transform.localPosition = Vector3.zero;
                muzzleFire101.transform.localRotation = Quaternion.identity;
                SetFishTime();
                SetFishID();
                Invoke("GetFish", fishingTime);
            }
            else
            {
                EndReady(0.2f);
            }
        });
        pos_Standing = actorManager_Owner.transform.position;
    }
    /// <summary>
    /// 收竿
    /// </summary>
    public void StartReaping(float val)
    {
        if (rodState_Cur == RodState.InWater)
        {
            ChangeFishRodState(RodState.Sleep);
            tran_Rod.DOKill();
            tran_Rod.DOLocalMove(new Vector3(-0.5f, 1f, 0f), val);
            tran_Rod.DOLocalRotate(new Vector3(0, 0, 90), val);
            Vector3 hookStart = tran_LineEnd.position;
            DOTween.To(setter: value => { pos_FishHookCur = Parabola(hookStart, tran_LineStart.position, 3, value); }, startValue: 0, endValue: 1, duration: val).SetEase(Ease.Linear).OnComplete(() =>
            {
                EndReady(0.2f);
            });
        }
        if(rodState_Cur == RodState.InCurl)
        {
            spriteRenderer_Fish.sprite = spriteAtlas_Item.GetSprite("Item_" + fishingID);
            ChangeFishRodState(RodState.Sleep);
            tran_Rod.DOKill();
            tran_Rod.DOLocalMove(new Vector3(-0.5f, 1f, 0f), val);
            tran_Rod.DOLocalRotate(new Vector3(0, 0, 90), val);
            Vector3 hookStart = tran_LineEnd.position;
            DOTween.To(setter: value => { pos_FishHookCur = Parabola(hookStart, tran_LineStart.position, 3, value); }, startValue: 0, endValue: 1, duration: val).SetEase(Ease.Linear).OnComplete(() =>
            {
                spriteRenderer_Fish.sprite = null;
                if (actorManager_Owner.actorAuthority.isLocal == true)
                {
                    Type type = Type.GetType("Item_" + fishingID.ToString());
                    ((ItemBase)Activator.CreateInstance(type)).StaticAction_InitData(fishingID, out ItemData initData);
                    MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryAddItemInBag()
                    {
                        itemData = initData
                    });
                }
                EndReady(0.2f);
            });
        }
        pos_Standing = actorManager_Owner.transform.position;
    }
    private Vector3 Parabola(Vector3 start,Vector3 to,float h,float t)
    {
        float Func(float x) => 4 * (-h * x * x + h * x);
        var mid = Vector3.Lerp(start,to,t);
        return new Vector3(mid.x, Func(t) + Mathf.Lerp(start.y, to.y, t), mid.z);
    }
    private void SetFishTime()
    {
        UnityEngine.Random.InitState(itemData.Item_Info);
        fishingTime = UnityEngine.Random.Range(float_MinFishTime, float_MaxFishTime);
    }
    private void SetFishID()
    {
        int weight = 0;
        int val;
        List<FishConfig> fishConfigs = FishConfigData.fishConfigs.FindAll((x) => { return x.AreaType == AreaType.Water; });
        for(int i = 0; i < fishConfigs.Count; i++)
        {
            weight += fishConfigs[i].ItemWeight;
        }
        UnityEngine.Random.InitState(itemData.Item_Info);
        val = UnityEngine.Random.Range(0, weight);
        for (int i = 0; i < fishConfigs.Count; i++)
        {
            if (fishConfigs[i].ItemWeight >= val)
            {
                fishingID = fishConfigs[i].ItemID;
                return;
            }
            else
            {
                val -= fishConfigs[i].ItemWeight;
            }
        }
    }
    private void GetFish()
    {
        ChangeFishRodState(RodState.InCurl);
        tran_Rod.DOShakePosition(20, 0.2f, 10, 90, false, false).SetEase(Ease.Linear);
    }

    private void ChangeFishRodState(RodState rodState)
    {
        rodState_Cur = rodState;
        if (rodState == RodState.InCurl)
        {
            gameObject_Spary.SetActive(true);
            spriteRenderer_Rod.sprite = sprite_Curl;
            tran_LineStart.position = tran_LineStart_Curl.position;
        }
        else
        {
            gameObject_Spary.SetActive(false);
            spriteRenderer_Rod.sprite = sprite_Common;
            tran_LineStart.position = tran_LineStart_Common.position;
        }
    }
    public void ChangeFishBar(bool show, float val)
    {
        gameObject_Bar.gameObject.SetActive(show);
        if (show)
        {
            tran_Bar.localScale = new Vector3(val, 1f, 1f);
        }
        else
        {
            tran_Bar.localScale = new Vector3(0, 1f, 1f);
        }
        CancelInvoke();
    }
    private enum RodState
    {
        /// <summary>
        /// 闲置
        /// </summary>
        Sleep,
        /// <summary>
        /// 准备
        /// </summary>
        InReady,
        /// <summary>
        /// 入水
        /// </summary>
        InWater,
        /// <summary>
        /// 弯曲
        /// </summary>
        InCurl
    }
}
