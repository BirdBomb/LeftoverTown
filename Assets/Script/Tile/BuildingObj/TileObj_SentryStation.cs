using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class TileObj_SentryStation : TileObj
{
    private ActorManager owner;
    #region//ÍßÆ¬ÉúÃüÖÜÆÚ
    public override void Init()
    {
        MessageBroker.Default.Receive<GameEvent.GameEvent_Local_SomeoneFindTargetTile>().Subscribe(_ =>
        {
            if (_.id == 1023)
            {
                if (Vector3.Distance(_.pos, transform.position) < _.distance)
                {
                    _.action(this);
                }
            }
        }).AddTo(this);
        CreateNPC();
        base.Init();
    }
    private void OnDestroy()
    {
        DestroyNPC();
    }
    #endregion
    #region//¸ÚÉÚ
    private void CreateNPC()
    {
        MessageBroker.Default.Publish(new GameEvent.GameEvent_State_SpawnActor()
        {
            name = "Actor/NPC_TownPatrol",
            pos = transform.position,
            callBack = BindOwner
        });
    }
    private void DestroyNPC()
    {
        if (owner != null)
        {
            Destroy(owner.gameObject);
        }
    }
    public void BindOwner(ActorManager actor)
    {
        owner = actor;
    }
    #endregion
}
