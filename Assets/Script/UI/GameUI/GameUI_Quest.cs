using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using TMPro;

public class GameUI_Quest : MonoBehaviour
{
    public TextMeshProUGUI text_QuestDesc;
    public Text text_Num;
    private List<int> questsList = new List<int>();
    private short questLevel;
    private int questCount;
    private QuestConfig questConfig;
    void Start()
    {
        MessageBroker.Default.Receive<UIEvent.UIEvent_UpdateQuest>().Subscribe(_ =>
        {
            questsList = _.Quests;
            questLevel = _.Level;
            UpdateQuest();
        }).AddTo(this);
    }
    private void UpdateQuest()
    {
        GetQuestList(questLevel);
    }
    /// <summary>
    /// 获取符合等级的任务
    /// </summary>
    /// <param name="questLevel"></param>
    private void GetQuestList(short questLevel)
    {
        List<QuestConfig> temp = QuestConfigData.questConfigs.FindAll((x) => { return x.QuestLevel == questLevel; });
        if (!GetQuest(temp) && questLevel < 9)
        {
            questLevel += 1;
            GetQuestList(questLevel);
        }
    }
    /// <summary>
    /// 获取随机任务
    /// </summary>
    /// <param name="temp"></param>
    /// <returns></returns>
    private bool GetQuest(List<QuestConfig> temp)
    {
        bool success = false;
        List<QuestConfig> random = new List<QuestConfig>();
        for(int i = 0;i< temp.Count;i++)
        {
            if (!questsList.Contains(temp[i].QuestID))
            {
                random.Add(temp[i]);
                success = true;
            }
        }
        if (success)
        {
            questConfig = random[new System.Random().Next(0, random.Count)];
            DrawQuest();
        }
        return success;
    }
    private void DrawQuest()
    {
        text_QuestDesc.text = LocalizationManager.Instance.GetLocalization("Quest_String", "Quest_" + questConfig.QuestID);
        text_Num.text = questConfig.QuestLevel.ToString();
    } 
}
