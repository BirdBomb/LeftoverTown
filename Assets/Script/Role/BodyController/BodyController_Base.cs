using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
/// <summary>
/// 基本身体控制器
/// </summary>
public class BodyController_Base : MonoBehaviour
{
    public Transform trans_Offset;
    public SortingGroup sortingGroup_Root;
    protected float float_Speed = 0;
    public float GetSpeed()
    {
        return float_Speed;
    }
    public virtual void Start()
    {
        Local_ResetPos(transform.position);

    }
    public virtual void Update()
    {
        UpdateVehicleFront();
        UpdateWave(Time.deltaTime);
    }
    #region//检查
    protected Vector2 vector2_Last;
    protected Vector2 vector2_Cur;
    protected Vector2 vector2_Dir;
    public virtual void Local_CheckPos(float dt,float dtRec)
    {
        vector2_Cur = transform.position;
        vector2_Dir = vector2_Cur - vector2_Last;
        if (bool_Ride)
        {
            float_Speed = 0;
        }
        else
        {
            //float distanceManhattan = Mathf.Abs(vector2_Dir.x) + Mathf.Abs(vector2_Dir.y);
            float distanceManhattan = Vector2.Distance(vector2_Cur, vector2_Last);
            float_Speed = Math.Min(distanceManhattan * dtRec, 10);
        }

        UpdateBody(dt);
        PlaySpeedEffect(dt);
        vector2_Last = vector2_Cur;
    }
    public void Local_ResetPos(Vector2 pos)
    {
        vector2_Cur = pos;
        vector2_Last = pos;
    }
    #endregion
    #region//脸部
    /// <summary>
    /// 绘制脸庞
    /// </summary>
    /// <param name="hairID"></param>
    /// <param name="eyeID"></param>
    /// <param name="hairColor"></param>
    public virtual void InitFace(int hairID, int eyeID, Color32 hairColor)
    {

    }
    #endregion
    #region//动作
    public BodyAction bodyAction_Cur;
    public void SetBodyAction(BodyActionType bodyActionType, Vector2? pos, float lockTime = 0)
    {
        bodyAction_Cur.bodyActionType = bodyActionType;
        bodyAction_Cur.bodyActionPos = pos;
        bodyAction_Cur.bodyLockTime = lockTime;
        if (bodyAction_Cur.bodyActionPos != null)
        {
            Local_ResetPos((Vector2)bodyAction_Cur.bodyActionPos);
        }
    }
    public BodyAction GetBodyAction()
    {
        return bodyAction_Cur;
    }
    public void UpdateBody(float dt)
    {
        if (float_Speed > 0.1f)
        {
            float anmiaSpeed = Mathf.Min(float_Speed, 5);
            PlayMove(anmiaSpeed);
        }
        else
        {
            PlayIdle(1);
        }

    }
    public void PlayMove(float anmiaSpeed)
    {
        if (bool_StandOnWater)
        {
            SetBodyAction(BodyActionType.Walk, null);
            SetAnimatorFloat(BodyPart.Body, "Speed", anmiaSpeed);
            SetAnimatorBool(BodyPart.Body, "Walk", false);
            SetAnimatorBool(BodyPart.Body, "Swim", true);
            SetAnimatorFloat(BodyPart.Hand, "Speed", anmiaSpeed);
            SetAnimatorBool(BodyPart.Hand, "Walk", false);
            SetAnimatorFloat(BodyPart.Head, "Speed", anmiaSpeed);
            SetAnimatorBool(BodyPart.Head, "Walk", false);
        }
        else
        {
            SetBodyAction(BodyActionType.Walk, null);
            SetAnimatorFloat(BodyPart.Body, "Speed", anmiaSpeed);
            SetAnimatorBool(BodyPart.Body, "Walk", true);
            SetAnimatorBool(BodyPart.Body, "Swim", false);
            SetAnimatorFloat(BodyPart.Hand, "Speed", anmiaSpeed);
            SetAnimatorBool(BodyPart.Hand, "Walk", true);
            SetAnimatorFloat(BodyPart.Head, "Speed", anmiaSpeed);
            SetAnimatorBool(BodyPart.Head, "Walk", true);
        }
        turnDir = (vector2_Cur - vector2_Last).normalized;
        if (turnDir.x > 0.1f) TurnRight();
        if (turnDir.x < -0.1f) TurnLeft();
    }
    public void PlayIdle(float anmiaSpeed)
    {
        if (GetBodyAction().bodyActionType == BodyActionType.Walk)
        {
            SetBodyAction(BodyActionType.Idle, null);

            SetAnimatorFloat(BodyPart.Body, "Speed", anmiaSpeed);
            SetAnimatorBool(BodyPart.Body, "Walk", false);
            SetAnimatorFloat(BodyPart.Hand, "Speed", anmiaSpeed);
            SetAnimatorBool(BodyPart.Hand, "Walk", false);
            SetAnimatorFloat(BodyPart.Head, "Speed", anmiaSpeed);
            SetAnimatorBool(BodyPart.Head, "Walk", false);
        }
    }
    public void PlayBodyAction(BodyActionType bodyActionType, float speed, Vector2? lockPos = null)
    {
        switch (bodyActionType)
        {
            case BodyActionType.LayToUp:
                SetBodyAction(bodyActionType, lockPos, 0.2f);
                SetAnimatorFloat(BodyPart.Body, "Speed", speed);
                SetAnimatorTrigger(BodyPart.Body, "LayToUp");
                SetAnimatorBool(BodyPart.Body, "Walk", false);
                SetAnimatorTrigger(BodyPart.Head, "Sleep");
                SetAnimatorBool(BodyPart.Head, "Walk", false);
                break;
            case BodyActionType.LayToLeft:
                SetBodyAction(bodyActionType, lockPos, 0.2f);
                TurnRight();
                SetAnimatorFloat(BodyPart.Body, "Speed", speed);
                SetAnimatorTrigger(BodyPart.Body, "LayToLeft");
                SetAnimatorBool(BodyPart.Body, "Walk", false);
                SetAnimatorTrigger(BodyPart.Head, "Sleep");
                SetAnimatorBool(BodyPart.Head, "Walk", false);
                break;
            case BodyActionType.LayToRight:
                SetBodyAction(bodyActionType, lockPos, 0.2f);
                TurnLeft();
                SetAnimatorFloat(BodyPart.Body, "Speed", speed);
                SetAnimatorTrigger(BodyPart.Body, "LayToRight");
                SetAnimatorBool(BodyPart.Body, "Walk", false);
                SetAnimatorTrigger(BodyPart.Head, "Sleep");
                SetAnimatorBool(BodyPart.Head, "Walk", false);
                break;
            case BodyActionType.SitToRight:
                SetBodyAction(bodyActionType, lockPos, 0.2f);
                TurnRight();
                SetAnimatorFloat(BodyPart.Body, "Speed", speed);
                SetAnimatorTrigger(BodyPart.Body, "SitDown");
                SetAnimatorBool(BodyPart.Body, "Walk", false);
                SetAnimatorBool(BodyPart.Head, "Walk", false);
                break;
            case BodyActionType.SitToLeft:
                SetBodyAction(bodyActionType, lockPos, 0.2f);
                TurnLeft();
                SetAnimatorFloat(BodyPart.Body, "Speed", speed);
                SetAnimatorTrigger(BodyPart.Body, "SitDown");
                SetAnimatorBool(BodyPart.Body, "Walk", false);
                SetAnimatorBool(BodyPart.Head, "Walk", false);
                break;
            case BodyActionType.SitToDown:
                SetBodyAction(bodyActionType, lockPos, 0.2f);
                TurnRight();
                SetAnimatorFloat(BodyPart.Body, "Speed", speed);
                SetAnimatorTrigger(BodyPart.Body, "SitDown");
                SetAnimatorBool(BodyPart.Body, "Walk", false);
                SetAnimatorBool(BodyPart.Head, "Walk", false);
                break;
            case BodyActionType.RideOn:
                SetBodyAction(bodyActionType, lockPos, 0.2f);
                TurnRight();
                SetAnimatorFloat(BodyPart.Body, "Speed", speed);
                SetAnimatorBool(BodyPart.Body, "Walk", false);
                SetAnimatorBool(BodyPart.Head, "Walk", false);
                SetAnimatorTrigger(BodyPart.Body, "RideOn");
                break;
            case BodyActionType.RideOff:
                SetBodyAction(bodyActionType, lockPos, 0.2f);
                TurnRight();
                SetAnimatorFloat(BodyPart.Body, "Speed", speed);
                SetAnimatorBool(BodyPart.Body, "Walk", false);
                SetAnimatorBool(BodyPart.Head, "Walk", false);
                SetAnimatorTrigger(BodyPart.Body, "RideOff");
                break;
        }
    }
    public void PlayHeadAction(HeadActionType headAction, float speed = 1)
    {
        switch (headAction)
        {
            case HeadActionType.Eat:
                SetAnimatorTrigger(BodyPart.Head, "Eat");
                break;
            case HeadActionType.Pick:
                SetAnimatorTrigger(BodyPart.Head, "Pick");
                break;
            case HeadActionType.Work:
                Debug.Log("aaahead");
                SetAnimatorTrigger(BodyPart.Head, "Work");
                break;
        }
    }
    public void PlayHandAction(HandActionType handAction, float speed = 1)
    {
        switch (handAction)
        {
            case HandActionType.Eat:
                SetAnimatorTrigger(BodyPart.Hand, "Eat");
                break;
            case HandActionType.Work:
                SetAnimatorTrigger(BodyPart.Hand, "Work");
                break;
            case HandActionType.Pick:
                SetAnimatorTrigger(BodyPart.Hand, "Pick");
                break;
            case HandActionType.PunchRight:
                SetAnimatorTrigger(BodyPart.Hand, "PunchRight");
                break;
            case HandActionType.PunchLeft:
                SetAnimatorTrigger(BodyPart.Hand, "PunchLeft");
                break;
        }
    }

    #endregion
    #region//动画
    public virtual void SetAnimatorTrigger(BodyPart bodyPart, string name)
    {

    }
    public virtual void SetAnimatorFloat(BodyPart bodyPart, string name, float val)
    {

    }
    public virtual void SetAnimatorBool(BodyPart bodyPart, string name, bool val)
    {

    }
    public virtual void SetAnimatorFunc(BodyPart bodyPart, Func<string, bool> func)
    {

    }
    public virtual void Shake()
    {

    }
    public virtual void Flash()
    {

    }
    #endregion
    #region//转向
    [HideInInspector]
    public Vector2 faceDir;
    [HideInInspector]
    public Vector2 turnDir;
    protected bool turnRight = true;
    protected bool faceRight = true;
    /// <summary>
    /// 面向左边
    /// </summary>
    public virtual void FaceLeft()
    {
        faceRight = false;
    }
    /// <summary>
    /// 面向右边
    /// </summary>
    public virtual void FaceRight()
    {
        faceRight = true;
    }
    /// <summary>
    /// 转向左边
    /// </summary>
    public virtual void TurnLeft()
    {
        turnRight = false;
        if (faceRight) { FaceRight(); }
        else { FaceLeft(); }
    }
    /// <summary>
    /// 转向右边
    /// </summary>
    public virtual void TurnRight()
    {
        turnRight = true;
        if (faceRight) { FaceRight(); }
        else { FaceLeft(); }
    }
    #endregion
    #region//步伐
    protected int int_StepAudioIndex = 0;
    public void SetStep(int stepAudioIndex)
    {
        int_StepAudioIndex = stepAudioIndex;
    }
    public virtual void PlayStep()
    {
        if (int_StepAudioIndex > 0) AudioManager.Instance.Play3DEffect(int_StepAudioIndex + new System.Random().Next(0, 9), transform.position);
    }
    public virtual void PlaySpeedEffect(float dt)
    {

    }
    #endregion
    #region//隐藏
    /// <summary>
    /// 隐藏角色
    /// </summary>
    public virtual void HideActor()
    {

    }
    /// <summary>
    /// 隐藏角色身体
    /// </summary>
    public virtual void ShowActorBody(bool show)
    {

    }
    /// <summary>
    /// 显示角色
    /// </summary>
    public virtual void ShowActor()
    {

    }
    #endregion
    #region//骑乘状态

    public SpriteRenderer spriteRenderer_VehicleFront; 
    private SpriteRenderer spriteRenderer_VehicleFrontRef;
    protected VehicleState vehicleState;
    protected bool bool_Ride = false;
    public virtual void ShowAsRider(bool on,ActorManager vehicle)
    {
        bool_Ride = on;
        spriteRenderer_VehicleFront.enabled = bool_Ride;
        if (bool_Ride)
        {
            spriteRenderer_VehicleFrontRef = vehicle.bodyController.spriteRenderer_VehicleFront;
        }
        else
        {
            spriteRenderer_VehicleFrontRef = null;
            trans_Offset.localPosition = Vector3.zero;
            trans_Offset.localScale = Vector3.one;
        }
    }
    public virtual void ShowAsVehicle(bool on, ActorManager rider)
    {
        spriteRenderer_VehicleFront.enabled = !on;
    }
    public void UpdateVehicleFront()
    {
        if (bool_Ride)
        {
            spriteRenderer_VehicleFront.sprite = spriteRenderer_VehicleFrontRef.sprite;
            trans_Offset.localPosition = spriteRenderer_VehicleFrontRef.transform.localPosition + new Vector3(0, 0.0625f, 0);
            trans_Offset.localScale = spriteRenderer_VehicleFrontRef.transform.lossyScale;
        }
        else
        {
        }
    }
    #endregion
    #region//下水状态
    protected bool bool_StandOnWater = false;
    protected bool bool_EnterWater = false; 
    protected float float_WaveCD = 0.5f;
    protected float float_WaveTimer;
    protected Vector3 vector_WaveOffset = new Vector3(0, -0f, 0);
    public virtual void UpdateWave(float dt)
    {
        if (bool_EnterWater)
        {
            if (float_WaveTimer < -float_WaveCD)
            {
                float_WaveTimer = float_WaveCD;
                SetAnimatorTrigger(BodyPart.Hand, "Swim");
                SetAnimatorTrigger(BodyPart.Head, "Swim");
            }
            else
            {
                //if (float_WaveTimer > 0) LiquidManager.Instance.AddWave(transform.position + vector_WaveOffset);
                if (float_Speed > 0.1) LiquidManager.Instance.AddWave(transform.position + vector_WaveOffset);
                float_WaveTimer -= dt;
            }
        }
        else
        {
        }
    }
    public virtual void StandOnWater(bool on)
    {
        if (on) EnterWater();
        else ExitWater();
    }
    public virtual void EnterWater()
    {
        bool_EnterWater = true;
    }
    public virtual void ExitWater()
    {
        bool_EnterWater = false;
    }

    #endregion
    #region//死亡
    public virtual void Dead()
    {

    }
    #endregion
}
public enum BodyPart
{
    Body,
    Head,
    Hand,
}
public struct BodyAction
{
    public BodyActionType bodyActionType;
    public Vector2? bodyActionPos;
    public float bodyLockTime;
    public void SetBodyAction(BodyActionType type, Vector2? pos, float lockTime = 0)
    {
        bodyActionType = type;
        bodyActionPos = pos;
        bodyLockTime = lockTime;
    }
}
public enum BodyActionType
{
    Default,
    Idle,
    Walk,
    Stagger,
    LayToRight,
    LayToLeft,
    LayToUp,
    SitToRight,
    SitToLeft,
    SitToDown,
    RideOn,
    RideOff,
    Swim,
}
public enum HeadActionType
{
    Default,
    Eat,
    Pick,
    Work
}
public enum HandActionType
{
    Default,
    Eat,
    Pick,
    PunchRight,
    PunchLeft,
    Work,
}