using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingObj_Door : BuildingObj
{
    [SerializeField]
    private GameObject obj_Door;
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private Sprite sprite_Door_H_Close;
    [SerializeField]
    private Sprite sprite_Door_H_Open;
    [SerializeField]
    private Sprite sprite_Door_V_Close;
    [SerializeField]
    private Sprite sprite_Door_V_Open;
    private DoorDir doorDir;
    private DoorState doorState = DoorState.Close;
    public override void Draw()
    {
        ChangeDoorDir();
        ChangeDoorState(doorState);
        base.Draw();
    }
    private void ChangeDoorDir()
    {
        Around around = MapManager.Instance.CheckBuilding_FourSide((id) => { return id > 0; }, buildingTile.tilePos);
        if (around.U && around.D)
        {
            doorDir = DoorDir.V;
        }
        else
        {
            doorDir = DoorDir.H;
        }
    }
    private void ChangeDoorState(DoorState state)
    {
        if (doorState != state)
        {
            doorState = state;
            obj_Door.transform.DOKill();
            obj_Door.transform.localScale = Vector3.one;
            obj_Door.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
            AudioManager.Instance.PlayEffect(1011, transform.position);
        }
        if (doorDir == DoorDir.V)
        {
            if (doorState == DoorState.Open)
            {
                spriteRenderer.sprite = sprite_Door_V_Open;
            }
            else
            {
                spriteRenderer.sprite = sprite_Door_V_Close;
            }
        }
        if (doorDir == DoorDir.H)
        {
            if (doorState == DoorState.Open)
            {
                spriteRenderer.sprite = sprite_Door_H_Open;
            }
            else
            {
                spriteRenderer.sprite = sprite_Door_H_Close;
            }
        }
    }
    public override bool ActorNearby(ActorManager actor)
    {
        ChangeDoorState(DoorState.Open);
        return true;
    }
    public override bool ActorFaraway(ActorManager actor)
    {
        ChangeDoorState(DoorState.Close);
        return true;
    }
    public enum DoorDir
    {
        H, V
    }
    public enum DoorState
    {
        Open, Close
    }
}
//public class TileObj_Door : TileObj
//{
//    [SerializeField, Header("门物体")]
//    private GameObject obj_Door;
//    [SerializeField, Header("门渲染")]
//    private SpriteRenderer spriteRenderer_Door;
//    [SerializeField, Header("门碰撞")]
//    private BoxCollider2D collider_Door;
//    [SerializeField, Header("解锁UI")]
//    private GameObject obj_UnlockUI;
//    [SerializeField, Header("解锁几率")]
//    private Text text_UnlockPro;
//    [SerializeField, Header("解锁进度条")]
//    private Image image_UnlockBar;
//    [SerializeField, Header("锁定UI")]
//    private GameObject obj_LockUI;
//    [SerializeField, Header("开锁几率"), Range(0, 100)]
//    private int lockPro;
//    /// <summary>
//    /// 是否打开
//    /// </summary>
//    private bool _open = false;
//    /// <summary>
//    /// 是否上锁
//    /// </summary>
//    private bool _lock = false;
//    public Sprite sprite_Door_Close;
//    public Sprite sprite_Door_Lock;
//    public Sprite sprite_Door_Open;
//    #region//绘制
//    public override void Draw(int seed)
//    {
//        ChangeDoorState(DoorState.Close);
//        base.Draw(seed);
//    }
//    #endregion
//    #region//瓦片交互
//    public override bool PlayerHolding(PlayerCoreLocal player)
//    {
//        if (_lock)
//        {
//            CalculateUnLockProAndTime(player.actorManager_Bind, out int pro, out float time);
//            UpdateUnlockUI(pro);
//            OpenOrCloseUnlockUI(true);
//        }
//        else
//        {
//            if (CanLockDoor(player.actorManager_Bind))
//            {
//                OpenOrCloseLockUI(true);
//            }
//        }
//        return true;
//    }
//    public override bool PlayerRelease(PlayerCoreLocal player)
//    {
//        OpenOrCloseLockUI(false);
//        OpenOrCloseUnlockUI(false);
//        return true;
//    }
//    public override bool ActorNearby(ActorManager actor)
//    {
//        if (!_lock)
//        {
//            ChangeDoorState(DoorState.Open);
//        }
//        return true;
//    }
//    public override bool ActorFaraway(ActorManager actor)
//    {
//        ChangeDoorState(DoorState.Close);
//        return true;
//    }
//    public override void ActorInputKeycode(ActorManager actor, KeyCode code)
//    {
//        if (code == KeyCode.F)
//        {
//            if (_lock)
//            {
//                TryToUnlockDoor(actor);
//            }
//            else
//            {
//                TryToLockDoor(actor);
//            }
//        }
//        base.ActorInputKeycode(actor, code);
//    }
//    /// <summary>
//    /// 显示隐藏锁定界面
//    /// </summary>
//    /// <param name="open"></param>
//    private void OpenOrCloseLockUI(bool open)
//    {
//        obj_LockUI.transform.DOKill();
//        if (open)
//        {
//            obj_LockUI.SetActive(true);
//            obj_LockUI.transform.localScale = Vector3.one;
//            obj_LockUI.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
//        }
//        else
//        {
//            obj_LockUI.transform.DOScale(Vector3.zero, 0.1f).OnComplete(() =>
//            {
//                obj_LockUI.SetActive(false);
//            });
//        }
//    }
//    /// <summary>
//    /// 显示隐藏解锁界面
//    /// </summary>
//    /// <param name="open"></param>
//    private void OpenOrCloseUnlockUI(bool open)
//    {
//        obj_UnlockUI.transform.DOKill();
//        obj_UnlockUI.transform.localScale = Vector3.one;
//        image_UnlockBar.DOKill();
//        image_UnlockBar.transform.localScale = new Vector3(0, 1, 1);
//        if (open)
//        {
//            obj_UnlockUI.SetActive(true);
//            obj_UnlockUI.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
//        }
//        else
//        {
//            obj_UnlockUI.transform.DOScale(Vector3.zero, 0.1f).OnComplete(() =>
//            {
//                obj_UnlockUI.SetActive(false);
//            });
//        }
//    }
//    private void ShakeUnlockUI()
//    {
//        obj_UnlockUI.transform.DOKill();
//        obj_UnlockUI.SetActive(true);
//        obj_UnlockUI.transform.localScale = Vector3.one;
//        obj_UnlockUI.transform.DOShakePosition(0.2f, 5, 100);
//        AudioManager.Instance.PlayEffect(1010, transform.position);
//    }
//    /// <summary>
//    /// 更新解锁UI面板
//    /// </summary>
//    /// <param name="player"></param>
//    private void UpdateUnlockUI(int pro)
//    {
//        text_UnlockPro.text = pro + "%";
//    }
//    #endregion
//    #region//门
//    /// <summary>
//    /// 更改门的状态
//    /// </summary>
//    /// <param name="doorState"></param>
//    private void ChangeDoorState(DoorState doorState)
//    {
//        if (doorState == DoorState.Open)
//        {
//            if (!_open)
//            {
//                obj_Door.transform.DOKill();
//                obj_Door.transform.localScale = Vector3.one;
//                obj_Door.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
//                AudioManager.Instance.PlayEffect(1011, transform.position);
//            }
//            _open = true;
//            collider_Door.enabled = false;
//            spriteRenderer_Door.sprite = sprite_Door_Open;
//        }
//        if (doorState == DoorState.Close)
//        {
//            if (_open)
//            {
//                obj_Door.transform.DOKill();
//                obj_Door.transform.localScale = Vector3.one;
//                obj_Door.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
//                AudioManager.Instance.PlayEffect(1009, transform.position);
//            }
//            _open = false;
//            collider_Door.enabled = true;
//            if (_lock)
//            {
//                spriteRenderer_Door.sprite = sprite_Door_Lock;
//            }
//            else
//            {
//                spriteRenderer_Door.sprite = sprite_Door_Close;
//            }
//        }
//    }
//    /// <summary>
//    /// 尝试解锁
//    /// </summary>
//    private void TryToUnlockDoor(ActorManager actor)
//    {
//        UnlockDoor(actor);
//    }
//    /// <summary>
//    /// 解锁
//    /// </summary>
//    /// <param name="player"></param>
//    private void UnlockDoor(ActorManager actor)
//    {
//        CalculateUnLockProAndTime(actor, out int pro, out float time);
//        UpdateUnlockUI(pro);
//        if (CalculateUnlockResult(pro))
//        {
//            UnlockDoorSucces(time, actor);
//        }
//        else
//        {
//            UnlockDoorFail(time, actor);
//        }
//    }
//    /// <summary>
//    /// 计算开锁结果
//    /// </summary>
//    /// <param name="pro"></param>
//    /// <returns></returns>
//    private bool CalculateUnlockResult(int pro)
//    {
//        UnityEngine.Random.InitState(System.DateTime.Now.Second);
//        if (UnityEngine.Random.Range(0, 100) < pro)
//        {
//            return true;
//        }
//        else
//        {
//            return false;
//        }
//    }
//    /// <summary>
//    /// 解锁成功
//    /// </summary>
//    private void UnlockDoorSucces(float time, ActorManager actor)
//    {
//        AudioManager.Instance.PlayEffect(1008, transform.position);
//        image_UnlockBar.DOKill();
//        image_UnlockBar.color = Color.green;
//        image_UnlockBar.transform.localScale = new Vector3(0, 1, 1);
//        image_UnlockBar.transform.DOScaleX(1, time).SetEase(Ease.InSine).OnComplete(() =>
//        {
//            LockDoor("");
//            OpenOrCloseLockUI(true);
//            OpenOrCloseUnlockUI(false);
//            actor.AllClient_ShowText("成功", Color.green, 24, Vector2.up * 2);
//        });
//    }
//    /// <summary>
//    /// 解锁失败
//    /// </summary>
//    private void UnlockDoorFail(float time, ActorManager actor)
//    {
//        UnityEngine.Random.InitState(System.DateTime.Now.Second);
//        float val = UnityEngine.Random.Range(0.1f, 0.95f);

//        AudioManager.Instance.PlayEffect(1008, transform.position);
//        image_UnlockBar.DOKill();
//        image_UnlockBar.color = Color.green;
//        image_UnlockBar.transform.localScale = new Vector3(0, 1, 1);
//        image_UnlockBar.transform.DOScaleX(1 * val, time * val).SetEase(Ease.InSine).OnComplete(() =>
//        {
//            OpenOrCloseLockUI(false);
//            ShakeUnlockUI();
//            image_UnlockBar.color = Color.red;
//            actor.AllClient_ShowText("失败", Color.red, 24, Vector2.up * 2);
//        });

//    }
//    /// <summary>
//    /// 是否可以锁门
//    /// </summary>
//    /// <param name="player"></param>
//    /// <returns></returns>
//    private bool CanLockDoor(ActorManager actor)
//    {
//        return actor.actorAuthority.isLocal && actor.actorNetManager.Net_ItemInHand.Item_ID == 9901;
//    }
//    /// <summary>
//    /// 尝试锁门
//    /// </summary>
//    /// <param name="player"></param>
//    private void TryToLockDoor(ActorManager actor)
//    {
//        if (CanLockDoor(actor))
//        {
//            LockDoor(actor.actorNetManager.Net_ItemInHand.Item_Seed.ToString());
//            OpenOrCloseLockUI(false);
//            OpenOrCloseUnlockUI(false);
//        }
//    }
//    /// <summary>
//    /// 锁门
//    /// </summary>
//    /// <param name="password"></param>
//    private void LockDoor(string password)
//    {
//        TryToChangeInfo(password);
//    }
//    /// <summary>
//    /// 计算解锁概率与需要的时间
//    /// </summary>
//    /// <param name="player">玩家</param>
//    /// <param name="pro">概率</param>
//    /// <param name="time">时间</param>
//    private void CalculateUnLockProAndTime(ActorManager actor, out int pro, out float time)
//    {
//        if (actor.actorNetManager.Net_ItemInHand.Item_ID == 9005 && actor.actorNetManager.Net_ItemInHand.Item_Seed.ToString() == info)
//        {
//            pro = 100;
//            time = 0.2f;
//        }
//        else
//        {
//            pro = lockPro;
//            if (pro > 70) { time = 4; }
//            else if (pro > 40) { time = 4.5f; }
//            else
//            {
//                time = 5f;
//            }
//        }
//    }

//    #endregion
//    #region//信息更新与上传
//    public override void TryToUpdateInfo(string info)
//    {
//        base.TryToUpdateInfo(info);
//        if (info == "")
//        {
//            _lock = false;
//            ChangeDoorState(DoorState.Open);
//        }
//        else
//        {
//            _lock = true;
//            ChangeDoorState(DoorState.Close);
//        }
//    }
//    #endregion
//}
