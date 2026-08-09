using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using TMPro;
using System;
using DG.Tweening;

public class GameUI_Quest : MonoBehaviour
{
    public Transform transform_Panel;
    public TextMeshProUGUI text_QuestDesc;
    public Text text_Num;
    private QuestBase quest_Bind;
    void Start()
    {
        MessageBroker.Default.Receive<UIEvent.UIEvent_UpdateQuest>().Subscribe(_ =>
        {
            DrawQuest(_.Quest);
            BindQuesr(_.Quest);
        }).AddTo(this);
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_Action>().Subscribe(_ =>
        {
            if (quest_Bind != null) quest_Bind.Listen_PlayerAction(_.action);
        }).AddTo(this);
        MessageBroker.Default.Receive<MapEvent.MapEvent_Local_CreateBuildingArea>().Subscribe(_ =>
        {
            if (quest_Bind != null) quest_Bind.Listen_Build(_.buildingID);
        }).AddTo(this);
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_TryEarn>().Subscribe(_ =>
        {
            if (quest_Bind != null) quest_Bind.Listen_Earn(_.coin);
        }).AddTo(this);
    }
    private void DrawQuest(QuestConfig questConfig)
    {
        transform_Panel.DOPunchScale(new Vector3(0.1f, 0.1f, 0), 0.5f);
        text_QuestDesc.text = LocalizationManager.Instance.GetLocalization("Quest_String", "Quest_" + questConfig.QuestID);
        text_Num.text = questConfig.QuestExp.ToString();
    }
    private void BindQuesr(QuestConfig questConfig)
    {
        string className = "Quest" + questConfig.QuestID.ToString();
        Type type = Type.GetType(className);
        if (type != null && typeof(QuestBase).IsAssignableFrom(type))
        {
            quest_Bind = (QuestBase)Activator.CreateInstance(type);
        }
        else
        {
            Debug.LogError($"找不到任务类: {className}");
        }
    }
}
public enum PlayerAction
{
    OpenBag,
    OpenBuilding,
    OpenSkill,
    OpenEmoji,
}