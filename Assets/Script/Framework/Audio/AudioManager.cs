using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class AudioManager : SingleTon<AudioManager>, ISingleTon
{
    public SourceManager sourceManager;
    ClipManager clipManager;
    public void Init()
    {
        sourceManager = new SourceManager(gameObject);
        clipManager = new ClipManager();
    }
    /// <summary>
    /// 更新音效设置
    /// </summary>
    public void UpdateSetting()
    {
        sourceManager.UpdateSetting();
    }
    /// <summary>
    /// 播放BGM
    /// </summary>
    /// <param name="AudioName"></param>
    /// <param name="isLoop"></param>
    public void PlayMusic(int AudioID, bool isLoop)
    {
        SingleClip tmpClips = clipManager.FindClipByID(AudioID);
        sourceManager.UpdateSetting();
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
    public void PlayEffect(int AudioID)
    {
        sourceManager.ReleaseFreeAudio();
        sourceManager.UpdateSetting();
        AudioSource tmpSource = sourceManager.GetFreeAudio();
        SingleClip tmpClips = clipManager.FindClipByID(AudioID);
        if (tmpClips != null)
        {
            tmpClips.Play(tmpSource);
        }
    }

}
