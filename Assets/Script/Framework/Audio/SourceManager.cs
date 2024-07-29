using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SourceManager : MonoBehaviour
{
    /// <summary>
    /// BGM播放器
    /// </summary>
    public AudioSource MusicSource;
    /// <summary>
    /// 特效播放器
    /// </summary>
    public GameObject EffectSource;
    /// <summary>
    /// 特效播放器列表
    /// </summary>
    private List<AudioSource> EffectSources = new List<AudioSource>();
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
        ReleaseFreeAudio();
        for (int i = 0; i < EffectSources.Count; i++)
        {
            if (EffectSources[i] == null)
            {
                EffectSources[i] = Instantiate(EffectSource, transform).GetComponent<AudioSource>();
            }
            if (!EffectSources[i].isPlaying)
            {

                return EffectSources[i];
            }
        }
        AudioSource source = Instantiate(EffectSource, transform).GetComponent<AudioSource>();
        EffectSources.Add(source);
        return source;
    }
    /// <summary>
    /// 释放闲置播放器
    /// (多余12的销毁)
    /// </summary>
    public void ReleaseFreeAudio()
    {
        int tmpCount = 0;
        List<AudioSource> tmpSources = new List<AudioSource>();
        EffectSources.RemoveAll(x => x == null);
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
            GameObject.Destroy(tmpSource.gameObject);
        }
    }
}
