using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class UI_SkillPanel : MonoBehaviour
{
    public List<UI_SkillSlot> SkillSlotList_Use = new List<UI_SkillSlot>();
    public List<UI_SkillSlot> SkillSlotList_Know = new List<UI_SkillSlot>();
    private List<int> Skill_Know = new List<int>();
    private List<int> Skill_Use = new List<int>();
    private SpriteAtlas atlas;
    public void UpdateUsingSkill(List<int> skills)
    {
        if (!atlas) { atlas = Resources.Load<SpriteAtlas>("Atlas/SkillSprite"); }
        for (int i = 0; i < skills.Count; i++)
        {
            int index = i;
            SkillSlotList_Use[index].Init(skills[index], atlas.GetSprite(skills[index].ToString()));
        }
    }
    public void UpdateDraw()
    {
        if (!atlas) { atlas = Resources.Load<SpriteAtlas>("Atlas/SkillSprite"); }
        Skill_Know = GameLocalManager.Instance.localPlayer.playerData.Skills_Know;
        for (int i= 0; i < Skill_Know.Count; i++)
        {
            int index = i;
            SkillSlotList_Know[index].Init(Skill_Know[index], atlas.GetSprite(Skill_Know[index].ToString()));
        }
    }
}
