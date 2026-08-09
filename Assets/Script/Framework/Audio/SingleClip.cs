using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleClip
{
    public AudioClip audioClip { get; private set; }
    // 引用计数，用于内存管理
    public int referenceCount { get; private set; }
    public SingleClip(AudioClip clip)
    {
        audioClip = clip;
        referenceCount = 0;
    }
    public void Play(AudioSource source)
    {
        if (source == null || audioClip == null) return;

        source.clip = audioClip;
        source.PlayDelayed(0f); // 这里需要更优雅的方式

        AddReference();  // 播放时增加引用
        // 监听播放结束，自动减少引用
        MonoBehaviour mono = source.GetComponent<MonoBehaviour>();
        if (mono != null)
        {
            mono.StartCoroutine(WaitForPlaybackEnd(source));
        }
    }
    private IEnumerator WaitForPlaybackEnd(AudioSource source)
    {
        yield return new WaitWhile(() => source.isPlaying);
        RemoveReference();  // 播放结束减少引用
    }
    public void AddReference()
    {
        referenceCount++;
    }
    public void RemoveReference()
    {
        referenceCount--;
    }
}
