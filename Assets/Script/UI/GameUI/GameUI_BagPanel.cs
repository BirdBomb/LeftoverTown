using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class GameUI_BagPanel : MonoBehaviour
{
    [Header("背包格子")]
    public List<UI_GridCell> gridCells_BagCellList = new List<UI_GridCell>();
    [Header("背包排序")]
    public Button btn_PutSort;
    [Header("背包选定框")]
    public Transform transform_Switch;
    [Header("背包锁")]
    public List<Image> images_BagLockList = new List<Image>();
    private List<ItemData> itemDatas_BagList = new List<ItemData>();
    private int _bagCapacity;
    private void Start()
    {
        MessageBroker.Default.Receive<UIEvent.UIEvent_UpdateItemInBag>().Subscribe(_ =>
        {
            _bagCapacity = _.bagCapacity;
            itemDatas_BagList = new List<ItemData>(_.itemDatas);
            BagUpdateItem();
            BagDrawEveryLock();

        }).AddTo(this);
        MessageBroker.Default.Receive<UIEvent.UIEvent_SwitchItemInBag>().Subscribe(_ =>
        {
            int index = _.index % gridCells_BagCellList.Count;
            transform_Switch.transform.position = gridCells_BagCellList[index].transform.position;
            transform_Switch.transform.DOKill();
            transform_Switch.transform.localScale = Vector3.one;
            transform_Switch.transform.DOPunchScale(new Vector3(0.5f, -0.5f, 0), 0.2f);
        }).AddTo(this);
        MessageBroker.Default.Receive<GameEvent.GameEvent_All_UpdateHour>().Subscribe(_ =>
        {
            BagUpdateItem();
        }).AddTo(this);
        MessageBroker.Default.Receive<UIEvent.UIEvent_PutItemInBag>().Subscribe(_ =>
        {
            if (_.item.Item_ID > 0)
            {
                AddInBagInfo putInBagInfo = new AddInBagInfo();
                putInBagInfo.id = _.item.Item_ID;
                putInBagInfo.count = _.item.Item_Count;
                AddInfo(putInBagInfo);
            }
        }).AddTo(this);
        MessageBroker.Default.Receive<UIEvent.UIEvent_PutItemOutBag>().Subscribe(_ =>
        {
            if (_.item.Item_ID > 0)
            {
                AddInBagInfo putInBagInfo = new AddInBagInfo();
                putInBagInfo.id = _.item.Item_ID;
                putInBagInfo.count = -_.item.Item_Count;
                Debug.Log(_.item.Item_ID);
                AddInfo(putInBagInfo);
            }
        }).AddTo(this);
        BindAllCell();
        InvokeRepeating("ShowNextInfo", 2, float_DurTime);
    }
    private void BindAllCell()
    {
        for (int i = 0; i < gridCells_BagCellList.Count; i++)
        {
            int index = i;
            gridCells_BagCellList[index].BindGrid(new ItemPath(ItemFrom.Bag, index), PutIn, PutOut, ClickCellLeft, ClickCellRight);
        }
        btn_PutSort.onClick.AddListener(BatchSort);
    }
    #region//绘制
    private void BagUpdateItem()
    {
        for (int i = 0; i < gridCells_BagCellList.Count; i++)
        {
            if (i < itemDatas_BagList.Count)
            {
                gridCells_BagCellList[i].UpdateData(itemDatas_BagList[i]);
            }
            else
            {
                gridCells_BagCellList[i].CleanItemBase();
            }
        }
    }
    private void BagDrawEveryLock()
    {
        for (int i = 0; i < images_BagLockList.Count; i++)
        {
            if (i < _bagCapacity)
            {
                images_BagLockList[i].enabled = false;
            }
            else
            {
                images_BagLockList[i].enabled = true;
            }
        }

    }
    #endregion
    #region//绑定
    public void ClickCellLeft(UI_GridCell gridCell)
    {
        if (gridCell._bindItemBase != null) gridCell._bindItemBase.GridCell_LeftClick(gridCell, gridCell._bindItemBase.itemData);
    }
    public void ClickCellRight(UI_GridCell gridCell)
    {
        if (gridCell._bindItemBase != null) gridCell._bindItemBase.GridCell_RightClick(gridCell, gridCell._bindItemBase.itemData);
    }
    public void PutIn(ItemData data, ItemPath path)
    {
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryAddItemInBag()
        {
            index = path.itemIndex,
            itemData = data,
        });
    }
    public ItemData PutOut(ItemData itemData_From, ItemData itemData_Out, ItemPath itemPath)
    {
        ItemData itemData_New = GameToolManager.Instance.SplitItem(itemData_From, itemData_Out);
        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryChangeItemInBag()
        {
            itemData = itemData_New,
            index = itemPath.itemIndex
        });
        return itemData_Out;
    }
    private void BatchSort()
    {
        List<ItemData> itemDatas = GameLocalManager.Instance.playerCoreLocal.actorManager_Bind.actorNetManager.Local_GetBagItem();
        itemDatas = GameToolManager.Instance.SortItemList(itemDatas);
        GameLocalManager.Instance.playerCoreLocal.actorManager_Bind.actorNetManager.Local_SetBagItem(itemDatas);
    }
    #endregion
    #region//弹出信息
    public struct AddInBagInfo
    {
        public int id;
        public int count;
    }

    public SpriteAtlas spriteAtlas_Item;
    [Header("所有面板")]
    public List<Transform> transforms_AllPanel = new List<Transform>();
    private List<Transform> transforms_AwakePanel = new List<Transform>();
    private List<AddInBagInfo> inBagInfos = new List<AddInBagInfo>();
    [Header("两个面板间隔")]
    public float float_PutInBagInfoPanelDistance;
    [Header("面板默认横坐标")]
    public float float_PutInBagInfoPanelPosX;

    private float float_WaitTimer = 2.0f;
    private float float_DurTime = 0.2f;

    private void AddInfo(AddInBagInfo addInBagInfo)
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
    public void ShowInfo(AddInBagInfo addInBagInfo)
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
        else
        {
            for (int i = 0; i < transforms_AllPanel.Count; i++)
            {
                if (!transforms_AwakePanel.Contains(transforms_AllPanel[i]))
                {
                    return transforms_AllPanel[i];
                }
            }
            return transforms_AwakePanel[0];
        }
    }
    private void InitPanel(Transform panel,AddInBagInfo info)
    {
        panel.DOComplete();
        panel.DOKill();
        panel.localPosition = new Vector3(float_PutInBagInfoPanelPosX, 0, 0);
        panel.DOLocalMoveX(0, float_DurTime).SetEase(Ease.OutBack);
        string itemName = LocalizationManager.Instance.GetLocalization("Item_String", info.id + "_Name");
        itemName = ItemConfigData.Colour(itemName, ItemConfigData.GetItemConfig(info.id).Item_Rarity);
        string itemCount = info.count.ToString();
        panel.Find("Name").GetComponent<TextMeshProUGUI>().text = itemName;
        if (info.count > 0)
        {
            panel.Find("Count").GetComponent<Text>().text = "+" + itemCount;
        }
        else
        {
            panel.Find("Count").GetComponent<Text>().text = itemCount;
        }
        panel.Find("Icon").GetComponent<Image>().sprite = spriteAtlas_Item.GetSprite("Item_" + info.id.ToString());
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
    #endregion
}
