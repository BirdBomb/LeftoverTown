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
        ShowChooseMap();
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
    #region//ѡ���ͼ
    [Header("---ѡ���ͼ---")]
    public UI_ChooseMapPanel chooseMapPanel;
    /// <summary>
    /// ѡ���ͼ���_��ʾ
    /// </summary>
    public void ShowChooseMap()
    {
        chooseMapPanel.ShowPanel();
    }
    /// <summary>
    /// ѡ���ͼ���_���
    /// </summary>
    public void PassChooseMap()
    {
        ShowCreateRoom();
        CloseChooseMap();
    }
    /// <summary>
    /// ѡ���ͼ���_�ر�
    /// </summary>
    public void CloseChooseMap()
    {
        chooseMapPanel.HidePanel();
    }
    #endregion
    #region//��������
    [Header("---ѡ���ͼ---")]
    public UI_RoomCreate roomCreate;
    /// <summary>
    /// �����������_��ʾ
    /// </summary>
    public void ShowCreateRoom()
    {
        roomCreate.Init(chooseActorPanel.bind_ActorPath, chooseMapPanel.bind_MapIndex);
        roomCreate.ShowPanel();
    }
    /// <summary>
    /// �����������_����
    /// </summary>
    public void CloseCreateRoom()
    {
        roomCreate.HidePanel();
    }

    #endregion
}
