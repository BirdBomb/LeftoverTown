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
        /*温暖的胃/饱餐时缓慢回血*/
        new SkillConfig(){ Skill_ID = 10000,Skill_Cost = 1,Skill_Precondition = 0,Skill_Exclusion = 0},
        /*更加温暖的胃/饱餐时变得更抗打*/
        new SkillConfig(){ Skill_ID = 10001,Skill_Cost = 1,Skill_Precondition = 10000,Skill_Exclusion = 0},
        /*弹力胃袋/你现在可以吃更多东西，哪怕你已经很饱了*/
        new SkillConfig(){ Skill_ID = 10002,Skill_Cost = 1,Skill_Precondition = 10001,Skill_Exclusion = 10012},
        /*温暖睡眠/睡眠恢复生命值*/
        new SkillConfig(){ Skill_ID = 10010,Skill_Cost = 1,Skill_Precondition = 0,Skill_Exclusion = 0},
        /*更加温暖的睡眠/睡眠恢复更多生命值*/
        new SkillConfig(){ Skill_ID = 10011,Skill_Cost = 1,Skill_Precondition = 10010,Skill_Exclusion = 0},
        /*睡眠包治百病/睡眠恢复大量生命值*/
        new SkillConfig(){ Skill_ID = 10012,Skill_Cost = 1,Skill_Precondition = 10011,Skill_Exclusion = 10002},
        /*来自心脏的鼓点1/濒死时会以一个缓慢的速度回血*/
        new SkillConfig(){ Skill_ID = 10020,Skill_Cost = 1,Skill_Precondition = 0,Skill_Exclusion = 0},
        /*来自心脏的鼓点2/濒死时会一个中等的速度回血*/
        new SkillConfig(){ Skill_ID = 10021,Skill_Cost = 1,Skill_Precondition = 10020,Skill_Exclusion = 0},
        /*热烈的鼓点/濒死回血速度变快*/
        new SkillConfig(){ Skill_ID = 10022,Skill_Cost = 1,Skill_Precondition = 10021,Skill_Exclusion = 10023},
        /*舒缓的鼓点/濒死回血时间提前*/
        new SkillConfig(){ Skill_ID = 10023,Skill_Cost = 1,Skill_Precondition = 10021,Skill_Exclusion = 10022},
        /*舒缓的鼓点/濒死回血时间提前*/
        new SkillConfig(){ Skill_ID = 10023,Skill_Cost = 1,Skill_Precondition = 10021,Skill_Exclusion = 10022},
        /*超人体质/生命值低于精神值时缓慢回血*/
        new SkillConfig(){ Skill_ID = 10030,Skill_Cost = 9,Skill_Precondition = 10021,Skill_Exclusion = 10022},

        /*开锁高手/你有更高概率打开锁*/
        new SkillConfig(){ Skill_ID = 10100,Skill_Cost = 9,Skill_Precondition = 0,Skill_Exclusion = 0},
        /*开锁大师/打开锁速度增加*/
        new SkillConfig(){ Skill_ID = 10101,Skill_Cost = 9,Skill_Precondition = 10100,Skill_Exclusion = 0},
        /*锁头破坏者/开锁概率和速度大幅增加*/
        new SkillConfig(){ Skill_ID = 10102,Skill_Cost = 9,Skill_Precondition = 10101,Skill_Exclusion = 0},
        /*工匠/你有更高概率做出稀有物品*/
        new SkillConfig(){ Skill_ID = 10110,Skill_Cost = 9,Skill_Precondition = 0,Skill_Exclusion = 0},
        /*修补专家/修复物品必定成功*/
        new SkillConfig(){ Skill_ID = 10111,Skill_Cost = 9,Skill_Precondition = 10110,Skill_Exclusion = 0},
        /*精制专家/精制物品必定成功*/
        new SkillConfig(){ Skill_ID = 10112,Skill_Cost = 9,Skill_Precondition = 10111,Skill_Exclusion = 0},
        /*钓鱼爱好者/钓鱼成功后恢复精神值*/
        new SkillConfig(){ Skill_ID = 10120,Skill_Cost = 9,Skill_Precondition = 0,Skill_Exclusion = 0},
        /*钓鱼老手/钓鱼速度加快*/
        new SkillConfig(){ Skill_ID = 10121,Skill_Cost = 9,Skill_Precondition = 10120,Skill_Exclusion = 0},
        /*钓鱼大师/你可以提前知道是什么东西在咬钩*/
        new SkillConfig(){ Skill_ID = 10122,Skill_Cost = 9,Skill_Precondition = 10121,Skill_Exclusion = 0},
        /*拾荒者/采集砍树挖矿时有小概率获得额外物品*/
        new SkillConfig(){ Skill_ID = 10130,Skill_Cost = 9,Skill_Precondition = 0,Skill_Exclusion = 0},
        /*高级拾荒者/采集砍树挖矿时有概率获得额外物品*/
        new SkillConfig(){ Skill_ID = 10131,Skill_Cost = 9,Skill_Precondition = 10130,Skill_Exclusion = 0},


        /*书呆子/读书获取的经验值大幅度增加*/
        new SkillConfig(){ Skill_ID = 10200,Skill_Cost = 9,Skill_Precondition = 0,Skill_Exclusion = 0},
        /*奇闻怪谈/你可以从日常对话里获取经验值*/
        new SkillConfig(){ Skill_ID = 10210,Skill_Cost = 9,Skill_Precondition = 0,Skill_Exclusion = 0},
        /*老兵/你可以从战斗里获取经验值*/
        new SkillConfig(){ Skill_ID = 10220,Skill_Cost = 9,Skill_Precondition = 0,Skill_Exclusion = 0},
        /*美食品尝家/没吃过的东西可以获取经验值*/
        new SkillConfig(){ Skill_ID = 10230,Skill_Cost = 9,Skill_Precondition = 0,Skill_Exclusion = 0},
        /*预知梦/睡眠获取经验值*/
        new SkillConfig(){ Skill_ID = 10240,Skill_Cost = 9,Skill_Precondition = 0,Skill_Exclusion = 0},

        /*混混/缓慢恢复精神值你通缉值越高恢复的越多*/
        new SkillConfig(){ Skill_ID = 10300,Skill_Cost = 9,Skill_Precondition = 0,Skill_Exclusion = 0},
        /*坏蛋/你不再会被罪犯敌对，且你现在可以雇佣罪犯作为打手*/
        new SkillConfig(){ Skill_ID = 10301,Skill_Cost = 9,Skill_Precondition = 10300,Skill_Exclusion = 0},
        /*坏蛋头目/你雇佣打手时花费更低，而且打手只会攻击你攻击的目标*/
        new SkillConfig(){ Skill_ID = 10302,Skill_Cost = 9,Skill_Precondition = 10301,Skill_Exclusion = 0},
        /*老板/你觉得任何人都应该向你交税*/
        new SkillConfig(){ Skill_ID = 10303,Skill_Cost = 9,Skill_Precondition = 10302,Skill_Exclusion = 0},
        /*老板的老板/任何人都会向你交税，尤其是罪犯们*/
        new SkillConfig(){ Skill_ID = 10304,Skill_Cost = 9,Skill_Precondition = 10303,Skill_Exclusion = 0},

        /*神偷/你的偷窃行为不再会被发现*/
        new SkillConfig(){ Skill_ID = 10310,Skill_Cost = 9,Skill_Precondition = 0,Skill_Exclusion = 0},

        /*不起眼1/你会更快的脱离仇恨*/
        new SkillConfig(){ Skill_ID = 10320,Skill_Cost = 9,Skill_Precondition = 0,Skill_Exclusion = 0},
        /*不起眼2/被通缉后只要不主动惹事，你很难被其他人发现*/
        new SkillConfig(){ Skill_ID = 10321,Skill_Cost = 9,Skill_Precondition = 10320,Skill_Exclusion = 0},

        /*巧舌如簧1/犯罪后罚款会变低*/
        new SkillConfig(){ Skill_ID = 10330,Skill_Cost = 9,Skill_Precondition = 0,Skill_Exclusion = 0},
        /*巧舌如簧2/犯罪后罚款会变得更低*/
        new SkillConfig(){ Skill_ID = 10331,Skill_Cost = 9,Skill_Precondition = 10330,Skill_Exclusion = 0},
        /*巧舌如簧2/犯罪后罚款会变得非常低*/
        new SkillConfig(){ Skill_ID = 10332,Skill_Cost = 9,Skill_Precondition = 10331,Skill_Exclusion = 0},

        /*太阳科技/你现在可以制作太阳工作台*/
        new SkillConfig(){ Skill_ID = 10400,Skill_Cost = 9,Skill_Precondition = 10400,Skill_Exclusion = 0},
        /*太阳挂坠/解锁太阳挂坠配方*/
        new SkillConfig(){ Skill_ID = 10401,Skill_Cost = 9,Skill_Precondition = 10401,Skill_Exclusion = 0},
        /*太阳剑/解锁太阳剑配方*/
        new SkillConfig(){ Skill_ID = 10402,Skill_Cost = 9,Skill_Precondition = 10401,Skill_Exclusion = 0},
        /*太阳斧/解锁太阳斧配方*/
        new SkillConfig(){ Skill_ID = 10403,Skill_Cost = 9,Skill_Precondition = 10401,Skill_Exclusion = 0},
        /*太阳镐/解锁太阳镐配方*/
        new SkillConfig(){ Skill_ID = 10404,Skill_Cost = 9,Skill_Precondition = 10401,Skill_Exclusion = 0},
        /*太阳盔/解锁太阳盔配方*/
        new SkillConfig(){ Skill_ID = 10405,Skill_Cost = 9,Skill_Precondition = 10401,Skill_Exclusion = 0},
        /*太阳甲/解锁太阳甲配方*/
        new SkillConfig(){ Skill_ID = 10406,Skill_Cost = 9,Skill_Precondition = 10401,Skill_Exclusion = 0},
        /*太阳精炼液/解锁太阳精炼液配方*/
        new SkillConfig(){ Skill_ID = 10407,Skill_Cost = 9,Skill_Precondition = 10401,Skill_Exclusion = 0},
        /*万物本源/你现在可以制作能产出任何物品的小型太阳祭坛*/
        new SkillConfig(){ Skill_ID = 10408,Skill_Cost = 9,Skill_Precondition = 10401,Skill_Exclusion = 0},

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
