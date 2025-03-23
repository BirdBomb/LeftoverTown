using Fusion;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class UI_PlayerInfo : MonoBehaviour
{
    bool open = false;
    public GameObject mask;
    public GameObject bg;
    private SpriteAtlas atlasHair;
    private SpriteAtlas atlasEye;
    [SerializeField, Header("���ͷ��")]
    private Image playerHair;
    [SerializeField, Header("����۾�")]
    private Image playerEye;
    [SerializeField, Header("�������")]
    private TextMeshProUGUI playerName;
    [SerializeField, Header("�������")]
    private TextMeshProUGUI playerFine;
    [SerializeField, Header("������λ")]
    private List<UI_GridCell> _bagCellList = new List<UI_GridCell>();
    [SerializeField, Header("�ֲ���λ")]
    private UI_GridCell _handCell;
    [SerializeField, Header("�����λ")]
    private UI_GridCell _bodyCell;
    [SerializeField, Header("ͷ����λ")]
    private UI_GridCell _headCell;
    [SerializeField, Header("����")]
    public UI_PointPanel point_Strength;
    [SerializeField, Header("����")]
    public UI_PointPanel Point_Intelligence;
    [SerializeField, Header("����")]
    public UI_PointPanel Point_SPower;
    [SerializeField, Header("רע")]
    public UI_PointPanel Point_Focus;
    [SerializeField, Header("����")]
    public UI_PointPanel Point_Agility;
    [SerializeField, Header("����")]
    public UI_PointPanel Point_Make;
    [SerializeField, Header("����")]
    public UI_PointPanel Point_Build;
    [SerializeField, Header("���")]
    public UI_PointPanel Point_Cook;
    private SpriteAtlas atlas;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.C))
        {
            if (!open)
            {
                Open();
            }
            else
            {
                Close();
            }
        }
    }

    public void Open()
    {
        open = true;
        mask.SetActive(true);
        bg.SetActive(true);
        UpdateCell();
    }
    public void Close()
    {
        open = false;
        mask.SetActive(false);
        bg.SetActive(false);
        ResetCell();
    }
    public void UpdateCell()
    {
        NetworkLinkedList<ItemData> bagItem = GameLocalManager.Instance.playerCoreLocal.actorManager_Bind.actorNetManager.Net_ItemsInBag;
        ItemData handItem = GameLocalManager.Instance.playerCoreLocal.actorManager_Bind.actorNetManager.Net_ItemInHand;
        ItemData headItem = GameLocalManager.Instance.playerCoreLocal.actorManager_Bind.actorNetManager.Net_ItemOnHead;
        ItemData bodyItem = GameLocalManager.Instance.playerCoreLocal.actorManager_Bind.actorNetManager.Net_ItemOnBody;
        for (int i = 0; i < bagItem.Count; i++)
        {
            int index = i;
            if (_bagCellList.Count > index)
            {
                _bagCellList[index].UpdateData(bagItem[index]);
            }
        }
        _handCell.UpdateData(handItem);
        _headCell.UpdateData(headItem);
        _bodyCell.UpdateData(bodyItem);

        playerName.text = GameLocalManager.Instance.playerCoreLocal.actorManager_Bind.actorNetManager.Net_Name.ToString();
        playerFine.text = GameLocalManager.Instance.playerCoreLocal.actorManager_Bind.actorNetManager.Net_Fine.ToString();
        InitPlayerPhoto(GameLocalManager.Instance.playerCoreLocal.actorManager_Bind.actorNetManager.Net_HairID, 
            GameLocalManager.Instance.playerCoreLocal.actorManager_Bind.actorNetManager.Net_EyeID, 
            GameLocalManager.Instance.playerCoreLocal.actorManager_Bind.actorNetManager.Net_HairColor);

        point_Strength.UpdatePoint(GameLocalManager.Instance.playerCoreLocal.actorManager_Bind.actorNetManager.Net_Point_Strength);
        Point_Intelligence.UpdatePoint(GameLocalManager.Instance.playerCoreLocal.actorManager_Bind.actorNetManager.Net_Point_Intelligence);
        Point_SPower.UpdatePoint(GameLocalManager.Instance.playerCoreLocal.actorManager_Bind.actorNetManager.Net_Point_SPower);
        Point_Focus.UpdatePoint(GameLocalManager.Instance.playerCoreLocal.actorManager_Bind.actorNetManager.Net_Point_Focus);
        Point_Agility.UpdatePoint(GameLocalManager.Instance.playerCoreLocal.actorManager_Bind.actorNetManager.Net_Point_Agility);
        Point_Make.UpdatePoint(GameLocalManager.Instance.playerCoreLocal.actorManager_Bind.actorNetManager.Net_Point_Make);
        Point_Build.UpdatePoint(GameLocalManager.Instance.playerCoreLocal.actorManager_Bind.actorNetManager.Net_Point_Build);
        Point_Cook.UpdatePoint(GameLocalManager.Instance.playerCoreLocal.actorManager_Bind.actorNetManager.Net_Point_Cook);

    }
    public void ResetCell()
    {
        ItemData itemData = new ItemData();
        for (int i = 0; i < _bagCellList.Count; i++)
        {
            _bagCellList[i].UpdateData(itemData);
        }
        _handCell.UpdateData(itemData);
        _headCell.UpdateData(itemData);
        _bodyCell.UpdateData(itemData);
    }
    private void InitPlayerPhoto(int hairID, int eyeID, Color32 hairColor)
    {
        if (atlasEye == null || atlasHair == null)
        {
            atlasHair = Resources.Load<SpriteAtlas>("Atlas/HairSprite");
            atlasEye = Resources.Load<SpriteAtlas>("Atlas/EyeSprite");
        }
        playerHair.sprite = atlasHair.GetSprite("Hair_" + hairID.ToString());
        playerHair.color = hairColor;
        playerEye.sprite = atlasEye.GetSprite("Eye_" + eyeID.ToString());
    }
}
