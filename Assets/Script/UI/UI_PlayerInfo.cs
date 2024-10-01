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
    [SerializeField, Header("玩家头发")]
    private Image playerHair;
    [SerializeField, Header("玩家眼睛")]
    private Image playerEye;
    [SerializeField, Header("玩家名字")]
    private TextMeshProUGUI playerName;
    [SerializeField, Header("玩家悬赏")]
    private TextMeshProUGUI playerFine;
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
    public UI_PointPanel point_Strength;
    [SerializeField, Header("智力")]
    public UI_PointPanel Point_Intelligence;
    [SerializeField, Header("法力")]
    public UI_PointPanel Point_SPower;
    [SerializeField, Header("专注")]
    public UI_PointPanel Point_Focus;
    [SerializeField, Header("敏捷")]
    public UI_PointPanel Point_Agility;
    [SerializeField, Header("制造")]
    public UI_PointPanel Point_Make;
    [SerializeField, Header("建造")]
    public UI_PointPanel Point_Build;
    [SerializeField, Header("烹饪")]
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
