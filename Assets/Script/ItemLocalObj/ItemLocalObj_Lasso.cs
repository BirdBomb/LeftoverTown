using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemLocalObj_Lasso : ItemLocalObj
{
    [SerializeField]
    private SpriteRenderer spriteRenderer_Hand;
    private float config_ThrowCD = 1; 
    private float float_NextThrowTiming = 0;
    [Header("绳索力量")]
    public short int_Power = 0;
    [Header("绳索长度")]
    public float float_Distance = 0;
    public LineRenderer line_Lasso;
    protected InputData inputData = new InputData();
    protected ActorManager actorManager_Catch;
    private void FixedUpdate()
    {
        if (inputData.leftPressTimer == 0 && float_NextThrowTiming > 0)
        {
            float_NextThrowTiming -= Time.fixedDeltaTime;
        }
        if (actorManager_Catch)
        {
            line_Lasso.SetPosition(0, transform.position);
            line_Lasso.SetPosition(1, actorManager_Catch.transform.position);
            float distance = Vector2.Distance(transform.position, actorManager_Catch.transform.position);
            if (distance >= float_Distance)
            {
                Vector2 dir = transform.position - actorManager_Catch.transform.position;
                actorManager_Catch.actorNetManager.State_SetExternalForce(dir * Mathf.Lerp(0, (float)int_Power, distance - float_Distance), ActorNetManager.ExternalForceType.Pull);
            }
            else
            {
                actorManager_Catch.actorNetManager.State_CleanExternalForce(ActorNetManager.ExternalForceType.Pull);
            }
        }
    }
    public override void HoldingStart(ActorManager owner, BodyController_Human body)
    {
        actorManager = owner;

        body.AddItemOnRightHand(gameObject, Vector3.zero, Quaternion.identity, Vector3.one);
        spriteRenderer_Hand.color = body.ShowRightHand(false).color;

        base.HoldingStart(owner, body);
    }
    public override bool PressLeftMouse(float time, ActorAuthority actorAuthority)
    {
        if (inputData.leftPressTimer >= float_NextThrowTiming)
        {
            float_NextThrowTiming += config_ThrowCD + 0.1f;
            TryCatch(transform.position);
        }
        inputData.leftPressTimer = time;
        return base.PressLeftMouse(time, actorAuthority);
    }
    public override void ReleaseLeftMouse()
    {
        if (inputData.leftPressTimer > 0)
        {
            float_NextThrowTiming -= inputData.leftPressTimer;
            inputData.leftPressTimer = 0;
        }
        base.ReleaseLeftMouse();
    }
    public override void UpdateMousePos(Vector3 mouse)
    {
        inputData.mousePosition = mouse;
        base.UpdateMousePos(mouse);
    }
    public virtual void TryCatch(Vector2 pos)
    {
        RaycastHit2D[] raycastHit2Ds = Physics2D.CircleCastAll(pos, 1, Vector2.zero);
        foreach (RaycastHit2D hit2D in raycastHit2Ds)
        {
            if (hit2D.collider.CompareTag("Actor"))
            {
                if (hit2D.collider.isTrigger && hit2D.transform.TryGetComponent(out ActorManager actor) && actor != actorManager)
                {
                    Catch(actor);
                }
            }
        }
    }
    public virtual void Catch(ActorManager actor)
    {
        actorManager_Catch = actor;
    }
}
