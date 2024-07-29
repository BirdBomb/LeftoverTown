using Fusion;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;
using static UnityEditor.Timeline.Actions.MenuPriority;

public class UI_PlayerInfo : MonoBehaviour
{
    bool open = false;
    public GameObject mask;
    public GameObject bg;
    [SerializeField, Header("背包槽位")]
    private List<UI_GridCell> _bagCellList = new List<UI_GridCell>();
    [SerializeField, Header("手部槽位")]
    private UI_GridCell _handCell;
    [SerializeField, Header("身体槽位")]
    private UI_GridCell _bodyCell;
    [SerializeField, Header("头部槽位")]
    private UI_GridCell _headCell;
    [SerializeField, Header("技能面板")]
    private UI_SkillPanel skillPanel;
    [SerializeField, Header("技能轮盘")]
    private List<UI_SkillSlot> skillUse;
    [SerializeField, Header("力量")]
    private Text _Strength;
    [SerializeField, Header("智力")]
    private Text _Intelligence;
    [SerializeField, Header("专注")]
    private Text _Focus;
    [SerializeField, Header("敏捷")]
    private Text _Agility;
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
        _Strength.text = GameLocalManager.Instance.localPlayer.actorManager.NetManager.Data_Point_Strength.ToString();
        _Intelligence.text = GameLocalManager.Instance.localPlayer.actorManager.NetManager.Data_Point_Intelligence.ToString();
        _Focus.text = GameLocalManager.Instance.localPlayer.actorManager.NetManager.Data_Point_Focus.ToString();
        _Agility.text = GameLocalManager.Instance.localPlayer.actorManager.NetManager.Data_Point_Agility.ToString();
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
}
