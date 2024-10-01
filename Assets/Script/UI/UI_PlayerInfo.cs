using Fusion;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;
using UnityEngine.XR;
using static UnityEditor.Timeline.Actions.MenuPriority;

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
    [SerializeField, Header("�������")]
    private UI_SkillPanel skillPanel;
    [SerializeField, Header("��������")]
    private List<UI_SkillSlot> skillUse;
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

    private void Start()
    {
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_BindSkill>().Subscribe(_ =>
        {
            skillPanel.UpdateUsingSkill(_.skillIDs);
            if (!atlas) { atlas = Resources.Load<SpriteAtlas>("Atlas/SkillSprite"); }
            for (int i = 0; i < _.skillIDs.Count; i++)
            {
                int index = i;
                skillUse[index].Init(_.skillIDs[index], atlas.GetSprite(_.skillIDs[index].ToString()));
            }
        }).AddTo(this);
    }
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
        skillPanel.UpdateDraw();
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
        Debug.Log(GameLocalManager.Instance.localPlayer);
        NetworkLinkedList<ItemData> bagItem = GameLocalManager.Instance.localPlayer.actorManager.NetManager.Data_ItemInBag;
        ItemData handItem = GameLocalManager.Instance.localPlayer.actorManager.NetManager.Data_ItemInHand;
        ItemData headItem = GameLocalManager.Instance.localPlayer.actorManager.NetManager.Data_ItemOnHead;
        ItemData bodyItem = GameLocalManager.Instance.localPlayer.actorManager.NetManager.Data_ItemOnBody;
        for (int i = 0; i < bagItem.Count; i++)
        {
            int index = i;
            if (_bagCellList.Count > index)
            {
                _bagCellList[index].UpdateGridCell(bagItem[index]);
            }
        }
        _handCell.UpdateGridCell(handItem);
        _headCell.UpdateGridCell(headItem);
        _bodyCell.UpdateGridCell(bodyItem);

        playerName.text = GameLocalManager.Instance.localPlayer.actorManager.NetManager.Data_Name.ToString();
        playerFine.text = GameLocalManager.Instance.localPlayer.actorManager.NetManager.Data_Fine.ToString();
        InitPlayerPhoto(GameLocalManager.Instance.localPlayer.actorManager.NetManager.Data_HairID, 
            GameLocalManager.Instance.localPlayer.actorManager.NetManager.Data_EyeID, 
            GameLocalManager.Instance.localPlayer.actorManager.NetManager.Data_HairColor);

        point_Strength.UpdatePoint(GameLocalManager.Instance.localPlayer.actorManager.NetManager.Data_Point_Strength);
        Point_Intelligence.UpdatePoint(GameLocalManager.Instance.localPlayer.actorManager.NetManager.Data_Point_Intelligence);
        Point_SPower.UpdatePoint(GameLocalManager.Instance.localPlayer.actorManager.NetManager.Data_Point_SPower);
        Point_Focus.UpdatePoint(GameLocalManager.Instance.localPlayer.actorManager.NetManager.Data_Point_Focus);
        Point_Agility.UpdatePoint(GameLocalManager.Instance.localPlayer.actorManager.NetManager.Data_Point_Agility);
        Point_Make.UpdatePoint(GameLocalManager.Instance.localPlayer.actorManager.NetManager.Data_Point_Make);
        Point_Build.UpdatePoint(GameLocalManager.Instance.localPlayer.actorManager.NetManager.Data_Point_Build);
        Point_Cook.UpdatePoint(GameLocalManager.Instance.localPlayer.actorManager.NetManager.Data_Point_Cook);

    }
    public void ResetCell()
    {
        ItemData itemData = new ItemData();
        for (int i = 0; i < _bagCellList.Count; i++)
        {
            _bagCellList[i].UpdateGridCell(itemData);
        }
        _handCell.UpdateGridCell(itemData);
        _headCell.UpdateGridCell(itemData);
        _bodyCell.UpdateGridCell(itemData);
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
