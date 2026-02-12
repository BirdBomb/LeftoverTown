using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillConfigData 
{
    public static SkillConfig GetStatusConfig(short ID)
    {
        return statusConfigs.Find((x) => { return x.Skill_ID == ID; });
    }
    public readonly static List<SkillConfig> statusConfigs = new List<SkillConfig>()
    {
        /*消化1*/
        new SkillConfig(){ Skill_ID = 10000,Skill_Cost = 1,Skill_Precondition = 0,Skill_Exclusion = 0},
        /*消化2*/
        new SkillConfig(){ Skill_ID = 10001,Skill_Cost = 1,Skill_Precondition = 10000,Skill_Exclusion = 0},
        /*消化3*/
        new SkillConfig(){ Skill_ID = 10002,Skill_Cost = 1,Skill_Precondition = 10001,Skill_Exclusion = 10012},
        /*睡眠1*/
        new SkillConfig(){ Skill_ID = 10010,Skill_Cost = 1,Skill_Precondition = 0,Skill_Exclusion = 0},
        /*睡眠2*/
        new SkillConfig(){ Skill_ID = 10011,Skill_Cost = 1,Skill_Precondition = 10010,Skill_Exclusion = 0},
        /*睡眠3*/
        new SkillConfig(){ Skill_ID = 10012,Skill_Cost = 1,Skill_Precondition = 10011,Skill_Exclusion = 10002},
        /*心脏鼓点1*/
        new SkillConfig(){ Skill_ID = 10020,Skill_Cost = 1,Skill_Precondition = 0,Skill_Exclusion = 0},
        /*心脏鼓点2*/
        new SkillConfig(){ Skill_ID = 10021,Skill_Cost = 1,Skill_Precondition = 10020,Skill_Exclusion = 0},
        /*热烈的鼓点*/
        new SkillConfig(){ Skill_ID = 10022,Skill_Cost = 1,Skill_Precondition = 10021,Skill_Exclusion = 10023},
        /*舒缓的鼓点*/
        new SkillConfig(){ Skill_ID = 10023,Skill_Cost = 1,Skill_Precondition = 10021,Skill_Exclusion = 10022},
    };
}
public struct SkillConfig
{
    /// <summary>
    /// 技能编号
    /// </summary>
    [SerializeField]
    public short Skill_ID;
    /// <summary>
    /// 技能消耗
    /// </summary>
    [SerializeField]
    public short Skill_Cost;
    /// <summary>
    /// 技能前置
    /// </summary>
    [SerializeField]
    public short Skill_Precondition;
    /// <summary>
    /// 技能互斥
    /// </summary>
    [SerializeField]
    public short Skill_Exclusion;
}
