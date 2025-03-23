using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class UI_ActorCreatePanel : MonoBehaviour
{
    public Transform transform_Panel;
    private void Awake()
    {
        Bind();
    }
    private void Bind()
    {
        BindCreatePanel();
        BindHeadPanel();
    }
    public void Init(string path,Action create)
    {
        bindPath = path;
        playerData = new PlayerData();
        createAction = create;
        UpdatePointPanel();
        UpdateHeadPanel();
    }
    public void Show()
    {
        transform_Panel.gameObject.SetActive(true);
    }
    public void Hide()
    {
        transform_Panel.gameObject.SetActive(false);
    }
    #region//角色点数
    public UI_PointPanel uI_PointPanel_Strength;
    public UI_PointPanel uI_PointPanel_Intelligence;
    public UI_PointPanel uI_PointPanel_SPower;
    public UI_PointPanel uI_PointPanel_Focus;
    public UI_PointPanel uI_PointPanel_Agility;
    public UI_PointPanel uI_PointPanel_Make;
    public UI_PointPanel uI_PointPanel_Build;
    public UI_PointPanel uI_PointPanel_Cook;

    private void UpdatePointPanel()
    {
        uI_PointPanel_Strength.UpdatePoint(playerData.Point_Strength);
        uI_PointPanel_Intelligence.UpdatePoint(playerData.Point_Intelligence);
        uI_PointPanel_SPower.UpdatePoint(playerData.Point_SPower);
        uI_PointPanel_Focus.UpdatePoint(playerData.Point_Focus);
        uI_PointPanel_Agility.UpdatePoint(playerData.Point_Agility);
        uI_PointPanel_Make.UpdatePoint(playerData.Point_Make);
        uI_PointPanel_Build.UpdatePoint(playerData.Point_Build);
        uI_PointPanel_Cook.UpdatePoint(playerData.Point_Cook);
    }
    #endregion
    #region//角色外貌
    public TMP_InputField input_Name;
    public Image image_Hair;
    public Image image_Eye;
    public Button btn_LastHair;
    public Button btn_NextHair;
    public Button btn_LastEye;
    public Button btn_NextEye;
    public Slider slider_HairColorH;
    private float hairColorValueH = 1;
    public Slider slider_HairColorV;
    private float hairColorValueV = 1;
    public Slider slider_HairColorS;
    private float hairColorValueS = 1;
    private SpriteAtlas spriteAtlas_Hair;
    private SpriteAtlas spriteAtlas_Eye;


    private int hairIndex = 0;
    private int eyeIndex = 0;
    private void BindHeadPanel()
    {
        spriteAtlas_Hair = Resources.Load<SpriteAtlas>("Atlas/HairSprite");
        spriteAtlas_Eye = Resources.Load<SpriteAtlas>("Atlas/EyeSprite");
        input_Name.onValueChanged.AddListener(ChangeName);
        btn_LastHair.onClick.AddListener(() =>
        {
            ChangeHairType(-1);
        });
        btn_NextHair.onClick.AddListener(() =>
        {
            ChangeHairType(1);
        });
        btn_LastEye.onClick.AddListener(() =>
        {
            ChangeEyeType(-1);
        });
        btn_NextEye.onClick.AddListener(() =>
        {
            ChangeEyeType(1);
        });
        slider_HairColorH.onValueChanged.AddListener(ChangeHairColorH);
        slider_HairColorV.onValueChanged.AddListener(ChangeHairColorV);
        slider_HairColorS.onValueChanged.AddListener(ChangeHairColorS);
    }
    private void UpdateHeadPanel()
    {
        hairIndex = 0;
        eyeIndex = 0;
        hairColorValueH = 1;
        hairColorValueV = 1;
        hairColorValueS = 1;
        slider_HairColorH.value = 1;
        slider_HairColorV.value = 1;
        slider_HairColorS.value = 1;
        input_Name.text = "";
        ChangeName("");
        ChangeHairType(0);
        ChangeEyeType(0);
        ChangeHairColorV(1);
    }
    public void ChangeName(string playerName)
    {
        playerData.Name = playerName;
    }
    public void ChangeEyeType(int offset)
    {
        eyeIndex += offset;
        if (eyeIndex >= EyeConfigData.eyeConfigs.Count)
        {
            eyeIndex = 0;
        }
        if (eyeIndex < 0)
        {
            eyeIndex = EyeConfigData.eyeConfigs.Count - 1;
        }

        playerData.Eye_ID = EyeConfigData.eyeConfigs[eyeIndex].Eye_ID;
        image_Eye.sprite = spriteAtlas_Eye.GetSprite("Eye_" + playerData.Eye_ID.ToString());
    }
    public void ChangeHairType(int offset)
    {
        hairIndex += offset;
        if (hairIndex >= HairConfigData.hairConfigs.Count)
        {
            hairIndex = 0;
        }
        if (hairIndex < 0)
        {
            hairIndex = HairConfigData.hairConfigs.Count - 1;
        }

        playerData.Hair_ID = HairConfigData.hairConfigs[hairIndex].Hair_ID;
        image_Hair.sprite = spriteAtlas_Hair.GetSprite("Hair_" + playerData.Hair_ID.ToString());
    }
    public void ChangeHairColorH(float slider)
    {
        hairColorValueH = slider;
        playerData.Hair_Color = Color.HSVToRGB(hairColorValueH / 1f, hairColorValueS / 1f, hairColorValueV / 1f);
        image_Hair.color = playerData.Hair_Color;
    }
    public void ChangeHairColorV(float slider)
    {
        hairColorValueV = slider;
        playerData.Hair_Color = Color.HSVToRGB(hairColorValueH / 1f, hairColorValueS / 1f, hairColorValueV / 1f);
        image_Hair.color = playerData.Hair_Color;
    }
    public void ChangeHairColorS(float slider)
    {
        hairColorValueS = slider;
        playerData.Hair_Color = Color.HSVToRGB(hairColorValueH / 1f, hairColorValueS / 1f, hairColorValueV / 1f);
        image_Hair.color = playerData.Hair_Color;
    }
    #endregion
    #region//角色创建
    public Button btn_Create;
    public Button btn_Return;
    private Action createAction;
    private PlayerData playerData = new PlayerData();
    private string bindPath;
    private void BindCreatePanel()
    {
        btn_Return.onClick.AddListener(Hide);
        btn_Create.onClick.AddListener(CreatePlayerData);
    }

    public void CreatePlayerData()
    {
        FileManager.Instance.WriteFile(bindPath, JsonConvert.SerializeObject(playerData));
        if(createAction != null)
        {
            createAction.Invoke();
        }
    }
    #endregion
    //#region//角色特性
    //public UI_BuffPanel uI_BuffPanel_MyBuff;
    //public UI_BuffPanel uI_BuffPanel_GoodBuff;
    //public UI_BuffPanel uI_BuffPanel_BadBuff;
    //public TextMeshProUGUI text_BuffPoint;
    //public void BindBuffPanel()
    //{
    //    List<BuffConfig> badTemp = BuffConfigData.buffConfigs.FindAll((x) => { return x.Buff_ID / 1000 == 8; });
    //    for (int i = 0; i < badTemp.Count; i++)
    //    {
    //        int index = i;
    //        uI_BuffPanel_BadBuff.DrawBuffCell(badTemp[index], (buff) => 
    //        {
    //            if (!playerData.BuffList.Contains(buff.Buff_ID))
    //            {
    //                AddBuff(buff);
    //            }
    //        });
    //    }
    //    List<BuffConfig> goodTemp = BuffConfigData.buffConfigs.FindAll((x) => { return x.Buff_ID / 1000 == 9; });
    //    for (int i = 0; i < goodTemp.Count; i++)
    //    {
    //        int index = i;
    //        uI_BuffPanel_GoodBuff.DrawBuffCell(goodTemp[index], (buff) => 
    //        {
    //            if(!playerData.BuffList.Contains(buff.Buff_ID))
    //            {
    //                AddBuff(buff);
    //            }
    //        });
    //    }
    //}
    //public void UpdateBuffPanel()
    //{
    //    uI_BuffPanel_MyBuff.ClearBuffCell();
    //}
    //public void AddBuff(BuffConfig buff)
    //{
    //    Type type = Type.GetType("Buff" + buff.Buff_ID.ToString());
    //    BuffBase buffLogic = (BuffBase)Activator.CreateInstance(type);
    //    buffLogic.Listen_AddOnPlayerCreation(ref playerData);
    //    UpdatePointPanel();

    //    playerData.BuffList.Add(buff.Buff_ID);
    //    playerData.BuffPoint -= buff.Buff_Cost;
    //    text_BuffPoint.text = playerData.BuffPoint.ToString();
    //    uI_BuffPanel_MyBuff.DrawBuffCell(buff, (x) =>
    //    {
    //        if (playerData.BuffList.Contains(x.Buff_ID))
    //        {
    //            RemoveBuff(x);
    //        }
    //    });

    //}
    //public void RemoveBuff(BuffConfig buff)
    //{
    //    Type type = Type.GetType("Buff" + buff.Buff_ID.ToString());
    //    BuffBase buffLogic = (BuffBase)Activator.CreateInstance(type);
    //    buffLogic.Listen_SubOnPlayerCreation(ref playerData);
    //    UpdatePointPanel();


    //    playerData.BuffList.Remove(buff.Buff_ID);
    //    text_BuffPoint.text = playerData.BuffPoint.ToString();
    //    playerData.BuffPoint += buff.Buff_Cost;
    //    uI_BuffPanel_MyBuff.DestroyBuffCell(buff);
    //}
    //#endregion

}
