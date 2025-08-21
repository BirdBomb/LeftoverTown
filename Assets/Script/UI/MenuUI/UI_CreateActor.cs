using DG.Tweening;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class UI_CreateActor : MonoBehaviour
{
    [SerializeField]
    private Transform transform_Panel;
    [SerializeField]
    private Image image_Hair;
    [SerializeField]
    private Image image_Eye;
    [SerializeField]
    private TMP_InputField inputField_Name;
    [SerializeField]
    private Button btn_LastHair;
    [SerializeField]
    private Button btn_NextHair;
    [SerializeField]
    private Text text_HairIndex;
    [SerializeField]
    private Button btn_LastEye;
    [SerializeField]
    private Button btn_NextEye;
    [SerializeField]
    private Text text_EyeIndex;
    [SerializeField]
    private Slider slider_HairColorH;
    private float hairColorValueH = 1;
    [SerializeField]
    private Slider slider_HairColorV;
    private float hairColorValueV = 1;
    [SerializeField]
    private Slider slider_HairColorS;
    private float hairColorValueS = 1;
    [SerializeField]
    private SpriteAtlas spriteAtlas_Hair;
    [SerializeField]
    private SpriteAtlas spriteAtlas_Eye;
    private int hairIndex = 0;
    private int eyeIndex = 0;
    private PlayerData playerData = new PlayerData();
    [SerializeField]
    private Button btn_Create;
    private Action action_Create;
    [SerializeField]
    private Button btn_Return;
    private Action action_Return;
    private string path;
    public void Awake()
    {
        Bind();
    }
    public void Init(int index, Action actionCreate, Action actionReturn)
    {
        path = "PlayerData/Player" + index;
        playerData = new PlayerData();
        transform_Panel.gameObject.SetActive(true);
        transform_Panel.transform.DOPunchScale(new Vector3(0.1f, -0.1f, 0), 0.1f);
        action_Create = actionCreate;
        action_Return = actionReturn;

        ChangeHairType(0);
        ChangeEyeType(0);
        ChangeHairColorH(0);
        ChangeHairColorV(0);
        ChangeHairColorS(0);
    }
    private void Bind()
    {
        inputField_Name.onValueChanged.AddListener(ChangeName);
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

        btn_Create.onClick.AddListener(Create);
        btn_Return.onClick.AddListener(Return);
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
        text_EyeIndex.text = eyeIndex.ToString();
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
        text_HairIndex.text = hairIndex.ToString();
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
    public void Create()
    {
        FileManager.Instance.WriteFile(path, JsonConvert.SerializeObject(playerData));
        transform_Panel.gameObject.SetActive(false);
        if (action_Create != null)
        {
            action_Create.Invoke();
        }
    }
    public void Return()
    {
        transform_Panel.gameObject.SetActive(false);
        if (action_Return != null)
        {
            action_Return.Invoke();
        }
    }
}
