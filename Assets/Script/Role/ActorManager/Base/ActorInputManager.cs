using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class ActorInputManager
{
    private ActorManager actorManager;
    
    public void Bind(ActorManager actorManager)
    {
        this.actorManager = actorManager;
    }
    #region//玩家输入
    private List<Action<ActorManager, KeyCode>> actions_InputKeycode = new List<Action<ActorManager, KeyCode>>();
    public void InputKeycode(KeyCode keyCode,bool on = true)
    {
        for (int i = 0; i < actions_InputKeycode.Count; i++)
        {
            actions_InputKeycode[i].Invoke(actorManager, keyCode);
        }
        if (keyCode == KeyCode.Space)
        {
            actorManager.actionManager.State_PickUp(0.5f);
        }
    }
    public void InputNumKeycode(int num)
    {
        MessageBroker.Default.Publish(new UIEvent.UIEvent_TryUseItemInBag() { index = num });
    }
    public void InputMouse(float leftPressTime, float rightPressTime, bool hasStateAuthority, bool hasInputAuthority)
    {
        if (leftPressTime > 0)
        {
            actorManager.itemManager.itemBase_OnHand.OnHand_UpdateLeftPress(leftPressTime, hasStateAuthority, hasInputAuthority, true);
        }
        else
        {
            actorManager.itemManager.itemBase_OnHand.OnHand_ReleaseLeftPress(hasStateAuthority, hasInputAuthority, true);
        }
        if (rightPressTime > 0)
        {
            actorManager.itemManager.itemBase_OnHand.OnHand_UpdateRightPress(rightPressTime, hasStateAuthority, hasInputAuthority, true);
        }
        else
        {
            actorManager.itemManager.itemBase_OnHand.OnHand_ReleaseRightPress(hasStateAuthority, hasInputAuthority, true);
        }
    }
    public void InputFace(Vector2 dir)
    {
        actorManager.actionManager.FaceTo(dir);
        actorManager.itemManager.itemBase_OnHand?.OnHand_UpdateMousePos(dir);
    }
    public Vector3 State_InputMove(float deltaTime, Vector2 dir)
    {
        if (actorManager.vehicleManager.vehicleState == VehicleState.AsRider)
        {
            Vector3 pos = actorManager.vehicleManager.actorManager_Vehicle.inputManager.State_InputMove(deltaTime, dir);
            actorManager.actorNetManager.State_UpdateNetworkRigidbody(pos + new Vector3(0, -0.0625f, 0), 0, deltaTime);
            return pos;
        }
        else
        {
            return actorManager.actorNetManager.State_MoveNetworkRigidbody(dir, deltaTime);
        }
    }
    public Vector3 Local_InputMove(float deltaTime, Vector2 dir)
    {
        if (actorManager.vehicleManager.vehicleState == VehicleState.AsRider)
        {
            Vector3 pos = actorManager.vehicleManager.actorManager_Vehicle.inputManager.Local_InputMove(deltaTime, dir);
            actorManager.actorNetManager.Local_UpdateSimulationRigidbody(pos + new Vector3(0, -0.0625f, 0), 0, deltaTime);
            return pos;
        }
        else
        {
            return actorManager.actorNetManager.Local_MoveNetworkRigidbody(dir, deltaTime);
        }
    }
    public void Local_AddInputKeycodeAction(Action<ActorManager, KeyCode> action)
    {
        if (!actions_InputKeycode.Contains(action)) actions_InputKeycode.Add(action);
    }
    public void Local_RemoveInputKeycodeAction(Action<ActorManager, KeyCode> action)
    {
        if (actions_InputKeycode.Contains(action)) actions_InputKeycode.Remove(action);
    }
    #endregion
    #region//AI输入
    private float float_MouseRightPressTimer;
    private float float_MouseLeftPressTimer;
    private Vector3 vector3_MouseLocation;
    /// <summary>
    /// 操作输入
    /// </summary>
    /// <param name="dt"></param>
    /// <param name="inputType"></param>
    /// <returns>有效输入操作</returns>
    public bool Simulate_InputMousePress(float dt, MouseInputType inputType)
    {
        if (inputType == MouseInputType.PressRightThenPressLeft)
        {
            float_MouseRightPressTimer += dt;
            if (actorManager.itemManager.itemBase_OnHand.OnHand_UpdateRightPress(float_MouseRightPressTimer, actorManager.actorAuthority.isState, actorManager.actorAuthority.isLocal, false))
            {
                float_MouseLeftPressTimer += dt;
                if (actorManager.itemManager.itemBase_OnHand.OnHand_UpdateLeftPress(float_MouseLeftPressTimer, actorManager.actorAuthority.isState, actorManager.actorAuthority.isLocal, false))
                {
                    float_MouseRightPressTimer = 0;
                    float_MouseLeftPressTimer = 0;
                    actorManager.itemManager.itemBase_OnHand.OnHand_ReleaseLeftPress(actorManager.actorAuthority.isState, actorManager.actorAuthority.isLocal, false);
                    actorManager.itemManager.itemBase_OnHand.OnHand_ReleaseRightPress(actorManager.actorAuthority.isState, actorManager.actorAuthority.isLocal, false);
                    return true;
                }
            }
        }
        return false;
    }
    public void Simulate_InputMousePos(Vector3 pos)
    {
        vector3_MouseLocation = pos;
        actorManager.actionManager.FaceTo(vector3_MouseLocation - actorManager.transform.position);
        actorManager.itemManager.itemBase_OnHand.OnHand_UpdateMousePos(vector3_MouseLocation - actorManager.transform.position);
    }
    public void Simulate_SmoothlyResetMouseDir(Vector3 dir)
    {
        actorManager.actionManager.FaceTo(dir);
        actorManager.itemManager.itemBase_OnHand.OnHand_UpdateMousePos(dir);
    }
    /// <summary>
    /// 鼠标按键按下方法
    /// </summary>
    public enum MouseInputType
    {
        /// <summary>
        /// 先按压右键再按压左键
        /// </summary>
        PressRightThenPressLeft
    }
    #endregion
}
