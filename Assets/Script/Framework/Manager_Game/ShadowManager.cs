using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Reflection;
using Unity.VisualScripting;
using System.Threading.Tasks;
using System.Threading;
using System;

public class ShadowManager : SingleTon<ShadowManager>, ISingleTon
{
    private bool needsUpdate = false;
    private float updateTimer = 0f;
    [Header("性能设置")]
    [SerializeField] private float updateInterval = 0.25f; // 更新间隔（秒）
    [SerializeField] private bool autoUpdate = true;
    [Header("阴影设置")]
    public bool selfShadows;
    private string[] sortingLayerNames = new string[] { "Default", "UnderGround", "Liquid", "Ground", "ShadowCatch" };
    private int[] shadowTargetLayer = new int[] { 0, 0, 0, 0, 0 };
    private static FieldInfo shapePathField = typeof(ShadowCaster2D).GetField("m_ShapePath", BindingFlags.NonPublic | BindingFlags.Instance);
    private static MethodInfo onEnableMethod = typeof(ShadowCaster2D).GetMethod("OnEnable", BindingFlags.NonPublic | BindingFlags.Instance);
    private static FieldInfo _hashField = typeof(ShadowCaster2D).GetField("m_ShapePathHash", BindingFlags.NonPublic | BindingFlags.Instance);
    private static FieldInfo _TaegetLayerField = typeof(ShadowCaster2D).GetField("m_ApplyToSortingLayers",BindingFlags.NonPublic | BindingFlags.Instance);
    [Header("异步设置")]
    [SerializeField] private bool useAsync = true;
    [SerializeField] private int maxShadowsPerFrame = 10;
    private bool isSyncing = false;
    private Coroutine syncCoroutine;

    public CompositeCollider2D compositeCollider;
    private Dictionary<Vector2Int, PolygonCollider2D> colliderDic = new Dictionary<Vector2Int, PolygonCollider2D>();
    public Transform pool;
    public Transform shadowGroup;

    public void Init()
    {
        for (int i = 0; i < sortingLayerNames.Length; i++)
        {
            shadowTargetLayer[i] = SortingLayer.NameToID(sortingLayerNames[i]);
        }
    }
    private void Update()
    {
        if (!autoUpdate) return;

        updateTimer += Time.deltaTime;
        if (updateTimer >= updateInterval && needsUpdate)
        {
            if (useAsync && Application.isPlaying)
            {
                SyncShadowsAsync();
            }
            else
            {
                SyncShadows();
            }

            updateTimer = 0f;
            needsUpdate = false;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void AddCollider(Vector2Int key, PolygonCollider2D collider2D)
    {
        PolygonCollider2D newCollider =  CreateCollider(collider2D);
        if (colliderDic.TryGetValue(key,out PolygonCollider2D val))
        {
            colliderDic[key] = newCollider;
            Destroy(val);
        }
        else
        {
            colliderDic.Add(key, newCollider);
        }
        needsUpdate = true;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    public void RemoveCollider(Vector2Int key)
    {
        if (colliderDic.TryGetValue(key, out PolygonCollider2D val))
        {
            colliderDic.Remove(key);
            Destroy(val);
            needsUpdate = true;
        }
    }
    private PolygonCollider2D CreateCollider(PolygonCollider2D copyFrom)
    {
        PolygonCollider2D collider2D = pool.AddComponent<PolygonCollider2D>();
        collider2D.pathCount = copyFrom.pathCount;
        collider2D.offset = copyFrom.transform.position;
        for (int i = 0; i < copyFrom.pathCount; i++)
        {
            Vector2[] path = copyFrom.GetPath(i);
            collider2D.SetPath(i, path);
        }
        collider2D.isTrigger = true;
        collider2D.usedByComposite = true;
        return collider2D;
    }
    public void SyncShadows()
    {
        // 1. 清理旧的 Shadow Caster 子物体
        for (int i = shadowGroup.childCount - 1; i >= 0; i--)
        {
            if (shadowGroup.GetChild(i).name.StartsWith("ShadowPath_"))
            {
                DestroyImmediate(shadowGroup.GetChild(i).gameObject);
            }
        }

        // 2. 遍历 Composite 的所有路径（包括外框和孔洞）
        for (int i = 0; i < compositeCollider.pathCount; i++)
        {
            Vector2[] pathPoints = new Vector2[compositeCollider.GetPathPointCount(i)];
            compositeCollider.GetPath(i, pathPoints);

            // 3. 创建专门承载阴影的子物体
            GameObject shadowObj = new GameObject($"ShadowPath_{i}");
            shadowObj.transform.SetParent(shadowGroup, false);
            ShadowCaster2D caster = shadowObj.AddComponent<ShadowCaster2D>();
            caster.useRendererSilhouette = false;
            
            // 4. 将顶点数据赋值给 ShadowCaster2D
            // 注意：ShadowCaster2D 的顶点是 Vector3
            Vector3[] pathPointsV3 = new Vector3[pathPoints.Length];
            for (int j = 0; j < pathPoints.Length; j++)
                pathPointsV3[j] = (Vector3)pathPoints[j];

            shapePathField.SetValue(caster, pathPointsV3);
            _hashField.SetValue(caster,UnityEngine.Random.Range(1, 99999));
            _TaegetLayerField.SetValue(caster, shadowTargetLayer);
            // 默认设置一些常用参数
            caster.selfShadows = selfShadows;
            
            MethodInfo onEnable = typeof(ShadowCaster2D).GetMethod("OnEnable", BindingFlags.NonPublic | BindingFlags.Instance);
            onEnable?.Invoke(caster, null);
            //onEnableMethod.Invoke(caster, null);

        }

        Debug.Log($"同步完成！共生成了 {compositeCollider.pathCount} 个阴影路径（含空洞）。");
    }



    /// <summary>
    /// 异步同步阴影（主线程分帧处理）
    /// </summary>
    public void SyncShadowsAsync()
    {
        if (isSyncing)
        {
            Debug.Log("阴影同步正在进行中，跳过本次请求");
            return;
        }

        StopAllSyncOperations();
        syncCoroutine = StartCoroutine(SyncShadowsCoroutine());
    }

    /// <summary>
    /// 停止所有同步操作
    /// </summary>
    public void StopAllSyncOperations()
    {
        if (syncCoroutine != null)
        {
            StopCoroutine(syncCoroutine);
            syncCoroutine = null;
        }
        isSyncing = false;
    }

    /// <summary>
    /// 协程实现异步阴影同步
    /// </summary>
    private IEnumerator SyncShadowsCoroutine()
    {
        isSyncing = true;

        try
        {
            // 1. 清理旧的阴影（分帧）
            yield return StartCoroutine(ClearOldShadowsCoroutine());

            // 2. 创建新的阴影（分帧）
            yield return StartCoroutine(CreateNewShadowsCoroutine());

            Debug.Log($"异步阴影同步完成！");
        }
        finally
        {
            isSyncing = false;
            syncCoroutine = null;
        }
    }

    /// <summary>
    /// 分帧清理旧的阴影
    /// </summary>
    private IEnumerator ClearOldShadowsCoroutine()
    {
        if (shadowGroup == null) yield break;

        int destroyedThisFrame = 0;

        for (int i = shadowGroup.childCount - 1; i >= 0; i--)
        {
            Transform child = shadowGroup.GetChild(i);
            if (child.name.StartsWith("ShadowPath_"))
            {
                if (Application.isPlaying)
                    Destroy(child.gameObject);
                else
                    DestroyImmediate(child.gameObject);

                destroyedThisFrame++;

                // 每帧最多销毁的数量
                if (destroyedThisFrame >= 20) // 可以调整这个值
                {
                    destroyedThisFrame = 0;
                    yield return null; // 等待一帧
                }
            }
        }

        // 等待一帧确保所有对象都被销毁
        yield return null;
    }

    /// <summary>
    /// 分帧创建新的阴影
    /// </summary>
    private IEnumerator CreateNewShadowsCoroutine()
    {
        if (compositeCollider == null || shadowGroup == null)
            yield break;

        int pathCount = compositeCollider.pathCount;
        int createdThisFrame = 0;

        for (int i = 0; i < pathCount; i++)
        {
            // 获取路径数据
            Vector2[] pathPoints = new Vector2[compositeCollider.GetPathPointCount(i)];
            compositeCollider.GetPath(i, pathPoints);

            // 创建阴影
            CreateSingleShadowCaster(i, pathPoints);

            createdThisFrame++;

            // 控制每帧创建的数量
            if (createdThisFrame >= maxShadowsPerFrame)
            {
                createdThisFrame = 0;
                yield return null; // 等待一帧

                // 可以在这里添加进度回调
                // OnSyncProgress?.Invoke((float)i / pathCount);
            }
        }

        // 强制等待一帧，确保所有对象都创建完成
        yield return null;
    }
    /// <summary>
    /// 创建单个阴影投射器
    /// </summary>
    private void CreateSingleShadowCaster(int index, Vector2[] pathPoints)
    {
        if (shadowGroup == null)
            return;

        GameObject shadowObj = new GameObject($"ShadowPath_{index}");
        shadowObj.transform.SetParent(shadowGroup, false);

        ShadowCaster2D caster = shadowObj.AddComponent<ShadowCaster2D>();
        caster.useRendererSilhouette = false;
        caster.selfShadows = selfShadows;

        // 转换顶点数据
        Vector3[] pathPointsV3 = new Vector3[pathPoints.Length];
        for (int j = 0; j < pathPoints.Length; j++)
            pathPointsV3[j] = pathPoints[j];

        // 设置ShadowCaster2D的路径
        shapePathField?.SetValue(caster, pathPointsV3);
        _hashField?.SetValue(caster, UnityEngine.Random.Range(1, 99999));
        _TaegetLayerField?.SetValue(caster, shadowTargetLayer);
        // 激活ShadowCaster2D
        onEnableMethod?.Invoke(caster, null);
    }
}
public struct ShadowPoint
{
    Vector2 Center;

}