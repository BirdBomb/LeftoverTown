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
    private void Update()
    {
        // 定期更新ClipManager（用于资源清理）
        clipManager.Update();
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
    #region BGM

    public void PlayMusic(int audioID, float volume, bool loop)
    {
        AudioConfig audioConfig = GetAudioConfig(audioID);
        SingleClip clip = clipManager.FindClipByID(audioConfig.Audio_Name);
        if (clip == null) return;
        AudioSource musicSource = sourceManager.GetMusicAudio();
        musicSource.loop = loop;

        // 使用 SingleClip 的 Play 方法
        clip.Play(musicSource);

        // 设置音量渐变
        StartCoroutine(FadeInMusic(volume));
    }
    private IEnumerator FadeInMusic(float targetVolume)
    {
        AudioSource musicSource = sourceManager.GetMusicAudio();
        float startVolume = 0f;
        float duration = 5f;
        float elapsed = 0f;

        musicSource.volume = startVolume;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(startVolume, targetVolume, elapsed / duration);
            yield return null;
        }

        musicSource.volume = targetVolume;
    }
    #endregion
    #region Effect

    public void Play3DEffect(int audioID, Vector3 pos)
    {
        AudioConfig audioConfig = GetAudioConfig(audioID);

        SingleClip clip = clipManager.FindClipByID(audioConfig.Audio_Name);
        if (clip == null) return;

        AudioSource source = sourceManager.GetFreeAudio();
        if (source == null)
        {
            Debug.LogWarning("[AudioManager] 没有可用的音频源");
            return;
        }

        // 配置3D音效
        source.spatialBlend = 1f;
        source.maxDistance = audioConfig.Audio_MaxDistance;
        source.transform.position = pos;

        // 使用 SingleClip 的 Play 方法
        clip.Play(source);
    }
    public void Play2DEffect(int audioID)
    {
        AudioConfig audioConfig = GetAudioConfig(audioID);

        SingleClip clip = clipManager.FindClipByID(audioConfig.Audio_Name);
        if (clip == null) return;

        AudioSource source = sourceManager.GetFreeAudio();
        if (source == null)
        {
            Debug.LogWarning("[AudioManager] 没有可用的音频源");
            return;
        }

        // 配置2D音效
        source.spatialBlend = 0f;
        source.maxDistance = audioConfig.Audio_MaxDistance;

        // 使用 SingleClip 的 Play 方法
        clip.Play(source);
    }
    #endregion
    #region 辅助方法

    private AudioConfig GetAudioConfig(int audioID)
    {
        AudioConfig config = AudioConfigData.audioConfigs.Find(x => x.Audio_ID == audioID);
        return config;
    }

    #endregion
}
