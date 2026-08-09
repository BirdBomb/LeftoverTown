using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
public class PoolManager : SingleTon<PoolManager>, ISingleTon
{
    [Header("对象池根节点")]
    public Transform tran_Pool;

    // 预制体缓存
    private Dictionary<string, GameObject> prefabCache = new Dictionary<string, GameObject>();
    // 对象池字典
    private Dictionary<string, IObjectPool<GameObject>> pools = new Dictionary<string, IObjectPool<GameObject>>();

    // 配置参数
    private const int DEFAULT_CAPACITY = 10;
    private const int MAX_SIZE = 50;

    public void Init()
    {
        // 确保对象池根节点存在
        if (tran_Pool == null)
        {
            GameObject poolObj = new GameObject("ObjectPool");
            tran_Pool = poolObj.transform;
            DontDestroyOnLoad(poolObj);
        }

        ClearAllPools();
    }

    // 清除所有对象池
    public void ClearAllPools()
    {
        foreach (var pool in pools.Values)
        {
            pool.Clear();
        }
        pools.Clear();
        // 注意：不清除prefabCache，因为预制体可以复用
    }
    // 加载预制体
    private GameObject LoadPrefab(string key)
    {
        if (string.IsNullOrEmpty(key))
        {
            Debug.LogError("预制体key不能为空");
            return null;
        }

        if (!prefabCache.ContainsKey(key) || prefabCache[key] == null)
        {
            GameObject prefab = Resources.Load<GameObject>(key);

            if (prefab == null)
            {
                Debug.LogError($"无法加载预制体: {key}");
                return null;
            }

            prefabCache[key] = prefab;
        }

        return prefabCache[key];
    }

    // 创建新的对象池
    public IObjectPool<GameObject> GetPool(string key, int defaultCapacity = DEFAULT_CAPACITY, int maxSize = MAX_SIZE)
    {
        if (string.IsNullOrEmpty(key))
        {
            Debug.LogError("对象池key不能为空");
            return null;
        }

        if (!pools.ContainsKey(key))
        {
            GameObject prefab = LoadPrefab(key);
            if (prefab == null) return null;

            var pool = new ObjectPool<GameObject>(
                createFunc: () => {
                    GameObject obj = Instantiate(prefab, tran_Pool);
                    obj.name = $"{key}_{Guid.NewGuid().ToString().Substring(0, 4)}";
                    return obj;
                },
                actionOnGet: obj => { obj.SetActive(true); },
                actionOnRelease: obj => { obj.SetActive(false); obj.transform.SetParent(tran_Pool, false); },
                actionOnDestroy: obj => { Destroy(obj); },
                collectionCheck: false,
                defaultCapacity: defaultCapacity,
                maxSize: maxSize
            );

            pools[key] = pool;
            //Debug.Log($"创建对象池: {key}, 容量:{defaultCapacity}, 最大:{maxSize}");
        }

        return pools[key];
    }

    // 从对象池中获取对象（基础方法）
    public GameObject GetObject(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            Debug.LogError("对象名称不能为空");
            return null;
        }

        var pool = GetPool(name);
        if (pool == null) return null;

        return pool.Get();
    }
    public GameObject GetEffectObj(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            Debug.LogError("对象名称不能为空");
            return null;
        }

        var pool = GetPool(name);
        if (pool == null) return null;

        return pool.Get();
    }
    // 获取并设置位置和旋转
    public GameObject GetObject(string name, Vector3 position, Quaternion rotation)
    {
        GameObject obj = GetObject(name);
        if (obj != null)
        {
            obj.transform.position = position;
            obj.transform.rotation = rotation;
        }
        return obj;
    }
    // 泛型方法：直接获取组件
    public T GetObject<T>(string name) where T : Component
    {
        GameObject obj = GetObject(name);
        if (obj == null) return null;

        T component = obj.GetComponent<T>();
        if (component == null)
        {
            Debug.LogWarning($"对象 {name} 上没有找到组件 {typeof(T)}");
            ReleaseObject(name, obj); 
            return null;
        }

        return component;
    }
    // 泛型方法：获取并设置位置和旋转
    public T GetObject<T>(string name, Vector3 position, Quaternion rotation) where T : Component
    {
        T component = GetObject<T>(name);
        if (component != null)
        {
            component.transform.position = position;
            component.transform.rotation = rotation;
        }
        return component;
    }
    // 将对象返回到对象池
    public void ReleaseObject(string name, GameObject obj)
    {
        if (obj == null)
        {
            Debug.LogWarning("尝试释放空对象");
            return;
        }

        if (pools.ContainsKey(name))
        {
            pools[name].Release(obj);
        }
        else
        {
            Debug.LogWarning($"对象池 {name} 不存在，直接销毁对象");
            Destroy(obj);
        }
    }
}

