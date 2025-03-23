using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_GameSetting : MonoBehaviour
{
    private void Start()
    {
        Bind();
    }
    private void Bind()
    {
        BindMain();
        BindAudioPanel();
    }
    #region//主要
    public Button btn_Return;
    public Transform Panel;

    private void BindMain()
    {
        btn_Return.onClick.AddListener(HidePanel);
    }
    public void ShowPanel()
    {
        Panel.gameObject.SetActive(true);
    }
    public void HidePanel()
    {
        Panel.gameObject.SetActive(false);
    }
    #endregion
    #region//音量控制
    public Slider slider_MusicVolume;
    public Slider slider_EffectVolume;
    private void BindAudioPanel()
    {
        slider_MusicVolume.onValueChanged.AddListener(MusicVolumeChange);
        slider_EffectVolume.onValueChanged.AddListener(EffectVolumeChange);
    }
    private void MusicVolumeChange(float val)
    {
        AudioManager.Instance.UpdateMusicSetting(val);
    }
    private void EffectVolumeChange(float val)
    {
        AudioManager.Instance.UpdateEffectSetting(val);
    }
    #endregion
}
