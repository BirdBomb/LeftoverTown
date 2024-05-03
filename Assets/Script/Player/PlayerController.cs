using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Windows;
using static UnityEditor.Progress;
using Input = UnityEngine.Input;
/// <summary>
/// ��ҿ�����
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Header("����")]
    public ActorManager actorManager;
    [Header("����")]
    public Rigidbody2D myRigidbody;
    [Header("����㼶")]
    public LayerMask itemLayer;
    /// <summary>
    /// ��ǰ���յ�������
    /// </summary>
    private int holdingIndex = 0;
    [HideInInspector]
    public bool thisPlayerIsMe = false;
    [HideInInspector]
    public bool thisPlayerIsState = false;
    [HideInInspector]
    public int thisPlayerID = 0;
    private void Awake()
    {
        myRigidbody.gravityScale = 0;
    }
    private void Start()
    {
        MessageBroker.Default.Receive<GameEvent.GameEvent_Local_SomeoneMove>().Subscribe(_ =>
        {
            if (_.moveActor == actorManager && thisPlayerIsMe)
            {
                UpdateNearByTile();
            }
        }).AddTo(this);
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_TryRemoveItemFromBag>().Subscribe(_ =>
        {
            if (thisPlayerIsMe)
            {
                actorManager.NetController.RPC_Local_LoseItem(_.item);
            }
        }).AddTo(this);
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_TryAddItemInBag>().Subscribe(_ =>
        {
            if (thisPlayerIsMe)
            {
                actorManager.NetController.RPC_AddItemInBag(_.item);
            }
        }).AddTo(this);
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_TryDropItem>().Subscribe(_ =>
        {
            if (thisPlayerIsMe)
            {
                MessageBroker.Default.Publish(new GameEvent.GameEvent_Local_SpawnItem()
                {
                    itemData = _.item,
                    pos = transform.position - new Vector3(0, 0.1f, 0)
                });
            }
        }).AddTo(this);
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_Local_TrySpawnActor>().Subscribe(_ =>
        {
            if (thisPlayerIsMe)
            {
                MessageBroker.Default.Publish(new GameEvent.GameEvent_Local_SpawnActor()
                {
                    name = _.name,
                    pos = transform.position
                });
            }
        }).AddTo(this);
        actorManager.InitByPlayer();
        itemLayer = LayerMask.GetMask("ItemObj");
    }
    private void Update()
    {
        if (thisPlayerIsMe)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (actorManager.NetController.Data_ItemInBag.Count > 0)
                {
                    holdingIndex++;
                    if (holdingIndex >= actorManager.NetController.Data_ItemInBag.Count)
                    {
                        holdingIndex = 0;
                    }
                    actorManager.NetController.RPC_Local_HoldItem(actorManager.NetController.Data_ItemInBag[holdingIndex]);
                }
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                var item = Physics2D.OverlapCircleAll(transform.position, 0.5f, itemLayer);
                if (item.Length <= 0) { return; }
                if (item[0].gameObject.transform.parent.TryGetComponent(out ItemNetObj obj)) 
                {
                    actorManager.NetController.RPC_Local_PickItem(obj.Object.Id);
                }
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                for (int i = 0; i < nearbyTiles.Count; i++)
                {
                    nearbyTiles[i].InvokeTile();
                }
            }
        }
    }
    #region//�������
    /// <summary>
    /// ����������
    /// </summary>
    private int lastLeftClickTime = 0;
    /// <summary>
    /// �������
    /// </summary>
    private bool lastLeftPress = false;
    /// <summary>
    /// �Ҽ��������
    /// </summary>
    private int lastRightClickTime = 0;
    /// <summary>
    /// �Ҽ�����
    /// </summary>
    private bool lastRightPress = false;

    /// <summary>
    /// �������
    /// </summary>
    /// <param name="leftClickTime">����������</param>
    /// <param name="rightClickTime">�Ҽ��������</param>
    /// <param name="leftPress">�������</param>
    /// <param name="rightPress">�Ҽ�����</param>
    public void All_PlayerInputMouse(float dt, int leftClickTime,int rightClickTime,bool leftPress,bool rightPress, bool hasStateAuthority, bool hasInputAuthority)
    {
        if (lastLeftClickTime != leftClickTime) 
        {
            lastLeftClickTime = leftClickTime;
            actorManager.holdingByHand.ClickLeftClick(dt, hasStateAuthority, hasInputAuthority, hasInputAuthority);
        }
        if (lastRightClickTime != rightClickTime) 
        {
            lastRightClickTime = rightClickTime;
            actorManager.holdingByHand.ClickRightClick(dt, hasStateAuthority, hasInputAuthority, hasInputAuthority);
        }
        if (leftPress) 
        {
            lastLeftPress = leftPress;
            actorManager.holdingByHand.PressLeftClick(dt, hasStateAuthority, hasInputAuthority, hasInputAuthority);
        }
        else if(lastLeftPress)
        {
            lastLeftPress = leftPress;
            actorManager.holdingByHand.ReleaseLeftClick(dt, hasStateAuthority, hasInputAuthority, hasInputAuthority);
        }
        if (rightPress) 
        {
            lastRightPress = rightPress;
            actorManager.holdingByHand.PressRightClick(dt, hasStateAuthority, hasInputAuthority, hasInputAuthority);
        }
        else if (lastRightPress)
        {
            lastRightPress = rightPress;
            actorManager.holdingByHand.ReleaseRightClick(dt, hasStateAuthority, hasInputAuthority, hasInputAuthority);
        }
    }
    /// <summary>
    /// λ������
    /// </summary>
    /// <param name="dir">����</param>
    /// <param name="speedUp">����</param>
    public void State_PlayerInputMove(float deltaTime, Vector2 dir, bool speedUp, bool hasStateAuthority, bool hasInputAuthority)
    {
        dir = dir.normalized;
        float speed;
        if (speedUp) 
        {
            speed = actorManager.actorConfig.Config_Speed * 2;
        }
        else
        {
            speed = actorManager.actorConfig.Config_Speed;
        }
        Vector3 newPos = transform.position + new UnityEngine.Vector3(dir.x * speed * deltaTime, dir.y * speed * deltaTime, 0);
        actorManager.NetController.UpdateNetworkTransform(newPos);
    }
    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="dir">����</param>
    public void All_PlayerInputFace(float dt, Vector2 dir, bool hasStateAuthority, bool hasInputAuthority)
    {
        actorManager.FaceTo(dir);
        actorManager.holdingByHand.FaceTo(dir, dt);
    }

    #endregion
    #region
    public List<MyTile> nearbyTiles = new List<MyTile>();
    private List<MyTile> tempTiles = new List<MyTile>();
    /// <summary>
    /// ���¸����ĵؿ�
    /// </summary>
    private void UpdateNearByTile()
    {
        /*��Χ�ؿ�*/
        tempTiles = actorManager.GetNearbyTile();
        /*�޳��ϴμ��ĵؿ�*/
        for (int i = 0; i < nearbyTiles.Count; i++)
        {
            if (nearbyTiles[i] == null) { continue; }
            if (!tempTiles.Contains(nearbyTiles[i]))
            {
                nearbyTiles[i].FarawayTileByPlayer(this);
                nearbyTiles.RemoveAt(i);
            }
        }
        /*��ӱ��μ���ĵؿ�*/
        for (int i = 0; i < tempTiles.Count; i++)
        {
            if (tempTiles[i] == null) { continue; }
            if (tempTiles[i].NearbyTileByPlayer(this))
            {
                if (!nearbyTiles.Contains(tempTiles[i]))
                {
                    nearbyTiles.Add(tempTiles[i]);
                }
            }
        }
    }
    #endregion
}
