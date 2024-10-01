using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Audio;
using DG.Tweening;

public class AudioManager : SingleTon<AudioManager>, ISingleTon
{
    [Header("混音器")]
    public AudioMixer audioMixer;
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

    /// <summary>
    /// 播放BGM
    /// </summary>
    /// <param name="AudioName"></param>
    /// <param name="isLoop"></param>
    public void PlayMusic(int AudioID, bool isLoop, Transform root = null)
    {
        AudioConfig audioConfig = AudioConfigData.audioConfigs.Find((x) => { return x.Audio_ID == AudioID; });
        SingleClip tmpClips = clipManager.FindClipByID(audioConfig.Audio_Name);
        if (isLoop)
        {
            sourceManager.MusicSource.loop = true;
        }
        else
        {
            sourceManager.MusicSource.loop = false;
        }
        if (tmpClips == null)
        {
            return;
        }
        tmpClips.Play(sourceManager.GetMusicAudio());
    }
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
    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="AudioID"></param>
    /// <param name="pos"></param>
    public void PlayEffect(int AudioID,Vector3 pos)
    {
        AudioSource tempSource = sourceManager.GetFreeAudio();

        AudioConfig audioConfig = AudioConfigData.audioConfigs.Find((x) => { return x.Audio_ID == AudioID; });

        tempSource.maxDistance = audioConfig.Audio_MaxDistance;
        SingleClip tmpClips = clipManager.FindClipByID(audioConfig.Audio_Name);
        tempSource.transform.position = pos + new Vector3(0, 0, -10);
        if (tmpClips != null)
        {
            tmpClips.Play(tempSource);
        }
    }

}
