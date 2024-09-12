using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using System;
using static Fusion.Allocator;
using static UnityEngine.Rendering.DebugUI;
using UniRx;
using UnityEngine.U2D;
/// <summary>
/// 控制器架构
/// </summary>
public class ConsoleUIArchitecture : Architecture<ConsoleUIArchitecture>
{
    protected override void Init()
    {
        RegisterModel<IConsoleDataModle>(new ConsoleDataModle());
        RegisterSystem<IConsoleInputSystem>(new ConsoleInputSystem());
        RegisterSystem<IConsoleItemListSystem>(new ConsoleItemListSystem());
        RegisterSystem<IConsoleBuildingListSystem>(new ConsoleBuildingListSystem());
        RegisterSystem<IConsoleFloorListSystem>(new ConsoleFloorListSystem());
    }
}
/// <summary>
/// 控制器数据
/// </summary>
public class ConsoleDataModle : IConsoleDataModle
{
    private ConsoleState state;
    public ConsoleState State 
    {
        get { return state; }
        set
        {
            if (state != value)
            {
                state = value;
                GetArchitecture().SendCommand(new ConsoleCommand_UpdateList());
            }
        }
    }
    public bool Initialized { get; set; }
    public void Init()
    {
        
    }

    public void Deinit()
    {
        
    }
    private IArchitecture _architecture;
    public IArchitecture GetArchitecture()
    {
        return _architecture;
    }
    public void SetArchitecture(IArchitecture architecture)
    {
        _architecture = architecture;
    }
    public string InputStr { get; set; }
    private int lastItemType;
    public int ItemListType 
    {
        get { return lastItemType; }
        set 
        {
            if (lastItemType != value)
            {
                lastItemType = value;
                GetArchitecture().SendCommand(new ConsoleCommand_UpdateList());
            }
        }
    }
    private int itemListPage;
    public int ListPage 
    {
        get { return itemListPage; }
        set
        {
            if (itemListPage != value)
            {
                itemListPage = value;
                GetArchitecture().SendCommand(new ConsoleCommand_UpdateList());
            }
        }
    }
    public int ListIndex { get; set; }
    private List<UnityEngine.UI.Image> iconList = new List<UnityEngine.UI.Image>();
    public List<UnityEngine.UI.Image> IconList 
    { 
        get { return iconList; }
        set 
        {
            iconList = value;
        }
    }
    public int IconListCount { get; set; }
}
public interface IConsoleDataModle : IModel
{
    public string InputStr { get; set; }
    public ConsoleState State { get; set; }
    public int ItemListType { get; set; }
    public int ListPage { get; set; }
    public int ListIndex { get; set; }
    public List<UnityEngine.UI.Image> IconList { get; set; }
    public int IconListCount { get; set; }
}
public enum ConsoleState
{
    Item,
    Building,
    Floor
}
#region//控制台指令
public class ConsoleCommand_EndInput : AbstractCommand
{
    protected override void OnExecute()
    {
        this.GetSystem<IConsoleInputSystem>().ExecuteInput();
    }
}
public class ConsoleCommand_UpdateInput : AbstractCommand
{
    protected override void OnExecute()
    {
        this.GetSystem<IConsoleInputSystem>().CheckInput();
    }
}
public class ConsoleCommand_UpdateList : AbstractCommand
{
    protected override void OnExecute()
    {
        if (this.GetModel<IConsoleDataModle>().State == ConsoleState.Item)
        {
            this.GetSystem<IConsoleItemListSystem>().Update();
        }
        if (this.GetModel<IConsoleDataModle>().State == ConsoleState.Building)
        {
            this.GetSystem<IConsoleBuildingListSystem>().Update();
        }
        if (this.GetModel<IConsoleDataModle>().State == ConsoleState.Floor)
        {
            this.GetSystem<IConsoleFloorListSystem>().Update();
        }
    }

}
public class ConsoleCommand_CreateItem : AbstractCommand
{
    protected override void OnExecute()
    {
        this.GetSystem<IConsoleItemListSystem>().Create();
    }

}
public class ConsoleCommand_CreateBuild : AbstractCommand
{
    protected override void OnExecute()
    {
        this.GetSystem<IConsoleBuildingListSystem>().Create();
    }
}
public class ConsoleCommand_CreateFloor : AbstractCommand
{
    protected override void OnExecute()
    {
        this.GetSystem<IConsoleFloorListSystem>().Create();
    }
}
#endregion
/// <summary>
/// 控制器系统
/// </summary>
public class ConsoleInputSystem : IConsoleInputSystem
{
    public bool Initialized { get; set; }
    public void Init()
    {
        
    }
    public void Deinit()
    {
        
    }
    private IArchitecture _architecture;
    public IArchitecture GetArchitecture()
    {
        return _architecture;
    }
    public void SetArchitecture(IArchitecture architecture)
    {
        _architecture = architecture;
    }
    /// <summary>
    /// 检查输入
    /// </summary>
    public void CheckInput()
    {
        string input = this.GetModel<IConsoleDataModle>().InputStr;
    }
    public void ExecuteInput()
    {
        //string input = this.GetModel<IConsoleDataModle>().InputStr;
        //string[] strs = input.Split('.');
        //if (strs.Length >= 2)
        //{
        //    if (strs[0] == "giveitem")
        //    {
        //        int id = 1;
        //        int val = 0;
        //        int count = 1;
        //        int seed = System.DateTime.Now.Second;
        //        UnityEngine.Random.InitState(seed);
        //        string[] itemStrs = strs[1].Split('_');
        //        if (itemStrs.Length >= 3)
        //        {
        //            id = int.Parse(itemStrs[0]);
        //            val = int.Parse(itemStrs[1]);
        //            count = int.Parse(itemStrs[2]);
        //        }
        //        else
        //        {
        //            id = int.Parse(itemStrs[0]);
        //        }
        //        Type type = Type.GetType("Item_" + id.ToString());
        //        ItemData itemData = ((ItemBase)Activator.CreateInstance(type)).GetRandomInitData(id, count, val, MapManager.Instance.mapSeed + (int)(System.DateTime.Now.Ticks * 1000));
        //        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryDropItem()
        //        {
        //            item = itemData
        //        });
        //    }
        //    else if (strs[0] == "spawn")
        //    {
        //        MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TrySpawnActor()
        //        {
        //            name = "Actor/" + strs[1]
        //        }); ;
        //    }
        //}
        //Debug.Log("输入框结束编辑：" + input);
    }
}
public interface IConsoleInputSystem:ISystem
{
    public void CheckInput();
    public void ExecuteInput();
}
/// <summary>
/// 物品清单系统
/// </summary>
public class ConsoleItemListSystem : IConsoleItemListSystem
{
    public bool Initialized { get; set; }
    public void Init()
    {

    }
    public void Deinit()
    {

    }
    private IArchitecture _architecture;
    public IArchitecture GetArchitecture()
    {
        return _architecture;
    }
    public void SetArchitecture(IArchitecture architecture)
    {
        _architecture = architecture;
    }

    public void Update()
    {
        var state = this.GetModel<IConsoleDataModle>().State;
        var itemType = this.GetModel<IConsoleDataModle>().ItemListType;
        var itemPage = this.GetModel<IConsoleDataModle>().ListPage;
        var itemIndex = this.GetModel<IConsoleDataModle>().ListIndex;
        var iconList = this.GetModel<IConsoleDataModle>().IconList;
        var iconListCount = this.GetModel<IConsoleDataModle>().IconListCount;

        List<ItemConfig> itemTargetConfigs = new List<ItemConfig>();
        itemTargetConfigs = ItemConfigData.itemConfigs.FindAll((x) => { return x.Item_ID / 1000 == itemType; });
        int startIndex = iconListCount * itemPage;
        if (startIndex < 0)
        {
            this.GetModel<IConsoleDataModle>().ListPage = (itemTargetConfigs.Count / iconListCount);
        }
        else
        {
            if (itemTargetConfigs.Count > startIndex)
            {
                for (int i = startIndex; i < startIndex + iconListCount; i++)
                {
                    if (itemTargetConfigs.Count > i)
                    {
                        iconList[i - startIndex].sprite = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_" + itemTargetConfigs[i].Item_ID);
                    }
                    else
                    {
                        iconList[i - startIndex].sprite = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_Default");
                    }
                }
            }
            else/*超限,页数归零*/
            {
                this.GetModel<IConsoleDataModle>().ListPage = 0;
                Debug.Log("归零");
            }
        }

    }
    public void Create()
    {
        var itemType = this.GetModel<IConsoleDataModle>().ItemListType;
        var page = this.GetModel<IConsoleDataModle>().ListPage;
        var index = this.GetModel<IConsoleDataModle>().ListIndex;
        var iconCount = this.GetModel<IConsoleDataModle>().IconListCount;
        List<ItemConfig> itemTargetConfigs = new List<ItemConfig>();
        itemTargetConfigs = ItemConfigData.itemConfigs.FindAll((x) => { return x.Item_ID / 1000 == itemType; });
        int startIndex = iconCount * page + index;
        if (itemTargetConfigs.Count > startIndex)
        {
            Type type = Type.GetType("Item_" + itemTargetConfigs[startIndex].Item_ID.ToString());
            ItemData itemData= new ItemData
                (itemTargetConfigs[startIndex].Item_ID,
                 MapManager.Instance.mapSeed + (int)(System.DateTime.Now.Ticks * 1000),
                 1,
                 0,
                 0,
                 0,
                 new ContentData());
            ((ItemBase)Activator.CreateInstance(type)).StaticAction_InitData(itemData,out ItemData initData);
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryAddItemInBag()
            {
                item = initData
            });
        }
    }
}
/// <summary>
/// 地块清单系统
/// </summary>
public class ConsoleBuildingListSystem : IConsoleBuildingListSystem
{
    public bool Initialized { get; set; }
    private IArchitecture _architecture;

    public void Init()
    {

    }
    public void Deinit()
    {

    }

    public void Create()
    {
        var page = this.GetModel<IConsoleDataModle>().ListPage;
        var index = this.GetModel<IConsoleDataModle>().ListIndex;
        var iconCount = this.GetModel<IConsoleDataModle>().IconListCount;
        List<BuildingConfig> tileTargetConfigs = BuildingConfigData.buildConfigs;
        int startIndex = iconCount * page + index;
        if (tileTargetConfigs.Count > startIndex)
        {
            int buildingID = tileTargetConfigs[startIndex].Building_ID;
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryBuildBuilding()
            {
                id = buildingID
            });
        }
    }
    public IArchitecture GetArchitecture()
    {
        return _architecture;
    }
    public void SetArchitecture(IArchitecture architecture)
    {
        _architecture = architecture;
    }

    public void Update()
    {
        var itemPage = this.GetModel<IConsoleDataModle>().ListPage;
        var iconList = this.GetModel<IConsoleDataModle>().IconList;
        var iconListCount = this.GetModel<IConsoleDataModle>().IconListCount;

        List<BuildingConfig> buildingTargetConfigs = BuildingConfigData.buildConfigs;
        int startIndex = iconListCount * itemPage;

        if (startIndex < 0)
        {
            this.GetModel<IConsoleDataModle>().ListPage = (buildingTargetConfigs.Count / iconListCount);
        }
        else
        {
            if (buildingTargetConfigs.Count > startIndex)
            {
                for (int i = startIndex; i < startIndex + iconListCount; i++)
                {
                    if (buildingTargetConfigs.Count > i)
                    {
                        iconList[i - startIndex].sprite = Resources.Load<SpriteAtlas>("Atlas/TileSprite").GetSprite(buildingTargetConfigs[i].Building_SpriteName);
                    }
                    else
                    {
                        iconList[i - startIndex].sprite = Resources.Load<SpriteAtlas>("Atlas/TileSprite").GetSprite("Default");
                    }
                }
            }
            else/*超限,页数归零*/
            {
                this.GetModel<IConsoleDataModle>().ListPage = 0;
                Debug.Log("归零");
            }
        }

    }
}
public class ConsoleFloorListSystem : IConsoleFloorListSystem
{
    public bool Initialized { get; set; }
    private IArchitecture _architecture;

    public void Init()
    {

    }
    public void Deinit()
    {

    }

    public void Create()
    {
        var page = this.GetModel<IConsoleDataModle>().ListPage;
        var index = this.GetModel<IConsoleDataModle>().ListIndex;
        var iconCount = this.GetModel<IConsoleDataModle>().IconListCount;
        List<FloorConfig> tileTargetConfigs = FloorConfigData.floorConfigs;
        int startIndex = iconCount * page + index;
        if (tileTargetConfigs.Count > startIndex)
        {
            int floorID = tileTargetConfigs[startIndex].Floor_ID;
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryBuildFloor()
            {
                id = floorID
            });
        }
    }
    public IArchitecture GetArchitecture()
    {
        return _architecture;
    }
    public void SetArchitecture(IArchitecture architecture)
    {
        _architecture = architecture;
    }

    public void Update()
    {
        var itemPage = this.GetModel<IConsoleDataModle>().ListPage;
        var iconList = this.GetModel<IConsoleDataModle>().IconList;
        var iconListCount = this.GetModel<IConsoleDataModle>().IconListCount;

        List<FloorConfig> floorTargetConfigs = FloorConfigData.floorConfigs;
        int startIndex = iconListCount * itemPage;

        if (startIndex < 0)
        {
            this.GetModel<IConsoleDataModle>().ListPage = (floorTargetConfigs.Count / iconListCount);
        }
        else
        {
            if (floorTargetConfigs.Count > startIndex)
            {
                for (int i = startIndex; i < startIndex + iconListCount; i++)
                {
                    if (floorTargetConfigs.Count > i)
                    {
                        iconList[i].sprite = Resources.Load<SpriteAtlas>("Atlas/TileSprite").GetSprite(floorTargetConfigs[i].Floor_SpriteName);
                    }
                    else
                    {
                        iconList[i].sprite = Resources.Load<SpriteAtlas>("Atlas/TileSprite").GetSprite("Default");
                    }
                }
            }
            else/*超限,页数归零*/
            {
                this.GetModel<IConsoleDataModle>().ListPage = 0;
                Debug.Log("归零");
            }
        }

    }
}
public interface IConsoleItemListSystem : ISystem
{
    public void Update();
    public void Create();
}
public interface IConsoleBuildingListSystem : ISystem
{
    public void Update();
    public void Create();
}
public interface IConsoleFloorListSystem : ISystem
{
    public void Update();
    public void Create();
}