using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Audio;
using DG.Tweening;
using UnityEngine.Rendering.Universal;
using System.Runtime.CompilerServices;

public class AudioManager : SingleTon<AudioManager>, ISingleTon
{
    [Header("混音器")]
    public AudioMixer audioMixer;
    [Header("播放器")]
    public SourceManager sourceManager;
    private ClipManager clipManager = new ClipManager();
    public void Init()
    {
    }
    /// <summary>
    /// 更新音效设置
    /// </summary>
    public void UpdateMusicSetting(float volumeEffect)
    {
        audioMixer.SetFloat("EffectVolume", Remap01ToDB(volumeEffect));
        Debug.Log(volumeEffect);
    }
    /// <summary>
    /// 更新音乐设置
    /// </summary>
    /// <param name="volumeMusic"></param>
    public void UpdateEffectSetting(float volumeMusic)
    {
        audioMixer.SetFloat("MusicVolume", Remap01ToDB(volumeMusic));
    }
    private float Remap01ToDB(float x)
    {

        if (x <= 0.0f) x = 0.0001f;

        return Mathf.Log10(x) * 20.0f;

    }
    #region//BGM
    private float volume_Music;
    /// <summary>
    /// 播放BGM
    /// </summary>
    /// <param name="AudioName"></param>
    /// <param name="loop"></param>
    public void PlayMusic(int AudioID, float volume, bool loop)
    {
        AudioConfig audioConfig = AudioConfigData.audioConfigs.Find((x) => { return x.Audio_ID == AudioID; });
        SingleClip tmpClips = clipManager.FindClipByID(audioConfig.Audio_Name);
        if (tmpClips == null)
        {
            return;
        }
        else
        {
            SetMusicLoop(loop);
            SetMusicVolume(volume);
            tmpClips.Play(sourceManager.GetMusicAudio());
        }
    }
    private void SetMusicLoop(bool loop)
    {
        sourceManager.MusicSource.loop = loop;
    }
    private void SetMusicVolume(float volume)
    {
        sourceManager.MusicSource.volume = 0;
        DOTween.To(() => volume_Music, x => volume_Music = x, volume, 5f).OnUpdate(() =>
        {
            sourceManager.MusicSource.volume = volume_Music;
        });
    }
    #endregion
    #region//Effect
    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="AudioID"></param>
    /// <param name="pos"></param>
    public void Play3DEffect(int AudioID, Vector3 pos)
    {
        AudioSource tempSource = sourceManager.GetFreeAudio();
        tempSource.spatialBlend = 1;

        AudioConfig audioConfig = AudioConfigData.audioConfigs.Find((x) => { return x.Audio_ID == AudioID; });
        tempSource.maxDistance = audioConfig.Audio_MaxDistance;
        SingleClip tmpClips = clipManager.FindClipByID(audioConfig.Audio_Name);

        tempSource.transform.position = pos + new Vector3(0, 0, -10);
        if (tmpClips != null)
        {
            tmpClips.Play(tempSource);
        }
    }
    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="AudioID"></param>
    public void Play2DEffect(int AudioID)
    {
        AudioSource tempSource = sourceManager.GetFreeAudio();
        tempSource.spatialBlend = 0;

        AudioConfig audioConfig = AudioConfigData.audioConfigs.Find((x) => { return x.Audio_ID == AudioID; });
        tempSource.maxDistance = audioConfig.Audio_MaxDistance;
        SingleClip tmpClips = clipManager.FindClipByID(audioConfig.Audio_Name);

        if (tmpClips != null)
        {
            tmpClips.Play(tempSource);
        }
    }
    #endregion
    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="AudioID"></param>
    public void PlayEffect(int AudioID,Transform root = null)
    {
        AudioSource tempSource = sourceManager.GetFreeAudio();

        AudioConfig audioConfig = AudioConfigData.audioConfigs.Find((x) => { return x.Audio_ID == AudioID; });

        tempSource.maxDistance = audioConfig.Audio_MaxDistance;
        SingleClip tmpClips = clipManager.FindClipByID(audioConfig.Audio_Name);
        if (root != null)
        {
            tempSource.transform.parent = root;
            tempSource.transform.localPosition = Vector3.zero;
        }
        if (tmpClips != null)
        {
            tmpClips.Play(tempSource);
        }
    }
}
