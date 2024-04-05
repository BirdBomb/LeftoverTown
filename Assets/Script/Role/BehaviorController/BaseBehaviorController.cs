using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Unity.VisualScripting;
using Vector3 = UnityEngine.Vector3;
using static UnityEditor.Progress;
using Fusion;
using static PlayerNetController;
using Random = UnityEngine.Random;
using static Fusion.Allocator;
using UnityEngine.U2D;
/// <summary>
/// ��Ϊ������
/// </summary>
public class BaseBehaviorController : NetworkBehaviour
{
    [Header("���������")]
    public BaseBodyController bodyController;
    [Header("����UI")]
    public ActorUI actorUI;
    [Header("����ָʾ��")]
    public SI_Sector skillSector;
    [Header("�ֲ�������")]
    public ItemBase holdingByHand = new ItemBase();
    private Vector2 faceTo = Vector2.zero;
    [SerializeField]
    public RoleData Data = new RoleData();
    [HideInInspector]
    public bool isPlayer = false;
    private void Start()
    {
        Init();
    }
    public virtual void FixedUpdate()
    {
        if (Object.HasStateAuthority)
        {
            State_Update();
        }
    }
    public virtual void Init()
    {
        navManager = GameObject.Find("Furniture").GetComponent<NavManager>();
    }
    /*�ƶ�*/
    #region
    [Header("������ڵ�")]
    public Transform Root;
    [Header("λ��ͬ�����")]
    public NetworkTransform NetTransform;

    [HideInInspector, Header("�ƶ�����")]
    public Vector2 tempVector;
    [HideInInspector, Header("��ǰλ��")]
    public MyTile curTile;
    [HideInInspector, Header("֮ǰλ��")]
    public MyTile lastTile;
    [HideInInspector, Header("����״̬")]
    public bool speedUp_State;
    [HideInInspector, Header("��������")]
    public float speedUp_Val = 2f;
    [HideInInspector, Header("��ʱ·��")]
    public List<MyTile> tempPathList = new List<MyTile>();
    [HideInInspector, Header("��һ��·����")]
    public MyTile tempPath = null;

    public virtual void InputFaceVector(Vector3 face)
    {
        faceTo = face;
        if (faceTo.x > 0)
        {
            FaceRight();
        }
        else
        {
            FaceLeft();
        }
    }
    /// <summary>
    /// �ƶ�����
    /// </summary>
    /// <param name="dir"></param>
    /// <param name="speed"></param>
    /// <param name="dt"></param>
    public void PlayMove(float speed)
    {
        bodyController.PlayBodyAction(BodyAction.Move, speed, null);
        bodyController.PlayHeadAction(HeadAction.Move, speed, null);
        bodyController.PlayLegAction(LegAction.Step, speed,null);
        bodyController.PlayHandAction(HandAction.Step, speed, null);
    }
    /// <summary>
    /// ת�򶯻�
    /// </summary>
    /// <param name="dir"></param>
    public void PlayTurn(Vector2 dir)
    {
        if (dir.x > 0)
        {
            TurnRight();
        }
        if (dir.x < 0)
        {
            TurnLeft();
        }
    }
    /// <summary>
    /// ֹͣ����
    /// </summary>
    public void PlayStop(float speed)
    {
        bodyController.PlayBodyAction(BodyAction.Idle, speed, null);
        bodyController.PlayHeadAction(HeadAction.Idle, speed, null);
        bodyController.PlayLegAction(LegAction.Idle, speed,null);
        bodyController.PlayHandAction(HandAction.Idle, speed, null);
    }
    #endregion
    /*Ѱ·*/
    #region
    public MyTile GetMyTile()
    {
        if (navManager != null)
        {
            Vector3Int vector3Int = navManager.grid.WorldToCell(transform.position);
            return (MyTile)navManager.tilemap.GetTile(vector3Int);
        }
        else
        {
            return null;
        }
    }
    List<MyTile> tempTiles = new List<MyTile>();
    public List<MyTile> GetNearbyTile()
    {
        tempTiles.Clear();
        MyTile tile = GetMyTile();

        tempTiles.Add((MyTile)navManager.tilemap.GetTile(new Vector3Int(tile.x, tile.y, 0)));
        tempTiles.Add((MyTile)navManager.tilemap.GetTile(new Vector3Int(tile.x + 1, tile.y, 0)));
        tempTiles.Add((MyTile)navManager.tilemap.GetTile(new Vector3Int(tile.x - 1, tile.y, 0)));
        tempTiles.Add((MyTile)navManager.tilemap.GetTile(new Vector3Int(tile.x, tile.y + 1, 0)));
        tempTiles.Add((MyTile)navManager.tilemap.GetTile(new Vector3Int(tile.x, tile.y - 1, 0)));
        return tempTiles;
    }
    public Vector2 GetMyPos()
    {
        Vector3Int vector3Int = navManager.grid.WorldToCell(transform.position);
        return new Vector2(vector3Int.x, vector3Int.y);
    }
    #endregion
    /*����*/
    #region
    /// <summary>
    /// ������ʱ�����
    /// </summary>
    public virtual void ListenTimeUpdate()
    {
    }
    #endregion




    /*����ͬ������*/
    #region

    [Networked]
    public int Net_Hp { get; set; }
    [Networked]
    public int Net_Food { get; set; }
    [Networked]
    public int Net_Water { get; set; }
    [Networked]
    public NetworkItemConfig Net_ItemInHand { get; set; }
    [Networked]
    public NetworkLinkedList<NetworkItemConfig> Net_ItemInBag { get; }
    [Networked]
    public NetworkLinkedList<int> Net_Buffs { get; }
    #endregion

    /*RPC����*/
    #region
    public void RPC_TryToAddItemInBag(ItemConfig config)
    {
        RPC_AddItemInBag(ItemConfigLocalToNet(config), Object.InputAuthority);
    }
    public void RPC_TryToAddItemInHand(ItemConfig config)
    {
        RPC_AddItemInHand(ItemConfigLocalToNet(config), Object.InputAuthority);
    }
    public void RPC_TryToRemoveItemFromBag(ItemConfig config)
    {
        RPC_RemoveItemFromBag(ItemConfigLocalToNet(config), Object.InputAuthority);
    }
    public void RPC_TryToChangeHP(int val)
    {
        RPC_ChangeHP(val);
    }
    public void RPC_TryToChangeFood(int val)
    {
        RPC_ChangeFood(val);
    }

    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_AddItemInBag(NetworkItemConfig itemConfig, PlayerRef player)
    {
        Debug.Log("���������" + player);
        if (player == Object.InputAuthority && isPlayer)
        {
            All_AddItem_Bag(ItemConfigNetToLocal(itemConfig), true);
        }
        else
        {
            All_AddItem_Bag(ItemConfigNetToLocal(itemConfig), false);
        }
    }
    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_AddItemInHand(NetworkItemConfig config, PlayerRef player)
    {
        Debug.Log("���������" + player);
        if (player == Object.InputAuthority && isPlayer)
        {
            All_AddItem_Hand(ItemConfigNetToLocal(config), true);
        }
        else
        {
            All_AddItem_Hand(ItemConfigNetToLocal(config), false);
        }
    }
    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_RemoveItemFromBag(NetworkItemConfig itemConfig, PlayerRef player)
    {
        if (player == Object.InputAuthority && isPlayer)
        {
            All_SubItem_Bag(ItemConfigNetToLocal(itemConfig), true);
        }
        else
        {
            All_SubItem_Bag(ItemConfigNetToLocal(itemConfig), false);
        }
        Debug.Log("���������" + player);
    }
    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_ChangeHP(int val)
    {
        if (val < 0)
        {
            GameObject obj_num = UIManager.Instance.ShowUI("UI/UI_DamageNum", (Vector2)transform.position + new Vector2(0, 1));
            obj_num.GetComponent<UI_DamageNum>().Play(val, new Color32(255, 50, 50, 255));

            if (Net_Hp + val > 0)
            {
                Net_Hp += val;
                actorUI.UpdateHPBar((float)Data.Data_Hp / (float)Data.Data_HpMax);
                bodyController.PlayHeadAction(HeadAction.TakeDamage, 1, null);
            }
            else
            {
                Data.Data_Hp = 0;
                Data.Data_Dead = true;
                actorUI.UpdateHPBar((float)Data.Data_Hp / (float)Data.Data_HpMax);
                bodyController.PlayHeadAction(HeadAction.Dead, 1, (str) =>
                {
                    if (str == "Dead")
                    {
                        Dead();
                    }
                });
                bodyController.PlayBodyAction(BodyAction.Dead, 1, null);
                bodyController.PlayHandAction(HandAction.Dead, 1, null);
                bodyController.PlayLegAction(LegAction.Dead, 1, null);
            }
        }
        if (isPlayer && Object.HasInputAuthority)
        {
            MessageBroker.Default.Publish(new UIEvent.UIEvent_UpdateData()
            {
                HP = Data.Data_Hp,
                Food = Data.Data_Food,
                Water = Data.Data_Water,
            });
        }
    }
    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_ChangeFood(int val)
    {
        if (val > 0)
        {
            Data.Data_Food += val;
            if (Data.Data_Food > Data.Data_FoodMax)
            {
                Data.Data_Food = Data.Data_FoodMax;
            }
        }
        else
        {
            Data.Data_Food -= val;
        }
        if (isPlayer && Object.HasInputAuthority)
        {
            MessageBroker.Default.Publish(new UIEvent.UIEvent_UpdateData()
            {
                HP = Data.Data_Hp,
                Food = Data.Data_Food,
                Water = Data.Data_Water,
            });
        }
    }
    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_AddBuff(int val)
    {
        Type type = Type.GetType("Buff_" + val.ToString());
        ItemBase food = (ItemBase)Activator.CreateInstance(type);
    }

    private ItemConfig ItemConfigNetToLocal(NetworkItemConfig config)
    {
        ItemConfig itemConfig = new ItemConfig();
        itemConfig.Item_ID = config.Item_ID;
        itemConfig.Item_Name = config.Item_Name.Value;
        itemConfig.Item_Desc = config.Item_Desc.Value;
        itemConfig.Item_CurCount = config.Item_CurCount;
        itemConfig.Item_MaxCount = config.Item_MaxCount;
        itemConfig.Item_Type = config.Item_Type;
        itemConfig.Average_Weight = config.Average_Weight;
        itemConfig.Average_Value = config.Average_Value;
        itemConfig.Item_Info = config.Item_Info.Value;
        return itemConfig;
    }
    private NetworkItemConfig ItemConfigLocalToNet(ItemConfig config)
    {
        NetworkItemConfig itemConfig = new NetworkItemConfig();
        itemConfig.Item_ID = config.Item_ID;
        itemConfig.Item_Name = config.Item_Name;
        itemConfig.Item_Desc = config.Item_Desc;
        itemConfig.Item_CurCount = config.Item_CurCount;
        itemConfig.Item_MaxCount = config.Item_MaxCount;
        itemConfig.Item_Type = config.Item_Type;
        itemConfig.Average_Weight = config.Average_Weight;
        itemConfig.Average_Value = config.Average_Value;
        itemConfig.Item_Info = config.Item_Info;
        return itemConfig;
    }
    #endregion

    /*����������*/
    #region
    public NavManager navManager;
    public List<MyTile> _State_MyLoad = new List<MyTile>();
    public MyTile _State_NextTile;

    /// <summary>
    /// ����������
    /// </summary>
    public virtual void State_Update()
    {
        if (Object.HasStateAuthority)
        {
            if (_State_NextTile)
            {
                State_MovePath(Time.fixedDeltaTime);
            }
        }
    }
    /// <summary>
    /// ����������,���ݵؿ��������λ��
    /// </summary>
    /// <param name="pos"></param>
    public void State_SetMyTile(Vector3Int pos)
    {
        if (navManager != null)
        {
            State_Teleport(navManager.grid.CellToWorld(pos));
        }
        else
        {
            navManager = GameObject.Find("Furniture").GetComponent<NavManager>();
            State_Teleport(navManager.grid.CellToWorld(pos));
        }
    }
    /// <summary>
    /// ����������,�����������λ��
    /// </summary>
    /// <param name="pos"></param>
    public void State_Teleport(Vector3 pos)
    {
        if (NetTransform && Object.HasStateAuthority)
        {
            NetTransform.Teleport(pos);
        }
    }
    /// <summary>
    /// ����������,����·��
    /// </summary>
    public void State_CalculatePath(Vector2 from, Vector2 to)
    {
        if (Object.HasStateAuthority)
        {
            if (_State_MyLoad != null)
            {
                _State_MyLoad.Clear();
            }
            _State_MyLoad = navManager.FindPath(navManager.FindTileByPos(to), navManager.FindTileByPos(from));
            State_ChangePath(_State_MyLoad);
        }
    }
    /// <summary>
    /// ����������,����·��
    /// </summary>
    /// <param name="path"></param>
    public void State_ChangePath(List<MyTile> path)
    {
        if (Object.HasStateAuthority)
        {
            tempPathList.Clear();
            for (int i = 1; i < path.Count; i++)
            {
                tempPathList.Add(path[i]);
                if (i == 1) { tempPath = tempPathList[0]; }
            }
        }
    }
    /// <summary>
    /// ����������,ִ��·��
    /// </summary>
    /// <param name="dt"></param>
    public void State_MovePath(float dt)
    {
        /*����·����*/
        if (Vector2.Distance(_State_NextTile.pos, transform.position) <= 0.1f)
        {
            _State_NextTile = null;
            if (_State_MyLoad.Count > 0)
            {
                _State_MyLoad.RemoveAt(0);
                if (_State_MyLoad.Count > 0)
                {
                    _State_NextTile = _State_MyLoad[0];
                }
            }
            State_CheckMyPos();
            return;
        }
        else
        {
            Vector2 temp = Vector2.zero;
            if (transform.position.x > _State_NextTile.x)
            {
                if ((transform.position.x - _State_NextTile.x) > 0.05f)
                {
                    temp += new Vector2(-1, 0);
                }
            }
            else
            {
                if ((transform.position.x - _State_NextTile.x) < -0.05f)
                {
                    temp += new Vector2(1, 0);
                }
            }
            if (transform.position.y > _State_NextTile.y)
            {
                if ((transform.position.y - _State_NextTile.y) > 0.05f)
                {
                    temp += new Vector2(0, -1);
                }
            }
            else
            {
                if ((transform.position.y - _State_NextTile.y) < -0.05f)
                {
                    temp += new Vector2(0, 1);
                }
            }
            temp = temp.normalized;
            State_Teleport(transform.position + new UnityEngine.Vector3(temp.x * dt, temp.y * dt, 0));

            if (Vector2.Distance(_State_NextTile.pos, transform.position) <= 0.1f)
            {
                State_CheckMyPos();
            }
        }
    }
    /// <summary>
    /// ����������,����ҵ�λ��
    /// </summary>
    public void State_CheckMyPos()
    {
        curTile = GetMyTile();
        if (!curTile) { return; }
        if (lastTile == null) { lastTile = curTile; }
        if (lastTile.pos != curTile.pos)
        {
            lastTile = curTile;
        }
    }
    #endregion
    /*ͨ�÷���*/
    #region
    public void All_AddItem_Hand(ItemConfig config, bool showInUI)
    {
        Data.Holding_ByHand = config;
        Type type = Type.GetType("Item_" + config.Item_ID.ToString());
        holdingByHand = (ItemBase)Activator.CreateInstance(type);
        //holdingByHand.Init(config);
        //holdingByHand.BeHolding(this, bodyController.Hand_RightItem);
        /*����UI*/
        if (showInUI)
        {
            MessageBroker.Default.Publish(new UIEvent.UIEvent_UpdateItemInHand()
            {
                itemConfig = Data.Holding_ByHand
            });
        }
    }
    public void All_UseItem_Hand(ItemConfig config)
    {

    }
    public void All_SubItem_Hand(bool showInUI)
    {
        ItemConfig item = new ItemConfig();
        item.Item_ID = -1;
        Data.Holding_ByHand = item;

        bodyController.Hand_RightItem.GetComponent<SpriteRenderer>().sprite = null;
        /*����UI*/
        if (showInUI)
        {
            MessageBroker.Default.Publish(new UIEvent.UIEvent_UpdateItemInHand()
            {
                itemConfig = Data.Holding_ByHand
            });
        }
    }
    public void All_AddItem_Bag(ItemConfig config, bool showInUI)
    {
        /*����������������Ϊ��*/
        if (config.Item_CurCount <= 0) { config.Item_CurCount = 1; }
        /*��������*/
        for (int i = 0; i < Data.Holding_BagList.Count; i++)
        {
            /*��鱳������ͬ������*/
            if (Data.Holding_BagList[i].Item_ID == config.Item_ID)
            {
                /*��鱳�����ͬ�������Ƿ�ﵽ�ѵ����ֵ*/
                if (Data.Holding_BagList[i].Item_CurCount < Data.Holding_BagList[i].Item_MaxCount)
                {
                    ItemConfig tempItem = Data.Holding_BagList[i];
                    /*ʣ�����ֵ�����������ĵ���*/
                    if (Data.Holding_BagList[i].Item_MaxCount - Data.Holding_BagList[i].Item_CurCount < config.Item_CurCount)
                    {
                        tempItem.Item_CurCount = tempItem.Item_MaxCount;
                        config.Item_CurCount -= (Data.Holding_BagList[i].Item_MaxCount - Data.Holding_BagList[i].Item_CurCount);
                        Data.Holding_BagList[i] = tempItem;
                    }
                    /*ʣ�����ֵ���������ĵ���*/
                    else
                    {
                        tempItem.Item_CurCount += config.Item_CurCount;
                        config.Item_CurCount = 0;
                        Data.Holding_BagList[i] = tempItem;
                    }
                }
                else
                {
                    continue;
                }
            }
        }
        /*����һ��֮����ʣ��*/
        if (config.Item_CurCount > 0)
        {
            Data.Holding_BagList.Add(config);
        }
        /*����UI*/
        if (showInUI)
        {
            MessageBroker.Default.Publish(new UIEvent.UIEvent_UpdateItemInBag()
            {
                itemConfigs = Data.Holding_BagList
            });
        }
    }
    public void All_SubItem_Bag(ItemConfig config, bool showInUI)
    {
        for (int i = 0; i < Data.Holding_BagList.Count; i++)
        {
            if (Data.Holding_BagList[i].Item_ID == config.Item_ID)
            {
                ItemConfig tempItem = Data.Holding_BagList[i];
                if (tempItem.Item_CurCount > config.Item_CurCount)
                {
                    tempItem.Item_CurCount -= config.Item_CurCount;
                    Data.Holding_BagList[i] = tempItem;
                }
                else
                {
                    Data.Holding_BagList.RemoveAt(i);
                    if (Data.Holding_ByHand.Item_ID == tempItem.Item_ID)
                    {
                        All_SubItem_Hand(showInUI);
                    }
                }
            }
        }
        if (showInUI)
        {
            MessageBroker.Default.Publish(new UIEvent.UIEvent_UpdateItemInBag()
            {
                itemConfigs = Data.Holding_BagList
            });
        }
    }
    public virtual void RunAway()
    {

    }
    public virtual void RunTo()
    {

    }
    public virtual void TurnLeft()
    {
        bodyController.Leg.localScale = new Vector3(-1, 1, 1);
    }
    public virtual void FaceLeft()
    {
        bodyController.Head.localScale = new Vector3(-1, 1, 1);
        bodyController.Body.localScale = new Vector3(-1, 1, 1);
        bodyController.Hand.localScale = new Vector3(-1, 1, 1);
    }
    public virtual void TurnRight()
    {
        bodyController.Leg.localScale = new Vector3(1, 1, 1);
    }
    public virtual void FaceRight()
    {
        bodyController.Head.localScale = new Vector3(1, 1, 1);
        bodyController.Body.localScale = new Vector3(1, 1, 1);
        bodyController.Hand.localScale = new Vector3(1, 1, 1);
    }
    public virtual void Dead()
    {
        Loot();
        Runner.Despawn(Object);
    }
    public virtual void Loot()
    {
        for (int i = 0; i < Data.LootCount; i++)
        {
            int id = GetNextLootID();
            if (id != 0)
            {
                GameObject obj = Resources.Load<GameObject>("ItemObj/ItemObj");
                GameObject item = Instantiate(obj);
                item.transform.position = transform.position;
                item.GetComponent<ItemObj>().Init(ItemConfigData.GetItemConfig(id));
                item.GetComponent<ItemObj>().PlayDropAnim(transform.position);
            }
        }
        for (int i = 0; i < Data.Holding_BagList.Count; i++)
        {
            GameObject obj = Resources.Load<GameObject>("ItemObj/ItemObj");
            GameObject item = Instantiate(obj);
            item.transform.position = transform.position;
            item.GetComponent<ItemObj>().Init(Data.Holding_BagList[i]);
            item.GetComponent<ItemObj>().PlayDropAnim(transform.position);
        }
    }
    /// <summary>
    /// ����Ȩ�ػ��һ��������id
    /// </summary>
    /// <returns></returns>
    private int GetNextLootID()
    {
        int weight_Main = 0;
        int weight_temp = 0;
        int random = 0;
        for (int j = 0; j < Data.Loot_List.Count; j++)
        {
            weight_Main += Data.Loot_List[j].Weight;
        }
        random = Random.Range(0, weight_Main);
        for (int j = 0; j < Data.Loot_List.Count; j++)
        {
            weight_temp += Data.Loot_List[j].Weight;
            if (weight_temp > random)
            {
                return Data.Loot_List[j].ID;
            }
        }
        return 0;
    }
    public void ChangeHP()
    {

    }
    #endregion
}
public struct NetworkItemConfig : INetworkStruct
{
    public int Item_ID;
    public NetworkString<_16> Item_Name;
    public NetworkString<_16> Item_Desc;
    public int Item_CurCount;
    public int Item_MaxCount;
    public ItemType Item_Type;
    public float Average_Weight;
    public float Average_Value;
    public NetworkString<_256> Item_Info;
}
