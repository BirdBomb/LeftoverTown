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
//    [SerializeField, Header("������")]
//    private GameObject obj_Door;
//    [SerializeField, Header("����Ⱦ")]
//    private SpriteRenderer spriteRenderer_Door;
//    [SerializeField, Header("����ײ")]
//    private BoxCollider2D collider_Door;
//    [SerializeField, Header("����UI")]
//    private GameObject obj_UnlockUI;
//    [SerializeField, Header("��������")]
//    private Text text_UnlockPro;
//    [SerializeField, Header("����������")]
//    private Image image_UnlockBar;
//    [SerializeField, Header("����UI")]
//    private GameObject obj_LockUI;
//    [SerializeField, Header("��������"), Range(0, 100)]
//    private int lockPro;
//    /// <summary>
//    /// �Ƿ��
//    /// </summary>
//    private bool _open = false;
//    /// <summary>
//    /// �Ƿ�����
//    /// </summary>
//    private bool _lock = false;
//    public Sprite sprite_Door_Close;
//    public Sprite sprite_Door_Lock;
//    public Sprite sprite_Door_Open;
//    #region//����
//    public override void Draw(int seed)
//    {
//        ChangeDoorState(DoorState.Close);
//        base.Draw(seed);
//    }
//    #endregion
//    #region//��Ƭ����
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
//    /// ��ʾ������������
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
//    /// ��ʾ���ؽ�������
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
//    /// ���½���UI���
//    /// </summary>
//    /// <param name="player"></param>
//    private void UpdateUnlockUI(int pro)
//    {
//        text_UnlockPro.text = pro + "%";
//    }
//    #endregion
//    #region//��
//    /// <summary>
//    /// �����ŵ�״̬
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
//    /// ���Խ���
//    /// </summary>
//    private void TryToUnlockDoor(ActorManager actor)
//    {
//        UnlockDoor(actor);
//    }
//    /// <summary>
//    /// ����
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
//    /// ���㿪�����
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
//    /// �����ɹ�
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
//            actor.AllClient_ShowText("�ɹ�", Color.green, 24, Vector2.up * 2);
//        });
//    }
//    /// <summary>
//    /// ����ʧ��
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
//            actor.AllClient_ShowText("ʧ��", Color.red, 24, Vector2.up * 2);
//        });

//    }
//    /// <summary>
//    /// �Ƿ��������
//    /// </summary>
//    /// <param name="player"></param>
//    /// <returns></returns>
//    private bool CanLockDoor(ActorManager actor)
//    {
//        return actor.actorAuthority.isLocal && actor.actorNetManager.Net_ItemInHand.Item_ID == 9901;
//    }
//    /// <summary>
//    /// ��������
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
//    /// ����
//    /// </summary>
//    /// <param name="password"></param>
//    private void LockDoor(string password)
//    {
//        TryToChangeInfo(password);
//    }
//    /// <summary>
//    /// ���������������Ҫ��ʱ��
//    /// </summary>
//    /// <param name="player">���</param>
//    /// <param name="pro">����</param>
//    /// <param name="time">ʱ��</param>
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
//    #region//��Ϣ�������ϴ�
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
