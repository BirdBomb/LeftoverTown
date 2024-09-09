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
        new SkillConfig(){ Skill_ID = 1001,Skill_Name = "���ܲ���",Skill_Desc = "�ܹ����ٶ̾����ƶ�����ʽ������ʲô��Ӫ����������������������԰������ʿ��ѧ��ĵ�һ������",Skill_Strength = 0,Skill_Intelligence = 0,Skill_Focus = 0, Skill_Agility = 0},
        new SkillConfig(){ Skill_ID = 1002,Skill_Name = "��ʽ����",Skill_Desc = "����������ܲ����������ڳ�̽������ٴ��ͷţ��Ա�ﵽ���䲻���Ч��",Skill_Strength = 0,Skill_Intelligence = 0,Skill_Focus = 0, Skill_Agility = 0},
        //new SkillConfig(){ Skill_ID = 1002,Skill_Name = "��˸��",Skill_Desc = "��ʹ���ߵ��������󼫸ߣ���Դ���޴ӿ�����������˲����ɵĴ��ͣ�����ʹ������Ŀ�ĵش������µ��Լ�Ȼ��ɱ���˹�ȥ���Լ�����������",Skill_Strength = 0,Skill_Intelligence = 0,Skill_Focus = 0, Skill_Agility = 0},
        //new SkillConfig(){ Skill_ID = 1003,Skill_Name = "ǰ��",Skill_Desc = "����Ҫ�κμ���˲���߳�����ʽ�������˻�ǳ�����(ʹ����)",Skill_Strength = 0,Skill_Intelligence = 0,Skill_Focus = 0, Skill_Agility = 0},
        //new SkillConfig(){ Skill_ID = 1004,Skill_Name = "��������",Skill_Desc = "������ʹ�����ڲ��ƶ����������ȥ���Σ�������ͼ��Ķ��鵫������������",Skill_Strength = 0,Skill_Intelligence = 0,Skill_Focus = 0, Skill_Agility = 0},
        //new SkillConfig(){ Skill_ID = 1005,Skill_Name = "��������",Skill_Desc = "������ʹ�����ڲ��������������ȥ���Σ�ֻ���൱���츳�Ľ�ͽ����ʹ��",Skill_Strength = 0,Skill_Intelligence = 0,Skill_Focus = 0, Skill_Agility = 0},
        //new SkillConfig(){ Skill_ID = 1006,Skill_Name = "������",Skill_Desc = "�ռ�ħ������ǿ���ħ��˺��ָ����Ŀռ䡣",Skill_Strength = 0,Skill_Intelligence = 0,Skill_Focus = 0, Skill_Agility = 0},
        //new SkillConfig(){ Skill_ID = 1007,Skill_Name = "�Ͽ�ն",Skill_Desc = "ʹ�ó���������ǰն�������Ի�������е�����",Skill_Strength = 0,Skill_Intelligence = 0,Skill_Focus = 0, Skill_Agility = 0},
        //new SkillConfig(){ Skill_ID = 1008,Skill_Name = "��Ծն",Skill_Desc = "������������ǰ���ն�������Ի�������е�����",Skill_Strength = 0,Skill_Intelligence = 0,Skill_Focus = 0, Skill_Agility = 0},
        //new SkillConfig(){ Skill_ID = 1009,Skill_Name = "����ն",Skill_Desc = "ʹ�ó���������ǰն��������һ������",Skill_Strength = 0,Skill_Intelligence = 0,Skill_Focus = 0, Skill_Agility = 0},
        //new SkillConfig(){ Skill_ID = 1010,Skill_Name = "������ն",Skill_Desc = "��ǰ������β������Ĵ�ն��",Skill_Strength = 0,Skill_Intelligence = 0,Skill_Focus = 0, Skill_Agility = 0},
        //new SkillConfig(){ Skill_ID = 1011,Skill_Name = "��תն",Skill_Desc = "ʹ�ó�������ն����Χ���е���",Skill_Strength = 0,Skill_Intelligence = 0,Skill_Focus = 0, Skill_Agility = 0},
        //new SkillConfig(){ Skill_ID = 1012,Skill_Name = "��ն",Skill_Desc = "һ˲�䷢�����ն��",Skill_Strength = 0,Skill_Intelligence = 0,Skill_Focus = 0, Skill_Agility = 0},
    };
}
[Serializable]
public struct SkillConfig
{
    [SerializeField]/*���*/
    public short Skill_ID;
    [SerializeField]/*����*/
    public string Skill_Name;
    [SerializeField]/*����*/
    public string Skill_Desc;
    [SerializeField]/*CD*/
    public string Skill_CD;
    [SerializeField]/*��Сʹ������*/
    public short Skill_Strength;
    [SerializeField]/*��Сʹ������*/
    public short Skill_Intelligence;
    [SerializeField]/*��Сʹ��רע*/
    public short Skill_Focus;
    [SerializeField]/*��Сʹ������*/
    public short Skill_Agility;
}
