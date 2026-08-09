using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_LockMask : MonoBehaviour
{
    public Button button;
    public TMPro.TextMeshProUGUI text;
    [Header("时间限制设置")]
    [Tooltip("允许游玩的截止日期 (UTC时间)")]
    public string ExpireDateTimeUTC = "2026-08-10 23:59:59";
    [Tooltip("过期提示信息")]
    public string ExpireMessage = "测试包已过期，请联系小鸟炸弹获取新版本。";
    public void Start()
    {
        button.onClick.AddListener(ClickBtn);
    }
    public void ClickBtn()
    {
        if (IsExpired()) { text.text = ExpireMessage; }
        else { gameObject.SetActive(false); }
    }
    /// <summary>
    /// 检查当前时间是否超过了设定的过期时间
    /// </summary>
    /// <returns>true=已过期, false=仍有效</returns>
    public bool IsExpired()
    {
        // 1. 解析设定的过期时间
        if (!DateTime.TryParse(ExpireDateTimeUTC, out DateTime expireTime))
        {
            Debug.LogError("时间格式错误，请使用 'yyyy-MM-dd HH:mm:ss' 格式");
            return false; // 解析失败时默认不过期，避免误伤
        }

        // 2. 获取当前UTC时间 (避免玩家修改系统时区作弊)
        DateTime currentTime = DateTime.UtcNow;

        // 3. 比较时间
        bool expired = currentTime > expireTime;

        // 4. 输出日志方便调试
        Debug.Log($"当前UTC时间: {currentTime:yyyy-MM-dd HH:mm:ss} | 过期时间: {expireTime:yyyy-MM-dd HH:mm:ss} | 已过期: {expired}");

        return expired;
    }
}
