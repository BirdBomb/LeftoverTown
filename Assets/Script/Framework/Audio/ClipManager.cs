using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClipManager
{
    private Dictionary<string, SingleClip> clipCache = new Dictionary<string, SingleClip>();
    private HashSet<string> loadingClips = new HashSet<string>(); // 防止重复加载
    private float unloadInterval = 60f; // 每60秒检查一次
    private float lastUnloadTime;
    public void Update()
    {
        if (Time.time - lastUnloadTime > unloadInterval)
        {
            UnloadUnusedClips();
            lastUnloadTime = Time.time;
        }
    }
    /// <summary>
    /// 同步查找音频剪辑
    /// </summary>
    public SingleClip FindClipByID(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            Debug.LogWarning("[ClipManager] 音频名称为空");
            return null;
        }

        // 从缓存获取
        if (clipCache.TryGetValue(name, out SingleClip clip))
        {
            return clip;
        }

        // 同步加载
        return LoadClipSync(name);
    }
    /// <summary>
    /// 异步查找音频剪辑
    /// </summary>
    public void FindClipByIDAsync(string name, System.Action<SingleClip> callback)
    {
        if (string.IsNullOrEmpty(name))
        {
            Debug.LogWarning("[ClipManager] 音频名称为空");
            callback?.Invoke(null);
            return;
        }

        // 从缓存获取
        if (clipCache.TryGetValue(name, out SingleClip clip))
        {
            callback?.Invoke(clip);
            return;
        }

        // 异步加载
        LoadClipAsync(name, callback);
    }
    private SingleClip LoadClipSync(string name)
    {
        string path = GetAudioPath(name);
        AudioClip audioClip = Resources.Load<AudioClip>(path);

        if (audioClip == null)
        {
            Debug.LogError($"[ClipManager] 同步加载音频失败: {path}");
            return null;
        }

        return CacheClip(name, audioClip);
    }
    private void LoadClipAsync(string name, System.Action<SingleClip> callback = null)
    {
        // 防止重复加载
        if (loadingClips.Contains(name))
        {
            // 可以在这里添加等待队列逻辑
            Debug.Log($"[ClipManager] 音频正在加载中: {name}");
            callback?.Invoke(null);
            return;
        }

        loadingClips.Add(name);
        string path = GetAudioPath(name);
        ResourceRequest request = Resources.LoadAsync<AudioClip>(path);

        request.completed += (operation) =>
        {
            loadingClips.Remove(name);

            AudioClip audioClip = request.asset as AudioClip;
            if (audioClip == null)
            {
                Debug.LogError($"[ClipManager] 异步加载音频失败: {path}");
                callback?.Invoke(null);
                return;
            }

            SingleClip clip = CacheClip(name, audioClip);
            callback?.Invoke(clip);
        };
    }
    private SingleClip CacheClip(string name, AudioClip audioClip)
    {
        var singleClip = new SingleClip(audioClip);
        clipCache[name] = singleClip;
        return singleClip;
    }
    private string GetAudioPath(string name)
    {
        return $"Audio/{name}";
    }
    /// <summary>
    /// 卸载未使用的音频
    /// </summary>
    public void UnloadUnusedClips()
    {
        List<string> toRemove = new List<string>();

        foreach (var kvp in clipCache)
        {
            // 检查引用计数或最后使用时间
            if (kvp.Value.referenceCount <= 0)
            {
                Resources.UnloadAsset(kvp.Value.audioClip);
                toRemove.Add(kvp.Key);
            }
        }

        foreach (string key in toRemove)
        {
            clipCache.Remove(key);
        }
        // 卸载未使用的资源
        Resources.UnloadUnusedAssets();
    }
}
