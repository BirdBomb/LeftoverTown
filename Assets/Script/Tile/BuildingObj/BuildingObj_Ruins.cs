using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using UniRx;
using UnityEngine;
/// <summary>
/// ����
/// </summary>
public class BuildingObj_Ruins : BuildingObj_Manmade
{
    public GameObject obj_SingalUI;
    public GameObject obj_HightlightUI;
    public Transform tran_BarFillUI;
    public int int_BaseLootTime = 2;
    [Header("�������б�")]
    public List<ExtraLootInfo> extraLootInfos = new List<ExtraLootInfo>();
    private int int_LootTime;
    public override void Start()
    {
        MessageBroker.Default.Receive<GameEvent.GameEvent_All_UpdateHour>().Subscribe(_ =>
        {
            if(_.hour == 0)
            {
                int_LootTime = int_BaseLootTime;
            }
        }).AddTo(this);
    }
    #region//��Ϣ�������ϴ�
    public override void All_UpdateInfo(string info)
    {
        ReadInfo(info);
        base.All_UpdateInfo(info);
    }
    public void ReadInfo(string info)
    {
        if (info != null && info != "")
        {
            int_LootTime = int.Parse(info);
        }
        else
        {
            int_LootTime = int_BaseLootTime;
        }
    }
    public void WriteInfo()
    {
        StringBuilder builder = new StringBuilder();
        builder.Append(int_LootTime.ToString());
        Local_ChangeInfo(builder.ToString());
    }
    #endregion
    #region//��Ƭ����
    public override void All_ActorInputKeycode(ActorManager actor, KeyCode code)
    {
        if (code == KeyCode.F)
        {
            LootStarting();
        }
        base.All_ActorInputKeycode(actor, code);
    }
    public override void All_PlayerHighlight(bool on)
    {
        OpenOrCloseHighlightUI(on);
        base.All_PlayerHighlight(on);
    }
    public override void All_PlayerFaraway()
    {
        LootStop();
        base.All_PlayerFaraway();
    }
    public override void OpenOrCloseHighlightUI(bool open)
    {
        obj_SingalUI.transform.DOKill();
        if (open)
        {
            obj_SingalUI.SetActive(true);
            obj_SingalUI.transform.localScale = Vector3.one;
            obj_SingalUI.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
        }
        else
        {
            obj_SingalUI.transform.DOScale(Vector3.zero, 0.1f).OnComplete(() =>
            {
                obj_SingalUI.SetActive(false);
            });
        }
        obj_HightlightUI.transform.DOKill();
        if (open)
        {
            obj_HightlightUI.SetActive(true);
            obj_HightlightUI.transform.localScale = Vector3.one;
            obj_HightlightUI.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
        }
        else
        {
            obj_HightlightUI.transform.DOScale(Vector3.zero, 0.1f).OnComplete(() =>
            {
                obj_HightlightUI.SetActive(false);
            });
        }
    }
    /// <summary>
    /// ��ʼ����
    /// </summary>
    private void LootStarting()
    {
        ReadInfo(info);
        LootStop();
        tran_BarFillUI.DOKill();
        tran_BarFillUI.localScale = new Vector3(0, 1, 1);
        tran_BarFillUI.DOScaleX(1, int_LootTime).SetEase(Ease.Linear);
        Invoke("LootEnding", int_LootTime);
    }
    /// <summary>
    /// ��ֹ����
    /// </summary>
    private void LootStop()
    {
        tran_BarFillUI.localScale = new Vector3(0, 1, 1);
        if (IsInvoking("LootEnding"))
        {
            CancelInvoke("LootEnding");
        }
    }
    /// <summary>
    /// ��������
    /// </summary>
    private void LootEnding()
    {
        tran_BarFillUI.localScale = new Vector3(0, 1, 1);
        int_LootTime += 1;

        short id = GetRandomItem();
        if (id != 0)
        {
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_ItemBag_Add()
            {
                itemData = Local_GetItemData(id, 1),
                itemFrom = ItemFrom.OutSide
            });
        }
        WriteInfo();
    }
    private short GetRandomItem()
    {
        int random = 0;
        int count = 0;
        for(int i = 0; i < extraLootInfos.Count; i++)
        {
            count += (int)extraLootInfos[i].Weight;
        }
        random = new System.Random().Next(0, count);
        count = 0;
        for (int i = 0; i < extraLootInfos.Count; i++)
        {
            count += (int)extraLootInfos[i].Weight;
            if (random< count)
            {
                return extraLootInfos[i].ID;
            }
        }
        return 0;
    }
    public override bool CanHighlight()
    {
        return true;
    }
    #endregion
}
