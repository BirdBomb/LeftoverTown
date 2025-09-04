using Fusion;
using System;
using UnityEngine;
using System.Collections;
public class ItemNetObj : NetworkBehaviour
{
    [Header("��Ʒ�ڵ�")]
    public Transform transform_Root;
    [Header("��Ʒͼ��")]
    public SpriteRenderer spriteRenderer_Icon;
    [Header("��Ʒ����")]
    public TextMesh textMesh_Count;
    [Networked, OnChangedRender(nameof(UpdateItem)), HideInInspector]
    public ItemData data { get; set; } = new ItemData();
    [Networked]
    public NetworkId owner { get; set; } = new NetworkId();
    private ItemBase _bindItem;
    private float radiu_Combine = 2;
    public override void Spawned()
    {
        base.Spawned();
        if (data.Item_ID != new ItemData().Item_ID)
        {
            UpdateItem();
        }
    }
    public virtual void UpdateItem()
    {
        if (_bindItem == null)
        {
            Type type = Type.GetType("Item_" + data.Item_ID.ToString());
            _bindItem = (ItemBase)Activator.CreateInstance(type);
        }
        else
        {
            if (_bindItem.itemData.Item_ID != data.Item_ID)
            {
                Type type = Type.GetType("Item_" + data.Item_ID.ToString());
                _bindItem = (ItemBase)Activator.CreateInstance(type);
            }
        }
        _bindItem.UpdateDataFromNet(data);
        _bindItem.NetObj_Draw(this, data);
        _bindItem.NetObj_PlayDrop(this);
    }
    /// <summary>
    /// ������_��ʼ��
    /// </summary>
    public virtual void State_Init(ItemData itemData)
    {
        data = itemData;
    }
    /// <summary>
    /// ������_��������
    /// </summary>
    public virtual void State_BindOwner(NetworkId networkId)
    {
        owner = networkId;
    }
    /// <summary>
    /// ������_���������
    /// </summary>
    public IEnumerator State_RemoveOwner()
    {
        yield return new WaitForSeconds(0.5f);
        owner = new NetworkId();
    }
    /// <summary>
    /// ������_����
    /// </summary>
    /// <param name="itemData"></param>
    public virtual void State_PickUp(NetworkId networkId, out ItemData itemData)
    {
        itemData = data;
        Runner.Despawn(Object);
    }
    /// <summary>
    /// ������_�ϲ�
    /// </summary>
    public virtual void State_CombineItem()
    {
        if (Object.HasStateAuthority)
        {
            var items = Physics2D.OverlapCircleAll(transform.position, radiu_Combine, LayerMask.GetMask("ItemObj"));
            foreach (Collider2D item in items)
            {
                if (item.gameObject.transform.parent.TryGetComponent(out ItemNetObj obj))
                {
                    if (obj.Equals(this)) { continue; }
                    if (obj.data.Item_ID == data.Item_ID)
                    {
                        Debug.Log(obj);
                        obj.data = GameToolManager.Instance.CombineItem(obj.data, data, out ItemData itemData_Res);
                        if (itemData_Res.Item_Count == 0 || itemData_Res.Item_ID == 0)
                        {
                            Runner.Despawn(Object);
                        }
                        break;
                    }
                    else
                    {
                        Debug.Log(obj.data.Item_ID + "/" + data.Item_ID);
                    }
                }
            }

        }
    }
}
