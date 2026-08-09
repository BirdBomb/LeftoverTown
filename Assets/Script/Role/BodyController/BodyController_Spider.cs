using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyController_Spider : BodyController_BodyHead
{
    public Transform trans_Front;
    public Transform trans_Middle;
    public Transform trans_Back;
    public override void Start()
    {
        animaEventListen_Body.BindCommonEvent((x) =>
        {
            if (x.Equals("FrontStep"))
            {
                AllClient_AnimaEvent_Dusk_Front();
            }
            if (x.Equals("MiddleStep"))
            {
                AllClient_AnimaEvent_Dusk_Middle();
            }
            if (x.Equals("BackStep"))
            {
                AllClient_AnimaEvent_Dusk_Back();
            }
        });
    }

    #region//½Å²½»Ò³¾
    public void AllClient_AnimaEvent_Dusk_Front()
    {
        GameObject effect = PoolManager.Instance.GetEffectObj("Effect/Effect_StepDust_Middle");
        effect.transform.position = trans_Front.position;

    }
    public void AllClient_AnimaEvent_Dusk_Middle()
    {
        GameObject effect = PoolManager.Instance.GetEffectObj("Effect/Effect_StepDust_Middle");
        effect.transform.position = trans_Middle.position;

    }
    public void AllClient_AnimaEvent_Dusk_Back()
    {
        GameObject effect = PoolManager.Instance.GetEffectObj("Effect/Effect_StepDust_Middle");
        effect.transform.position = trans_Back.position;

    }
    #endregion

}
