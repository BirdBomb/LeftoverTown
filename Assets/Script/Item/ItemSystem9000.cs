using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.U2D;

public class ItemSystem9000 
{
}
/// <summary>
/// ��Լ
/// </summary>
public class Item_9001 : ItemBase
{
    #region//ʹ���߼�
    public override bool UpdateLeftPress(float pressTimer, bool state, bool input, bool player)
    {
        if (inputData.leftPressTimer == 0)
        {
            if (owner)
            {
                owner.BodyController.SetHandTrigger("PutUp", 1, (string str) =>
                {
                    if (input && str.Equals("PutUp"))
                    {
                        OnlyInput_PutUp();
                    }
                });
                owner.BodyController.SetHandTrigger("PutUp", 1, null);
            }
        }
        inputData.leftPressTimer = pressTimer;
        return true;
    }
    public override void ReleaseLeftPress(bool state, bool input, bool player)
    {
        inputData.leftPressTimer = 0;
        base.ReleaseLeftPress(state, input, player);
    }
    private void OnlyInput_PutUp()
    {
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TrySpawnActor()
        {
            name = "Actor/Zombie_Spray"
        });
    }
    #endregion
}
/// <summary>
/// ����ľ��
/// </summary>
public class Item_9002 : ItemBase
{

}
/// <summary>
/// ����ľ��
/// </summary>
public class Item_9003 : ItemBase
{

}
/// <summary>
/// ����
/// </summary>
public class Item_9004 : ItemBase
{

}
/// <summary>
/// Կ��
/// </summary>
public class Item_9005 : ItemBase
{

}
