using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UniRx;
using QFramework;
using static UnityEngine.Rendering.DebugUI;
using Button = UnityEngine.UI.Button;

public class UI_Console : MonoBehaviour
{
    private void Start()
    {
        // 添加输入框值改变的监听器
        inputField.onValueChanged.AddListener(OnInputFieldValueChanged);
        // 添加输入框点击的监听器
        inputField.onEndEdit.AddListener(OnInputFieldEndEdit);
        btn_hideOrShow.onClick.AddListener(() => { HidePanel(!panel_Main.gameObject.activeSelf); });
        btn_changeItemType_1000.onClick.AddListener(() => { ChangeItemType(1); });
        btn_changeItemType_2000.onClick.AddListener(() => { ChangeItemType(2); });
        btn_changeItemType_3000.onClick.AddListener(() => { ChangeItemType(3); });
        btn_changeItemType_4000.onClick.AddListener(() => { ChangeItemType(4); });
        btn_changeItemType_5000.onClick.AddListener(() => { ChangeItemType(5); });
        btn_changeItemType_9000.onClick.AddListener(() => { ChangeItemType(9); });
        btn_changeBuildingType.onClick.AddListener(() => { ChangeBuildingType(); });
        btn_changeFloorType.onClick.AddListener(() => { ChangeFloorType(); });
        btn_Save.onClick.AddListener(() => 
        {
            MessageBroker.Default.Publish(new MapEvent.MapEvent_LocalTile_SaveMapData()
            {

            });
        });
        btn_lastPage.onClick.AddListener(() => { ChangePage(false); });
        btn_nextPage.onClick.AddListener(() => { ChangePage(true); });
        var data = ConsoleUIArchitecture.Interface.GetModel<IConsoleDataModle>();
        for (int i = 0; i < btn_list.Count; i++)
        {
            int index = i;
            btn_list[i].onClick.AddListener(() => { ClickBtn(index); });
            data.IconList.Add(btn_list[i].GetComponent<Image>());
        }
        data.IconListCount = btn_list.Count;
    }
    #region//输入
    [SerializeField, Header("输入框")]
    private InputField inputField;
    // 当输入框值改变时调用的方法
    private void OnInputFieldValueChanged(string value)
    {
        var data = ConsoleUIArchitecture.Interface.GetModel<IConsoleDataModle>();
        data.InputStr = value;
        ConsoleUIArchitecture.Interface.SendCommand(new ConsoleCommand_UpdateInput());
    }
    // 当输入框结束编辑时调用的方法
    private void OnInputFieldEndEdit(string value)
    {
        var data = ConsoleUIArchitecture.Interface.GetModel<IConsoleDataModle>();
        data.InputStr = value;
        ConsoleUIArchitecture.Interface.SendCommand(new ConsoleCommand_EndInput());
    }
    #endregion
    #region//图鉴
    [SerializeField, Header("主面板")]
    private Transform panel_Main;
    [SerializeField, Header("按钮列表")]
    private List<Button> btn_list = new List<Button>(20);
    [SerializeField, Header("显示隐藏")]
    private Button btn_hideOrShow;
    [SerializeField, Header("切换材料")]
    private Button btn_changeItemType_1000;
    [SerializeField, Header("切换工具")]
    private Button btn_changeItemType_2000;
    [SerializeField, Header("切换食材")]
    private Button btn_changeItemType_3000;
    [SerializeField, Header("切换容器")]
    private Button btn_changeItemType_4000;
    [SerializeField, Header("切换衣物")]
    private Button btn_changeItemType_5000;
    [SerializeField, Header("切换杂项")]
    private Button btn_changeItemType_9000;
    [SerializeField, Header("切换建筑建造")]
    private Button btn_changeBuildingType;
    [SerializeField, Header("切换地板建造")]
    private Button btn_changeFloorType;
    [SerializeField, Header("上一页")]
    private Button btn_lastPage;
    [SerializeField, Header("下一页")]
    private Button btn_nextPage;
    private void HidePanel(bool hide)
    {
        panel_Main.gameObject.SetActive(hide);
    }
    /// <summary>
    /// 更改目标物体种类
    /// </summary>
    /// <param name="value"></param>
    private void ChangeItemType(int value)
    {
        var data = ConsoleUIArchitecture.Interface.GetModel<IConsoleDataModle>();
        data.State = ConsoleState.Item;
        data.ItemListType = value;
    }
    /// <summary>
    /// 更改目标地块种类
    /// </summary>
    /// <param name="value"></param>
    private void ChangeBuildingType()
    {
        var data = ConsoleUIArchitecture.Interface.GetModel<IConsoleDataModle>();
        data.State = ConsoleState.Building;
    }
    /// <summary>
    /// 更改目标地块种类
    /// </summary>
    /// <param name="value"></param>
    private void ChangeFloorType()
    {
        var data = ConsoleUIArchitecture.Interface.GetModel<IConsoleDataModle>();
        data.State = ConsoleState.Floor;
    }

    /// <summary>
    /// 翻页
    /// </summary>
    /// <param name="nextPage"></param>
    private void ChangePage(bool nextPage)
    {
        var data = ConsoleUIArchitecture.Interface.GetModel<IConsoleDataModle>();
        if (nextPage)
        {
            data.ListPage++;
        }
        else
        {
            data.ListPage--;
        }
    }
    private void ClickBtn(int index)
    {
        var data = ConsoleUIArchitecture.Interface.GetModel<IConsoleDataModle>();
        data.ListIndex = index;

        if (data.State == ConsoleState.Item)
        {
            ConsoleUIArchitecture.Interface.SendCommand(new ConsoleCommand_CreateItem());
        }
        if (data.State == ConsoleState.Building)
        {
            ConsoleUIArchitecture.Interface.SendCommand(new ConsoleCommand_CreateBuild());
        }
        if (data.State == ConsoleState.Floor)
        {
            ConsoleUIArchitecture.Interface.SendCommand(new ConsoleCommand_CreateFloor());
        }
    }
    #endregion
    #region//控制
    [SerializeField, Header("保存")]
    private Button btn_Save;
    #endregion
}
