using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Windows;
using Input = UnityEngine.Input;
/// <summary>
/// ��ҿ�����
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Header("��Ϊ������")]
    public BaseBehaviorController baseBehaviorController;
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
        MessageBroker.Default.Receive<GameEvent.GameEvent_SomeoneMove>().Subscribe(_ =>
        {
            if (_.moveRole == baseBehaviorController)
            {
                CheckAround();
            }
        }).AddTo(this);
        baseBehaviorController.isPlayer = true;
    }
    #region//�������
    private int lastLeftClickTime = 0;
    private int lastRightClickTime = 0;
    private int lastQClickTime = 0;
    private int lastFClickTime = 0;
    private int lastSpaceClickTime = 0;
    /// <summary>
    /// �������
    /// </summary>
    /// <param name="leftClick"></param>
    /// <param name="rightClick"></param>
    /// <param name="leftPress"></param>
    /// <param name="rightPress"></param>
    public void PlayerInputMouse(int leftClickTime,int rightClickTime,bool leftPress,bool rightPress, float time,bool hasStateAuthority, bool hasInputAuthority)
    {
        if (lastLeftClickTime != leftClickTime) 
        {
            lastLeftClickTime = leftClickTime;
            baseBehaviorController.holdingByHand.ClickLeftClick(time, hasInputAuthority); 
        }
        if (lastRightClickTime != rightClickTime) 
        {
            lastRightClickTime = rightClickTime;
            baseBehaviorController.holdingByHand.ClickRightClick(time, hasInputAuthority); 
        }
        if (leftPress) { baseBehaviorController.holdingByHand.PressLeftClick(time, hasInputAuthority); }
        else { baseBehaviorController.holdingByHand.ReleaseLeftClick(time, hasInputAuthority); }
        if (rightPress) { baseBehaviorController.holdingByHand.PressRightClick(time, hasInputAuthority); }
        else { baseBehaviorController.holdingByHand.ReleaseRightClick(time, hasInputAuthority); }
    }
    /// <summary>
    /// λ������
    /// </summary>
    /// <param name="dir">����</param>
    /// <param name="deltaTime">���</param>
    /// <param name="speedUp">����</param>
    public void PlayerInputMove(Vector2 dir,float deltaTime, bool speedUp, bool hasStateAuthority, bool hasInputAuthority)
    {
        baseBehaviorController.InputMoveVector(dir, deltaTime);
        baseBehaviorController.SpeedUp(speedUp);
        baseBehaviorController.MoveByVector(deltaTime);
    }
    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="dir">����</param>
    public void PlayerInputFace(Vector2 dir, float time, bool hasStateAuthority, bool hasInputAuthority)
    {
        baseBehaviorController.InputFaceVector(dir);
        baseBehaviorController.holdingByHand.MousePosition(dir, time);
    }
    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="clickQ">Q</param>
    /// <param name="clickF">F</param>
    /// <param name="clickSpace">Space</param>
    public void PlayerInputControl(int clickQ,int clickF,int clickSpace, bool hasStateAuthority, bool hasInputAuthority)
    {
        if (thisPlayerIsMe)
        {
            if (lastQClickTime != clickQ)
            {
                lastQClickTime = clickQ;
                if (baseBehaviorController.Data.Holding_BagList.Count > 0)
                {
                    holdingIndex++;
                    if (holdingIndex >= baseBehaviorController.Data.Holding_BagList.Count)
                    {
                        holdingIndex = 0;
                    }
                    MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_AddItemInHand
                    {
                        itemConfig = baseBehaviorController.Data.Holding_BagList[holdingIndex]
                    });
                }
            }
        }
        if (lastFClickTime != clickF)
        {
            lastFClickTime = clickF;
            if (thisPlayerIsMe)
            {
                for (int i = 0; i < nearbyTiles.Count; i++)
                {
                    nearbyTiles[i].InvokeTile();
                }
            }
        }
        if (lastSpaceClickTime != clickSpace)
        {
            lastSpaceClickTime = clickSpace;
            var item = Physics2D.OverlapCircleAll(transform.position, 0.5f, itemLayer);
            if (item.Length > 0)
            {
                if (item[0].gameObject.TryGetComponent(out ItemObj obj))
                {
                    baseBehaviorController.PickUpItem_Bag(obj);
                }
            }
        }
        #region//��������
        #endregion
        #region//��������
        if (Input.GetKey(KeyCode.M))
        {
            MessageBroker.Default.Publish(new MapEvent.MapEvent_SaveMap
            {

            });
        }
        #endregion
    }

    #endregion
    public List<MyTile> nearbyTiles = new List<MyTile>();
    private List<MyTile> tempTiles = new List<MyTile>();
    /// <summary>
    /// ��鸽���ؿ�
    /// </summary>
    private void CheckAround()
    {
        /*��Χ�ؿ�*/
        tempTiles = baseBehaviorController.GetNearbyTile();
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
}
