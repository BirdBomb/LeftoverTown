using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestConfigData 
{
    public static QuestConfig GetQuestConfig(int ID)
    {
        return questConfigs.Find((x) => { return x.QuestID == ID; });
    }
    public readonly static List<QuestConfig> questConfigs = new List<QuestConfig>()
    {
        new QuestConfig(){QuestID = 1000,QuestLevel = 1,QuestExp = 20},/*按"B"打开背包*/
        new QuestConfig(){QuestID = 1001,QuestLevel = 1,QuestExp = 20},/*按"C"打开建造*/
        new QuestConfig(){QuestID = 1002,QuestLevel = 1,QuestExp = 20},/*按"V"打开技能*/
        new QuestConfig(){QuestID = 1003,QuestLevel = 1,QuestExp = 20},/*按"H"打开表情轮盘*/
        new QuestConfig(){QuestID = 2000,QuestLevel = 2,QuestExp = 20},/*按"C"然后建造木工作台*/
        new QuestConfig(){QuestID = 2001,QuestLevel = 2,QuestExp = 20},/*通过交易至少获得1金币*/
    };
}
public struct QuestConfig
{
    /// <summary>
    /// ID
    /// </summary>
    public short QuestID;
    /// <summary>
    /// Level
    /// </summary>
    public int QuestLevel;
    /// <summary>
    /// 获得经验
    /// </summary>
    public short QuestExp;
}