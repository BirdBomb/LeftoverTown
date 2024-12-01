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
    public Transform panel_Main;
    public Button btn_Return;
    private PlayerData playerData = new PlayerData();
    private void Awake()
    {
        Bind();
    }
    private void Bind()
    {
        BindCreatePanel();
        BindHeadPanel();
        BindBuffPanel();
    }
    public void Init(string path,Action create)
    {
        _path = path;
        playerData = new PlayerData();
        createAction = create;
        UpdatePointPanel();
        UpdateHeadPanel();
        UpdateBuffPanel();
    }
    public void Show()
    {
        panel_Main.gameObject.SetActive(true);
    }
    public void Hide()
    {
        panel_Main.gameObject.SetActive(false);
    }
    #region//角色点数
    public UI_PointPanel point_Strength;
    public UI_PointPanel Point_Intelligence;
    public UI_PointPanel Point_SPower;
    public UI_PointPanel Point_Focus;
    public UI_PointPanel Point_Agility;
    public UI_PointPanel Point_Make;
    public UI_PointPanel Point_Build;
    public UI_PointPanel Point_Cook;

    private void UpdatePointPanel()
    {
        point_Strength.UpdatePoint(playerData.Point_Strength);
        Point_Intelligence.UpdatePoint(playerData.Point_Intelligence);
        Point_SPower.UpdatePoint(playerData.Point_SPower);
        Point_Focus.UpdatePoint(playerData.Point_Focus);
        Point_Agility.UpdatePoint(playerData.Point_Agility);
        Point_Make.UpdatePoint(playerData.Point_Make);
        Point_Build.UpdatePoint(playerData.Point_Build);
        Point_Cook.UpdatePoint(playerData.Point_Cook);
    }
    #endregion
    #region//角色外貌
    [Tooltip("---外貌---")]
    public TMP_InputField input_Name;
    public Image image_Hair;
    public Image image_Eye;
    public Button btn_LastHair;
    public Button btn_NextHair;
    public Button btn_LastEye;
    public Button btn_NextEye;
    public Slider slider_HairColorH;
    private float slider_HairColorValueH = 1;
    public Slider slider_HairColorV;
    private float slider_HairColorValueV = 1;
    public Slider slider_HairColorS;
    private float slider_HairColorValueS = 1;
    private SpriteAtlas atlasHair;
    private SpriteAtlas atlasEye;


    private int hairIndex = 0;
    private int eyeIndex = 0;
    private void BindHeadPanel()
    {
        atlasHair = Resources.Load<SpriteAtlas>("Atlas/HairSprite");
        atlasEye = Resources.Load<SpriteAtlas>("Atlas/EyeSprite");
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
        slider_HairColorValueH = 1;
        slider_HairColorValueV = 1;
        slider_HairColorValueS = 1;
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

        playerData.HairID = HairConfigData.hairConfigs[hairIndex].Hair_ID;
        image_Hair.sprite = atlasHair.GetSprite("Hair_" + playerData.HairID.ToString());
    }
    public void ChangeHairColorH(float slider)
    {
        slider_HairColorValueH = slider;
        playerData.HairColor = Color.HSVToRGB(slider_HairColorValueH / 1f, slider_HairColorValueS / 1f, slider_HairColorValueV / 1f);
        image_Hair.color = playerData.HairColor;
    }
    public void ChangeHairColorV(float slider)
    {
        slider_HairColorValueV = slider;
        playerData.HairColor = Color.HSVToRGB(slider_HairColorValueH / 1f, slider_HairColorValueS / 1f, slider_HairColorValueV / 1f);
        image_Hair.color = playerData.HairColor;
    }
    public void ChangeHairColorS(float slider)
    {
        slider_HairColorValueS = slider;
        playerData.HairColor = Color.HSVToRGB(slider_HairColorValueH / 1f, slider_HairColorValueS / 1f, slider_HairColorValueV / 1f);
        image_Hair.color = playerData.HairColor;
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

        playerData.EyeID = EyeConfigData.eyeConfigs[eyeIndex].Eye_ID;
        image_Eye.sprite = atlasEye.GetSprite("Eye_" + playerData.EyeID.ToString());
    }
    #endregion
    #region//角色特性
    public UI_BuffPanel buffPanel_MyBuff;
    public UI_BuffPanel buffPanel_GoodBuff;
    public UI_BuffPanel buffPanel_BadBuff;
    public TextMeshProUGUI text_BuffPoint;
    public void BindBuffPanel()
    {
        List<BuffConfig> badTemp = BuffConfigData.buffConfigs.FindAll((x) => { return x.Buff_ID / 1000 == 8; });
        for (int i = 0; i < badTemp.Count; i++)
        {
            int index = i;
            buffPanel_BadBuff.DrawBuffCell(badTemp[index], (buff) => 
            {
                if (!playerData.BuffList.Contains(buff.Buff_ID))
                {
                    AddBuff(buff);
                }
            });
        }
        List<BuffConfig> goodTemp = BuffConfigData.buffConfigs.FindAll((x) => { return x.Buff_ID / 1000 == 9; });
        for (int i = 0; i < goodTemp.Count; i++)
        {
            int index = i;
            buffPanel_GoodBuff.DrawBuffCell(goodTemp[index], (buff) => 
            {
                if(!playerData.BuffList.Contains(buff.Buff_ID))
                {
                    AddBuff(buff);
                }
            });
        }
    }
    public void UpdateBuffPanel()
    {
        buffPanel_MyBuff.ClearBuffCell();
    }
    public void AddBuff(BuffConfig buff)
    {
        Type type = Type.GetType("Buff" + buff.Buff_ID.ToString());
        BuffBase buffLogic = (BuffBase)Activator.CreateInstance(type);
        buffLogic.Listen_AddOnPlayerCreation(ref playerData);
        UpdatePointPanel();

        playerData.BuffList.Add(buff.Buff_ID);
        playerData.BuffPoint -= buff.Buff_Cost;
        text_BuffPoint.text = playerData.BuffPoint.ToString();
        buffPanel_MyBuff.DrawBuffCell(buff, (x) =>
        {
            if (playerData.BuffList.Contains(x.Buff_ID))
            {
                RemoveBuff(x);
            }
        });

    }
    public void RemoveBuff(BuffConfig buff)
    {
        Type type = Type.GetType("Buff" + buff.Buff_ID.ToString());
        BuffBase buffLogic = (BuffBase)Activator.CreateInstance(type);
        buffLogic.Listen_SubOnPlayerCreation(ref playerData);
        UpdatePointPanel();


        playerData.BuffList.Remove(buff.Buff_ID);
        text_BuffPoint.text = playerData.BuffPoint.ToString();
        playerData.BuffPoint += buff.Buff_Cost;
        buffPanel_MyBuff.DestroyBuffCell(buff);
    }
    #endregion
    #region//角色创建
    private string _path;
    public Button btn_Create;
    private Action createAction;

    private void BindCreatePanel()
    {
        btn_Return.onClick.AddListener(Hide);
        btn_Create.onClick.AddListener(CreatePlayerData);
    }

    public void CreatePlayerData()
    {
        FileManager.Instance.WriteFile(_path, JsonConvert.SerializeObject(playerData));
        if(createAction != null)
        {
            createAction.Invoke();
        }
    }
    #endregion
}
