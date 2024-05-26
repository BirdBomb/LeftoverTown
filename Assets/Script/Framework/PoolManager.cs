using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
public class PoolManager : SingleTon<PoolManager>, ISingleTon
{
    // 对象池字典，用于存储不同类型的对象池
    private Dictionary<string, IObjectPool<GameObject>> pools = new Dictionary<string, IObjectPool<GameObject>>();
    public void Init()
    {
        pools = new Dictionary<string, IObjectPool<GameObject>>();
    }
    // 创建新的对象池或获取现有的对象池
    public IObjectPool<GameObject> GetPool(string key, int defaultCapacity = 10, int maxSize = 50)
    {
        if (!pools.ContainsKey(key))
        {
            pools[key] = new ObjectPool<GameObject>(
                createFunc:() => Instantiate(Resources.Load<GameObject>(key)),
                actionOnGet: obj => obj.SetActive(true),
                actionOnRelease: obj => obj.SetActive(false),
                actionOnDestroy: obj => { },
                collectionCheck: false,
                defaultCapacity: defaultCapacity,
                maxSize: maxSize
            );
        }
        return pools[key];
    }
    // 从对象池中获取对象
    public GameObject GetObject(string name)
    {
        var pool = GetPool(name);
        return pool.Get();
    }
    // 将对象返回到对象池中
    public void ReleaseObject(string name, GameObject obj)
    {
        if (pools.ContainsKey(name))
        {
            pools[name].Release(obj);
        }
        else
        {
            
        }
    }
}
