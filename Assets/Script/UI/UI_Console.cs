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
        btn_changeItemType_1000.onClick.AddListener(() => { ChangeItemType(1); });
        btn_changeItemType_2000.onClick.AddListener(() => { ChangeItemType(2); });
        btn_changeItemType_3000.onClick.AddListener(() => { ChangeItemType(3); });
        btn_changeItemType_4000.onClick.AddListener(() => { ChangeItemType(4); });
        btn_changeItemType_5000.onClick.AddListener(() => { ChangeItemType(9); });
        btn_lastPage.onClick.AddListener(() => { ChangePage(false); });
        btn_nextPage.onClick.AddListener(() => { ChangePage(true); });
        var data = ConsoleUIArchitecture.Interface.GetModel<IConsoleDataModle>();
        for (int i = 0; i < btn_list.Count; i++)
        {
            int index = i;
            btn_list[i].onClick.AddListener(() => { ClickItemBtn(index); });
            data.ItemIconList.Add(btn_list[i].GetComponent<Image>());
        }

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
    [SerializeField, Header("按钮列表")]
    private List<Button> btn_list = new List<Button>(10);
    [SerializeField, Header("切换材料")]
    private Button btn_changeItemType_1000;
    [SerializeField, Header("切换工具")]
    private Button btn_changeItemType_2000;
    [SerializeField, Header("切换食材")]
    private Button btn_changeItemType_3000;
    [SerializeField, Header("切换容器")]
    private Button btn_changeItemType_4000;
    [SerializeField, Header("切换杂项")]
    private Button btn_changeItemType_5000;
    [SerializeField, Header("上一页")]
    private Button btn_lastPage;
    [SerializeField, Header("下一页")]
    private Button btn_nextPage;
    /// <summary>
    /// 更改目标物体种类
    /// </summary>
    /// <param name="value"></param>
    private void ChangeItemType(int value)
    {
        var data = ConsoleUIArchitecture.Interface.GetModel<IConsoleDataModle>();
        data.ItemListType = value;
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
            data.ItemListPage++;
        }
        else
        {
            data.ItemListPage--;
        }
    }
    private void ClickItemBtn(int index)
    {
        var data = ConsoleUIArchitecture.Interface.GetModel<IConsoleDataModle>();
        data.ItemListIndex = index;
        ConsoleUIArchitecture.Interface.SendCommand(new ConsoleCommand_CreateItem());
    }
    #endregion

}
