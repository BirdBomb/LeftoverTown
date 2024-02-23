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
    private int lastLeftClickTime = 0;
    private int lastRightClickTime = 0;
    /// <summary>
    /// ��ǰ���յ�������
    /// </summary>
    private int holdingIndex = 0;
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
        MessageBroker.Default.Receive<PlayerEvent.PlayerEvent_AddItemInBag>().Subscribe(_ =>
        {
            baseBehaviorController.AddItem_Bag(_.itemConfig);
        }).AddTo(this);
        baseBehaviorController.isPlayer = true;
    }
    private void Update()
    {
        PlayerInput();
    }
    #region//�������
    /// <summary>
    /// �������
    /// </summary>
    /// <param name="leftClick"></param>
    /// <param name="rightClick"></param>
    /// <param name="leftPress"></param>
    /// <param name="rightPress"></param>
    public void InputMouse(int leftClickTime,int rightClickTime,bool leftPress,bool rightPress, float time)
    {
        if (lastLeftClickTime != leftClickTime) 
        {
            lastLeftClickTime = leftClickTime;
            baseBehaviorController.holdingByHand.ClickLeftClick(time); 
        }
        if (lastRightClickTime != rightClickTime) 
        {
            lastRightClickTime = rightClickTime;
            baseBehaviorController.holdingByHand.ClickRightClick(time); 
        }
        if (leftPress) { baseBehaviorController.holdingByHand.PressLeftClick(time); }
        else { baseBehaviorController.holdingByHand.ReleaseLeftClick(time); }
        if (rightPress) { baseBehaviorController.holdingByHand.PressRightClick(time); }
        else { baseBehaviorController.holdingByHand.ReleaseRightClick(time); }
    }
    /// <summary>
    /// λ������
    /// </summary>
    /// <param name="dir">����</param>
    /// <param name="deltaTime">���</param>
    /// <param name="speedUp">����</param>
    public void InputMoveDir(Vector2 dir,float deltaTime, bool speedUp)
    {
        if (dir.x > 0)
        {
            baseBehaviorController.TurnRight();
        }
        if (dir.x < 0)
        {
            baseBehaviorController.TurnLeft();
        }
        baseBehaviorController.InputMoveVector(dir, deltaTime);
        baseBehaviorController.SpeedUp(speedUp);
        baseBehaviorController.MoveByVector(deltaTime);
    }
    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="dir">����</param>
    public void InputFaceDir(Vector2 dir, float time)
    {
        baseBehaviorController.InputFaceVector(dir);
        baseBehaviorController.holdingByHand.MousePosition(dir, time);
    }
    /// <summary>
    /// Ĭ������
    /// </summary>
    private void PlayerInput()
    {
        #region//��������
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (baseBehaviorController.Data.Holding_BagList.Count > 0)
            {
                holdingIndex++;
                if (holdingIndex >= baseBehaviorController.Data.Holding_BagList.Count)
                {
                    holdingIndex = 0;
                }
                baseBehaviorController.AddItem_Hand(baseBehaviorController.Data.Holding_BagList[holdingIndex]);
                MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_AddItemInHand
                {
                    itemConfig = baseBehaviorController.Data.Holding_BagList[holdingIndex]
                });
                MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_UI_AddItemInHand()
                {
                    itemConfig = baseBehaviorController.Data.Holding_BagList[holdingIndex]
                });
            }
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            for (int i = 0; i < nearbyTiles.Count; i++)
            {
                nearbyTiles[i].InvokeTile();
            }

        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var item = Physics2D.OverlapCircleAll(transform.position, 0.5f, itemLayer);
            if (item.Length > 0)
            {
                Debug.Log("PickUp0");
                if (item[0].gameObject.TryGetComponent(out ItemObj obj))
                {
                    baseBehaviorController.PickUpItem_Bag(obj);
                }
            }
        }
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
                nearbyTiles[i].FarawayTile();
                nearbyTiles.RemoveAt(i);
            }
        }
        /*��ӱ��μ���ĵؿ�*/
        for (int i = 0; i < tempTiles.Count; i++)
        {
            if (tempTiles[i] == null) { continue; }
            if (tempTiles[i].NearbyTile())
            {
                if (!nearbyTiles.Contains(tempTiles[i]))
                {
                    nearbyTiles.Add(tempTiles[i]);
                }
            }
        }
    }
}
