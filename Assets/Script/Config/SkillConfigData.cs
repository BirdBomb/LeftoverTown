using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillConfigData
{
    public static SkillConfig GetSkillConfig(short ID)
    {
        return skillConfigs.Find((x) => { return x.Skill_ID == ID; });
    }
    public readonly static List<SkillConfig> skillConfigs = new List<SkillConfig>()
    {
        new SkillConfig(){ Skill_ID = 1001,Skill_Name = "闪避步伐",Skill_Desc = "能够快速短距离移动的招式。无论什么阵营，是所有拿起武器保卫家园的人类士兵学会的第一个技能",Skill_Strength = 0,Skill_Intelligence = 0,Skill_Focus = 0, Skill_Agility = 0},
        new SkillConfig(){ Skill_ID = 1002,Skill_Name = "断式步伐",Skill_Desc = "改良后的闪避步伐，可以在冲刺结束后再次释放，以便达到出其不意的效果",Skill_Strength = 0,Skill_Intelligence = 0,Skill_Focus = 0, Skill_Agility = 0},
        //new SkillConfig(){ Skill_ID = 1002,Skill_Name = "闪烁术",Skill_Desc = "对使用者的造诣需求极高，来源已无从考究。究竟是瞬间完成的传送，还是使用者在目的地创造了新的自己然后杀死了过去的自己，仍有争论",Skill_Strength = 0,Skill_Intelligence = 0,Skill_Focus = 0, Skill_Agility = 0},
        //new SkillConfig(){ Skill_ID = 1003,Skill_Name = "前蹴",Skill_Desc = "不需要任何架势瞬间踢出的招式，踢中了会非常开心(使用者)",Skill_Strength = 0,Skill_Intelligence = 0,Skill_Focus = 0, Skill_Agility = 0},
        //new SkillConfig(){ Skill_ID = 1004,Skill_Name = "静隐身术",Skill_Desc = "可以让使用者在不移动的情况下隐去身形，虽是最低级的遁虚但仍是难以掌握",Skill_Strength = 0,Skill_Intelligence = 0,Skill_Focus = 0, Skill_Agility = 0},
        //new SkillConfig(){ Skill_ID = 1005,Skill_Name = "动隐身术",Skill_Desc = "可以让使用者在不攻击的情况下隐去身形，只有相当有天赋的教徒才能使用",Skill_Strength = 0,Skill_Intelligence = 0,Skill_Focus = 0, Skill_Agility = 0},
        //new SkillConfig(){ Skill_ID = 1006,Skill_Name = "刺破术",Skill_Desc = "空间魔法，用强大的魔法撕裂指向方向的空间。",Skill_Strength = 0,Skill_Intelligence = 0,Skill_Focus = 0, Skill_Agility = 0},
        //new SkillConfig(){ Skill_ID = 1007,Skill_Name = "断空斩",Skill_Desc = "使用持有武器向前斩击，可以击落飞行中的物体",Skill_Strength = 0,Skill_Intelligence = 0,Skill_Focus = 0, Skill_Agility = 0},
        //new SkillConfig(){ Skill_ID = 1008,Skill_Name = "大跃斩",Skill_Desc = "短暂蓄力后向前冲刺斩击，可以击落飞行中的物体",Skill_Strength = 0,Skill_Intelligence = 0,Skill_Focus = 0, Skill_Agility = 0},
        //new SkillConfig(){ Skill_ID = 1009,Skill_Name = "剑气斩",Skill_Desc = "使用持有武器向前斩击并发出一道剑气",Skill_Strength = 0,Skill_Intelligence = 0,Skill_Focus = 0, Skill_Agility = 0},
        //new SkillConfig(){ Skill_ID = 1010,Skill_Name = "三步四斩",Skill_Desc = "朝前冲刺三次并发出四次斩击",Skill_Strength = 0,Skill_Intelligence = 0,Skill_Focus = 0, Skill_Agility = 0},
        //new SkillConfig(){ Skill_ID = 1011,Skill_Name = "回转斩",Skill_Desc = "使用持有武器斩击周围所有敌人",Skill_Strength = 0,Skill_Intelligence = 0,Skill_Focus = 0, Skill_Agility = 0},
        //new SkillConfig(){ Skill_ID = 1012,Skill_Name = "解斩",Skill_Desc = "一瞬间发动多次斩击",Skill_Strength = 0,Skill_Intelligence = 0,Skill_Focus = 0, Skill_Agility = 0},
    };
}
[Serializable]
public struct SkillConfig
{
    [SerializeField]/*编号*/
    public short Skill_ID;
    [SerializeField]/*名字*/
    public string Skill_Name;
    [SerializeField]/*描述*/
    public string Skill_Desc;
    [SerializeField]/*CD*/
    public string Skill_CD;
    [SerializeField]/*最小使用力量*/
    public short Skill_Strength;
    [SerializeField]/*最小使用智力*/
    public short Skill_Intelligence;
    [SerializeField]/*最小使用专注*/
    public short Skill_Focus;
    [SerializeField]/*最小使用敏捷*/
    public short Skill_Agility;
}
