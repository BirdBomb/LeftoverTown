using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;
using Fusion;
using DG.Tweening;
public class GameUI_Skill : MonoBehaviour
{
    private bool bool_Awake = false;
    private int int_Type = 1;
    public Transform tran_Panel;
    public Text text_SkillPoint;
    public Button btn_Close;
    [Header("心类型")]
    public Button button_Heart;
    [Header("心面板")]
    public GameObject panel_Heart;
    [Header("心技能列表")]
    public List<GameUI_SkillIcon> buttons_Heart = new List<GameUI_SkillIcon>();
    [Header("火类型")]
    public Button button_Fire;
    [Header("火面板")]
    public GameObject panel_Fire;
    [Header("火技能列表")]
    public List<GameUI_SkillIcon> buttons_Fire = new List<GameUI_SkillIcon>();
    [Header("金类型")]
    public Button button_Coin;
    [Header("金面板")]
    public GameObject panel_Coin;
    [Header("金技能列表")]
    public List<GameUI_SkillIcon> buttons_Coin = new List<GameUI_SkillIcon>();
    [Header("脑类型")]
    public Button button_Brain;
    [Header("脑面板")]
    public GameObject panel_Brain;
    [Header("脑技能列表")]
    public List<GameUI_SkillIcon> buttons_Brain = new List<GameUI_SkillIcon>();
    [Header("药类型")]
    public Button button_Potion;
    [Header("药面板")]
    public GameObject panel_Potion;
    [Header("药技能列表")]
    public List<GameUI_SkillIcon> buttons_Potion = new List<GameUI_SkillIcon>();

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
            if (bool_Awake)
            {
                UpdateSkillPanel();
            }
        }).AddTo(this);
        MessageBroker.Default.Receive<UIEvent.UIEvent_UpdateSkill>().Subscribe(_ =>
        {
            if (bool_Awake)
            {
                UpdateSkillPanel();
            }
        }).AddTo(this);

        button_Heart.onClick.AddListener(() => { int_Type = 1; UpdateSkillPanel(); });
        button_Fire.onClick.AddListener(() => { int_Type = 2; UpdateSkillPanel(); });
        button_Coin.onClick.AddListener(() => { int_Type = 3; UpdateSkillPanel(); });
        button_Brain.onClick.AddListener(() => { int_Type = 4; UpdateSkillPanel(); });
        button_Potion.onClick.AddListener(() => { int_Type = 5; UpdateSkillPanel(); });
        btn_Close.onClick.AddListener(HidePanel);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
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
        bool_Awake = true;
        tran_Panel.gameObject.SetActive(true);
        tran_Panel.DOKill();
        tran_Panel.localScale = Vector3.one;
        tran_Panel.DOPunchScale(new Vector3(0.1f, -0.1f, 0), 0.2f);
        UpdateSkillPanel();
    }
    private void HidePanel()
    {
        bool_Awake = false;
        tran_Panel.gameObject.SetActive(false);
    }
    private void UpdateSkillPanel()
    {
        PlayerCoreLocal playerCoreLocal = WorldManager.Instance.playerCoreLocal;
        if (playerCoreLocal && playerCoreLocal.actorManager_Bind)
        {
            skills_Cur = playerCoreLocal.actorManager_Bind.actorNetManager.Local_GetSkillList();
            int_SkillPoint = playerCoreLocal.actorManager_Bind.actorNetManager.Local_GetLevel();
            for (int i = 0; i < skills_Cur.Count; i++)
            {
                int_SkillPoint -= SkillConfigData.GetStatusConfig(skills_Cur[i]).Skill_Cost;
            }
            text_SkillPoint.text = int_SkillPoint.ToString();
        }
        UpdateSkillBtn(int_Type);
    }
    private void UpdateSkillBtn(int type)
    {
        skillConfigs_Show = SkillConfigData.statusConfigs.FindAll((x) => { return x.Skill_ID / 10000 == type; });
        List<GameUI_SkillIcon> buttons_Temp = ChooseBtns(type);
        for(int i = 0; i < buttons_Temp.Count; i++)
        {
            buttons_Temp[i].Clean();
        }
        for (int i = 0; i < buttons_Temp.Count; i++)
        {
            if (i <= skillConfigs_Show.Count) 
            {
                if (skills_Cur.Contains(skillConfigs_Show[i].Skill_ID))
                {
                    buttons_Temp[i].Set(skillConfigs_Show[i].Skill_ID, skillConfigs_Show[i].Skill_Cost, SkillIconState.Awake);
                }
                else if (skills_Cur.Contains(skillConfigs_Show[i].Skill_Precondition) || skillConfigs_Show[i].Skill_Precondition == 0)
                {
                    if (int_SkillPoint>= skillConfigs_Show[i].Skill_Cost)
                    {
                        buttons_Temp[i].Set(skillConfigs_Show[i].Skill_ID, skillConfigs_Show[i].Skill_Cost, SkillIconState.Enable);
                    }
                    else
                    {
                        buttons_Temp[i].Set(skillConfigs_Show[i].Skill_ID, skillConfigs_Show[i].Skill_Cost, SkillIconState.Disable);
                    }
                }
                else
                {
                    buttons_Temp[i].Set(skillConfigs_Show[i].Skill_ID, skillConfigs_Show[i].Skill_Cost, SkillIconState.Disable);
                }
                if (skills_Cur.Contains(skillConfigs_Show[i].Skill_Exclusion))
                {
                    buttons_Temp[i].Set(skillConfigs_Show[i].Skill_ID, skillConfigs_Show[i].Skill_Cost, SkillIconState.Lock);
                }
            }
        }
    }
    private List<GameUI_SkillIcon> ChooseBtns(int type)
    {
        panel_Heart.SetActive(type == 1);
        panel_Fire.SetActive(type == 2);
        panel_Coin.SetActive(type == 3);
        panel_Brain.SetActive(type == 4);
        switch (type)
        {
            case 1:
                return buttons_Heart;
            case 2:
                return buttons_Fire;
            case 3:
                return buttons_Coin;
            case 4:
                return buttons_Brain;
        }
        return new List<GameUI_SkillIcon>();
    }
}
