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

    #region//ѡ���ɫ
    [Header("---ѡ���ɫ---")]
    public UI_ChooseActorPanel chooseActorPanel;
    /// <summary>
    /// ѡ���ɫ���_��ʾ
    /// </summary>
    public void ShowChooseActorPanel()
    {
        chooseActorPanel.ShowPanel();
    }
    /// <summary>
    /// ѡ���ɫ���_���
    /// </summary>
    public void PassChooseActor()
    {
        ShowRoomJoinPanel();
        CloseChooseActor();
    }
    /// <summary>
    /// ѡ���ɫ���_�ر�
    /// </summary>
    public void CloseChooseActor()
    {
        chooseActorPanel.HidePanel();
    }
    #endregion
    #region//ѡ�񷿼�
    [Header("---ѡ���ɫ---")]
    public UI_RoomJoin roomJoin;
    /// <summary>
    /// ѡ���ɫ���_��ʾ
    /// </summary>
    public void ShowRoomJoinPanel()
    {
        roomJoin.Init(chooseActorPanel.bind_ActorPath);
        roomJoin.ShowPanel();
    }
    /// <summary>
    /// ѡ���ɫ���_�ر�
    /// </summary>
    public void CloseRoomJoin()
    {
        roomJoin.HidePanel();
    }
    #endregion
}
