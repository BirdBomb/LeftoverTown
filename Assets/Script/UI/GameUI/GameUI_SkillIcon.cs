using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class GameUI_SkillIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler,IPointerUpHandler
{
    [Header("技能ID")]
    public short int_SkillID;
    public Image image_Icon;
    public Image image_Mark;
    public Image image_Signal;
    public Image image_full;
    public Transform tran_Point;
    public Text text_Cost; 
    public ParticleSystem particle_Explode‌;
    private SkillIconState bind_IconState;
    private bool bool_PointingOn;
    private bool bool_Press = false;
    private float float_Timer = 0;

    public void Update()
    {
        if (bool_Press)
        {
            if (float_Timer < 1 && bind_IconState == SkillIconState.Enable)
            {
                float_Timer += Time.deltaTime;
                image_full.transform.localScale = new Vector3(1, Mathf.Lerp(0, 1, float_Timer));
                if (float_Timer >= 1)
                {
                    particle_Explode‌.Clear();
                    particle_Explode‌.Play();
                    MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_AddSkill()
                    {
                        id = int_SkillID,
                    });
                }
            }
        }
        else
        {
            if (float_Timer > 0 && bind_IconState == SkillIconState.Enable)
            {
                float_Timer -= Time.deltaTime;
                image_full.transform.localScale = new Vector3(1, Mathf.Lerp(0, 1, float_Timer));
            }
        }
    }
    private void OnDisable()
    {
        if (bool_PointingOn)
        {
            MessageBroker.Default.Publish(new UIEvent.UIEvent_HidenfoTextUI()
            {

            });
        }
    }

    public void UpdateState(int cost, SkillIconState skillIconState)
    {
        bind_IconState = skillIconState;
        text_Cost.text = cost.ToString();

        image_Icon.transform.DOKill();
        image_Icon.transform.rotation = Quaternion.identity;
        image_full.transform.localScale = new Vector3(1, 0);
        image_Signal.gameObject.SetActive(false);
        image_Mark.gameObject.SetActive(false);

        switch (skillIconState)
        {
            case SkillIconState.Awake:
                image_Icon.color = Color.white;
                text_Cost.color = Color.white;
                break;
            case SkillIconState.Enable:
                image_Signal.gameObject.SetActive(true);
                image_Icon.color = new Color(1f, 1f, 1f, 0.5f);
                text_Cost.color = new Color(1f, 1f, 1f, 0.5f);
                break;
            case SkillIconState.Disable:
                image_Icon.color = new Color(1f, 1f, 1f, 0.25f);
                text_Cost.color = new Color(1f, 1f, 1f, 0.25f);
                break;
            case SkillIconState.Lock:
                image_Icon.color = Color.red;
                text_Cost.color = Color.red;
                break;
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        bool_PointingOn = true;

        string[] parts = LocalizationManager.Instance.GetLocalization("Skill_String", "Skill_" + int_SkillID).Split('/');
        string name = parts.Length > 0 ? parts[0] : "Error";
        string desc = parts.Length > 1 ? parts[1] : "Error";

        //image_Icon.transform.DOKill();
        image_Mark.gameObject.SetActive(true);
        image_Icon.transform.localScale = Vector3.one;
        MessageBroker.Default.Publish(new UIEvent.UIEvent_ShowInfoTextUI()
        {
            anchor = tran_Point.position,
            text = name + ":\n" + desc,
        });
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        bool_PointingOn = false;
        image_Mark.gameObject.SetActive(false);
        image_Icon.transform.localScale = Vector3.one;
        MessageBroker.Default.Publish(new UIEvent.UIEvent_HidenfoTextUI()
        {

        });
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        bool_Press = true;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        bool_Press = false;
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