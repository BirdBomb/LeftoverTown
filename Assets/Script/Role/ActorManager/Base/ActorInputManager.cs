using System;
using System.Collections;
using System.Collections.Generic;
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
    private Action<ActorManager, float, Vector2, bool> action_InputMove = null;
    private List<Action<ActorManager, KeyCode>> actions_InputKeycode = new List<Action<ActorManager, KeyCode>>();
    private float float_shiftPressTimer = 0;

    public void InputKeycode(KeyCode keyCode)
    {
        for (int i = 0; i < actions_InputKeycode.Count; i++)
        {
            actions_InputKeycode[i].Invoke(actorManager, keyCode);
        }
        if (keyCode == KeyCode.Q)
        {
            actorManager.actionManager.PickUp();
        }
    }
    public void InputMouse(float leftPressTime, float rightPressTime, bool hasStateAuthority, bool hasInputAuthority)
    {
        if (leftPressTime > 0)
        {
            actorManager.itemManager.itemBase_OnHand.Holding_UpdateLeftPress(leftPressTime, hasStateAuthority, hasInputAuthority, true);
        }
        else
        {
            actorManager.itemManager.itemBase_OnHand.Holding_ReleaseLeftPress(hasStateAuthority, hasInputAuthority, true);
        }
        if (rightPressTime > 0)
        {
            actorManager.itemManager.itemBase_OnHand.Holding_UpdateRightPress(rightPressTime, hasStateAuthority, hasInputAuthority, true);
        }
        else
        {
            actorManager.itemManager.itemBase_OnHand.Holding_ReleaseRightPress(hasStateAuthority, hasInputAuthority, true);
        }
    }
    public void InputFace(Vector2 dir, bool hasStateAuthority, bool hasInputAuthority)
    {
        actorManager.actionManager.FaceTo(dir);
        if (actorManager.itemManager.itemBase_OnHand != null)
        {
            actorManager.itemManager.itemBase_OnHand.Holding_UpdateMousePos(dir);
        }
    }
    public void InputMove(float deltaTime, Vector2 dir, bool speedUp, bool hasStateAuthority, bool hasInputAuthority)
    {
        action_InputMove?.Invoke(actorManager, deltaTime, dir, speedUp);

        dir = dir.normalized;
        float commonSpeed = actorManager.actorNetManager.Net_SpeedCommon * 0.1f;
        float maxSpeed = actorManager.actorNetManager.Net_SpeedMax * 0.1f;
        float speed;
        if (speedUp)
        {
            if (actorManager.actionManager.EnSub((int)(deltaTime * 1000)))
            {
                if (float_shiftPressTimer < 1)
                {
                    float_shiftPressTimer += deltaTime;
                }
                else
                {
                    float_shiftPressTimer = 1;
                }
            }
            else
            {
                if (float_shiftPressTimer > deltaTime)
                {
                    float_shiftPressTimer -= deltaTime;
                }
                else
                {
                    float_shiftPressTimer = 0;
                }
            }
        }
        else
        {
            if (float_shiftPressTimer > deltaTime)
            {
                float_shiftPressTimer -= deltaTime;
            }
            else
            {
                float_shiftPressTimer = 0;
            }
            actorManager.actionManager.EnAdd((int)(deltaTime * actorManager.actorNetManager.Net_EnRelease));
        }
        if (float_shiftPressTimer > 1)
        {
            speed = Mathf.Lerp(commonSpeed, maxSpeed, 1);
        }
        else
        {
            speed = Mathf.Lerp(commonSpeed, maxSpeed, float_shiftPressTimer);
        }

        Vector2 velocity = new Vector2(dir.x * speed, dir.y * speed);
        Vector3 newPos = actorManager.transform.position + new UnityEngine.Vector3(velocity.x * deltaTime, velocity.y * deltaTime, 0);
        actorManager.actorNetManager.OnlyState_UpdateNetworkRigidbody(newPos, velocity.magnitude);


        //if (actorManager.vehicleManager_Bind == null)
        //{
        //}
    }
    public void Local_AddInputKeycodeAction(Action<ActorManager, KeyCode> action)
    {
        if (!actions_InputKeycode.Contains(action))
        {
            actions_InputKeycode.Add(action);
        }
    }
    public void Local_RemoveInputKeycodeAction(Action<ActorManager, KeyCode> action)
    {
        if (actions_InputKeycode.Contains(action))
        {
            actions_InputKeycode.Remove(action);
        }
    }
    public void AllClient_AddInputMove(Action<ActorManager, float, Vector2, bool> action)
    {
        action_InputMove = action;
    }
    public void AllClient_RemoveInputMove(Action<ActorManager, float, Vector2, bool> action)
    {
        action_InputMove = null;
    }

    #endregion
    #region//AI输入
    private float float_MouseRightPressTimer;
    private float float_MouseLeftPressTimer;
    private Vector3 vector3_MouseLocation;
    public bool Simulate_InputMousePress(float dt, MouseInputType inputType)
    {
        if (inputType == MouseInputType.PressRightThenPressLeft)
        {
            float_MouseRightPressTimer += dt;
            if (actorManager.itemManager.itemBase_OnHand.Holding_UpdateRightPress(float_MouseRightPressTimer, actorManager.actorAuthority.isState, actorManager.actorAuthority.isLocal, false))
            {
                float_MouseLeftPressTimer += dt;
                if (actorManager.itemManager.itemBase_OnHand.Holding_UpdateLeftPress(float_MouseLeftPressTimer, actorManager.actorAuthority.isState, actorManager.actorAuthority.isLocal, false))
                {
                    float_MouseRightPressTimer = 0;
                    float_MouseLeftPressTimer = 0;
                    actorManager.itemManager.itemBase_OnHand.Holding_ReleaseLeftPress(actorManager.actorAuthority.isState, actorManager.actorAuthority.isLocal, false);
                    actorManager.itemManager.itemBase_OnHand.Holding_ReleaseRightPress(actorManager.actorAuthority.isState, actorManager.actorAuthority.isLocal, false);
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
        actorManager.itemManager.itemBase_OnHand.Holding_UpdateMousePos(vector3_MouseLocation - actorManager.transform.position);
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
