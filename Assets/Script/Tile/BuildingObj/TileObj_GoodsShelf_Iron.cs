using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TileObj_GoodsShelf_Iron : TileObj
{
    [SerializeField, Header("SingalF")]
    private GameObject obj_singalF;
    [SerializeField, Header("Cabinet")]
    private GameObject obj_cabinet;
    [SerializeField, Header("����UI")]
    private UI_Grid_Cabinet uI_Grid_Cabinet;
    #region//��Ƭ����
    public override void PlayerInput(PlayerController player, KeyCode code)
    {
        if (code == KeyCode.F)
        {
            OpenOrCloseSingal(obj_cabinet.activeSelf);
            OpenOrCloseCabinetUI(!obj_cabinet.activeSelf);
        }
        base.PlayerInput(player, code);
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
            uI_Grid_Cabinet.Open(this);
            uI_Grid_Cabinet.UpdateInfoFromTile(info);
        }
        else
        {
            obj_cabinet.transform.DOScale(Vector3.zero, 0.1f).OnComplete(() =>
            {
                obj_cabinet.SetActive(false);
            });
            uI_Grid_Cabinet.Close(this);
        }
    }
    public override bool PlayerHolding(PlayerController player)
    {
        /*���������Լ�*/
        if (player.thisPlayerIsMe)
        {
            OpenOrCloseSingal(true);
            return true;
        }
        return false;
    }
    public override bool PlayerRelease(PlayerController player)
    {
        /*�뿪�����Լ�*/
        if (player.thisPlayerIsMe)
        {
            OpenOrCloseSingal(false);
            OpenOrCloseCabinetUI(false);
            return true;
        }
        return false;
    }

    #endregion
    #region//��Ϣ�������ϴ�
    public override void TryToUpdateInfo(string info)
    {
        uI_Grid_Cabinet.UpdateInfoFromTile(info);
        base.TryToUpdateInfo(info);
        Draw(0);
    }
    #endregion
    #region//����
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private Sprite[] GoodsShelf_Group;
    [SerializeField]
    private Sprite GoodsShelf_Empty;
    public override void Draw(int seed)
    {
        if (info == null || info == "")
        {
            spriteRenderer.sprite = GoodsShelf_Empty;
        }
        else
        {
            spriteRenderer.sprite = GoodsShelf_Group[new System.Random().Next(0, GoodsShelf_Group.Length)];
        }
        base.Draw(seed);
    }
    #endregion
}
