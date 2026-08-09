using DG.Tweening;
using Fusion;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class UI_BossInfo : SingleTon<UI_BossInfo>,ISingleTon
{
    [System.Serializable]
    public class BossBarData
    {
        public Transform backGround;
        public Transform hpBar;
        public Text hpText;
        public TextMeshProUGUI nameText;

        [HideInInspector] public float activeTimer;
        [HideInInspector] public NetworkId networkId;
        [HideInInspector] public bool isActive;
        [HideInInspector] public int currentHp;
        [HideInInspector] public int currentMaxHp;
    }

    public List<BossBarData> bossBars = new List<BossBarData>();
    private Dictionary<NetworkId, int> dic_NetworkIdToIndex = new Dictionary<NetworkId, int>();
    private Queue<NetworkId> queue_ActiveOrder = new Queue<NetworkId>();

    private const float FLOAT_BAR_LIFE = 15f;


    public void Init()
    {
        // 初始化所有Boss条，设置为不可见
        foreach (var bar in bossBars)
        {
            bar.isActive = false;
            bar.activeTimer = 0;
            if (bar.backGround != null)
                bar.backGround.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        // 使用Update而不是FixedUpdate，因为UI更新不需要物理同步
        List<NetworkId> toRemove = new List<NetworkId>();

        foreach (var kvp in dic_NetworkIdToIndex)
        {
            int index = kvp.Value;
            if (bossBars[index].isActive)
            {
                bossBars[index].activeTimer -= Time.deltaTime;
                if (bossBars[index].activeTimer <= 0)
                {
                    toRemove.Add(kvp.Key);
                }
            }
        }

        // 隐藏过期的Boss条
        foreach (var networkId in toRemove)
        {
            HideBar(networkId);
        }
    }

    /// <summary>
    /// 显示或更新Boss信息
    /// </summary>
    public void Show(int Hp, int MaxHp, string Name, NetworkId networkId)
    {
        if (dic_NetworkIdToIndex.ContainsKey(networkId))
        {
            // 已存在的Boss，更新信息并重置计时器
            int index = dic_NetworkIdToIndex[networkId];
            UpdateBar(index, Hp, MaxHp, Name);
            ResetBarTimer(index);
        }
        else
        {
            // 新的Boss，分配一个条并显示
            int index = FindAvailableBar(networkId);
            if (index >= 0)
            {
                ShowBar(index, Hp, MaxHp, Name, networkId);
            }
            else
            {
                Debug.LogWarning("没有可用的Boss信息条！");
            }
        }
    }

    /// <summary>
    /// 更新指定索引的Boss条信息
    /// </summary>
    private void UpdateBar(int index, int Hp, int MaxHp, string Name)
    {
        if (index < 0 || index >= bossBars.Count) return;

        var bar = bossBars[index];
        bar.currentHp = Hp;
        bar.currentMaxHp = MaxHp;

        // 更新文本
        if (bar.hpText != null)
            bar.hpText.text = $"{Hp}/{MaxHp}";
        if (bar.nameText != null)
            bar.nameText.text = Name;
        // 更新血条
        if (bar.hpBar != null)
        {
            bar.hpBar.DOKill();
            float fillAmount = (float)Hp / Mathf.Max(MaxHp, 1);
            bar.hpBar.DOScaleX(fillAmount, 0.1f);
        }
    }
    /// <summary>
    /// 显示新的Boss条
    /// </summary>
    private void ShowBar(int index, int Hp, int MaxHp, string Name, NetworkId networkId)
    {
        var bar = bossBars[index];
        bar.isActive = true;
        bar.activeTimer = FLOAT_BAR_LIFE;
        bar.networkId = networkId;
        bar.currentHp = Hp;
        bar.currentMaxHp = MaxHp;

        // 添加到字典和队列
        dic_NetworkIdToIndex.Add(networkId, index);
        queue_ActiveOrder.Enqueue(networkId);

        // 显示UI
        if (bar.backGround != null)
        {
            bar.backGround.gameObject.SetActive(true); 
            bar.backGround.DOKill();
            bar.backGround.localScale = Vector3.one;
            bar.backGround.DOPunchScale(new Vector3(-0.1f, 0.2f, 0), 0.2f).SetEase(Ease.InOutBack);
        }

        // 更新内容
        if (bar.hpText != null)
            bar.hpText.text = $"{Hp}/{MaxHp}";
        if (bar.nameText != null)
            bar.nameText.text = Name;

        // 初始化血条
        if (bar.hpBar != null)
        {
            bar.hpBar.DOKill();
            float fillAmount = (float)Hp / Mathf.Max(MaxHp, 1);
            bar.hpBar.localScale = new Vector3(fillAmount, 1, 1);
        }

        // 入场动画
        if (bar.backGround != null)
        {
            bar.backGround.DOScale(Vector3.one, 0.2f).From(Vector3.zero).SetEase(Ease.OutBack);
        }
        SortBarsByYAxis();
    }
    /// <summary>
    /// 重置计时器
    /// </summary>
    private void ResetBarTimer(int index)
    {
        if (index >= 0 && index < bossBars.Count && bossBars[index].isActive)
        {
            bossBars[index].activeTimer = FLOAT_BAR_LIFE;
        }
    }
    /// <summary>
    /// 查找可用的Boss条
    /// </summary>
    private int FindAvailableBar(NetworkId networkId)
    {
        int index = -1;

        if (queue_ActiveOrder.Count < bossBars.Count)
        {
            // 有空闲的条，找到第一个未使用的
            for (int i = 0; i < bossBars.Count; i++)
            {
                if (!dic_NetworkIdToIndex.ContainsValue(i))
                {
                    index = i;
                    break;
                }
            }
        }
        else
        {
            // 没有空闲条，复用最早的一个
            NetworkId oldestId = queue_ActiveOrder.Dequeue();
            index = dic_NetworkIdToIndex[oldestId];

            // 移除旧的映射
            dic_NetworkIdToIndex.Remove(oldestId);

            // 隐藏旧的Boss条
            HideBarImmediate(index);
        }

        return index;
    }
    /// <summary>
    /// 隐藏指定的Boss条（通过NetworkId）
    /// </summary>
    public void HideBar(NetworkId networkId)
    {
        if (!dic_NetworkIdToIndex.ContainsKey(networkId)) return;

        int index = dic_NetworkIdToIndex[networkId];

        // 从队列中移除
        Queue<NetworkId> newQueue = new Queue<NetworkId>();
        while (queue_ActiveOrder.Count > 0)
        {
            NetworkId id = queue_ActiveOrder.Dequeue();
            if (!id.Equals(networkId))
            {
                newQueue.Enqueue(id);
            }
        }
        queue_ActiveOrder = newQueue;

        // 从字典中移除
        dic_NetworkIdToIndex.Remove(networkId);

        // 播放隐藏动画
        HideBarWithAnimation(index);
    }
    /// <summary>
    /// 立即隐藏Boss条（无动画）
    /// </summary>
    private void HideBarImmediate(int index)
    {
        if (index < 0 || index >= bossBars.Count) return;

        var bar = bossBars[index];
        bar.isActive = false;
        bar.activeTimer = 0;

        if (bar.backGround != null)
        {
            bar.backGround.DOKill();
            bar.backGround.gameObject.SetActive(false);
        }
        SortBarsByYAxis();
    }
    /// <summary>
    /// 带动画隐藏Boss条
    /// </summary>
    private void HideBarWithAnimation(int index)
    {
        if (index < 0 || index >= bossBars.Count) return;

        var bar = bossBars[index];
        bar.isActive = false;

        if (bar.backGround != null)
        {
            bar.backGround.DOKill();
            bar.backGround.DOScale(Vector3.zero, 0.2f).OnComplete(() =>
            {
                if (!bar.isActive && bar.backGround != null)
                {
                    bar.backGround.gameObject.SetActive(false);
                    SortBarsByYAxis();
                }
            });
        }
    }
    private void SortBarsByYAxis()
    {
        int activeIndex = 0;
        for (int i = 0; i < bossBars.Count; i++)
        {
            if (bossBars[i].isActive)
            {
                Vector3 pos = bossBars[i].backGround.localPosition;
                pos.y = -activeIndex * 40; // 向下排列，第一个在0，第二个在-40，第三个在-80
                bossBars[i].backGround.DOLocalMoveY(pos.y, 0.1f);
                activeIndex++;
            }
        }
    }
    /// <summary>
    /// 强制隐藏所有Boss条
    /// </summary>
    public void HideAllBars()
    {
        List<NetworkId> allIds = new List<NetworkId>(dic_NetworkIdToIndex.Keys);
        foreach (var id in allIds)
        {
            HideBar(id);
        }
    }

    /// <summary>
    /// 检查指定Boss是否存在
    /// </summary>
    public bool IsBossExists(NetworkId networkId)
    {
        return dic_NetworkIdToIndex.ContainsKey(networkId);
    }

    /// <summary>
    /// 获取指定Boss的当前血量
    /// </summary>
    public int GetBossHp(NetworkId networkId)
    {
        if (dic_NetworkIdToIndex.TryGetValue(networkId, out int index))
        {
            return bossBars[index].currentHp;
        }
        return -1;
    }

    /// <summary>
    /// 手动刷新指定Boss的显示
    /// </summary>
    public void RefreshBoss(NetworkId networkId, int newHp, int newMaxHp)
    {
        if (dic_NetworkIdToIndex.TryGetValue(networkId, out int index))
        {
            UpdateBar(index, newHp, newMaxHp, bossBars[index].nameText?.text ?? "");
            ResetBarTimer(index);
        }
    }
}
