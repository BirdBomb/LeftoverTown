using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TileObj_Door : TileObj
{
    [SerializeField, Header("������")]
    private GameObject obj_Door;
    [SerializeField, Header("����Ⱦ")]
    private SpriteRenderer spriteRenderer_Door;
    [SerializeField, Header("����ײ")]
    private BoxCollider2D collider_Door;
    [SerializeField, Header("����UI")]
    private GameObject obj_UnlockUI;
    [SerializeField, Header("��������")]
    private Text text_UnlockPro;
    [SerializeField, Header("����������")]
    private Image image_UnlockBar;
    [SerializeField, Header("����UI")]
    private GameObject obj_LockUI;
    [SerializeField, Header("��������"), Range(0, 100)]
    private int lockPro;
    /// <summary>
    /// �Ƿ��
    /// </summary>
    private bool _open = false;
    /// <summary>
    /// �Ƿ�����
    /// </summary>
    private bool _lock = false;
    /// <summary>
    /// �Ƿ���ֱ����
    /// </summary>
    private bool _vertical = false;
    public Sprite sprite_Door_H_Close;
    public Sprite sprite_Door_H_Lock;
    public Sprite sprite_Door_H_Open;
    public Sprite sprite_Door_V_Close;
    public Sprite sprite_Door_V_Lock;
    public Sprite sprite_Door_V_Open;
    #region//����
    public override void Draw(int seed)
    {
        if (MapManager.Instance.GetBuildingObj(bindTile._posInCell + Vector3Int.up, out TileObj tileObjUp) && MapManager.Instance.GetBuildingObj(bindTile._posInCell + Vector3Int.down, out TileObj tileObjDown))
        {
            _vertical = true;
        }
        else
        {
            _vertical = false;
        }
        ChangeDoorState(DoorState.Close);
        base.Draw(seed);
    }
    #endregion
    #region//��Ƭ����
    public override bool PlayerHolding(PlayerController player)
    {
        if (_lock)
        {
            CalculateUnLockProAndTime(player, out int pro, out float time);
            UpdateUnlockUI(pro);
            OpenOrCloseUnlockUI(true);
        }
        else
        {
            if (CanLockDoor(player))
            {
                OpenOrCloseLockUI(true);
            }
        }
        return true;
    }
    public override bool PlayerRelease(PlayerController player)
    {
        OpenOrCloseLockUI(false);
        OpenOrCloseUnlockUI(false);
        return true;
    }
    public override bool ActorNearby(ActorManager actor)
    {
        if (!_lock)
        {
            ChangeDoorState(DoorState.Open);
        }
        return true;
    }
    public override bool ActorFaraway(ActorManager actor)
    {
        ChangeDoorState(DoorState.Close);
        return true;
    }
    public override void PlayerInput(PlayerController player, KeyCode code)
    {
        if (code == KeyCode.F)
        {
            if (_lock)
            {
                TryToUnlockDoor(player);
            }
            else
            {
                TryToLockDoor(player);
            }
        }
        base.PlayerInput(player, code);
    }
    /// <summary>
    /// ��ʾ������������
    /// </summary>
    /// <param name="open"></param>
    private void OpenOrCloseLockUI(bool open)
    {
        obj_LockUI.transform.DOKill();
        if (open)
        {
            obj_LockUI.SetActive(true);
            obj_LockUI.transform.localScale = Vector3.one;
            obj_LockUI.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
        }
        else
        {
            obj_LockUI.transform.DOScale(Vector3.zero, 0.1f).OnComplete(() =>
            {
                obj_LockUI.SetActive(false);
            });
        }
    }
    /// <summary>
    /// ��ʾ���ؽ�������
    /// </summary>
    /// <param name="open"></param>
    private void OpenOrCloseUnlockUI(bool open)
    {
        obj_UnlockUI.transform.DOKill();
        obj_UnlockUI.transform.localScale = Vector3.one;
        image_UnlockBar.DOKill();
        image_UnlockBar.transform.localScale = new Vector3(0, 1, 1);
        if (open)
        {
            obj_UnlockUI.SetActive(true);
            obj_UnlockUI.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
        }
        else
        {
            obj_UnlockUI.transform.DOScale(Vector3.zero, 0.1f).OnComplete(() =>
            {
                obj_UnlockUI.SetActive(false);
            });
        }
    }
    private void ShakeUnlockUI()
    {
        obj_UnlockUI.transform.DOKill();
        obj_UnlockUI.SetActive(true);
        obj_UnlockUI.transform.localScale = Vector3.one;
        obj_UnlockUI.transform.DOShakePosition(0.2f, 5, 100);
        AudioManager.Instance.PlayEffect(1010, transform.position);
    }
    /// <summary>
    /// ���½���UI���
    /// </summary>
    /// <param name="player"></param>
    private void UpdateUnlockUI(int pro)
    {
        text_UnlockPro.text = pro + "%";
    }
    #endregion
    #region//��
    /// <summary>
    /// �����ŵ�״̬
    /// </summary>
    /// <param name="doorState"></param>
    private void ChangeDoorState(DoorState doorState)
    {
        if (doorState == DoorState.Open)
        {
            if (!_open)
            {
                obj_Door.transform.DOKill();
                obj_Door.transform.localScale = Vector3.one;
                obj_Door.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
                AudioManager.Instance.PlayEffect(1011, transform.position);
            }
            _open = true;
            collider_Door.enabled = false;
            if (_vertical)
            {
                spriteRenderer_Door.sprite = sprite_Door_V_Open;
            }
            else
            {
                spriteRenderer_Door.sprite = sprite_Door_H_Open;
            }
        }
        if (doorState == DoorState.Close)
        {
            if(_open)
            {
                obj_Door.transform.DOKill();
                obj_Door.transform.localScale = Vector3.one;
                obj_Door.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
                AudioManager.Instance.PlayEffect(1009, transform.position);
            }
            _open = false;
            collider_Door.enabled = true;
            if (_vertical)
            {
                if (_lock)
                {
                    spriteRenderer_Door.sprite = sprite_Door_V_Lock;
                }
                else
                {
                    spriteRenderer_Door.sprite = sprite_Door_V_Close;
                }
            }
            else
            {
                if (_lock)
                {
                    spriteRenderer_Door.sprite = sprite_Door_H_Lock;
                }
                else
                {
                    spriteRenderer_Door.sprite = sprite_Door_H_Close;
                }
            }
        }
    }
    /// <summary>
    /// ���Խ���
    /// </summary>
    private void TryToUnlockDoor(PlayerController player)
    {
        UnlockDoor(player);
    }
    /// <summary>
    /// ����
    /// </summary>
    /// <param name="player"></param>
    private void UnlockDoor(PlayerController player)
    {
        CalculateUnLockProAndTime(player, out int pro, out float time);
        UpdateUnlockUI(pro);
        if (CalculateUnlockResult(pro))
        {
            UnlockDoorSucces(time, player.actorManager);
        }
        else
        {
            UnlockDoorFail(time, player.actorManager);
        }
    }
    /// <summary>
    /// ���㿪�����
    /// </summary>
    /// <param name="pro"></param>
    /// <returns></returns>
    private bool CalculateUnlockResult(int pro)
    {
        UnityEngine.Random.InitState(System.DateTime.Now.Second);
        if (UnityEngine.Random.Range(0, 100) < pro)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    /// <summary>
    /// �����ɹ�
    /// </summary>
    private void UnlockDoorSucces(float time, ActorManager actor)
    {
        AudioManager.Instance.PlayEffect(1008, transform.position);
        image_UnlockBar.DOKill();
        image_UnlockBar.color = Color.green;
        image_UnlockBar.transform.localScale = new Vector3(0, 1, 1);
        image_UnlockBar.transform.DOScaleX(1, time).SetEase(Ease.InSine).OnComplete(() =>
        {
            LockDoor("");
            OpenOrCloseLockUI(true);
            OpenOrCloseUnlockUI(false);
            actor.ShowText("�ɹ�", Color.green, 24, Vector2.up * 2);
        });
    }
    /// <summary>
    /// ����ʧ��
    /// </summary>
    private void UnlockDoorFail(float time, ActorManager actor)
    {
        UnityEngine.Random.InitState(System.DateTime.Now.Second);
        float val = UnityEngine.Random.Range(0.1f, 0.95f);

        AudioManager.Instance.PlayEffect(1008, transform.position);
        image_UnlockBar.DOKill();
        image_UnlockBar.color = Color.green;
        image_UnlockBar.transform.localScale = new Vector3(0, 1, 1);
        image_UnlockBar.transform.DOScaleX(1 * val, time * val).SetEase(Ease.InSine).OnComplete(() =>
        {
            OpenOrCloseLockUI(false);
            ShakeUnlockUI();
            image_UnlockBar.color = Color.red;
            actor.ShowText("ʧ��", Color.red, 24, Vector2.up * 2);
        });

    }
    /// <summary>
    /// �Ƿ��������
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    private bool CanLockDoor(PlayerController player)
    {
        return player.thisPlayerIsMe && player.actorManager.NetManager.Data_ItemInHand.Item_ID == 9005;
    }
    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="player"></param>
    private void TryToLockDoor(PlayerController player)
    {
        if (CanLockDoor(player))
        {
            LockDoor(player.actorManager.NetManager.Data_ItemInHand.Item_Seed.ToString());
            OpenOrCloseLockUI(false);
            OpenOrCloseUnlockUI(false);
        }
    }
    /// <summary>
    /// ����
    /// </summary>
    /// <param name="password"></param>
    private void LockDoor(string password)
    {
        TryToChangeInfo(password);
    }
    /// <summary>
    /// ���������������Ҫ��ʱ��
    /// </summary>
    /// <param name="player">���</param>
    /// <param name="pro">����</param>
    /// <param name="time">ʱ��</param>
    private void CalculateUnLockProAndTime(PlayerController player,out int pro,out float time)
    {
        if (player.actorManager.NetManager.Data_ItemInHand.Item_ID == 9005 && player.actorManager.NetManager.Data_ItemInHand.Item_Seed.ToString() == info)
        {
            pro = 100;
            time = 0.2f;
        }
        else
        {
            pro = lockPro;
            if (pro > 70) { time = 4; }
            else if (pro > 40) { time = 4.5f; }
            else
            {
                time = 5f;
            }
        }
    }

    #endregion
    #region//��Ϣ�������ϴ�
    public override void TryToUpdateInfo(string info)
    {
        base.TryToUpdateInfo(info);
        if (info == "")
        {
            _lock = false;
            ChangeDoorState(DoorState.Open);
        }
        else
        {
            _lock = true;
            ChangeDoorState(DoorState.Close);
        }
    }
    #endregion
}
public enum DoorState
{
    Open,
    Close,
}

