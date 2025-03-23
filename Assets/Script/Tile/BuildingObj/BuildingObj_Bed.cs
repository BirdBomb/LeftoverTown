using DG.Tweening;
using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingObj_Bed : BuildingObj
{
    [SerializeField, Header("床板渲染")]
    private SpriteRenderer spriteRenderer_Bed;
    [SerializeField, Header("床单渲染")]
    private SpriteRenderer spriteRenderer_Sheet;
    [SerializeField, Header("头部位置")]
    private Transform tran_Head;
    [SerializeField, Header("头发渲染")]
    private SpriteRenderer spriteRenderer_Hair;
    [SerializeField, Header("睡眠特效")]
    private GameObject obj_Effect;

    public Sprite sprite_Bed_Base;
    public Sprite sprite_BedSheet_Empty;
    public Sprite sprite_BedSheet_Full;

    private ActorManager actorManager_Sleeper;

    public override void Draw()
    {
        DrawSleep();
        base.Draw();
    }
    public override void ActorStandOn(ActorManager actor)
    {
        if (actor.TryGetComponent<BodyController_Human>(out _))
        {
            uint sleeperID = actor.actorNetManager.Object.Id.Raw;
            ChangeInfo(sleeperID.ToString());
        }
        base.ActorStandOn(actor);
    }
    private void DrawSleep()
    {
        if (actorManager_Sleeper != null)
        {
            spriteRenderer_Sheet.DOKill();
            spriteRenderer_Sheet.transform.localScale = Vector3.one;
            spriteRenderer_Sheet.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);

            obj_Effect.SetActive(true);
            tran_Head.gameObject.SetActive(true);
            spriteRenderer_Hair.sprite = actorManager_Sleeper.GetComponent<BodyController_Human>().spriteRenderer_Hair.sprite;
            spriteRenderer_Hair.color = actorManager_Sleeper.GetComponent<BodyController_Human>().spriteRenderer_Hair.color;
            spriteRenderer_Sheet.sprite = sprite_BedSheet_Full;
        }
        else
        {
            obj_Effect.SetActive(false);
            tran_Head.gameObject.SetActive(false);
            spriteRenderer_Sheet.sprite = sprite_BedSheet_Empty;
        }
    }
    private void ShakeBed()
    {
        transform.DOKill();
        transform.localScale = Vector3.one;
        transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
    }
    public override void ChangeInfo(string info)
    {
        base.ChangeInfo(info);
    }
    public override void UpdateInfo(string info)
    {
        actorManager_Sleeper = null;
        if (!info.Equals(""))
        {
            uint id = uint.Parse(info);
            NetworkId networkId = new NetworkId();
            networkId.Raw = id;
            NetworkObject target = Fusion.NetworkRunner.Instances[0].FindObject(networkId);
            if (target != null && Vector3.Distance(target.transform.position, transform.position) < 2)
            {
                actorManager_Sleeper = target.GetComponent<ActorManager>();
            }
        }
        Draw();
        base.UpdateInfo(info);
    }
}
