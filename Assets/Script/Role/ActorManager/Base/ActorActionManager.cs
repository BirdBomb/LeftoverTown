using Fusion;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class ActorActionManager 
{
    private ActorManager actorManager;
    private BodyController_Base bodyController;
    private LayerMask layerMask_ItemObj;
    private LayerMask layerMask_Wall;
    private System.Random random = new System.Random();
    public void Bind(ActorManager actorManager)
    {
        this.actorManager = actorManager;
        bodyController = actorManager.bodyController;
        layerMask_ItemObj = LayerMask.GetMask("ItemObj");
        layerMask_Wall = LayerMask.GetMask("TileObj_Wall");
    }
    #region//物体相关
    /// <summary>
    /// 捡起
    /// </summary>
    /// <param name="radiu"></param>
    public void State_PickUp(float radiu)
    {
        var items = Physics2D.OverlapCircleAll(actorManager.transform.position, radiu, layerMask_ItemObj);
        foreach (Collider2D item in items)
        {
            if (item.gameObject.transform.parent.TryGetComponent(out ItemNetObj obj))
            {
                actorManager.actorNetManager.RPC_LocalInput_PickItemManual(obj.Object.Id);
                break;
            }
        }
    }
    /// <summary>
    /// 快速持握
    /// </summary>
    public void ItemHand_Switch(int index)
    {
        List<ItemData> items = actorManager.actorNetManager.Local_ItemBag_Get();
        index = index % items.Count;
        ItemData oldBagItem = items[index];
        ItemData oldHandItem = actorManager.actorNetManager.Local_ItemHand;
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemBag_Change()
        {
            index = index,
            itemData = oldHandItem,
        });
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemHand_Change()
        {
            oldItem = oldHandItem,
            newItem = oldBagItem,
        });
    }
    /// <summary>
    /// 快速穿戴
    /// </summary>
    /// <param name="index"></param>
    public void ItemHead_Switch(int index)
    {
        List<ItemData> items = actorManager.actorNetManager.Local_ItemBag_Get();
        index = index % items.Count;
        ItemData oldBagItem = items[index];
        ItemData oldHeadItem = actorManager.actorNetManager.Local_ItemHead;
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemBag_Change()
        {
            index = index,
            itemData = oldHeadItem,
        });
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemHead_Change()
        {
            oldItem = oldHeadItem,
            newItem = oldBagItem,
        });
    }
    /// <summary>
    /// 快速穿戴
    /// </summary>
    /// <param name="index"></param>
    public void ItemBody_Switch(int index)
    {
        List<ItemData> items = actorManager.actorNetManager.Local_ItemBag_Get();
        index = index % items.Count;
        ItemData oldBagItem = items[index];
        ItemData oldBodyItem = actorManager.actorNetManager.Local_ItemBody;
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemBag_Change()
        {
            index = index,
            itemData = oldBodyItem,
        });
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemBody_Change()
        {
            oldItem = oldBodyItem,
            newItem = oldBagItem,
        });
    }
    /// <summary>
    /// 快速替换饰品
    /// </summary>
    public void ItemAccessory_Switch(int index)
    {
        List<ItemData> items = actorManager.actorNetManager.Local_ItemBag_Get();
        index = index % items.Count;
        ItemData oldBagItem = items[index];
        ItemData oldConsumablesItem = actorManager.actorNetManager.Local_ItemConsumables;
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemBag_Change()
        {
            index = index,
            itemData = oldConsumablesItem,
        });
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemAccessory_Change()
        {
            oldItem = oldConsumablesItem,
            newItem = oldBagItem,
        });
    }
    /// <summary>
    /// 快速替换耗材
    /// </summary>
    public void ItemConsumables_Switch(int index)
    {
        List<ItemData> items = actorManager.actorNetManager.Local_ItemBag_Get();
        index = index % items.Count;
        ItemData oldBagItem = items[index];
        ItemData oldConsumablesItem = actorManager.actorNetManager.Local_ItemConsumables;
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemBag_Change()
        {
            index = index,
            itemData = oldConsumablesItem,
        });
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemConsumables_Change()
        {
            oldItem = oldConsumablesItem,
            newItem = oldBagItem,
        });
    }
    /// <summary>
    /// 快速丢弃
    /// </summary>
    /// <param name="index"></param>
    public void Drop(int index)
    {
        List<ItemData> items = actorManager.actorNetManager.Local_ItemBag_Get();
        index = index % items.Count;
        ItemData dropItem = items[index];
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemBag_Change()
        {
            index = index,
            itemData = new ItemData(),
        });
        if (dropItem.I != 0)
        {
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryDropItem()
            {
                item = dropItem
            });
        }
    }

    #endregion
    #region//身体相关
    public void FaceTo(Vector2 dir)
    {
        bodyController.faceDir = dir.normalized;
        if(bodyController.faceDir.x >= 0)
        {
            bodyController.FaceRight();
        }
        else
        {
            bodyController.FaceLeft();
        }
    }
    public void TurnTo(Vector2 dir)
    {
        bodyController.turnDir = dir.normalized;
        if (bodyController.turnDir.x > 0.1f) { bodyController.TurnRight(); return; }
        if (bodyController.turnDir.x < -0.1f) { bodyController.TurnLeft(); return; }
    }
    public bool LookAt(ActorManager who, float view)
    {
        if (who == null || who.actorState == ActorState.Dead) return false;
        float viewSqr = view * view;
        float distanceSqr = (actorManager.transform.position - who.transform.position).sqrMagnitude;
        if (distanceSqr >= viewSqr || Physics2D.LinecastAll(who.transform.position, actorManager.transform.position, layerMask_Wall).Length > 0)
        {
            return false;
        }
        return true;
    }
    public bool HearTo(ActorManager who, float view)
    {
        if (who == null || who.actorState == ActorState.Dead) return false;
        float viewSqr = view * view;
        float distanceSqr = (actorManager.transform.position - who.transform.position).sqrMagnitude;
        if (distanceSqr >= viewSqr)
        {
            return false;
        }
        return true;
    }
    #endregion
    #region//伤害相关
    /// <summary>
    /// 是否是合法攻击目标
    /// </summary>
    /// <param name="actor"></param>
    /// <param name="damageTarget"></param>
    /// <returns></returns>
    public bool CheckApplyDamageTarget(ActorManager actor, DamageTarget damageTarget)
    {
        switch(damageTarget)
        {
            case DamageTarget.All: return true;
            case DamageTarget.WithoutMe:
                {
                    if (actor == actorManager ||
                        actor == actorManager.vehicleManager.actorManager_Vehicle ||
                        actor == actorManager.vehicleManager.actorManager_Rider)
                    {
                        return false;
                    }
                    break;
                }
        }
        return true;
    }
    /// <summary>
    /// 造成伤害
    /// </summary>
    /// <param name="val"></param>
    /// <param name="damageState"></param>
    /// <param name="to"></param>
    public void ApplyDamageToActors(int val, DamageState damageState,DamageTarget damageTarget, List<ActorManager> targets,out List<ApplyActorDamageCallBack> callBack)
    {
        callBack = new List<ApplyActorDamageCallBack>();
        foreach (ActorManager actor in targets)
        {
            if (!CheckApplyDamageTarget(actor, damageTarget)) continue;
            int realDamage = actor.actionManager.TakeDamage(val, damageState, actorManager);
            callBack.Add(new ApplyActorDamageCallBack(actor, realDamage));
        }
    }
    public void ApplyDamageToActor(int val, DamageState damageState, DamageTarget damageTarget, ActorManager target, out ApplyActorDamageCallBack callBack)
    {
        if (!CheckApplyDamageTarget(target, damageTarget)) 
        {
            callBack = null;
            return;
        }
        int realDamage = target.actionManager.TakeDamage(val, damageState, actorManager);
        callBack = new ApplyActorDamageCallBack(target, realDamage);
    }

    /// <summary>
    /// 造成伤害
    /// </summary>
    /// <param name="val"></param>
    /// <param name="damageState"></param>
    /// <param name="targets"></param>
    /// <param name="callBack"></param>
    public void ApplyDamageToBuilidngs(int val,DamageState damageState,List<BuildingObj> targets,out List<ApplyBuildingDamageCallBack> callBack)
    {
        callBack = new List<ApplyBuildingDamageCallBack>();
        foreach (BuildingObj buildingObj in targets)
        {
            int realDamage = buildingObj.Local_TakeDamage(val, damageState, actorManager.actorNetManager);
            callBack.Add(new ApplyBuildingDamageCallBack(buildingObj, realDamage));
        }
    }
    /// <summary>
    /// 受到伤害
    /// </summary>
    /// <param name="val"></param>
    /// <param name="damageState"></param>
    /// <param name="from"></param>
    public int TakeDamage(int val, DamageState damageState, ActorManager from)
    {
        NetworkId networkId = new NetworkId();
        if (actorManager.actorState == ActorState.Dead) return 0;
        if (from == null || from.actorNetManager.Object.Id == null) 
        {

        }
        else
        {
            networkId = from.actorNetManager.Object.Id;
        }
        switch (damageState)
        {
            case DamageState.AttackPiercingDamage:
                {
                    val = val > actorManager.actorNetManager.Net_Armor ? val - actorManager.actorNetManager.Net_Armor : 0;
                    actorManager.actorNetManager.RPC_AllClient_HpChange(-val, (int)HpChangeReason.AttackDamage, networkId);
                }
                break;
            case DamageState.AttackSlashingDamage:
                {
                    val = val > actorManager.actorNetManager.Net_Armor ? val - actorManager.actorNetManager.Net_Armor : 0;
                    actorManager.actorNetManager.RPC_AllClient_HpChange(-val, (int)HpChangeReason.AttackDamage, networkId);
                }
                break;
            case DamageState.AttackBludgeoningDamage:
                {
                    val = val > actorManager.actorNetManager.Net_Armor ? val - actorManager.actorNetManager.Net_Armor : 0;
                    actorManager.actorNetManager.RPC_AllClient_HpChange(-val, (int)HpChangeReason.AttackDamage, networkId);
                }
                break;
            case DamageState.MagicDamage:
                {
                    val = val > actorManager.actorNetManager.Net_Resistance ? val - actorManager.actorNetManager.Net_Resistance : 0;
                    actorManager.actorNetManager.RPC_AllClient_HpChange(-val, (int)HpChangeReason.MagicDamage, networkId);
                }
                break;
            case DamageState.RealDamage:
                {
                    actorManager.actorNetManager.RPC_AllClient_HpChange(-val, (int)HpChangeReason.RealDamage, networkId);
                }
                break;
        }
        actorManager.bodyController.Flash();
        actorManager.bodyController.Shake();
        return val;
    }

    #endregion
    #region//死亡相关
    public void Dead()
    {
        if (actorManager.actorState != ActorState.Dead)
        {
            actorManager.vehicleManager.AllClient_CleanVehicle();
            actorManager.vehicleManager.AllClient_CleanRider();
            actorManager.actorState = ActorState.Dead;
            if (actorManager.actorAuthority.isPlayer)
            {
                DeadPlayer();
            }
            else
            {
                DeadNPC();
            }
        }
    }
    private void DeadPlayer()
    {
        if (actorManager.actorAuthority.isState)
        {
            MessageBroker.Default.Publish(new GameEvent.GameEvent_State_KillPlayer()
            {
                playerRef = actorManager.actorPlayerRef
            });
        }
    }
    private void DeadNPC()
    {
        if (actorManager.actorAuthority.isState)
        {
            DropDown();
            Despawn();
        }
    }
    /// <summary>
    /// 销毁
    /// </summary>
    public void Despawn()
    {
        actorManager.actorNetManager.Runner.Despawn(actorManager.actorNetManager.Object);
    }
    /// <summary>
    /// 掉落所有物体
    /// </summary>
    public void DropDown()
    {
        List<ItemData> dropItem = actorManager.actorNetManager.Local_ItemBag_Get();
        if (actorManager.actorNetManager.Local_ItemHand.I > 0) dropItem.Add(actorManager.actorNetManager.Local_ItemHand);
        if (actorManager.actorNetManager.Local_ItemHead.I > 0) dropItem.Add(actorManager.actorNetManager.Local_ItemHead);
        if (actorManager.actorNetManager.Local_ItemBody.I > 0) dropItem.Add(actorManager.actorNetManager.Local_ItemBody);
        if (actorManager.actorNetManager.Local_ItemAccessory.I > 0) dropItem.Add(actorManager.actorNetManager.Local_ItemAccessory);
        if (actorManager.actorNetManager.Local_ItemConsumables.I > 0) dropItem.Add(actorManager.actorNetManager.Local_ItemConsumables);

        int weightCount = 0;
        if (actorManager.actorConfig.LootFixed_List != null)
        {
            LootFixedInfo[] lootFixedInfos = actorManager.actorConfig.LootFixed_List;
            for (int i = 0; i < lootFixedInfos.Length; i++)
            {
                Type type = Type.GetType("Item_" + lootFixedInfos[i].ID.ToString());
                ((ItemBase)Activator.CreateInstance(type)).StaticAction_InitData(lootFixedInfos[i].ID, out ItemData initData);
                initData.C = (short)random.Next(lootFixedInfos[i].CountMin, lootFixedInfos[i].CountMax);
                dropItem.Add(initData);
            }
        }
        if (actorManager.actorConfig.LootRandom_List != null)
        {
            LootRandomInfo[] lootRandomInfos = actorManager.actorConfig.LootRandom_List;
            for (int i = 0; i < lootRandomInfos.Length; i++)
            {
                weightCount += (int)lootRandomInfos[i].Weight;
            }
            for (int i = 0; i < actorManager.actorConfig.LootRandom_Count; i++)
            {
                int temp = 0;
                for (int j = 0; j < lootRandomInfos.Length; j++)
                {
                    temp += (int)lootRandomInfos[i].Weight;
                    if (random.Next(0, weightCount) < temp)
                    {
                        Type type = Type.GetType("Item_" + lootRandomInfos[i].ID.ToString());
                        ((ItemBase)Activator.CreateInstance(type)).StaticAction_InitData(lootRandomInfos[i].ID, out ItemData initData);
                        initData.C = (short)random.Next(lootRandomInfos[i].CountMin, lootRandomInfos[i].CountMax);
                        dropItem.Add(initData);
                        break;
                    }
                }
            }
        }

        for (int i = 0; i < dropItem.Count; i++)
        {
            float angle = i * (360 / dropItem.Count);
            float angleRad = angle * Mathf.Deg2Rad;

            float x = Mathf.Cos(angleRad) * 0.5f;
            float y = Mathf.Sin(angleRad) * 0.5f;
            Debug.Log(dropItem[i].I + "/" + dropItem[i].C);
            Vector3 position = new Vector3(x, y, 0);
            MessageBroker.Default.Publish(new GameEvent.GameEvent_State_SpawnItem()
            {
                itemData = dropItem[i],
                itemOwner = new NetworkId(),
                pos = position + actorManager.transform.position,
            });
        }
    }
    #endregion
    #region//速度相关
    public float Client_GetSpeed()
    {
        float temp = actorManager.actorNetManager.Net_SpeedCommon * 0.1f;
        if (actorManager.actorAuthority.isPlayer)
        {
            temp = Client_CalculateMoveSpeedBySan(temp);
            temp = Client_CalculateMoveSpeedByFloor(temp);
        }
        else
        {
            if (actorManager.vehicleManager.vehicleState == VehicleState.AsVehicle)
            {
                temp *= 2;
            }
        }
        return temp;
    }
    public float Client_CalculateMoveSpeedBySan(float speed)
    {
        //float sanRatio = actorManager.sanManager.GetSanRatio();
        float sanRatio = 1;
        float sanOffset = (sanRatio < 0.3f) ? Mathf.Lerp(0.5f, 1.0f, sanRatio / 0.3f) : 1f;
        speed = speed * sanOffset;
        return speed; 
    }
    public float Client_CalculateMoveSpeedByFloor(float speed)
    {
        speed = actorManager.pathManager.ForAll_GetSpeedOffset() * speed;
        return speed;
    }
    #endregion
    #region//护甲与魔抗相关
    public void Local_ResetArmor()
    {
        int armor = 0;
        armor = actorManager.itemManager.itemBase_OnHead != null ? actorManager.itemManager.itemBase_OnHead.OnHead_CalculateArmor(armor) : armor;
        armor = actorManager.itemManager.itemBase_OnBody != null ? actorManager.itemManager.itemBase_OnBody.OnBody_CalculateArmor(armor) : armor;
        armor = actorManager.buffManager.Local_CalculateArmor(armor);
        actorManager.actorNetManager.RPC_LocalInput_ChangeArmor((short)armor);
    }
    public void Local_ResetResistance()
    {
        int resistance = 0;
        resistance = actorManager.itemManager.itemBase_OnHead != null ? actorManager.itemManager.itemBase_OnHead.OnHead_CalculateResistance(resistance) : resistance;
        resistance = actorManager.itemManager.itemBase_OnBody != null ? actorManager.itemManager.itemBase_OnBody.OnBody_CalculateResistance(resistance) : resistance;
        resistance = actorManager.buffManager.Local_CalculateResistance(resistance);
        actorManager.actorNetManager.RPC_LocalInput_ChangeResistance((short)resistance);
    }
    #endregion
    #region//其他相关
    /// <summary>
    /// 受力
    /// </summary>
    /// <param name="dir"></param>
    /// <param name="force"></param>
    public void Client_TakeForce(Vector2 dir, short force)
    {
        return;
        actorManager.actorNetManager.RPC_AllClient_AddForce(dir, force);
    }
    public bool PayCoin(int coin)
    {
        if (actorManager.actorNetManager.Local_Coin >= coin)
        {
            actorManager.actorNetManager.RPC_Local_PayCoin(coin);
            return true;
        }
        else
        {
            return false;
        }
    }
    public int EarnCoin(int coin)
    {
        actorManager.actorNetManager.RPC_Local_EarnCoin(coin);
        return actorManager.actorNetManager.Local_Coin;
    }
    /// <summary>
    /// 犯罪
    /// </summary>
    /// <param name="fine"></param>
    public void AllClient_Commit(CommitState commit, short fine)
    {
        MessageBroker.Default.Publish(new GameEvent.GameEvent_AllClient_SomeoneCommit
        {
            actor = actorManager,
            commit = commit,
            fine = fine
        });
    }
    /// <summary>
    /// 悬赏
    /// </summary>
    /// <param name="val"></param>
    public void AllClient_SetFine(CommitState commit, short val)
    {
        switch (commit)
        {
            case CommitState.Steal:
                break;
            case CommitState.Attacking:
                break;
            case CommitState.Murder:
                break;
        }
        if (actorManager.actorNetManager.Local_Fine < val)
        {
            actorManager.actorNetManager.RPC_Local_ChangeFine(val);
        }
    }
    public void ClearFine()
    {
        if (actorManager.actorAuthority.isLocal)
        {
            actorManager.actorNetManager.RPC_Local_ChangeFine(0);
        }
    }
    public void AllClient_SendEmoji(short emojiID, float duration, bool loop, short distance)
    {
        actorManager.actorNetManager.RPC_LocalInput_SendEmoji(emojiID, duration, loop, distance);
    }
    public void AllClient_SendText(string text, short emojiID, float duration, bool loop, short distance)
    {
       actorManager.actorNetManager.RPC_LocalInput_SendText(text, emojiID, duration, loop, distance);
    }
    #endregion
    #region//Play
    public void PlayDead(float speed, Func<string,bool> func)
    {
        bodyController.SetAnimatorTrigger(BodyPart.Body, "Dead");
        bodyController.SetAnimatorFunc(BodyPart.Body, func);
    }
    public void PlayTakeDamage(float speed)
    {
        
    }
    public void PlayBloodSplash(float sleep,Vector3 dir)
    {
        GameObject effect = PoolManager.Instance.GetEffectObj("Effect/Effect_Blood");
        effect.GetComponent<EffectBase>().SetEffect(dir);
        effect.transform.position = actorManager.transform.position;
    }
    public void PlayPickUp(float speed, Func<string,bool> func)
    {
        bodyController.SetAnimatorTrigger(BodyPart.Head, "Pick");
        bodyController.SetAnimatorTrigger(BodyPart.Hand, "Pick");
        bodyController.SetAnimatorFunc(BodyPart.Hand, func);
    }

    #endregion

}
/// <summary>
/// 伤害回调
/// </summary>
public class ApplyActorDamageCallBack
{
    public ActorManager target;
    public int realDamage;
    public ApplyActorDamageCallBack(ActorManager actor, int damage)
    {
        target = actor;
        realDamage = damage;
    }
}
/// <summary>
/// 伤害回调
/// </summary>
public struct ApplyBuildingDamageCallBack
{
    public BuildingObj target;
    public int realDamage;
    public ApplyBuildingDamageCallBack(BuildingObj buildingObj, int damage)
    {
        target = buildingObj;
        realDamage = damage;
    }
}