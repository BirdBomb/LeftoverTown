using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class SkillSystem 
{
    
}
/// <summary>
/// Òþ²Ø
/// </summary>
public class Skill_100
{

}
/// <summary>
/// ÉÁ±Ü²½·¥
/// </summary>
public class Skill_1001 : SkillBase
{
    public override void ClickSpace()
    {
        bindActor.NetManager.networkRigidbody.Rigidbody.velocity = bindActor.BodyController.faceDir * 25;
        bindActor.BodyController.SetBodyTrigger("Roll", 2.5f, null);
        bindActor.BodyController.SetHeadTrigger("Roll", 2.5f, null);
        bindActor.BodyController.SetLegTrigger("Roll", 2.5f, null);
        GameObject effect = PoolManager.Instance.GetObject("Effect/Effect_BombSmoke");
        effect.transform.position = bindActor.BodyController.Tran_BothLeg.position;

        //bindActor.NetManager.networkRigidbody.Rigidbody.AddRelativeForce(bindActor.face * 10, ForceMode2D.Impulse);
        //bindActor.NetManager.networkRigidbody.Rigidbody.MovePosition(bindActor.transform.position);
        base.ClickSpace();
    }
}
/// <summary>
/// ¶ÏÊ½²½·¥
/// </summary>
public class Skill_1002 : SkillBase
{
    public override void ClickSpace()
    {
        bindActor.NetManager.networkRigidbody.Rigidbody.velocity = bindActor.BodyController.faceDir * 50;
        bindActor.BodyController.SetBodyTrigger("Roll", 2.5f, null);
        bindActor.BodyController.SetHeadTrigger("Roll", 2.5f, null);
        bindActor.BodyController.SetLegTrigger("Roll", 2.5f, null);
        GameObject effect = PoolManager.Instance.GetObject("Effect/Effect_BombSmoke");
        effect.transform.position = bindActor.BodyController.Tran_BothLeg.position;

        //bindActor.NetManager.networkRigidbody.Rigidbody.AddRelativeForce(bindActor.face * 10, ForceMode2D.Impulse);
        //bindActor.NetManager.networkRigidbody.Rigidbody.MovePosition(bindActor.transform.position);
        base.ClickSpace();
    }
}