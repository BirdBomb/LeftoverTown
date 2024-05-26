using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
public class PoolManager : SingleTon<PoolManager>, ISingleTon
{
    // ������ֵ䣬���ڴ洢��ͬ���͵Ķ����
    private Dictionary<string, IObjectPool<GameObject>> pools = new Dictionary<string, IObjectPool<GameObject>>();
    public void Init()
    {
        pools = new Dictionary<string, IObjectPool<GameObject>>();
    }
    // �����µĶ���ػ��ȡ���еĶ����
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
    // �Ӷ�����л�ȡ����
    public GameObject GetObject(string name)
    {
        var pool = GetPool(name);
        return pool.Get();
    }
    // �����󷵻ص��������
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
