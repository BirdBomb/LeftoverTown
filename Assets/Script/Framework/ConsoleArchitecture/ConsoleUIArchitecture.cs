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
    }
}
/// <summary>
/// 控制器数据
/// </summary>
public class ConsoleDataModle : IConsoleDataModle
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
                GetArchitecture().SendCommand(new ConsoleCommand_UpdateItemList());
            }
        }
    }
    private int itemListPage;
    public int ItemListPage 
    {
        get { return itemListPage; }
        set
        {
            if (itemListPage != value)
            {
                itemListPage = value;
                GetArchitecture().SendCommand(new ConsoleCommand_UpdateItemList());
            }
        }
    }
    public int ItemListIndex { get; set; }
    private List<UnityEngine.UI.Image> itemIconList = new List<UnityEngine.UI.Image>();
    public List<UnityEngine.UI.Image> ItemIconList 
    { 
        get { return itemIconList; }
        set 
        {
            itemIconList = value;
        }
    }

}
public interface IConsoleDataModle : IModel
{
    public string InputStr { get; set; }
    public int ItemListType { get; set; }
    public int ItemListPage { get; set; }
    public int ItemListIndex { get; set; }
    public List<UnityEngine.UI.Image> ItemIconList { get; set; }
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
public class ConsoleCommand_UpdateItemList : AbstractCommand
{
    protected override void OnExecute()
    {
        this.GetSystem<IConsoleItemListSystem>().Update();
    }

}
public class ConsoleCommand_CreateItem : AbstractCommand
{
    protected override void OnExecute()
    {
        this.GetSystem<IConsoleItemListSystem>().Create();
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
        string input = this.GetModel<IConsoleDataModle>().InputStr;
        string[] strs = input.Split('.');
        if (strs.Length >= 2)
        {
            if (strs[0] == "giveitem")
            {
                int id = 1;
                int val = 0;
                int count = 1;
                int seed = System.DateTime.Now.Second;
                UnityEngine.Random.InitState(seed);
                string[] itemStrs = strs[1].Split('_');
                if (itemStrs.Length >= 3)
                {
                    id = int.Parse(itemStrs[0]);
                    val = int.Parse(itemStrs[1]);
                    count = int.Parse(itemStrs[2]);
                }
                else
                {
                    id = int.Parse(itemStrs[0]);
                }
                Type type = Type.GetType("Item_" + id.ToString());
                ItemData itemData = ((ItemBase)Activator.CreateInstance(type)).GetRandomInitData(id, count,val);
                MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryDropItem()
                {
                    item = itemData
                });
            }
            else if (strs[0] == "spawn")
            {
                MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TrySpawnActor()
                {
                    name = "Actor/" + strs[1]
                }); ;
            }
        }
        Debug.Log("输入框结束编辑：" + input);
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
        var itemType = this.GetModel<IConsoleDataModle>().ItemListType;
        var itemPage = this.GetModel<IConsoleDataModle>().ItemListPage;
        var itemIndex = this.GetModel<IConsoleDataModle>().ItemListIndex;
        var iconList = this.GetModel<IConsoleDataModle>().ItemIconList;
        List<ItemConfig> itemTargetConfigs = new List<ItemConfig>();
        itemTargetConfigs = ItemConfigData.itemConfigs.FindAll((x) => { return x.Item_ID / 1000 == itemType; });
        int startIndex = 10 * itemPage;
        if (startIndex < 0)
        {
            this.GetModel<IConsoleDataModle>().ItemListPage = (itemTargetConfigs.Count / 10);
        }
        else
        {
            if (itemTargetConfigs.Count > startIndex)
            {
                for (int i = startIndex; i < startIndex + 10; i++)
                {
                    if (itemTargetConfigs.Count > i)
                    {
                        iconList[i].sprite = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_" + itemTargetConfigs[i].Item_ID);
                    }
                    else
                    {
                        iconList[i].sprite = Resources.Load<SpriteAtlas>("Atlas/ItemSprite").GetSprite("Item_Default");
                    }
                }
            }
            else/*超限,页数归零*/
            {
                this.GetModel<IConsoleDataModle>().ItemListPage = 0;
                Debug.Log("归零");
            }
        }
    }
    public void Create()
    {
        var itemType = this.GetModel<IConsoleDataModle>().ItemListType;
        var itemPage = this.GetModel<IConsoleDataModle>().ItemListPage;
        var itemIndex = this.GetModel<IConsoleDataModle>().ItemListIndex;
        var iconList = this.GetModel<IConsoleDataModle>().ItemIconList;
        List<ItemConfig> itemTargetConfigs = new List<ItemConfig>();
        itemTargetConfigs = ItemConfigData.itemConfigs.FindAll((x) => { return x.Item_ID / 1000 == itemType; });
        int startIndex = 10 * itemPage + itemIndex;
        if (itemTargetConfigs.Count > startIndex)
        {
            Type type = Type.GetType("Item_" + itemTargetConfigs[startIndex].Item_ID.ToString());
            ItemData itemData = ((ItemBase)Activator.CreateInstance(type)).GetRandomInitData(itemTargetConfigs[startIndex].Item_ID, 1, 0);
            //MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryDropItem()
            //{
            //    item = itemData
            //});
            MessageBroker.Default.Publish(new PlayerEvent.PlayerEvent_Local_TryAddItemInBag()
            {
                item = itemData
            });
        }

    }
}
public interface IConsoleItemListSystem : ISystem
{
    public void Update();
    public void Create();
}