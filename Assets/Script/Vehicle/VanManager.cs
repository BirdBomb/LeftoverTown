using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Windows;

public class VanManager : VehicleManager
{
    [SerializeField, Header("车旋转")]
    private Transform tran_Rotate;
    [SerializeField, Header("车身")]
    private Transform tran_Body;
    [SerializeField, Header("车轮")]
    private Transform tran_Wheel;
    [SerializeField, Header("左车轮烟雾")]
    private ParticleSystem particle_WheelLeftSmoke;
    [SerializeField, Header("右车轮烟雾")]
    private ParticleSystem particle_WheelRightSmoke;
    private MaterialPropertyBlock materialPropertyBlock;
    private float float_WheelSpeed;
    [SerializeField, Header("上车轮")]
    private GameObject obj_WheelUp;
    [SerializeField, Header("下车轮")]
    private GameObject obj_WheelDown;
    [SerializeField, Header("左车轮")]
    private GameObject obj_WheelLeft;
    [SerializeField, Header("右车轮")]
    private GameObject obj_WheelRight;
    [SerializeField, Header("上车身")]
    private GameObject obj_BodyUp;
    [SerializeField, Header("下车身")]
    private GameObject obj_BodyDown;
    [SerializeField, Header("右车身")]
    private GameObject obj_BodyRight;
    [SerializeField, Header("左车身")]
    private GameObject obj_BodyLeft;
    [SerializeField, Header("下车灯")]
    private GameObject obj_LightDown;
    [SerializeField, Header("右车灯光")]
    private Light2D light2D_Right;
    [SerializeField, Header("左车灯光")]
    private Light2D light2D_Left;

    private VehicleDirection vehicleDirection_cur = VehicleDirection.Up;
    private Vector2 vector2_curDirection;
    private Vector2[] vector2s_cardinalDirections = new Vector2[]
    {
        Vector2.up,
        Vector2.down,
        Vector2.left,
        Vector2.right,
    };
    private float float_speedCur;
    private float float_angleCur;
    private bool bool_engineOn = false;
    [SerializeField, Header("最大速度")]
    private float float_speedMax;
    [SerializeField, Header("最大搭载人数")]
    private int int_passengerCountMax;
    [SerializeField, Header("马力")]
    private float float_horsePower;
    [SerializeField, Header("抓地力")]
    private float float_roadHolding;
    [SerializeField, Header("转向速度")]
    private float float_turnPower;
    [SerializeField, Header("回正速度")]
    private float float_backPower;
    [SerializeField, Header("SingalGetOn")]
    private GameObject obj_singalGetOn;
    [SerializeField, Header("SingalGetOff")]
    private GameObject obj_singalGetOff;
    [SerializeField, Header("SingalE")]
    private GameObject obj_singalE;
    [SerializeField, Header("右轮胎印")]
    private TrailRenderer trailRenderer_rightWheel;
    [SerializeField, Header("左轮胎印")]
    private TrailRenderer trailRenderer_leftWheel;
    [SerializeField, Header("下车位置")]
    private Transform tran_DoorPos;
    [SerializeField, Header("乘客位置")]
    private Transform tran_PassengerPos;
    //#region//监听
    //public override void AllClient_ListenSomeoneMove(ActorManager actorManager)
    //{
    //    if (actorManager.actorAuthority.isPlayer && actorManager.actorAuthority.isLocal)
    //    {
    //        if (Vector3.Distance(actorManager.transform.position, transform.position) < 3)
    //        {
    //            OpenOrCloseSingalE(true);
    //            actorManager.inputManager.Local_AddInputKeycodeAction(AllClient_ActorInputKeycode);
    //            if (actorManager_Passenger.Contains(actorManager))
    //            {
    //                OpenOrCloseSingalGetOff(true);
    //                OpenOrCloseSingalGetOn(false);
    //            }
    //            else
    //            {
    //                OpenOrCloseSingalGetOn(true);
    //                OpenOrCloseSingalGetOff(false);
    //            }
    //        }
    //        else
    //        {
    //            actorManager.inputManager.Local_RemoveInputKeycodeAction(AllClient_ActorInputKeycode);
    //            OpenOrCloseSingalGetOn(false);
    //            OpenOrCloseSingalGetOff(false);
    //            OpenOrCloseSingalE(false);
    //            OpenOrCloseCabinetUI(false);
    //        }
    //    }
    //    base.AllClient_ListenSomeoneMove(actorManager);
    //}
    //#endregion
    //#region//车辆控制
    //public override void AllClient_GetOn(ActorManager actor)
    //{
    //    base.AllClient_GetOn(actor);
    //}
    //public override void AllClient_GetOff(ActorManager actor)
    //{
    //    base.AllClient_GetOff(actor);
    //}
    //public override void FromRPC_AllClient_GetOn(ActorManager actor)
    //{
    //    base.FromRPC_AllClient_GetOn(actor);
    //    //actor.actorNetManager.OnlyState_UpdateNetworkRigidbody(tran_PassengerPos.position, 100);
    //}
    //public override void FromRPC_AllClient_GetOff(ActorManager actor)
    //{
    //    base.FromRPC_AllClient_GetOff(actor);
    //    //actor.actorNetManager.OnlyState_UpdateNetworkRigidbody(tran_DoorPos.position, 100);
    //}
    ///// <summary>
    ///// 玩家输入按钮
    ///// </summary>
    //public override void AllClient_ActorInputKeycode(ActorManager actor, KeyCode code)
    //{
    //    if (code == KeyCode.F)
    //    {
    //        OpenOrCloseSingalGetOn(false);
    //        if (actorManager_Passenger.Contains(actor))
    //        {
    //            AllClient_GetOff(actor);
    //        }
    //        else
    //        {
    //            AllClient_GetOn(actor);
    //        }
    //    }
    //    if (code == KeyCode.E)
    //    {
    //    }
    //    base.AllClient_ActorInputKeycode(actor, code);
    //}
    ///// <summary>
    ///// 玩家输入方向
    ///// </summary>
    ///// <param name="actor"></param>
    ///// <param name="dir"></param>
    ///// <param name="dt"></param>
    //public override void AllClient_ActorInputMove(ActorManager actor, float dt, Vector2 dir)
    //{
    //    if (VehicleNetManager_NetManager.Object.HasStateAuthority)
    //    {
    //        if (actorManager_Drive && actorManager_Drive.Equals(actor))
    //        {
    //            if (dir.y > 0.2f)
    //            {
    //                OnlyState_EngineRunningOn(dt, true);
    //                OnlyState_Turn(dir.x, dt);
    //            }
    //            else if (dir.y < -0.2f)
    //            {
    //                OnlyState_EngineRunningOn(dt, false);
    //                OnlyState_Turn(-dir.x, dt);
    //            }
    //            else
    //            {
    //                OnlyState_EngineRunningOff(dt);
    //            }
    //            OnlyState_Move(dt);
    //        }
    //    }
    //    base.AllClient_ActorInputMove(actor, dt, dir);
    //}
    ///// <summary>
    ///// 引擎打开
    ///// </summary>
    //private void OnlyState_EngineRunningOn(float dt, bool speedUp)
    //{
    //    if (!bool_engineOn)
    //    {
    //        VehicleNetManager_NetManager.Rpc_LocalInput_Engine(true);
    //        bool_engineOn = true;
    //    }
    //    if (speedUp)
    //    {
    //        if (float_speedCur < float_speedMax)
    //        {
    //            OnlyState_SpeedUp(dt);
    //        }
    //    }
    //    else
    //    {
    //        if (float_speedCur > -float_speedMax * 0.5f)
    //        {
    //            OnlyState_SpeedDown(dt);
    //        }
    //    }
    //}
    ///// <summary>
    ///// 引擎关闭
    ///// </summary>
    //private void OnlyState_EngineRunningOff(float dt)
    //{
    //    if (bool_engineOn)
    //    {
    //        VehicleNetManager_NetManager.Rpc_LocalInput_Engine(false);
    //        bool_engineOn = false;
    //    }
    //    if (float_speedCur > 0)
    //    {
    //        float_speedCur -= float_roadHolding * dt;
    //        if (float_speedCur < 0)
    //        {
    //            float_speedCur = 0;
    //        }
    //    }
    //    else if (float_speedCur < 0)
    //    {
    //        float_speedCur += float_roadHolding * dt;
    //        if (float_speedCur > 0)
    //        {
    //            float_speedCur = 0;
    //        }
    //    }
    //}
    //public override void FromRPC_AllClient_Engine(bool engineOn)
    //{

    //    if (engineOn)
    //    {
    //        FromRPC_AllClient_WheelPlay(15);
    //        particle_WheelLeftSmoke.Play();
    //        particle_WheelRightSmoke.Play();
    //        tran_Body.DOKill();
    //        tran_Body.DOShakeScale(0.1f, new Vector3(0, 0.05f, 0)).SetLoops(-1);
    //        light2D_Right.intensity = 0.5f;
    //        light2D_Left.intensity = 0.5f;
    //        obj_LightDown.SetActive(true);
    //    }
    //    else
    //    {
    //        FromRPC_AllClient_WheelPlay(0);
    //        particle_WheelLeftSmoke.Stop();
    //        particle_WheelRightSmoke.Stop();
    //        light2D_Right.intensity = 0;
    //        light2D_Left.intensity = 0;
    //        obj_LightDown.SetActive(false);
    //        tran_Body.DOKill();
    //        tran_Body.DOScale(Vector3.one, 0.1f);
    //    }
    //    base.FromRPC_AllClient_Engine(engineOn);
    //}
    //private void FromRPC_AllClient_WheelPlay(float speed)
    //{
    //    if (materialPropertyBlock == null) { materialPropertyBlock = new MaterialPropertyBlock(); }
    //    materialPropertyBlock.SetFloat("_Speed", speed);
    //    obj_WheelUp.GetComponent<Renderer>().SetPropertyBlock(materialPropertyBlock);
    //    obj_WheelDown.GetComponent<Renderer>().SetPropertyBlock(materialPropertyBlock);
    //    obj_WheelLeft.GetComponent<Renderer>().SetPropertyBlock(materialPropertyBlock);
    //    obj_WheelRight.GetComponent<Renderer>().SetPropertyBlock(materialPropertyBlock);
    //    float_WheelSpeed = speed;
    //}
    ///// <summary>
    ///// 加速
    ///// </summary>
    ///// <param name="dt"></param>
    //public virtual void OnlyState_SpeedUp(float dt)
    //{
    //    float_speedCur += float_horsePower * dt;
    //}
    ///// <summary>
    ///// 减速
    ///// </summary>
    ///// <param name="dt"></param>
    //public virtual void OnlyState_SpeedDown(float dt)
    //{
    //    float_speedCur -= float_horsePower * dt;
    //}
    ///// <summary>
    ///// 转向
    ///// </summary>
    ///// <param name="x"></param>
    ///// <param name="dt"></param>
    //private void OnlyState_Turn(float x, float dt)
    //{
    //    if (Mathf.Abs(float_speedCur) > 2)
    //    {
    //        float_angleCur = Mathf.Atan2(vector2_curDirection.y, vector2_curDirection.x) * Mathf.Rad2Deg;
    //        if (Mathf.Abs(x) > 0.01f)
    //        {
    //            float adjustment = x * float_turnPower * dt;
    //            float angle = float_angleCur - adjustment;
    //            vector2_curDirection = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
    //        }
    //        else
    //        {
    //            Vector2 closestDirection = OnlyState_TurnBack(vector2_curDirection);
    //            vector2_curDirection = Vector2.Lerp(vector2_curDirection, closestDirection, float_backPower * dt);
    //        }
    //    }
    //    OnlyState_UpdateDirection();
    //}
    ///// <summary>
    ///// 方向盘回正
    ///// </summary>
    //private Vector2 OnlyState_TurnBack(Vector2 currentDirection)
    //{
    //    Vector2 closest = vector2s_cardinalDirections[0];
    //    float minAngleDifference = Vector2.Angle(currentDirection, closest);

    //    foreach (var cardinalDir in vector2s_cardinalDirections)
    //    {
    //        float angleDifference = Vector2.Angle(currentDirection, cardinalDir);
    //        if (angleDifference < minAngleDifference)
    //        {
    //            closest = cardinalDir;
    //            minAngleDifference = angleDifference;
    //        }
    //    }

    //    return closest;
    //}
    //#endregion
    //#region//UI
    //private void OpenOrCloseSingalGetOn(bool open)
    //{
    //    obj_singalGetOn.transform.DOKill();
    //    obj_singalGetOn.transform.localScale = Vector3.one;
    //    if (open)
    //    {
    //        if (!obj_singalGetOn.activeSelf)
    //        {
    //            obj_singalGetOn.SetActive(true);
    //            obj_singalGetOn.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
    //        }
    //    }
    //    else
    //    {
    //        if (obj_singalGetOn.activeSelf)
    //        {
    //            obj_singalGetOn.transform.DOScale(Vector3.zero, 0.1f).OnComplete(() =>
    //            {
    //                obj_singalGetOn.SetActive(false);
    //            });
    //        }
    //    }
    //}
    //private void OpenOrCloseSingalGetOff(bool open)
    //{
    //    obj_singalGetOff.transform.DOKill();
    //    obj_singalGetOff.transform.localScale = Vector3.one;
    //    if (open)
    //    {
    //        if (!obj_singalGetOff.activeSelf)
    //        {
    //            obj_singalGetOff.SetActive(true);
    //            obj_singalGetOff.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
    //        }
    //    }
    //    else
    //    {
    //        if (obj_singalGetOff.activeSelf)
    //        {
    //            obj_singalGetOff.transform.DOScale(Vector3.zero, 0.1f).OnComplete(() =>
    //            {
    //                obj_singalGetOff.SetActive(false);
    //            });
    //        }
    //    }
    //}
    //private void OpenOrCloseSingalE(bool open)
    //{
    //    obj_singalE.transform.DOKill();
    //    obj_singalE.transform.localScale = Vector3.one;
    //    if (open)
    //    {
    //        if (!obj_singalE.activeSelf)
    //        {
    //            obj_singalE.SetActive(true);
    //            obj_singalE.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
    //        }
    //    }
    //    else
    //    {
    //        if (obj_singalE.activeSelf)
    //        {
    //            obj_singalE.transform.DOScale(Vector3.zero, 0.1f).OnComplete(() =>
    //            {
    //                obj_singalE.SetActive(false);
    //            });
    //        }
    //    }
    //}
    //private void OpenOrCloseCabinetUI(bool open)
    //{
    //}
    //#endregion
    //#region//信息
    //public void TryToChangeInfo(string info)
    //{
    //   VehicleNetManager_NetManager.RPC_LocalInput_ChangeInfo(info);
    //}
    //public override void FromRPC_AllClient_UpdateInfo(string info)
    //{
    //    base.FromRPC_AllClient_UpdateInfo(info);
    //}
    //#endregion
    //#region//移动
    //private void OnlyState_UpdateDirection()
    //{
    //    Vector2 direction = vector2_curDirection;
    //    tran_Rotate.rotation = Quaternion.Euler(0f, 0f, float_angleCur); 
    //    if (Mathf.Abs(Mathf.Abs(direction.x) - Mathf.Abs(direction.y)) > 0.5f)
    //    {
    //        /*正方向*/
    //        if (direction.x > 0.7f)
    //        {
    //            VehicleNetManager_NetManager.Rpc_LocalInput_SetDirection(VehicleDirection.Right);
    //        }
    //        if (direction.x < -0.7f)
    //        {
    //            VehicleNetManager_NetManager.Rpc_LocalInput_SetDirection(VehicleDirection.Left);
    //        }
    //        if (direction.y > 0.5f)
    //        {
    //            VehicleNetManager_NetManager.Rpc_LocalInput_SetDirection(VehicleDirection.Up);
    //        }
    //        if (direction.y < -0.5f)
    //        {
    //            VehicleNetManager_NetManager.Rpc_LocalInput_SetDirection(VehicleDirection.Down);
    //        }
    //    }
    //}
    //public override void FromRPC_AllClient_SetDirection(VehicleDirection vehicleDirection)
    //{
    //    if (vehicleDirection_cur != vehicleDirection)
    //    {
    //        obj_BodyUp.SetActive(false);
    //        obj_BodyDown.SetActive(false);
    //        obj_BodyLeft.SetActive(false);
    //        obj_BodyRight.SetActive(false);
    //        obj_WheelUp.SetActive(false);
    //        obj_WheelDown.SetActive(false);
    //        obj_WheelLeft.SetActive(false);
    //        obj_WheelRight.SetActive(false);
    //        vehicleDirection_cur = vehicleDirection;
    //        if (vehicleDirection == VehicleDirection.Up)
    //        {
    //            obj_BodyUp.SetActive(true);
    //            obj_WheelUp.SetActive(true);
    //        }
    //        else if (vehicleDirection == VehicleDirection.Down)
    //        {
    //            obj_BodyDown.SetActive(true);
    //            obj_WheelDown.SetActive(true);
    //        }
    //        else if (vehicleDirection == VehicleDirection.Left)
    //        {
    //            obj_BodyLeft.SetActive(true);
    //            obj_WheelLeft.SetActive(true);
    //        }
    //        else if (vehicleDirection == VehicleDirection.Right)
    //        {
    //            obj_BodyRight.SetActive(true);
    //            obj_WheelRight.SetActive(true);
    //        }
    //    }
    //    base.FromRPC_AllClient_SetDirection(vehicleDirection);
    //}
    //private void OnlyState_Move(float dt)
    //{
    //    Vector2 velocity = new Vector2(vector2_curDirection.x * float_speedCur, vector2_curDirection.y * float_speedCur);
    //    Vector3 newPos = transform.position + new UnityEngine.Vector3(velocity.x * dt, velocity.y * dt, 0);

    //    VehicleNetManager_NetManager.OnlyState_UpdateNetworkTransform(newPos, velocity.magnitude);
    //}
    //#endregion
}
