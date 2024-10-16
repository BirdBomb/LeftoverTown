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
        // ��������ֵ�ı�ļ�����
        inputField.onValueChanged.AddListener(OnInputFieldValueChanged);
        // �����������ļ�����
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
    #region//����
    [SerializeField, Header("�����")]
    private InputField inputField;
    // �������ֵ�ı�ʱ���õķ���
    private void OnInputFieldValueChanged(string value)
    {
        var data = ConsoleUIArchitecture.Interface.GetModel<IConsoleDataModle>();
        data.InputStr = value;
        ConsoleUIArchitecture.Interface.SendCommand(new ConsoleCommand_UpdateInput());
    }
    // �����������༭ʱ���õķ���
    private void OnInputFieldEndEdit(string value)
    {
        var data = ConsoleUIArchitecture.Interface.GetModel<IConsoleDataModle>();
        data.InputStr = value;
        ConsoleUIArchitecture.Interface.SendCommand(new ConsoleCommand_EndInput());
    }
    #endregion
    #region//ͼ��
    [SerializeField, Header("�����")]
    private Transform panel_Main;
    [SerializeField, Header("��ť�б�")]
    private List<Button> btn_list = new List<Button>(20);
    [SerializeField, Header("��ʾ����")]
    private Button btn_hideOrShow;
    [SerializeField, Header("�л�����")]
    private Button btn_changeItemType_1000;
    [SerializeField, Header("�л�����")]
    private Button btn_changeItemType_2000;
    [SerializeField, Header("�л�ʳ��")]
    private Button btn_changeItemType_3000;
    [SerializeField, Header("�л�����")]
    private Button btn_changeItemType_4000;
    [SerializeField, Header("�л�����")]
    private Button btn_changeItemType_5000;
    [SerializeField, Header("�л�����")]
    private Button btn_changeItemType_9000;
    [SerializeField, Header("�л���������")]
    private Button btn_changeBuildingType;
    [SerializeField, Header("�л��ذ彨��")]
    private Button btn_changeFloorType;
    [SerializeField, Header("��һҳ")]
    private Button btn_lastPage;
    [SerializeField, Header("��һҳ")]
    private Button btn_nextPage;
    private void HidePanel(bool hide)
    {
        panel_Main.gameObject.SetActive(hide);
    }
    /// <summary>
    /// ����Ŀ����������
    /// </summary>
    /// <param name="value"></param>
    private void ChangeItemType(int value)
    {
        var data = ConsoleUIArchitecture.Interface.GetModel<IConsoleDataModle>();
        data.State = ConsoleState.Item;
        data.ItemListType = value;
    }
    /// <summary>
    /// ����Ŀ��ؿ�����
    /// </summary>
    /// <param name="value"></param>
    private void ChangeBuildingType()
    {
        var data = ConsoleUIArchitecture.Interface.GetModel<IConsoleDataModle>();
        data.State = ConsoleState.Building;
    }
    /// <summary>
    /// ����Ŀ��ؿ�����
    /// </summary>
    /// <param name="value"></param>
    private void ChangeFloorType()
    {
        var data = ConsoleUIArchitecture.Interface.GetModel<IConsoleDataModle>();
        data.State = ConsoleState.Floor;
    }

    /// <summary>
    /// ��ҳ
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
    #region//����
    [SerializeField, Header("����")]
    private Button btn_Save;
    #endregion
}
