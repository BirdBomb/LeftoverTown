using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;
using Fusion;
using DG.Tweening;
public class GameUI_Skill : MonoBehaviour
{
    public bool bool_Show = false;
    public Transform tran_Panel;
    public Text text_SkillPoint;
    public Button btn_Close;
    [Header("技能列表")]
    public List<GameUI_SkillIcon> buttons_SkillBtn = new List<GameUI_SkillIcon>();
    private short bind_SkillID;
    private List<short> skills_Cur = new List<short>();
    private List<SkillConfig> skillConfigs_Show = new List<SkillConfig>();
    /// <summary>
    /// 可用天赋点
    /// </summary>
    private int int_SkillPoint = 0;
    void Start()
    {
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_AddSkill>().Subscribe(_ =>
        {
            if (bool_Show)
            {
                UpdateSkillPanel();
            }
        }).AddTo(this);
        MessageBroker.Default.Receive<UIEvent.UIEvent_UpdateSkill>().Subscribe(_ =>
        {
            if (bool_Show)
            {
                UpdateSkillPanel();
            }
        }).AddTo(this);

        btn_Close.onClick.AddListener(HidePanel);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (tran_Panel.gameObject.activeSelf)
            {
                HidePanel();
            }
            else
            {
                ShowPanel();
            }
        }
    }

    private void ShowPanel()
    {
        bool_Show = true;
        tran_Panel.gameObject.SetActive(true);
        tran_Panel.DOKill();
        tran_Panel.localScale = Vector3.one;
        tran_Panel.DOPunchScale(new Vector3(0.1f, -0.1f, 0), 0.2f);
        UpdateSkillPanel();
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_Action()
        {
            action = PlayerAction.OpenSkill
        });
    }
    public void HidePanel()
    {
        bool_Show = false;
        tran_Panel.gameObject.SetActive(false);
    }
    private void UpdateSkillPanel()
    {
        PlayerCoreLocal playerCoreLocal = WorldActorManager.Instance.GetPlayer();
        if (playerCoreLocal && playerCoreLocal.actorManager_Bind)
        {
            skills_Cur = playerCoreLocal.Local_GetSkillList();
            int_SkillPoint = playerCoreLocal.Local_GetLevel();
            for (int i = 0; i < skills_Cur.Count; i++)
            {
                int_SkillPoint -= SkillConfigData.GetStatusConfig(skills_Cur[i]).Skill_Cost;
            }
            text_SkillPoint.text = int_SkillPoint.ToString();
        }
        UpdateSkillBtn();
    }
    private void UpdateSkillBtn()
    {
        skillConfigs_Show = SkillConfigData.statusConfigs;
        for (int i = 0; i < buttons_SkillBtn.Count; i++)
        {
            GameUI_SkillIcon skillBtn = buttons_SkillBtn[i];
            SkillConfig skillConfig = SkillConfigData.GetStatusConfig(skillBtn.int_SkillID);
            if (skills_Cur.Contains(skillConfig.Skill_ID))
            {
                skillBtn.UpdateState(skillConfig.Skill_Cost, SkillIconState.Awake);

            }
            else if (skills_Cur.Contains(skillConfig.Skill_Precondition) || skillConfig.Skill_Precondition == 0)
            {
                if (int_SkillPoint >= skillConfig.Skill_Cost)
                {
                    skillBtn.UpdateState(skillConfig.Skill_Cost, SkillIconState.Enable);
                }
                else
                {
                    skillBtn.UpdateState(skillConfig.Skill_Cost, SkillIconState.Disable);
                }
            }
            else
            {
                skillBtn.UpdateState(skillConfig.Skill_Cost, SkillIconState.Disable);
            }
            if (skills_Cur.Contains(skillConfigs_Show[i].Skill_Exclusion))
            {
                skillBtn.UpdateState(skillConfig.Skill_Cost, SkillIconState.Lock);
            }
        }
    }
}
