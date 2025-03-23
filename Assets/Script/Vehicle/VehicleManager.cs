using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class VehicleManager : MonoBehaviour
{
    [SerializeField, Header("���ظ���")]
    private Rigidbody2D rigidbody2D_VehicleBody;
    public Rigidbody2D Rigidbody2D_VehicleBody
    {
        get { return rigidbody2D_VehicleBody; }
    }
    [SerializeField, Header("���������")]
    private VehicleNetManager vehicleNetManager_NetManager;
    public VehicleNetManager VehicleNetManager_NetManager
    {
        get { return vehicleNetManager_NetManager; }
    }
    /// <summary>
    /// ��ʻԱ
    /// </summary>
    public ActorManager actorManager_Drive;
    /// <summary>
    /// �˿�
    /// </summary>
    public List<ActorManager> actorManager_Passenger;

    public virtual void Awake()
    {
        Bind();
        Rigidbody2D_VehicleBody.gravityScale = 0f;
    }
    public virtual void Start()
    {
        AllClient_AddListener();
    }
    public virtual void Bind()
    {

    }
    #region//�ϳ��³�
    public virtual void AllClient_GetOn(ActorManager actor)
    {
        vehicleNetManager_NetManager.RPC_LocalInput_ActorGetOn(actor.actorNetManager.Object.Id);
    }
    public virtual void AllClient_GetOff(ActorManager actor)
    {
        vehicleNetManager_NetManager.RPC_LocalInput_ActorGetOff(actor.actorNetManager.Object.Id);
    }
    public virtual void FromRPC_AllClient_GetOn(ActorManager actor)
    {
        if (!actorManager_Drive) { actorManager_Drive = actor; }
        actorManager_Passenger.Add(actor);
        StartCoroutine(actor.vehicleManager.AllClient_GetOnVehicle(this));
    }
    public virtual void FromRPC_AllClient_GetOff(ActorManager actor)
    {
        if (actorManager_Drive == actor) { actorManager_Drive = null; }
        actorManager_Passenger.Remove(actor);
        StartCoroutine(actor.vehicleManager.AllClient_GetOffVehicle(this));
    }
    #endregion
    #region//�����ر�
    public virtual void FromRPC_AllClient_Engine(bool engineOn)
    {
        
    }

    #endregion
    #region//��Ϣ
    public virtual void FromRPC_AllClient_UpdateInfo(string info)
    {

    }
    public virtual void FromRPC_AllClient_SetDirection(VehicleDirection vehicleDirection)
    {
        
    }
    #endregion
    #region//����
    /// <summary>
    /// ��ʼ����
    /// </summary>
    private void AllClient_AddListener()
    {
        MessageBroker.Default.Receive<GameEvent.GameEvent_AllClient_SomeoneMove>().Subscribe(_ =>
        {
            AllClient_ListenSomeoneMove(_.moveActor);
        }).AddTo(this);
    }
    /// <summary>
    /// ������ĳ���ƶ�
    /// </summary>
    /// <param name="actorManager"></param>
    public virtual void AllClient_ListenSomeoneMove(ActorManager actorManager)
    {

    }
    #endregion
    #region//����
    public virtual void AllClient_ActorInputMove(ActorManager actor, float dt, Vector2 dir, bool speed)
    {

    }
    public virtual void AllClient_ActorInputKeycode(ActorManager actor, KeyCode code)
    {

    }
    #endregion
}
public enum VehicleDirection
{
    Up,
    Down,
    Left,
    Right,
}