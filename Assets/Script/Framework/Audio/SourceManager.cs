using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SourceManager
{
    /// <summary>
    /// BGM播放器
    /// </summary>
    public AudioSource MusicSource;
    /// <summary>
    /// 所有音效播放器
    /// </summary>
    List<AudioSource> EffectSources;
    GameObject ower;
    public SourceManager(GameObject tmpOwer)
    {
        ower = tmpOwer;
        Init();
    }
    public void Init()
    {
        MusicSource = ower.AddComponent<AudioSource>();
        MusicSource.loop = true;
        MusicSource.rolloffMode = AudioRolloffMode.Linear;
        EffectSources = new List<AudioSource>();
        for (int i = 0; i < 12; i++)
        {
            AudioSource source = ower.AddComponent<AudioSource>();
            EffectSources.Add(source);
        }
    }
    /// <summary>
    /// 更新音效设置
    /// </summary>
    public void UpdateSetting()
    {

    }
    /// <summary>
    /// 获取BGM播放器
    /// </summary>
    /// <returns>BGM播放器</returns>
    public AudioSource GetMusicAudio()
    {
        return MusicSource;
    }
    /// <summary>
    /// 获取闲置播放器
    /// </summary>
    /// <returns>闲置播放器</returns>
    public AudioSource GetFreeAudio()
    {
        for (int i = 0; i < EffectSources.Count; i++)
        {
            if (!EffectSources[i].isPlaying)
            {
                return EffectSources[i];
            }
        }
        AudioSource source = ower.AddComponent<AudioSource>();
        EffectSources.Add(source);
        UpdateSetting();
        return source;
    }
    /// <summary>
    /// 释放闲置播放器
    /// </summary>
    public void ReleaseFreeAudio()
    {
        int tmpCount = 0;
        List<AudioSource> tmpSources = new List<AudioSource>();
        for (int i = 0; i < EffectSources.Count; i++)
        {
            if (!EffectSources[i].isPlaying)
            {
                tmpCount++;
                if (tmpCount > 12)
                {
                    tmpSources.Add(EffectSources[i]);
                }
            }
        }
        for (int i = 0; i < tmpSources.Count; i++)
        {
            AudioSource tmpSource = tmpSources[i];
            EffectSources.Remove(tmpSource);
            GameObject.Destroy(tmpSource);
        }
    }
}
