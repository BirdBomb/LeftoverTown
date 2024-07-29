using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.U2D;
using Newtonsoft.Json;

public class UI_ActorShowPanel : MonoBehaviour
{
    public Transform panel;
    public Button btn_Return;
    public Image image_Hair;
    public Image image_Eye;
    public TextMeshProUGUI text_Name;
    [Header("饥饿")]
    public TextMeshProUGUI Food;
    [Header("饥渴")]
    public TextMeshProUGUI Water;
    [Header("生命")]
    public TextMeshProUGUI Hp;
    [Header("护甲")]
    public TextMeshProUGUI Armor;
    [Header("理智")]
    public TextMeshProUGUI San;
    [Header("心情")]
    public TextMeshProUGUI Happy;
    [Header("力量")]
    public UI_PointPanel Point_Strength;
    [Header("智力")]
    public UI_PointPanel Point_Intelligence;
    [Header("专注")]
    public UI_PointPanel Point_Focus;
    [Header("敏捷")]
    public UI_PointPanel Point_Agility;
    [Header("法力")]
    public UI_PointPanel Point_SPower;
    [Header("制造")]
    public UI_PointPanel Point_Make;
    [Header("建造")]
    public UI_PointPanel Point_Build;
    [Header("烹饪")]
    public UI_PointPanel Point_Cook;
    [Header("Buff")]
    public UI_BuffPanel ui_BuffPanel;
    private SpriteAtlas atlasHair;
    private SpriteAtlas atlasEye;

    private void Awake()
    {
        Bind();
    }
    private void Bind()
    {
        btn_Return.onClick.AddListener(Hide);
        atlasHair = Resources.Load<SpriteAtlas>("Atlas/HairSprite");
        atlasEye = Resources.Load<SpriteAtlas>("Atlas/EyeSprite");
    }
    public void Init(PlayerData playerData)
    {
        panel.gameObject.SetActive(true);
        InitHead(playerData);
        InitInfo(playerData);
        InitPoint(playerData);
        InitBuff(playerData);
    }
    private void InitHead(PlayerData playerData)
    {
        image_Hair.sprite = atlasHair.GetSprite("Hair_" + playerData.HairID.ToString());
        image_Hair.color = playerData.HairColor;
        image_Eye.sprite = atlasEye.GetSprite("Eye_" + playerData.EyeID.ToString());
        text_Name.text = playerData.Name;
    }
    private void InitInfo(PlayerData playerData)
    {
        Food.text = playerData.CurFood.ToString();
        Water.text = playerData.Water.ToString();
        Hp.text = playerData.CurHp.ToString();
        Armor.text = playerData.Armor.ToString();
        San.text = playerData.CurSan.ToString();
        Happy.text = playerData.Happy.ToString();
    }
    private void InitPoint(PlayerData playerData)
    {
        Point_Strength.UpdatePoint(playerData.Point_Strength);
        Point_Intelligence.UpdatePoint(playerData.Point_Intelligence);
        Point_Focus.UpdatePoint(playerData.Point_Focus);
        Point_Agility.UpdatePoint(playerData.Point_Agility);
        Point_SPower.UpdatePoint(playerData.Point_SPower);
        Point_Make.UpdatePoint(playerData.Point_Make);
        Point_Build.UpdatePoint(playerData.Point_Build);
        Point_Cook.UpdatePoint(playerData.Point_Cook);
    }
    private void InitBuff(PlayerData playerData)
    {
        ui_BuffPanel.ClearBuffCell();
        for(int i = 0; i < playerData.BuffList.Count; i++)
        {
            ui_BuffPanel.DrawBuffCell(BuffConfigData.GetBuffConfig(playerData.BuffList[i]));
        }
    }
    public void Hide()
    {
        panel.gameObject.SetActive(false);
    }
}
