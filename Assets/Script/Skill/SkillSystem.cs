using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class SkillSystem 
{
    
}
/// <summary>
/// …¡±‹≤Ω∑•
/// </summary>
public class Skill_1001 : SkillBase
{
    public override void ClickSpace()
    {
        bindActor.NetManager.networkRigidbody.Rigidbody.velocity = bindActor.face * 25;
        bindActor.BodyController.SetBodyTrigger("Roll", 2.5f, null);
        bindActor.BodyController.SetHeadTrigger("Roll", 2.5f, null);
        bindActor.BodyController.SetLegTrigger("Roll", 2.5f, null);

        //bindActor.NetManager.networkRigidbody.Rigidbody.AddRelativeForce(bindActor.face * 10, ForceMode2D.Impulse);
        //bindActor.NetManager.networkRigidbody.Rigidbody.MovePosition(bindActor.transform.position);
        base.ClickSpace();
    }
}
/// <summary>
/// …¡±‹≤Ω∑•
/// </summary>
public class Skill_1002 : SkillBase
{
    public override void ClickSpace()
    {
        bindActor.NetManager.networkRigidbody.Rigidbody.velocity = bindActor.face * 50;
        bindActor.BodyController.SetBodyTrigger("Roll", 2.5f, null);
        bindActor.BodyController.SetHeadTrigger("Roll", 2.5f, null);
        bindActor.BodyController.SetLegTrigger("Roll", 2.5f, null);

        //bindActor.NetManager.networkRigidbody.Rigidbody.AddRelativeForce(bindActor.face * 10, ForceMode2D.Impulse);
        //bindActor.NetManager.networkRigidbody.Rigidbody.MovePosition(bindActor.transform.position);
        base.ClickSpace();
    }
}