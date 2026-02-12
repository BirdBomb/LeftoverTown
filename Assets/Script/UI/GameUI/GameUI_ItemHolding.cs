using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class GameUI_ItemHolding : MonoBehaviour
{
    public Image image_ItemIcon;
    public Text text_ItemCount;
    public SpriteAtlas spriteAtlas_ItemAtlas;

    private ItemData itemData_Holding;
    private bool isHolding;
    private void Start()
    {
        MessageBroker.Default.Receive<UIEvent.UIEvent_StartHoldingItem>().Subscribe(_ =>
        {
            ItemData data = PutOutFromCell(_.itemCell, _.itemData_From, _.itemData_Out);
            PutInToCell(_.itemCell, itemData_Holding);
            itemData_Holding = data;
            UpdateHolding();

        }).AddTo(this);

    }
    private void Update()
    {
        if (isHolding)
        {
            image_ItemIcon.transform.position = Input.mousePosition;
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                Drop();
                UpdateHolding();
            }
        }
    }

    private void UpdateHolding()
    {
        if (itemData_Holding.I > 0 && itemData_Holding.C > 0)
        {
            isHolding = true;
            StartHolding();
        }
        else
        {
            isHolding = false;
            OverHolding();
        }
    }
    public void StartHolding()
    {
        image_ItemIcon.sprite = spriteAtlas_ItemAtlas.GetSprite("Item_" + itemData_Holding.I);
        text_ItemCount.text = itemData_Holding.C.ToString();
        image_ItemIcon.gameObject.SetActive(true);
        image_ItemIcon.transform.DOKill();
        image_ItemIcon.transform.localScale = Vector3.one;
        image_ItemIcon.transform.DOShakeRotation(0.2f, new Vector3(0, 0, 5), 30, 720, false).SetEase(Ease.Linear).SetLoops(-1);
        image_ItemIcon.transform.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
    }
    public void OverHolding()
    {
        image_ItemIcon.transform.DOKill();
        image_ItemIcon.transform.rotation = Quaternion.identity;
        image_ItemIcon.transform.DOScale(Vector3.zero, 0.05f).OnComplete(() =>
        {
            image_ItemIcon.gameObject.SetActive(false);
        });
    }
    public ItemData PutOutFromCell(UI_GridCell gridCell, ItemData itemData_From, ItemData itemData_Out)
    {
        if (itemData_Out.I <= 0 || itemData_Out.I == itemData_Holding.I)
        {
            return new ItemData();
        }
        else
        {
            return gridCell.PutOut(itemData_From, itemData_Out);
        }
    }
    public void PutInToCell(UI_GridCell gridCell, ItemData itemData_In)
    {
        if (itemData_In.I > 0)
        {
            gridCell.PutIn(itemData_In);
        }
    }
    public void Drop()
    {
        if (itemData_Holding.I > 0 && itemData_Holding.C > 0)
        {
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryDropItem()
            {
                item = itemData_Holding
            });
            itemData_Holding = new ItemData(0);
        }
    }
}
