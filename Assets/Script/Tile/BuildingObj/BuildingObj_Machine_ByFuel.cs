using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class BuildingObj_Machine_ByFuel : BuildingObj_Manmade
{
    public override void Start()
    {
        MessageBroker.Default.Receive<GameEvent.GameEvent_All_UpdateSecond>().Subscribe(AllClinet_OnSecondUpdate).AddTo(this);
    }
    public virtual void AllClinet_OnSecondUpdate(GameEvent.GameEvent_All_UpdateSecond eventData)
    {

    }
    /// <summary>
    /// 计算燃料
    /// </summary>
    /// <param name="curTimeSign"></param>
    /// <param name="buildingData"></param>
    /// <returns>燃料提供的能量</returns>
    protected virtual int All_CheckFuel(int curTimeSign, BuildingData_Machine_ByFuel buildingData)
    {
        buildingData.ReadFuelDepletedTimeSign(out int fuelDepletedTimeSign);
        buildingData.ReadLastTimeSign(out int lastTimeSign);
        buildingData.ReadFuelItemData(out ItemData itemData_Fuel);
        FuelConfig fuelConfig = FuelConfigData.GetFuelConfig(itemData_Fuel.I);

        int energy;
        /*燃烧值剩余*/
        if (fuelDepletedTimeSign > curTimeSign)
        {
            energy = curTimeSign - lastTimeSign;//提供的能量为时间戳差值
        }
        /*燃烧值耗尽,消耗燃料补充燃烧值*/
        else
        {
            /*有剩余燃料*/
            if (itemData_Fuel.I != 0 && itemData_Fuel.C > 0)
            {
                var fuelShort = curTimeSign - fuelDepletedTimeSign;//缺少这么多燃料
                short expendCount = (short)Mathf.Min(itemData_Fuel.C, Mathf.CeilToInt(fuelShort / (float)fuelConfig.FuelSecond));


                ItemData itemData_Expend = itemData_Fuel;
                itemData_Expend.C = expendCount;
                itemData_Fuel = GameToolManager.Instance.SplitItem(itemData_Fuel, itemData_Expend);

                energy = Mathf.Min(curTimeSign - lastTimeSign, fuelConfig.FuelSecond * expendCount + lastTimeSign);//提供的能量为时间戳差值
                fuelDepletedTimeSign = fuelDepletedTimeSign + fuelConfig.FuelSecond * expendCount;
            }
            /*无剩余燃料*/
            else
            {
                energy = Mathf.Max(fuelDepletedTimeSign - lastTimeSign, 0);//提供的能量为剩余燃烧值
            }
        }
        All_UpdateFuelState(energy > 0);

        buildingData.WriteFuelDepletedTimeSign(fuelDepletedTimeSign);
        buildingData.WriteFuelItemData(itemData_Fuel);
        buildingData.WriteFuelMax(fuelConfig.FuelSecond);


        if (WorldManager.Instance.gameNetManager.Object.HasStateAuthority) All_TryToPush();
        return energy;
    }
    protected virtual void All_UpdateFuelState(bool on)
    {

    }
    protected virtual void All_TryToPushData()
    {

    } 
    public virtual void All_TryToPush()
    {

    }
}
public class BuildingData_Machine_ByFuel
{
    public ItemData itemData_Fuel;
    /// <summary>
    /// 记录时间
    /// </summary>
    protected int gameTime_LastTimeSign = 0;
    /// <summary>
    /// 下次完全燃烧的时间
    /// </summary>
    protected int gameTime_FuelDepletedTimeSign = 0;
    /// <summary>
    /// 本次完全燃烧总耗时
    /// </summary>
    protected int gameTime_FuelMax = 0;
    public void ReadFuelItemData(out ItemData itemData)
    {
        itemData = itemData_Fuel;
    }
    public void WriteFuelItemData(ItemData itemData)
    {
        itemData_Fuel = itemData;
    }
    public void ReadLastTimeSign(out int lastTimeSign)
    {
        lastTimeSign = gameTime_LastTimeSign;
    }
    public void WriteLastTimeSign(int lastTimeSign)
    {
        gameTime_LastTimeSign = lastTimeSign;
    }
    public void ReadFuelDepletedTimeSign(out int fuelDepletedTimeSign)
    {
        fuelDepletedTimeSign = gameTime_FuelDepletedTimeSign;
    }
    public void WriteFuelDepletedTimeSign(int fuelDepletedTimeSign)
    {
        gameTime_FuelDepletedTimeSign = fuelDepletedTimeSign;
    }
    public void ReadFuelMax(out int fuelDepletedTimeSign)
    {
        fuelDepletedTimeSign = gameTime_FuelMax;
    }
    public void WriteFuelMax(int fuelDepletedTimeSign)
    {
        gameTime_FuelMax = fuelDepletedTimeSign;
    }
    public virtual byte[] Serialize()
    {
        return null;
    }
    public virtual void Deserialize(byte[] data)
    {

    }
}