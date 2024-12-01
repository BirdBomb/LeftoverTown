using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Fusion;
using Fusion.Sockets;
using UnityEditor.Timeline;

public class TileObj_Bed : TileObj
{
    /// <summary>
    /// 床摆放方向
    /// </summary>
    private BedDir bedDir;
    /// <summary>
    /// 床上睡觉的人
    /// </summary>
    private ActorManager bedSleeper = null;
    /// <summary>
    /// 床的主人
    /// </summary>
    private ActorManager bedOwner = null;
    [SerializeField, Header("床渲染")]
    private SpriteRenderer spriteRenderer_Bed;
    [SerializeField, Header("床单渲染")]
    private SpriteRenderer spriteRenderer_Sheet;
    [SerializeField, Header("床头渲染")]
    private SpriteRenderer spriteRenderer_Front;
    [SerializeField, Header("头部位置")]
    private Transform tran_Head;
    [SerializeField, Header("头发渲染")]
    private SpriteRenderer spriteRenderer_Hair;
    [SerializeField, Header("睡眠特效")]
    private GameObject obj_Effect;

    [SerializeField, Header("设置UI")]
    private GameObject obj_SetUI;
    [SerializeField, Header("设置方向")]
    private TextMeshProUGUI text_SetDir;

    public Sprite sprite_Bed;
    public Sprite sprite_BedToDown;
    public Sprite sprite_BedToLeft;
    public Sprite sprite_BedToRight;
    public Sprite sprite_BedToUp;

    public Sprite sprite_BedSheetToDown_Empty;
    public Sprite sprite_BedSheetToLeft_Empty;
    public Sprite sprite_BedSheetToRight_Empty;
    public Sprite sprite_BedSheetToUp_Empty;

    public Sprite sprite_BedSheetToDown_Full;
    public Sprite sprite_BedSheetToLeft_Full;
    public Sprite sprite_BedSheetToRight_Full;
    public Sprite sprite_BedSheetToUp_Full;

    public Sprite sprite_BedToDown_Front;
    public Sprite sprite_BedToLeft_Front;
    public Sprite sprite_BedToRight_Front;
    public Sprite sprite_BedToUp_Front;

    #region//瓦片生命周期
    public override void Init()
    {
        MessageBroker.Default.Receive<GameEvent.GameEvent_Local_SomeoneFindTargetTile>().Subscribe(_ =>
        {
            if (_.id == 1005)
            {
                if (!bedOwner)
                {
                    if (Vector3.Distance(_.pos, transform.position) < _.distance)
                    {
                        bedOwner = _.actor;
                        _.action(this);
                    }
                }
            }
        }).AddTo(this);
        base.Init();
    }
    #endregion

    #region//绘制
    public override void Draw(int seed)
    {
        DrawBedBase();
        DrawBedSheet();
        DrawSleeper();
        ShakeBed();
        base.Draw(seed);
    }
    private void DrawBedBase()
    {
        if (bedDir == BedDir.Up)
        {
            MapManager.Instance.GetBuildingObj(bindTile._posInCell + Vector3Int.up, out TileObj tileObj);
            if (tileObj && tileObj.bindTile.name == "WoodBed_Part")
            {
                spriteRenderer_Bed.sprite = sprite_BedToUp;
            }
            else
            {
                bedDir = BedDir.None;
            }

        }
        else if (bedDir == BedDir.Down)
        {
            MapManager.Instance.GetBuildingObj(bindTile._posInCell + Vector3Int.down, out TileObj tileObj);
            if (tileObj && tileObj.bindTile.name == "WoodBed_Part")
            {
                spriteRenderer_Bed.sprite = sprite_BedToDown;
            }
            else
            {
                bedDir = BedDir.None;
            }
        }
        else if (bedDir == BedDir.Left)
        {
            MapManager.Instance.GetBuildingObj(bindTile._posInCell + Vector3Int.left, out TileObj tileObj);
            if (tileObj && tileObj.bindTile.name == "WoodBed_Part")
            {
                spriteRenderer_Bed.sprite = sprite_BedToLeft;
            }
            else
            {
                bedDir = BedDir.None;
            }
        }
        else if (bedDir == BedDir.Right)
        {
            MapManager.Instance.GetBuildingObj(bindTile._posInCell + Vector3Int.right, out TileObj tileObj);
            if (tileObj && tileObj.bindTile.name == "WoodBed_Part")
            {
                spriteRenderer_Bed.sprite = sprite_BedToRight;
            }
            else
            {
                bedDir = BedDir.None;
            }
        }
        else 
        {
            spriteRenderer_Bed.sprite = sprite_Bed;
        }
    }
    private void DrawBedSheet()
    {
        if (bedDir == BedDir.Up)
        {
            if (bedSleeper != null)
            {
                spriteRenderer_Sheet.sprite = sprite_BedSheetToUp_Full;
            }
            else
            {
                spriteRenderer_Sheet.sprite = sprite_BedSheetToUp_Empty;
            }
        }
        else if (bedDir == BedDir.Down)
        {
            if (bedSleeper != null)
            {
                spriteRenderer_Sheet.sprite = sprite_BedSheetToDown_Full;
            }
            else
            {
                spriteRenderer_Sheet.sprite = sprite_BedSheetToDown_Empty;
            }
        }
        else if (bedDir == BedDir.Left)
        {
            if (bedSleeper != null)
            {
                spriteRenderer_Sheet.sprite = sprite_BedSheetToLeft_Full;
            }
            else
            {
                spriteRenderer_Sheet.sprite = sprite_BedSheetToLeft_Empty;
            }
        }
        else if (bedDir == BedDir.Right)
        {
            if (bedSleeper != null)
            {
                spriteRenderer_Sheet.sprite = sprite_BedSheetToRight_Full;
            }
            else
            {
                spriteRenderer_Sheet.sprite = sprite_BedSheetToRight_Empty;
            }
        }
        else
        {
            spriteRenderer_Sheet.sprite = null;
        }

    }
    private void DrawSleeper()
    {
        if (bedSleeper != null)
        {
            spriteRenderer_Sheet.DOKill();
            spriteRenderer_Sheet.transform.localScale = Vector3.one;
            spriteRenderer_Sheet.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);

            obj_Effect.SetActive(true);
            tran_Head.gameObject.SetActive(true);
            spriteRenderer_Hair.sprite = bedSleeper.GetComponent<HumanBodyController>().spriteRenderer_Hair.sprite;
            spriteRenderer_Hair.color = bedSleeper.GetComponent<HumanBodyController>().spriteRenderer_Hair.color;
        }
        else
        {
            obj_Effect.SetActive(false);
            tran_Head.gameObject.SetActive(false);
        }

        if (bedDir == BedDir.Up)
        {
            spriteRenderer_Front.sprite = sprite_BedToUp_Front;
            tran_Head.rotation = Quaternion.Euler(0, 0, 180);
        }
        else if (bedDir == BedDir.Down)
        {
            spriteRenderer_Front.sprite = sprite_BedToDown_Front;
            tran_Head.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (bedDir == BedDir.Left)
        {
            spriteRenderer_Front.sprite = sprite_BedToLeft_Front;
            tran_Head.rotation = Quaternion.Euler(0, 0, -90);
        }
        else if (bedDir == BedDir.Right)
        {
            spriteRenderer_Front.sprite = sprite_BedToRight_Front;
            tran_Head.rotation = Quaternion.Euler(0, 0, 90);
        }
        else
        {
            spriteRenderer_Front.sprite = null;
        }
    }
    private void ShakeBed()
    {
        transform.DOKill();
        transform.localScale = Vector3.one;
        transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
    }
    #endregion
    #region//交互
    public override bool PlayerHolding(PlayerController player)
    {
        /*床未展开*/
        if (bedDir == BedDir.None)
        {
            Vector3Int bedTo = player.actorManager.record_curTile._posInCell;
            if (bedTo.x > bindTile._posInCell.x)
            {
                ShowSettingUI("右展");
            }
            else if (bedTo.x < bindTile._posInCell.x)
            {
                ShowSettingUI("左展");
            }
            else if (bedTo.y > bindTile._posInCell.y)
            {
                ShowSettingUI("上展");
            }
            else if (bedTo.y < bindTile._posInCell.y)
            {
                ShowSettingUI("下展");
            }
            else
            {
                ShowSettingUI("选择方向");
            }
        }
        return true;
    }
    public override bool PlayerRelease(PlayerController player)
    {
        HideSettingUI();
        return true;
    }
    public override void PlayerInput(PlayerController player, KeyCode code)
    {
        if (code == KeyCode.F)
        {
            if (bedDir == BedDir.None)
            {
                SetBed(player.actorManager);
            }
        }
        base.PlayerInput(player, code);
    }
    public override void ActorStandOn(ActorManager actor)
    {
        if (bedDir != BedDir.None && bedSleeper == null)
        {
            SleepOnBed(actor);
        }
        base.ActorStandOn(actor);
    }
    public void SetBed(ActorManager actor)
    {
        Vector3Int bedTo = actor.record_curTile._posInCell;
        if (actor.Tool_GetMyTileWithOffset(Vector3Int.zero).name == "Default")
        {
            if (bedTo.x > bindTile._posInCell.x)
            {
                if (bedSleeper != null)
                {
                    TryToChangeInfo("right" + "_" + bedSleeper.GetComponent<NetworkObject>().Id.Raw);
                }
                else
                {
                    TryToChangeInfo("right" + "_");
                }
            }
            else if (bedTo.x < bindTile._posInCell.x)
            {
                if (bedSleeper != null)
                {
                    TryToChangeInfo("left" + "_" + bedSleeper.GetComponent<NetworkObject>().Id.Raw);
                }
                else
                {
                    TryToChangeInfo("left" + "_");
                }
            }
            else if (bedTo.y > bindTile._posInCell.y)
            {
                if (bedSleeper != null)
                {
                    TryToChangeInfo("up" + "_" + bedSleeper.GetComponent<NetworkObject>().Id.Raw);
                }
                else
                {
                    TryToChangeInfo("up" + "_");
                }
            }
            else if (bedTo.y < bindTile._posInCell.y)
            {
                if (bedSleeper != null)
                {
                    TryToChangeInfo("down" + "_" + bedSleeper.GetComponent<NetworkObject>().Id.Raw);
                }
                else
                {
                    TryToChangeInfo("down" + "_");
                }
            }
            MessageBroker.Default.Publish(new MapEvent.MapEvent_LocalTile_ChangeBuilding()
            {
                buildingID = 1005,
                buildingPos = actor.record_curTile._posInCell
            });
            HideSettingUI();
        }

    }
    public void SleepOnBed(ActorManager actor)
    {
        Debug.Log(">>" + bedDir.ToString());
        actor.NetManager.RPC_LocalInput_AddBuff(100, bindTile._posInCell.ToString());
        uint sleeperID = actor.NetManager.Object.Id.Raw;
        if (bedDir == BedDir.Right)
        {
            TryToChangeInfo("right" + "_" + sleeperID);
        }
        else if (bedDir == BedDir.Left)
        {
            TryToChangeInfo("left" + "_" + sleeperID);
        }
        else if (bedDir == BedDir.Up)
        {
            TryToChangeInfo("up" + "_" + sleeperID);
        }
        else if (bedDir == BedDir.Down)
        {
            TryToChangeInfo("down" + "_" + sleeperID);
        }
    }
    public void GetUpOnBed(ActorManager actor)
    {
        if (bedSleeper && actor == bedSleeper)
        {
            if (bedDir == BedDir.Right)
            {
                TryToChangeInfo("right" + "_");
            }
            else if (bedDir == BedDir.Left)
            {
                TryToChangeInfo("left" + "_");
            }
            else if (bedDir == BedDir.Up)
            {
                TryToChangeInfo("up" + "_");
            }
            else if (bedDir == BedDir.Down)
            {
                TryToChangeInfo("down" + "_");
            }
        }
    }
    private void ShowSettingUI(string val)
    {
        obj_SetUI.SetActive(true);
        obj_SetUI.transform.DOKill();
        obj_SetUI.transform.localScale = Vector3.one;
        obj_SetUI.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);

        text_SetDir.text = val;
    }
    private void HideSettingUI()
    {
        obj_SetUI.SetActive(false);
    }
    #endregion
    #region//信息更新与上传
    public override void TryToChangeInfo(string info)
    {
        base.TryToChangeInfo(info);
    }
    public override void TryToUpdateInfo(string info)
    {
        //Debug.Log(info);
        bedSleeper = null;
        string[] infos = info.Split('_');
        if (infos.Length > 0)
        {
            if (infos[0] == "")
            {
                bedDir = BedDir.None;
            }
            else if (infos[0] == "up")
            {
                bedDir = BedDir.Up;
            }
            else if (infos[0] == "down")
            {
                bedDir = BedDir.Down;
            }
            else if (infos[0] == "left")
            {
                bedDir = BedDir.Left;
            }
            else if (infos[0] == "right")
            {
                bedDir = BedDir.Right;
            }

        }
        if (infos.Length > 1)
        {
            if (infos[1] != "")
            {
                uint id = uint.Parse(infos[1]);
                NetworkId networkId = new NetworkId();
                networkId.Raw = id;
                NetworkObject target = Fusion.NetworkRunner.Instances[0].FindObject(networkId);
                if (target != null && Vector3.Distance(target.transform.position, transform.position) < 2)
                {
                    bedSleeper = target.GetComponent<ActorManager>();
                }
            }
        }
        DrawBedSheet();
        DrawSleeper();

        base.TryToUpdateInfo(info);
    }
    #endregion
}
public enum BedDir
{
    None,
    Up,
    Down,
    Left,
    Right
}