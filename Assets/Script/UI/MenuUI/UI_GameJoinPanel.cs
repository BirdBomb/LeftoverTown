using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_GameJoinPanel : MonoBehaviour
{
    private void Start()
    {
        Bind();
    }
    private void Bind()
    {
        chooseActorPanel.Bind(PassChooseActor, CloseChooseActor);
        roomJoin.Bind(CloseRoomJoin);
    }
    public void ShowGameJoinPanel()
    {
        ShowChooseActorPanel();
    }
    public void HideGameJoinPanel()
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
        ShowRoomJoinPanel();
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
    #region//选择房间
    [Header("---选择角色---")]
    public UI_RoomJoin roomJoin;
    /// <summary>
    /// 选择角色面板_显示
    /// </summary>
    public void ShowRoomJoinPanel()
    {
        roomJoin.Init(chooseActorPanel.bind_ActorPath);
        roomJoin.ShowPanel();
    }
    /// <summary>
    /// 选择角色面板_关闭
    /// </summary>
    public void CloseRoomJoin()
    {
        roomJoin.HidePanel();
    }
    #endregion
}
