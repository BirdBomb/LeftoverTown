using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UniRx;
using UnityEngine;
using WebSocketSharp;

public class BuildingObj_Growth_TwoState : BuildingObj
{
    public override void Start()
    {
        SubscribeToEvents();
        LoadInitialState();
        base.Start();
    }
    public enum State
    {
        Default, Growing, Mature
    }
    #region 序列化字段
    [Header("初始年龄")]
    public int int_AgeInit = int.MaxValue;
    [Header("生长状态持续时间（小时）")]
    public int int_GrowingDuration = int.MaxValue;
    [Header("生长状态生命值")]
    public int int_GrowingHp = int.MaxValue;
    [Header("成熟状态生命值")]
    public int int_MatureHp = int.MaxValue;
    #endregion
    #region 私有字段
    protected State state_Now;
    #endregion
    public BuildingData_Growth_TwoState buildingData_Growth_Two = new BuildingData_Growth_TwoState();
    #region 初始化
    public virtual void SubscribeToEvents()
    {
        MessageBroker.Default.Receive<GameEvent.GameEvent_All_UpdateHour>().Subscribe(All_OnHourUpdated).AddTo(this);
        MessageBroker.Default.Receive<GameEvent.GameEvent_State_UpdateHour>().Subscribe(State_OnHourUpdated).AddTo(this);
    }
    public virtual void LoadInitialState()
    {
        WorldManager.Instance.GetTime_NowHour(out int now);
        if (buildingData_Growth_Two.ReadSignTime() == int.MinValue) { buildingData_Growth_Two.WriteSignTime(now - int_AgeInit); }
        All_TryToUpdateState(); 
    }
    #endregion
    #region 信息上传与同步
    public override void All_OnRawDataUpdate()
    {
        buildingData_Growth_Two.Deserialize(local_ByteData);
        All_TryToUpdateState();
        base.All_OnRawDataUpdate();
    }
    public void TryToPush()
    {
        All_PushData(buildingData_Growth_Two.Serialize());
    }
    #endregion
    #region 基类方法
    public override void All_UpdateHP(int newHp)
    {
        if (newHp <= 0) { All_Broken(); }
        else
        {
            if (newHp <= local_Hp)
            {
                All_HpDown(newHp - local_Hp);
            }
            else
            {
                All_HpUp(newHp - local_Hp);
            }
            Local_SetHp(newHp);
        }
    }
    public override void All_Broken()
    {
        if (WorldManager.Instance?.gameNetManager?.Object?.HasStateAuthority == true)
        {
            LootRandomInfo[] lootRandomInfos = new LootRandomInfo[] { };
            LootFixedInfo[] lootFixedInfos = new LootFixedInfo[] { };
            switch (state_Now)
            {
                case State.Growing:
                    lootRandomInfos = LootItemConfigData.GetLootRandomConfig(buildingTile.tileID * 10).Loot_List;
                    lootFixedInfos = LootItemConfigData.GetLootFixedConfig(buildingTile.tileID * 10).Loot_List;
                    break;
                case State.Mature:
                    lootRandomInfos = LootItemConfigData.GetLootRandomConfig(buildingTile.tileID * 10 + 1).Loot_List;
                    lootFixedInfos = LootItemConfigData.GetLootFixedConfig(buildingTile.tileID * 10 + 1).Loot_List;
                    break;
            }
            State_CreateLootItem(Tool_GetFixedItemList(lootFixedInfos));
            State_CreateLootItem(Tool_GetRandomItemList(lootRandomInfos, 1));
        }

        if (state_Now == State.Growing)
        {
            base.All_Broken();
        }
        else
        {
            WorldManager.Instance.GetTime_NowHour(out int hourCounter);
            buildingData_Growth_Two.WriteSignTime(hourCounter);
            TryToPush();
        }
    }
    #endregion
    #region 生长计算
    public State All_GetCurState()
    {
        return state_Now;
    }
    public virtual void All_OnHourUpdated(GameEvent.GameEvent_All_UpdateHour eventData)
    {
        All_TryToUpdateState();
    }
    public virtual void State_OnHourUpdated(GameEvent.GameEvent_State_UpdateHour eventData)
    {
        ForState_PushData(buildingData_Growth_Two.Serialize());
    }
    /// <summary>
    /// 计算目标状态
    /// </summary>
    private State CalculateTargetState(int elapsedTime)
    {
        if (elapsedTime < int_GrowingDuration)
            return State.Growing;
        else
            return State.Mature;
    }
    #endregion
    #region 状态切换
    public void All_TryToUpdateState()
    {
        WorldManager.Instance.GetTime_NowHour(out int now);

        if (buildingData_Growth_Two.ReadSignTime() == int.MinValue) { buildingData_Growth_Two.WriteSignTime(now - int_AgeInit); }

        State targetState = CalculateTargetState(now - buildingData_Growth_Two.ReadSignTime());
        if (targetState != state_Now) All_UpdateState(targetState);
    }
    private void All_UpdateState(State newState)
    {
        state_Now = newState;
        switch (state_Now)
        {
            case State.Growing:
                All_ConfigureGrowingState();
                break;
            case State.Mature:
                All_ConfigureMatureState();
                break;
        }
    }
    public virtual void All_ConfigureGrowingState()
    {

    }
    public virtual void All_ConfigureMatureState()
    {

    }
    #endregion
}
public class BuildingData_Growth_TwoState
{
    public int int_SignTime = int.MinValue;
    public int ReadSignTime() { return int_SignTime; }
    public void WriteSignTime(int val) { int_SignTime = val; }
    public byte[] Serialize()
    {
        using (var ms = new MemoryStream())
        using (var writer = new BinaryWriter(ms))
        {
            writer.Write(int_SignTime);
            return ms.ToArray();
        }
    }
    public void Deserialize(byte[] data)
    {
        if (data == null || data.Length == 0)
        {
            return;
        }
        using (var ms = new MemoryStream(data))
        using (var reader = new BinaryReader(ms))
        {
            int_SignTime = reader.ReadInt32();
        }
    }
}