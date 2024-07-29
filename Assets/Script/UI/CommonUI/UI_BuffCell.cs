using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_BuffCell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Button button;
    public Image icon;
    public TextMeshProUGUI buffName;
    public TextMeshProUGUI buffPoint;
    public TextMeshProUGUI buffDesc;
    [HideInInspector]
    public BuffConfig buffData;
    private System.Action<BuffConfig> bindAction;
    public void Start()
    {
        button.onClick.AddListener(ClickBtn);
    }
    public void InitBuff(BuffConfig buff, System.Action<BuffConfig> action = null)
    {
        buffData = buff;
        buffName.text = buff.Buff_Name;
        buffPoint.text = buff.Buff_Cost.ToString();
        bindAction = action;
    }
    public void ClickBtn()
    {
        if(bindAction != null) 
        {
            bindAction.Invoke(buffData);
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        buffDesc.gameObject.SetActive(true);
        buffDesc.text = buffData.Buff_Desc;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buffDesc.gameObject.SetActive(false);

    }

}
