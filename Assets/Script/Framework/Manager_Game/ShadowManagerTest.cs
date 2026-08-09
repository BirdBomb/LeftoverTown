using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Reflection;
using Unity.VisualScripting;
using System.Threading.Tasks;
using System.Threading;
using System;

public class ShadowManagerTest : SingleTon<ShadowManagerTest>, ISingleTon
{
    private bool needsUpdate = false;
    private float updateTimer = 0f;
    [Header("性能设置")]
    [SerializeField] private float updateInterval = 0.25f; // 更新间隔（秒）
    [SerializeField] private bool autoUpdate = true;
    [Header("阴影设置")]
    public bool selfShadows;
    private Vector2[] pathBuffer = new Vector2[16];
    private string[] sortingLayerNames = new string[] { "Default", "UnderGround", "Liquid", "Ground", "ShadowCatch" };
    private int[] shadowTargetLayer = new int[] { 0, 0, 0, 0, 0 };
    private static FieldInfo shapePathField = typeof(ShadowCaster2D).GetField("m_ShapePath", BindingFlags.NonPublic | BindingFlags.Instance);
    private static MethodInfo onEnableMethod = typeof(ShadowCaster2D).GetMethod("OnEnable", BindingFlags.NonPublic | BindingFlags.Instance);
    private static FieldInfo _hashField = typeof(ShadowCaster2D).GetField("m_ShapePathHash", BindingFlags.NonPublic | BindingFlags.Instance);
    private static FieldInfo _TaegetLayerField = typeof(ShadowCaster2D).GetField("m_ApplyToSortingLayers", BindingFlags.NonPublic | BindingFlags.Instance);
    [Header("异步设置")]
    [SerializeField] private bool useAsync = true;
    [SerializeField] private int maxShadowsPerFrame = 10;
    private bool isSyncing = false;
    private Coroutine syncCoroutine;

    public CompositeCollider2D compositeCollider;
    private HashSet<Vector2Int> activeColliders = new HashSet<Vector2Int>();
    private Dictionary<Vector2Int, PolygonCollider2D> dic_ColliderPool = new Dictionary<Vector2Int, PolygonCollider2D>();
    private Dictionary<Vector2Int, PolygonCollider2D> dic_ColliderAwake = new Dictionary<Vector2Int, PolygonCollider2D>();
    private Queue<PolygonCollider2D> colliderPool = new Queue<PolygonCollider2D>();
    private HashSet<Vector2Int> set_ColliderTemp = new HashSet<Vector2Int>();
    private HashSet<Vector2Int> set_ColliderAwake = new HashSet<Vector2Int>();

    public Transform pool;
    private bool bool_useShadowGroup0;
    public Transform shadowGroup_0;
    public Transform shadowGroup_1;

    private Vector2Int vector3_Center = new Vector2Int(0, 0);
    private const int int_ShadowDrawDistance_X = 20;
    private const int int_ShadowDrawDistance_Y = 10;
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

            updateTimer = 0f;
            needsUpdate = false;
        }
    }
    public void UpdateCenter(Vector2Int pos)
    {
        vector3_Center = pos;
        needsUpdate = true;
    }
    /// <summary>
    /// 添加collider
    /// </summary>
    public void AddCollider(Vector2Int key, PolygonCollider2D collider2D)
    {
        if (!activeColliders.Contains(key)) activeColliders.Add(key);
        if (!dic_ColliderPool.TryAdd(key, collider2D)) dic_ColliderPool[key] = collider2D;
        needsUpdate = true;
    }
    /// <summary>
    /// 移除collider
    /// </summary>
    /// <param name="key"></param>
    public void RemoveCollider(Vector2Int key)
    {
        activeColliders.Remove(key);
        dic_ColliderPool.Remove(key);
        needsUpdate = true;
    }
    /// <summary>
    /// 绘制需要的collider
    /// </summary>
    public void DrawCollider()
    {
        set_ColliderTemp.Clear();
        for (int x = vector3_Center.x - int_ShadowDrawDistance_X; x < vector3_Center.x + int_ShadowDrawDistance_X; x++)
        {
            for (int y = vector3_Center.y - int_ShadowDrawDistance_Y; y < vector3_Center.y + int_ShadowDrawDistance_Y; y++)
            {
                var gridPos = new Vector2Int(x, y);
                if (activeColliders.Contains(gridPos))
                {
                    set_ColliderTemp.Add(gridPos);
                }
            }
        }
        foreach (var pos in set_ColliderAwake)
        {
            if (!set_ColliderTemp.Contains(pos))
            {
                if (dic_ColliderAwake.TryGetValue(pos, out PolygonCollider2D val))
                {
                    ReturnColliderToPool(val);
                    dic_ColliderAwake.Remove(pos);
                }
            }
        }
        foreach (var pos in set_ColliderTemp)
        {
            if (!set_ColliderAwake.Contains(pos))
            {
                PolygonCollider2D newCollider = CreateCollider(dic_ColliderPool[pos]);

                if (dic_ColliderAwake.ContainsKey(pos))
                {
                    Destroy(dic_ColliderAwake[pos]);
                    dic_ColliderAwake[pos] = newCollider;
                }
                else
                {
                    dic_ColliderAwake.Add(pos, newCollider);
                }
            }
        }

        set_ColliderAwake.Clear();
        foreach (var pos in set_ColliderTemp)
        {
            set_ColliderAwake.Add(pos);
        }
    }
    private PolygonCollider2D CreateCollider(PolygonCollider2D copyFrom)
    {
        PolygonCollider2D collider2D = GetColliderFromPool();

        collider2D.pathCount = copyFrom.pathCount;
        collider2D.offset = copyFrom.transform.position;

        for (int i = 0; i < copyFrom.pathCount; i++)
        {
            collider2D.SetPath(i, copyFrom.GetPath(i));
        }

        collider2D.isTrigger = true;
        collider2D.usedByComposite = true;

        return collider2D;
    }
    private PolygonCollider2D GetColliderFromPool()
    {
        if (colliderPool.Count > 0)
        {
            PolygonCollider2D col = colliderPool.Dequeue();
            col.enabled = true;
            return col;
        }

        PolygonCollider2D newCol = pool.gameObject.AddComponent<PolygonCollider2D>();
        return newCol;
    }
    private void ReturnColliderToPool(PolygonCollider2D col)
    {
        col.enabled = false;
        col.pathCount = 0;
        colliderPool.Enqueue(col);
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
        DrawCollider();
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

            // 创建新的阴影（分帧）
            yield return StartCoroutine(CreateNewShadowsCoroutine());
            // 清理旧的阴影（分帧）
            yield return StartCoroutine(ClearOldShadowsCoroutine());
            bool_useShadowGroup0 = !bool_useShadowGroup0;
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
        Transform temp = bool_useShadowGroup0 ? shadowGroup_1 : shadowGroup_0;
        if (temp == null) yield break;

        int destroyedThisFrame = 0;

        for (int i = temp.childCount - 1; i >= 0; i--)
        {
            Transform child = temp.GetChild(i);
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
        Transform temp = bool_useShadowGroup0 ? shadowGroup_0 : shadowGroup_1;
        if (compositeCollider == null || temp == null)
            yield break;

        int pathCount = compositeCollider.pathCount;
        int createdThisFrame = 0;

        for (int i = 0; i < pathCount; i++)
        {
            // 获取路径数据
            if (i >= compositeCollider.pathCount) continue;
            int pointCount = compositeCollider.GetPathPointCount(i);


            EnsurePathBufferCapacity(pointCount);
            // 直接写入缓存数组
            compositeCollider.GetPath(i, pathBuffer);
            // 创建阴影
            CreateSingleShadowCaster(i, pathBuffer, pointCount, temp);

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
    private void CreateSingleShadowCaster(int index, Vector2[] pathPoints, int pointCount, Transform shadowGroup)
    {
        if (shadowGroup == null)
            return;

        GameObject shadowObj = new GameObject($"ShadowPath_{index}");
        shadowObj.transform.SetParent(shadowGroup, false);

        ShadowCaster2D caster = shadowObj.AddComponent<ShadowCaster2D>();
        caster.useRendererSilhouette = false;
        caster.selfShadows = selfShadows;


        // 转换顶点数据
        Vector3[] pathPointsV3 = new Vector3[pointCount];
        for (int j = 0; j < pointCount; j++)
        {
            pathPointsV3[j] = pathPoints[j];
        }
        // 设置ShadowCaster2D的路径
        shapePathField?.SetValue(caster, pathPointsV3);
        _hashField?.SetValue(caster, UnityEngine.Random.Range(1, 99999));
        _TaegetLayerField?.SetValue(caster, shadowTargetLayer);
        // 激活ShadowCaster2D
        onEnableMethod?.Invoke(caster, null);
    }
    private void EnsurePathBufferCapacity(int requiredSize)
    {
        if (pathBuffer.Length < requiredSize)
        {
            int newSize = Mathf.NextPowerOfTwo(requiredSize);
            pathBuffer = new Vector2[newSize];
        }
    }
    #region 调试
    private void OnDrawGizmos()
    {
        // 只在编辑器模式下画线
        if (shadowGroup_0 == null && shadowGroup_1 == null) return;

        Gizmos.color = Color.cyan; // 设置调试线条颜色

        // 检查两个组里的所有 ShadowCaster2D
        DrawGroupGizmos(shadowGroup_0);
        DrawGroupGizmos(shadowGroup_1);
    }
    private void DrawGroupGizmos(Transform group)
    {
        if (group == null) return;

        foreach (Transform child in group)
        {
            ShadowCaster2D caster = child.GetComponent<ShadowCaster2D>();
            if (caster == null) continue;

            // 通过反射获取你设置进去的路径数据
            Vector3[] path = (Vector3[])shapePathField?.GetValue(caster);

            if (path != null && path.Length > 1)
            {
                for (int i = 0; i < path.Length; i++)
                {
                    Vector3 start = child.TransformPoint(path[i]);
                    Vector3 end = child.TransformPoint(path[(i + 1) % path.Length]);
                    Gizmos.DrawLine(start, end);

                    // 画一个小箭头或者圆点，表示顶点的顺序（很重要！）
                    if (i == 0) Gizmos.DrawSphere(start, 0.1f);
                }
            }
        }
    }
    #endregion
}
