using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class GameUI_SkillIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Button button;
    public Image image;
    public Image mark;
    public Text text;
    private bool bool_Pointing;
    private string string_SkillStr;
    public void Clean()
    {
        button.onClick.RemoveAllListeners();
    }
    public void Set(short id, int cost, SkillIconState skillIconState)
    {
        string[] parts = LocalizationManager.Instance.GetLocalization("Skill_String", "Skill_" + id).Split('/');
        string name = parts.Length > 0 ? parts[0] : "Error";
        string desc = parts.Length > 1 ? parts[1] : "Error";
        string_SkillStr = name + ":" + desc;

        
        switch (skillIconState)
        {
            case SkillIconState.Awake:
                mark.gameObject.SetActive(false);
                text.text = cost.ToString();
                text.color = new Color(0.7f, 0.7f, 0.7f);
                image.color = new Color(0.7f, 0.7f, 0.7f);
                break;
            case SkillIconState.Enable:
                mark.gameObject.SetActive(true);
                text.text = cost.ToString();
                text.color = Color.white;
                image.color = Color.white;
                button.onClick.AddListener(() =>
                {
                    MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_AddSkill()
                    {
                        id = id,
                    });
                });
                break;
            case SkillIconState.Disable:
                mark.gameObject.SetActive(false);
                text.text = cost.ToString();
                text.color = new Color(0.3f, 0.3f, 0.3f);
                image.color = new Color(0.3f, 0.3f, 0.3f);
                break;
            case SkillIconState.Lock:
                mark.gameObject.SetActive(false);
                text.text = cost.ToString();
                text.color = Color.red;
                image.color = Color.red;
                break;
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        bool_Pointing = true;
        MessageBroker.Default.Publish(new UIEvent.UIEvent_ShowInfoTextUI()
        {
            anchor = eventData.position,
            text = string_SkillStr
        });
        image.transform.DOKill();
        image.transform.localScale = Vector3.one;
        image.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        bool_Pointing = false;
        MessageBroker.Default.Publish(new UIEvent.UIEvent_HidenfoTextUI()
        {

        });
    }
    private void OnDisable()
    {
        if (bool_Pointing)
        {
            MessageBroker.Default.Publish(new UIEvent.UIEvent_HidenfoTextUI()
            {

            });
        }
    }

}
public enum SkillIconState
{
    /// <summary>
    /// 已激活
    /// </summary>
    Awake,
    /// <summary>
    /// 可激活
    /// </summary>
    Enable,
    /// <summary>
    /// 不可激活
    /// </summary>
    Disable,
    /// <summary>
    /// 锁定
    /// </summary>
    Lock,
}