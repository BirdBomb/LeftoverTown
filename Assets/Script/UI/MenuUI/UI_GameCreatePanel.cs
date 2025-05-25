using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_GameCreatePanel : MonoBehaviour
{
    private void Start()
    {
        Bind();
    }
    private void Bind()
    {
        chooseActorPanel.Bind(PassChooseActor, CloseChooseActor);
        chooseMapPanel.Bind(PassChooseMap, CloseChooseMap);
        roomCreate.Bind(CloseCreateRoom);
    }
    public void ShowGameCreatePanel()
    {
        ShowChooseActorPanel();
    }
    public void HideGameCreatePanel()
    {

    }
    #region//选择角色
    [Header("---选择角色---")]
    public UI_ChooseActorPanel chooseActorPanel;
    /// <summary>
    /// 选择角色面板_显示
    /// </summary>
    public void ShowChooseActorPanel()
    {
        chooseActorPanel.ShowPanel();
    }
    /// <summary>
    /// 选择角色面板_完成
    /// </summary>
    public void PassChooseActor()
    {
        ShowChooseMap();
        CloseChooseActor();
    }
    /// <summary>
    /// 选择角色面板_关闭
    /// </summary>
    public void CloseChooseActor()
    {
        chooseActorPanel.HidePanel();
    }
    #endregion
    #region//选择地图
    [Header("---选择地图---")]
    public UI_ChooseMapPanel chooseMapPanel;
    /// <summary>
    /// 选择地图面板_显示
    /// </summary>
    public void ShowChooseMap()
    {
        chooseMapPanel.ShowPanel();
    }
    /// <summary>
    /// 选择地图面板_完成
    /// </summary>
    public void PassChooseMap()
    {
        ShowCreateRoom();
        CloseChooseMap();
    }
    /// <summary>
    /// 选择地图面板_关闭
    /// </summary>
    public void CloseChooseMap()
    {
        chooseMapPanel.HidePanel();
    }
    #endregion
    #region//创建房间
    [Header("---选择地图---")]
    public UI_RoomCreate roomCreate;
    /// <summary>
    /// 创建房间面板_显示
    /// </summary>
    public void ShowCreateRoom()
    {
        roomCreate.Init(chooseActorPanel.bind_ActorPath, chooseMapPanel.bind_MapIndex);
        roomCreate.ShowPanel();
    }
    /// <summary>
    /// 创建房间面板_隐藏
    /// </summary>
    public void CloseCreateRoom()
    {
        roomCreate.HidePanel();
    }

    #endregion
}
