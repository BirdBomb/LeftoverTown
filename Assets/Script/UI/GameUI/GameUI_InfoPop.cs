using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class GameUI_InfoPop : MonoBehaviour
{
    public struct PopInfoData
    {
        public int id;
        public int count;
        public string Name;
        public string Count;
        public string Sprite;
    }

    public SpriteAtlas spriteAtlas_Item;
    [Header("所有面板")]
    public List<Transform> transforms_AllPanel = new List<Transform>();
    private List<Transform> transforms_AwakePanel = new List<Transform>();
    private List<PopInfoData> inBagInfos = new List<PopInfoData>();
    [Header("两个面板间隔")]
    public float float_PutInBagInfoPanelDistance;
    [Header("面板默认横坐标")]
    public float float_PutInBagInfoPanelPosX;

    private float float_WaitTimer = 2.0f;
    private float float_DurTime = 0.2f;
    public void Start()
    {
        MessageBroker.Default.Receive<UIEvent.UIEvent_PutItemInBag>().Subscribe(Listen_PutItemInBag).AddTo(this);
        MessageBroker.Default.Receive<UIEvent.UIEvent_PutItemOutBag>().Subscribe(Listen_PutItemOutBag).AddTo(this);
        InvokeRepeating("ShowNextInfo", 2, float_DurTime);
    }
    
    private void Listen_PutItemInBag(UIEvent.UIEvent_PutItemInBag eventData)
    {
        if (eventData.item.I > 0)
        {
            PopInfoData popInfoData = new PopInfoData();
            string[] parts = LocalizationManager.Instance.GetLocalization("Item_String", "Item_" + eventData.item.I.ToString()).Split('_');
            string itemName = parts.Length > 0 ? parts[0] : "Error";
            string itemCount = "+" + eventData.item.C.ToString();
            itemName = ItemConfigData.Colour(itemName, ItemConfigData.GetItemConfig(eventData.item.I).Item_Rarity);

            popInfoData.Name = itemName;
            popInfoData.Count = itemCount;
            popInfoData.Sprite = "Item_" + eventData.item.I.ToString();
            AddInfo(popInfoData);
        }

    }
    private void Listen_PutItemOutBag(UIEvent.UIEvent_PutItemOutBag eventData)
    {
        if (eventData.item.I > 0)
        {
            PopInfoData popInfoData = new PopInfoData();
            string[] parts = LocalizationManager.Instance.GetLocalization("Item_String", "Item_" + eventData.item.I.ToString()).Split('_');
            string itemName = parts.Length > 0 ? parts[0] : "Error";
            string itemCount = (-eventData.item.C).ToString();
            itemName = ItemConfigData.Colour(itemName, ItemConfigData.GetItemConfig(eventData.item.I).Item_Rarity);

            popInfoData.Name = itemName;
            popInfoData.Count = itemCount;
            popInfoData.Sprite = "Item_" + eventData.item.I.ToString();
            AddInfo(popInfoData);
        }
    }
    private void AddInfo(PopInfoData addInBagInfo)
    {
        inBagInfos.Add(addInBagInfo);
    }
    private void ShowNextInfo()
    {
        if (inBagInfos.Count > 0)
        {
            ShowInfo(inBagInfos[0]);
            inBagInfos.RemoveAt(0);
        }
    }
    public void ShowInfo(PopInfoData addInBagInfo)
    {
        CancelInvoke("HideAllPanel");
        Invoke("HideAllPanel", float_WaitTimer);
        Transform panel = GetPanel();
        transforms_AwakePanel.Add(panel);
        InitPanel(panel, addInBagInfo);
        SortPanel(panel);
    }
    private Transform GetPanel()
    {
        if (transforms_AwakePanel.Count >= transforms_AllPanel.Count)
        {
            Transform panel = transforms_AwakePanel[0];
            transforms_AwakePanel.RemoveAt(0);
            return panel;
        }

        return transforms_AllPanel.FirstOrDefault(p => !transforms_AwakePanel.Contains(p))
               ?? transforms_AwakePanel[0];
    }
    private void InitPanel(Transform panel, PopInfoData info)
    {
        panel.DOComplete();
        panel.DOKill();
        panel.localPosition = new Vector3(float_PutInBagInfoPanelPosX, 0, 0);
        panel.DOLocalMoveX(0, float_DurTime).SetEase(Ease.OutBack);

        panel.Find("Name").GetComponent<TextMeshProUGUI>().text = info.Name;
        panel.Find("Count").GetComponent<Text>().text = info.Count;
        panel.Find("Icon").GetComponent<Image>().sprite = spriteAtlas_Item.GetSprite(info.Sprite);
        AudioManager.Instance.Play2DEffect(1002);
    }
    private void SortPanel(Transform panel)
    {
        for (int i = 0; i < transforms_AwakePanel.Count; i++)
        {
            if (transforms_AwakePanel[i] != panel)
            {
                transforms_AwakePanel[i].DOComplete();
                transforms_AwakePanel[i].DOKill();
                float y = transforms_AwakePanel[i].localPosition.y;
                transforms_AwakePanel[i].DOLocalMoveY(y + float_PutInBagInfoPanelDistance, float_DurTime);
            }
        }
    }
    private void HideAllPanel()
    {
        transforms_AwakePanel.Clear();
        for (int i = 0; i < transforms_AllPanel.Count; i++)
        {
            transforms_AllPanel[i].DOKill();
            transforms_AllPanel[i].DOLocalMoveX(float_PutInBagInfoPanelPosX, float_DurTime);
        }
    }

}
