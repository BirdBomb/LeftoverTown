using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class TileUI_Dialog : TileUI
{
    [SerializeField, Header("真面板")]
    private Transform transform_RealPanel;
    [SerializeField, Header("名字")]
    private LocalizeStringEvent localizeString_Name;
    [SerializeField, Header("信息")]
    private LocalizeStringEvent localizeString_Info;

    public List<TileUI_DialogOption> dialogOptions = new List<TileUI_DialogOption>();
    public override bool NeedToOpenBagPanel()
    {
        return false;
    }
    public void InitDialog(string nameTable, string nameEntry, string infoTable, string infoEntry)
    {
        transform_RealPanel.DOKill();
        transform_RealPanel.localScale = Vector3.one;
        transform_RealPanel.DOPunchScale(new Vector3(0.1f, -0.1f, 0), 0.1f);
        localizeString_Name.StringReference.SetReference(nameTable, nameEntry);
        localizeString_Info.StringReference.SetReference(infoTable, infoEntry);
    }
    public void ResetDialog()
    {
        for (int i = 0; i < dialogOptions.Count; i++)
        {
            dialogOptions[i].Hide();
        }
        MoveDialog(0);
    }
    public IEnumerator InitOption(List<DialogOption> options)
    {
        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < options.Count; i++)
        {
            if (i < dialogOptions.Count)
            {
                dialogOptions[options.Count - i - 1].Init("Role_String", options[i].optionEntry, options[i].optionAction);
            }
        }
        MoveDialog(options.Count);
    }
    private void MoveDialog(int count)
    {
        transform_RealPanel.DOLocalMoveY(count * 52, 0.1f);
    }
}
public class DialogOption
{
    public string optionTable;
    public string optionEntry;
    public Action optionAction;
    public DialogOption(string textKey, Action action)
    {
        optionEntry = textKey; optionAction = action;
    }
    public DialogOption()
    {
        
    }
}
