using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.U2D;
using Newtonsoft.Json;

public class UI_ActorShowPanel : MonoBehaviour
{
    public Transform transform_Panel;
    public Button btn_Return;
    public Image image_Hair;
    public Image image_Eye;
    [Header("����")]
    public TextMeshProUGUI text_Name;
    [Header("����_����")]
    public TextMeshProUGUI text_Food;
    [Header("����_����")]
    public TextMeshProUGUI text_Water;
    [Header("����_����")]
    public TextMeshProUGUI text_Hp;
    [Header("����_����")]
    public TextMeshProUGUI text_Armor;
    [Header("����_����")]
    public TextMeshProUGUI text_San;
    [Header("����_����")]
    public TextMeshProUGUI text_Happy;
    [Header("Buff")]
    public UI_BuffPanel ui_BuffPanel;
    private SpriteAtlas spriteAtlas_Hair;
    private SpriteAtlas spriteAtlas_Eye;

    private void Awake()
    {
        Bind();
    }
    private void Bind()
    {
        btn_Return.onClick.AddListener(Hide);
        spriteAtlas_Hair = Resources.Load<SpriteAtlas>("Atlas/HairSprite");
        spriteAtlas_Eye = Resources.Load<SpriteAtlas>("Atlas/EyeSprite");
    }
    public void Init(PlayerData playerData)
    {
        InitHead(playerData);
        InitInfo(playerData);
        InitBuff(playerData);
    }
    private void InitHead(PlayerData playerData)
    {
        image_Eye.sprite = spriteAtlas_Eye.GetSprite("Eye_" + playerData.Eye_ID.ToString());
        image_Hair.sprite = spriteAtlas_Hair.GetSprite("Hair_" + playerData.Hair_ID.ToString());
        image_Hair.color = playerData.Hair_Color;
        text_Name.text = playerData.Name;
    }
    private void InitInfo(PlayerData playerData)
    {
        text_Food.text = playerData.Food_Cur.ToString();
        text_Water.text = playerData.Water_Cur.ToString();
        text_Hp.text = playerData.Hp_Cur.ToString();
        text_Armor.text = playerData.Armor_Cur.ToString();
        text_San.text = playerData.San_Cur.ToString();
        text_Happy.text = playerData.Happy_Cur.ToString();
    }
    private void InitBuff(PlayerData playerData)
    {
        ui_BuffPanel.ClearBuffCell();
        for(int i = 0; i < playerData.BuffList.Count; i++)
        {
            ui_BuffPanel.DrawBuffCell(BuffConfigData.GetBuffConfig(playerData.BuffList[i]));
        }
    }
    public void Show()
    {
        transform_Panel.gameObject.SetActive(true);
    }
    public void Hide()
    {
        transform_Panel.gameObject.SetActive(false);
    }
}
