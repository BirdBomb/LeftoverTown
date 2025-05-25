using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class TileUI_SunRock : TileUI
{
    [SerializeField, Header("格子面板")]
    private Transform transform_Panel;
    [SerializeField, Header("格子列表")]
    private UI_GridCell gridCell_Food;
    [SerializeField, Header("信息")]
    private Text text_Info;

}
