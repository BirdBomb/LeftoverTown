 using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;

public class TileObj_Refrigerator : TileObj
{
    [SerializeField, Header("SingalF")]
    private GameObject obj_singalF;
    [SerializeField, Header("Cabinet")]
    private GameObject obj_cabinet;
    [SerializeField, Header("����UI")]
    private UI_Grid_Refrigerator uI_Grid_Refrigerator;
    #region//��ҽ���
    public override void Invoke(PlayerController player, KeyCode code)
    {
        if (code == KeyCode.F)
        {
            OpenOrCloseSingal(obj_cabinet.activeSelf);
            OpenOrCloseCabinetUI(!obj_cabinet.activeSelf);
        }
        base.Invoke(player, code);
    }
    private void OpenOrCloseSingal(bool open)
    {
        obj_singalF.transform.DOKill();
        if (open)
        {
            obj_singalF.SetActive(true);
            obj_singalF.transform.localScale = Vector3.one;
            obj_singalF.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
        }
        else
        {
            obj_singalF.transform.DOScale(Vector3.zero, 0.1f).OnComplete(() =>
            {
                obj_singalF.SetActive(false);
            });
        }
    }
    private void OpenOrCloseCabinetUI(bool open)
    {
        if (open)
        {
            obj_cabinet.transform.localScale = Vector3.one;
            obj_cabinet.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
            obj_cabinet.SetActive(true);
            uI_Grid_Refrigerator.Open(this);
            uI_Grid_Refrigerator.UpdateInfoFromTile(info);
        }
        else
        {
            obj_cabinet.transform.DOScale(Vector3.zero, 0.1f).OnComplete(() =>
            {
                obj_cabinet.SetActive(false);
            });
            uI_Grid_Refrigerator.Close(this);
        }
    }
    public override bool PlayerNearby(PlayerController player)
    {
        /*���������Լ�*/
        if (player.thisPlayerIsMe)
        {
            OpenOrCloseSingal(true);
            return true;
        }
        return false;
    }
    public override bool PlayerFaraway(PlayerController player)
    {
        /*�뿪�����Լ�*/
        if (player.thisPlayerIsMe)
        {
            OpenOrCloseSingal(false);
            OpenOrCloseCabinetUI(false);
            return true;
        }
        return true;
    }
    #endregion
    #region//�����ϴ�
    public override void TryToUpdateInfo(string info)
    {
        uI_Grid_Refrigerator.UpdateInfoFromTile(info);
        base.TryToUpdateInfo(info);
    }
    public override void TryToUpdateHp(int newHp)
    {
        if (newHp <= CurHp)
        {
            PlayDamagedAnim();
        }
        base.TryToUpdateHp(newHp);
    }
    #endregion
    #region//����
    public override void TryToDestroyMyObj()
    {
        PlayBreakAnim();
        Invoke("DestroyMyObj", 0.3f);
    }
    public override void PlayDamagedAnim()
    {
        transform.DOPunchScale(new Vector3(0.2f, -0.1f, 0), 0.2f).SetEase(Ease.InOutBack);
        base.PlayDamagedAnim();
    }
    public override void PlayBreakAnim()
    {
        transform.DOPunchScale(new Vector3(0.2f, -0.1f, 0), 0.2f).SetEase(Ease.InOutBack).OnComplete(() =>
        {
            transform.DOScaleX(0, 0.05f);
        });
        base.PlayBreakAnim();
    }
    #endregion
}
