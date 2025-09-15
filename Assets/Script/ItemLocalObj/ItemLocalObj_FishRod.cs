using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using Unity.VisualScripting;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.U2D;
using UniRx;

public class ItemLocalObj_FishRod : ItemLocalObj
{
    private enum RodState
    {
        Default,
        Ready,
        Swinging,
        FishOff,
        FishOn
    }

    private Vector3Int rodDir;
    private RodState rodState = RodState.Default;

    public Transform tran_Rod;
    public Transform tran_RodHead;
    public Transform tran_RightHand;
    public Transform tran_LeftHand;


    [Header("鱼线")]
    public LineRenderer lineRenderer_FishLine;
    [Header("鱼线起点")]
    public Transform tran_LineStart;
    [Header("鱼线终点")]
    public Transform tran_LineEnd;
    [Header("鱼钩")]
    public SpriteRenderer spriteRenderer_Hook;
    [Header("水花")]
    public ParticleSystem particle_Spray;
    [Header("物品")]
    public SpriteRenderer spriteRenderer_Item;
    [Header("物品图集")]
    public SpriteAtlas spriteAtlas_Item;
    /// <summary>
    /// 渔力
    /// </summary>
    private float float_FishPower;
    /// <summary>
    /// 控制鱼的时间
    /// </summary>
    private float float_KeepingFishTime = 2;
    /// <summary>
    /// 抛竿时间
    /// </summary>
    public float config_readyTime = 0.5f;
    /// <summary>
    /// 收杆时间
    /// </summary>
    public float config_reapingTime = 0.5f;

    [HideInInspector]
    public ActorManager actorManager_Owner;
    private Vector3 pos_FishHookCur = Vector3.zero;
    private short areaID = 0;

    private InputData inputData = new InputData();
    private void OnDisable()
    {
        if (actorManager.actorAuthority.isPlayer && actorManager.actorAuthority.isLocal)
        {
            MapPreviewManager.Instance.Local_HideSingal();
        }
    }
    private void Start()
    {
        MessageBroker.Default.Receive<GameEvent.GameEvent_AllClient_SomeoneMove>().Subscribe(_ =>
        {
            if (_.moveActor.actorAuthority.isLocal && _.moveActor.actorAuthority.isPlayer)
            {
                if (rodState != RodState.Default)
                {
                    Reaping(config_reapingTime);
                }
            }
        }).AddTo(this);
    }
    private void FixedUpdate()
    {
        DrawLine(Time.fixedDeltaTime);
    }
    public override void HoldingStart(ActorManager owner, BodyController_Human body)
    {
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
        ShowLine(false);
        base.HoldingStart(owner, body);
    }
    public void UpdateFishRodData(float fishPower,ItemQuality itemQuality)
    {
        float_FishPower = fishPower;
    }
    public override bool PressRightMouse(float time, ActorAuthority actorAuthority)
    {
        if (time > 0)
        {
            if (inputData.rightPressTimer == 0)
            {

            }
            inputData.rightPressTimer = time;
        }

        return base.PressRightMouse(time, actorAuthority);
    }
    public override void ReleaseRightMouse()
    {
        base.ReleaseRightMouse();
    }
    public override bool PressLeftMouse(float time, ActorAuthority actorAuthority)
    {
        if (time > 0)
        {
            if (inputData.leftPressTimer == 0)
            {
                if (rodState == RodState.Default)
                {
                    TryToSwinging(actorManager.pathManager.vector3Int_CurPos + rodDir, config_readyTime);
                }
                else
                {
                    Reaping(config_reapingTime);
                }
            }
            inputData.leftPressTimer = time;
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
        mouse = mouse.normalized;
        Vector3Int rodDir_New;
        if (mouse.x > 0.9f) 
        {
            rodDir_New = Vector3Int.right;
        }
        else if (mouse.x < -0.9f) 
        {
            rodDir_New = Vector3Int.left;
        }
        else if (mouse.y > 0.9f) 
        {
            rodDir_New = Vector3Int.up;
        }
        else if (mouse.y < -0.9f)
        {
            rodDir_New = Vector3Int.down;
        }
        else if (mouse.x > 0.38f && mouse.y > 0.38f)
        {
            rodDir_New = new Vector3Int(1, 1, 0);
        }
        else if (mouse.x < -0.38f && mouse.y < -0.38f)
        {
            rodDir_New = new Vector3Int(-1, -1, 0);
        }
        else if (mouse.x > 0.38f && mouse.y < -0.38f)
        {
            rodDir_New = new Vector3Int(1, -1, 0);
        }
        else if (mouse.x < -0.38f && mouse.y > 0.38f)
        {
            rodDir_New = new Vector3Int(-1, 1, 0);
        }
        else
        {
            rodDir_New = Vector3Int.zero;
        }
        if (rodDir != rodDir_New)
        {
            rodDir = rodDir_New;
            if (actorManager.actorAuthority.isPlayer && actorManager.actorAuthority.isLocal)
            {
                MapPreviewManager.Instance.Local_ShowSingal(rodDir);
            }
        }
        base.UpdateMousePos(mouse);
    }
    #region//鱼竿
    /// <summary>
    /// 尝试抛竿
    /// </summary>
    /// <param name="hookTo"></param>
    /// <param name="time"></param>
    private void TryToSwinging(Vector3Int hookTo, float time)
    {
        if (Check(hookTo))
        {
            Vector3 pos = MapManager.Instance.grid_Ground.CellToWorld(hookTo) + new Vector3(0.5f, 0.5f, 0);
            Ready(pos, time);
        }
        else
        {
            if (actorManager.actorAuthority.isLocal)
            {
                MapPreviewManager.Instance.Local_FailSingal(0.2f);
            }
        }
    }
    /// <summary>
    /// 检查
    /// </summary>
    /// <param name="hookTo"></param>
    /// <returns></returns>
    private bool Check(Vector3Int hookTo)
    {
        MapManager.Instance.GetGround(hookTo, out GroundTile target);
        if (target != null && target.tileID == 9000)
        {
            areaID = 9000;
            return true;
        }
        else
        {
            return false;
        }
    }
    /// <summary>
    /// 准备
    /// </summary>
    /// <param name="hookTo"></param>
    /// <param name="time"></param>
    private void Ready(Vector3 hookTo, float time)
    {
        rodState = RodState.Ready;
        ShowLine(false);
        tran_Rod.DOKill();
        tran_Rod.DOLocalMove(new Vector3(0, 0, 0f), time);
        tran_Rod.DOLocalRotate(new Vector3(0, 0, 90), time).OnComplete(() =>
        {
            Swinging(hookTo, time);
        });
    }
    /// <summary>
    /// 抛竿
    /// </summary>
    /// <param name="hookTo"></param>
    /// <param name="time"></param>
    private void Swinging(Vector3 hookTo, float time)
    {
        rodState = RodState.Swinging;
        ShowLine(true);
        tran_Rod.DOKill();
        tran_Rod.DOLocalMove(new Vector3(0, 0, 0f), time * 0.5f);
        tran_Rod.DOLocalRotate(new Vector3(0, 0, 0), time * 0.5f);
        Vector3 hookStart = tran_LineStart.position;
        hookTo = hookTo + Vector3.down * 0.25f;
        DOTween.To(setter: value => { pos_FishHookCur = Parabola(hookStart, hookTo, 2, value); }, startValue: 0, endValue: 1, duration: time).SetEase(Ease.InOutSine).OnComplete(() =>
        {
            InWater();
        });
    }
    /// <summary>
    /// 收杆
    /// </summary>
    /// <param name="time"></param>
    private void Reaping(float time)
    {
        AudioManager.Instance.Play3DEffect(2007, tran_LineEnd.position);
        particle_Spray.Play();
        if (IsInvoking("GetFish")) CancelInvoke("GetFish");
        if (IsInvoking("LoseFish")) CancelInvoke("LoseFish");
        short fishID = 0;
        if (rodState == RodState.FishOn)
        {
            fishID = GetFishID();
            ShowItem(fishID);
        }
        rodState = RodState.Default;
        tran_Rod.DOKill();
        tran_Rod.DOLocalMove(Vector3.zero, time);
        tran_Rod.DOLocalRotate(new Vector3(0, 0, 90), time);
        Vector3 hookStart = tran_LineEnd.position;
        DOTween.To(setter: value => { pos_FishHookCur = Parabola(hookStart, tran_LineStart.position, 3, value); }, startValue: 0, endValue: 1, duration: time).SetEase(Ease.Linear).OnComplete(() =>
        {
            Over(0.2f);
            if (fishID != 0 && actorManager.actorAuthority.isLocal && actorManager.actorAuthority.isPlayer)
            {
                Type type = Type.GetType("Item_" + fishID.ToString());
                ((ItemBase)Activator.CreateInstance(type)).StaticAction_InitData(fishID, out ItemData initData);
                AudioManager.Instance.Play2DEffect(1000);
                MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemBag_Add()
                {
                    itemData = initData,
                    itemFrom = ItemFrom.OutSide
                });
            }
        });
    }
    /// <summary>
    /// 复位
    /// </summary>
    /// <param name="time"></param>
    private void Over(float time)
    {
        ShowLine(false);
        tran_Rod.DOKill();
        tran_Rod.DOLocalMove(Vector3.zero, time);
        tran_Rod.DOLocalRotate(new Vector3(0, 0, 0), time);
    }
    /// <summary>
    /// 入水
    /// </summary>
    private void InWater()
    {
        AudioManager.Instance.Play3DEffect(2007, tran_LineEnd.position);
        particle_Spray.Play();
        rodState = RodState.FishOff;
        float fishTime = GetFishTime();
        if (IsInvoking("GetFish")) CancelInvoke("GetFish");
        if (fishTime < 10) { Invoke("GetFish", fishTime); }
        DrawWave();
    }
    /// <summary>
    /// 上钩
    /// </summary>
    private void GetFish()
    {
        rodState = RodState.FishOn;
        AudioManager.Instance.Play3DEffect(2007, tran_LineEnd.position);
        particle_Spray.Play();
        tran_Rod.DOKill();
        tran_Rod.DOLocalRotate(new Vector3(0, 0, -30), 0.2f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            tran_Rod.DOShakeRotation(0.2f, new Vector3(0, 0, 10)).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
        });
        if (IsInvoking("LoseFish")) CancelInvoke("LoseFish");
        Invoke("LoseFish", float_KeepingFishTime);
    }
    /// <summary>
    /// 脱钩
    /// </summary>
    private void LoseFish()
    {
        rodState = RodState.FishOff;
        tran_Rod.DOKill();
        tran_Rod.DOLocalRotate(new Vector3(0, 0, 0), 0.2f);
        float fishTime = GetFishTime();
        if (IsInvoking("GetFish")) CancelInvoke("GetFish");
        Invoke("GetFish", fishTime);
    }
    private Vector3 Parabola(Vector3 start, Vector3 to, float h, float t)
    {
        float Func(float x) => 4 * (-h * x * x + h * x);
        var mid = Vector3.Lerp(start, to, t);
        return new Vector3(mid.x, Func(t) + Mathf.Lerp(start.y, to.y, t), mid.z);
    }
    private void UpdateRodData()
    {
        ItemData _oldItem = itemData;
        ItemData _newItem = _oldItem;
        _newItem.Item_Durability--;
        UpdateDataByLocal(_newItem);
        if (actorManager.actorAuthority.isPlayer && actorManager.actorAuthority.isLocal)
        {
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemHand_Change()
            {
                oldItem = _oldItem,
                newItem = _newItem,
            });
        }
    }
    #endregion
    #region//鱼线
    private float float_DrawWaveCD_FishOff = 0.5f;
    private float float_DrawWaveCD_FishOn = 0.1f;
    private float float_DrawWaveTime = 0f;
    private float float_HookAngle = 0;
    private float float_HookRadiu = 0.2f;
    private float float_HookSpeed = 10;
    private void DrawLine(float dt)
    {
        if (rodState == RodState.FishOn)
        {
            float_HookAngle += float_HookSpeed * dt;

            // 计算新位置
            float x = pos_FishHookCur.x + Mathf.Cos(float_HookAngle) * float_HookRadiu;
            float y = pos_FishHookCur.y + Mathf.Sin(float_HookAngle) * float_HookRadiu;

            // 更新物体位置
            tran_LineEnd.position = new Vector3(x, y, pos_FishHookCur.z);


            float_DrawWaveTime += dt;
            if (float_DrawWaveTime > float_DrawWaveCD_FishOn)
            {
                particle_Spray.Play();
                DrawWave();
                float_DrawWaveTime = 0;
            }
        }
        else if(rodState == RodState.FishOff)
        {
            tran_LineEnd.position = pos_FishHookCur;
            float_DrawWaveTime += dt;
            if (float_DrawWaveTime > float_DrawWaveCD_FishOff)
            {
                DrawWave();
                float_DrawWaveTime = 0;
            }
        }
        else
        {
            tran_LineEnd.position = pos_FishHookCur;
        }
        lineRenderer_FishLine.SetPosition(0, tran_LineStart.transform.position);
        lineRenderer_FishLine.SetPosition(1, tran_LineEnd.position);
    }
    private void ShowLine(bool show)
    {
        ShowItem(0);
        pos_FishHookCur = tran_LineStart.position;
        lineRenderer_FishLine.gameObject.SetActive(show);
        spriteRenderer_Hook.gameObject.SetActive(show);
    }
    private void ShowItem(short id)
    {
        if (id == 0)
        {
            spriteRenderer_Item.enabled = false;
        }
        else
        {
            spriteRenderer_Item.enabled = true;
            spriteRenderer_Item.sprite = spriteAtlas_Item.GetSprite("Item_" + id);
        }
    }
    private void DrawWave()
    {
        LiquidManager.Instance.AddWave(tran_LineEnd.position);
    }

    #endregion
    #region//渔力计算
    /// <summary>
    /// 获取上钩时间
    /// </summary>
    /// <returns></returns>
    private float GetFishTime()
    {
        float temp_FishPower = 0;
        float temp_BaseFishTime = 9999;
        if(areaID == 9000)/*水域钓鱼*/
        {
            temp_FishPower = float_FishPower - 0;
        }
        if (temp_FishPower > 0)
        {
            if (temp_FishPower > 10) temp_FishPower = 10;
            temp_BaseFishTime = Mathf.Lerp(10, 1, temp_FishPower / 10f);
        }
        UnityEngine.Random.InitState(itemData.Item_Info);
        float fishTime = UnityEngine.Random.Range(temp_BaseFishTime, temp_BaseFishTime + 2);
        return fishTime;
    }
    /// <summary>
    /// 获取上钩种类
    /// </summary>
    /// <returns></returns>
    private short GetFishID()
    {
        UpdateRodData();
        int weight = 0;
        int val;
        List<FishConfig> fishConfigs = FishConfigData.fishConfigs.FindAll((x) => { return x.AreaID == areaID; });
        for (int i = 0; i < fishConfigs.Count; i++)
        {
            weight += fishConfigs[i].ItemWeight;
        }
        UnityEngine.Random.InitState(itemData.Item_Info + itemData.Item_Durability);
        val = UnityEngine.Random.Range(0, weight);
        for (int i = 0; i < fishConfigs.Count; i++)
        {
            if (fishConfigs[i].ItemWeight >= val)
            {
                return fishConfigs[i].ItemID;
            }
            else
            {
                val -= fishConfigs[i].ItemWeight;
            }
        }
        return 0;
    }

    #endregion

}
