using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using UniRx;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class UI_ItemKeeping : MonoBehaviour
{
    [SerializeField]
    private Image image_Item;
    [SerializeField]
    private Text text_ItemCount;
    [SerializeField]
    private SpriteAtlas spriteAtlas_ItemAtlas;
    private ItemData itemData_Bind = new ItemData(0);
    private bool keeping = false;
    void Start()
    {
        MessageBroker.Default.Receive<UIEvent.UIEvent_StartKeepingItem>().Subscribe(_ =>
        {
            ItemData data = PutOut(_.itemCell, _.itemData);
            PutIn(_.itemCell, itemData_Bind);
            itemData_Bind = data;
            UpdateKeeping();

        }).AddTo(this);
    }
    private void Update()
    {
        if (keeping)
        {
            image_Item.transform.position = Input.mousePosition;
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                Drop(itemData_Bind);
                itemData_Bind = new ItemData(0);
                UpdateKeeping();
            }
        }
    }
    private void UpdateKeeping()
    {
        if (itemData_Bind.Item_ID > 0 && itemData_Bind.Item_Count > 0)
        {
            keeping = true;
            KeepingOn();
        }
        else
        {
            keeping = false;
            KeepingOff();
        }
    }
    public void KeepingOn()
    {
        image_Item.sprite = spriteAtlas_ItemAtlas.GetSprite("Item_" + itemData_Bind.Item_ID);
        text_ItemCount.text = itemData_Bind.Item_Count.ToString();
        image_Item.gameObject.SetActive(true);
        image_Item.transform.DOKill();
        image_Item.transform.localScale = Vector3.one;
        image_Item.transform.DOShakeRotation(0.2f, new Vector3(0, 0, 10), 30, 720, false).SetEase(Ease.Linear).SetLoops(-1);
        image_Item.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
    }
    public void KeepingOff()
    {
        image_Item.transform.DOKill();
        image_Item.transform.rotation = Quaternion.identity;
        image_Item.transform.DOScale(Vector3.zero, 0.1f).OnComplete(() =>
        {
            image_Item.gameObject.SetActive(false);
        });
    }
    /// <summary>
    /// 放入
    /// </summary>
    /// <param name="gridCell"></param>
    /// <param name="itemData"></param>
    public void PutIn(UI_GridCell gridCell, ItemData itemData)
    {
        if (itemData.Item_ID > 0)
        {
            gridCell.PutIn(itemData);
        }
    }
    /// <summary>
    /// 取出
    /// </summary>
    /// <param name="gridCell"></param>
    /// <param name="itemData"></param>
    /// <returns></returns>
    public ItemData PutOut(UI_GridCell gridCell, ItemData itemData)
    {
        if (itemData.Item_ID <= 0)
        {
            /*目标是空气,不取出*/
            return new ItemData();
        }
        if (itemData.Equals(itemData_Bind))
        {
            /*目标是同类物品,不取出*/
            return new ItemData();
        }
        return gridCell.PutOut(itemData);
    }
    /// <summary>
    /// 掉略
    /// </summary>
    /// <param name="itemData"></param>
    public void Drop(ItemData itemData)
    {
        if (itemData.Item_ID > 0 && itemData_Bind.Item_Count > 0)
        {
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryDropItem()
            {
                item = itemData
            });
        }
    }
}
