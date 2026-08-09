using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Reflection;
using Unity.VisualScripting;
using System.Threading.Tasks;
using System.Threading;
using System;
using Clipper2Lib;

public class ShadowManager : SingleTon<ShadowManager>, ISingleTon
{
    void Update()  
    {
        if (float_UpdateTimer < float_UpdateInterval) float_UpdateTimer += Time.deltaTime;
        else
        {
            if (!bool_Dirty || bool_isProcessing) return;
            float_UpdateTimer = 0f;
            bool_Dirty = false;
            StopAllSyncOperations();
            syncCoroutine = StartCoroutine(SyncShadowsCoroutine());
        }
    }
    public void Init()
    {
        for (int i = 0; i < sortingLayerNames.Length; i++)
        {
            shadowTargetLayer[i] = SortingLayer.NameToID(sortingLayerNames[i]);
        }
    }

    [Header("更新设置")]
    public float float_UpdateInterval = 0.25f;
    public int int_MaxShadowsPerFrame = 5;
    [Header("阴影设置")]
    public bool bool_SelfShadows = false;
    [Header("阴影父物体")]
    public Transform trans_ShadowGroup_0;
    public Transform trans_ShadowGroup_1;

    private Coroutine syncCoroutine;

    private float float_UpdateTimer = 0f;
    private bool bool_Dirty = false;
    private bool bool_isProcessing = false;
    private bool bool_useShadowGroup0;

    #region Collider字段
    private Dictionary<Vector2Int, PolygonCollider2D> dic_ColliderRecorded = new Dictionary<Vector2Int, PolygonCollider2D>();
    private HashSet<Vector2Int> set_ColliderRecorded = new HashSet<Vector2Int>();
    #endregion
    #region 阴影绘制范围
    private Vector2Int vector3_Center = new Vector2Int(0, 0);
    private const int int_ShadowDrawDistance_X = 20;
    private const int int_ShadowDrawDistance_Y = 10;
    #endregion
    #region 阴影设置
    private string[] sortingLayerNames = new string[] { "Default", "UnderGround", "Liquid", "Ground", "ShadowCatch" };
    private int[] shadowTargetLayer = new int[] { 0, 0, 0, 0, 0 };

    static FieldInfo shapeField =
        typeof(ShadowCaster2D).GetField("m_ShapePath",
        BindingFlags.NonPublic | BindingFlags.Instance);
    static MethodInfo onEnableMethod =
        typeof(ShadowCaster2D).GetMethod("OnEnable",
            BindingFlags.NonPublic | BindingFlags.Instance);
    private static FieldInfo hashField =
        typeof(ShadowCaster2D).GetField("m_ShapePathHash",
        BindingFlags.NonPublic | BindingFlags.Instance);
    private static FieldInfo targetLayerField =
        typeof(ShadowCaster2D).GetField("m_ApplyToSortingLayers",
        BindingFlags.NonPublic | BindingFlags.Instance);
    #endregion


    #region Polygons
    public void AddPolygons(Vector2Int pos, PolygonCollider2D col)
    {
        if (!set_ColliderRecorded.Contains(pos)) set_ColliderRecorded.Add(pos);
        dic_ColliderRecorded[pos] = col;
        bool_Dirty = CheckPolygonsRange(pos);
    }
    public void RemovePolygons(Vector2Int pos)
    {
        set_ColliderRecorded.Remove(pos);
        dic_ColliderRecorded.Remove(pos);
        bool_Dirty = CheckPolygonsRange(pos);
    }
    public void UpdateCenter(Vector2Int pos)
    {
        vector3_Center = pos;
        bool_Dirty = true;
    }
    public bool CheckPolygonsRange(Vector2Int pos)
    {
        if (pos.x > vector3_Center.x - int_ShadowDrawDistance_X && pos.x < vector3_Center.x + int_ShadowDrawDistance_X)
        {
            if (pos.y > vector3_Center.y - int_ShadowDrawDistance_Y && pos.y < vector3_Center.y + int_ShadowDrawDistance_Y)
            {
                return true;
            }
        }
        return false;
    }
    /// <summary>
    /// 准备需要的Polygons
    /// </summary>
    /// <returns></returns>
    private List<List<Vector2>> PreparePolygons()
    {
        var results = new List<List<Vector2>>();
        for (int x = vector3_Center.x - int_ShadowDrawDistance_X; x < vector3_Center.x + int_ShadowDrawDistance_X; x++)
        {
            for (int y = vector3_Center.y - int_ShadowDrawDistance_Y; y < vector3_Center.y + int_ShadowDrawDistance_Y; y++)
            {
                var gridPos = new Vector2Int(x, y);
                if (dic_ColliderRecorded.TryGetValue(gridPos, out var col) && col != null)
                {
                    Vector2 offset = col.transform.position;
                    for (int i = 0; i < col.pathCount; i++)
                    {
                        var path = new List<Vector2>();
                        Vector2[] points = col.GetPath(i);
                        for (int j = 0; j < points.Length; j++) path.Add(points[j] + offset);
                        results.Add(path);
                    }
                }
            }
        }
        return results;
    }

    #endregion
    #region 合并Polygons
    private List<Vector3[]> DoMergePolygons(List<List<Vector2>> inputs)

    {

        ClipperD clipper = new ClipperD();

        foreach (var poly in inputs)

        {

            PathD path = new PathD(poly.Count);

            foreach (var p in poly) path.Add(new PointD(p.x, p.y));

            clipper.AddSubject(path);

        }





        // 1. 创建 PolyTreeD

        PolyTreeD polyTree = new PolyTreeD();

        clipper.Execute(Clipper2Lib.ClipType.Union, FillRule.NonZero, polyTree);



        var result = new List<Vector3[]>();



        // 2. 遍历顶层节点 (外壳)

        // 注意：显式指定 PolyPathD 类型，避免编译器将其识别为 object

        foreach (PolyPathD outerNode in polyTree)

        {

            // 使用 .Polygon 获取路径数据

            PathD outerPath = Clipper.SimplifyPath(outerNode.Polygon, 0.2);

            if (Math.Abs(Clipper.Area(outerPath)) < 0.1) continue;



            // 3. 检查是否有孔洞 (子节点)

            if (outerNode.Count > 0)

            {

                List<PathD> holes = new List<PathD>();

                // 同样显式指定孔洞节点的类型

                foreach (PolyPathD holeNode in outerNode)

                {

                    PathD simplifiedHole = Clipper.SimplifyPath(holeNode.Polygon, 0.2);

                    if (Math.Abs(Clipper.Area(simplifiedHole)) > 0.1)

                    {

                        holes.Add(simplifiedHole);

                    }

                }

                // 缝合外壳与孔洞

                result.Add(CreateBridgePathNew(outerPath, holes));

            }

            else

            {

                result.Add(ConvertToVector3Array(outerPath));

            }

        }

        return result;

    }
    private List<Vector3[]> DoMergePolygonsNew(List<List<Vector2>> inputs)
    {
        if (inputs == null || inputs.Count == 0) return new List<Vector3[]>();

        // 1. 将输入转换为 PathsD
        PathsD subjectPaths = new PathsD();
        foreach (var poly in inputs)
        {
            PathD path = new PathD(poly.Count);
            foreach (var p in poly) path.Add(new PointD(p.x, p.y));
            subjectPaths.Add(path);
        }

        // 膨胀：让角与角之间产生重叠 (0.05是一个平衡值，足以抵消浮点误差)
        PathsD inflated = Clipper.InflatePaths(subjectPaths, 0.05, Clipper2Lib.JoinType.Miter, Clipper2Lib.EndType.Polygon);

        // 合并：在膨胀状态下执行 Union
        PathsD unionPaths = Clipper.Union(inflated, FillRule.NonZero);

        // 收缩：缩回原始尺寸
        PathsD finalPaths = Clipper.InflatePaths(unionPaths, -0.05, Clipper2Lib.JoinType.Miter, Clipper2Lib.EndType.Polygon);
        // --------------------------------
        // 利用最终路径生成 PolyTree 以处理孔洞
        ClipperD clipper = new ClipperD();
        clipper.AddSubject(finalPaths);

        PolyTreeD polyTree = new PolyTreeD();
        clipper.Execute(Clipper2Lib.ClipType.Union, FillRule.NonZero, polyTree);

        var result = new List<Vector3[]>();

        // 遍历顶层节点 (外壳)
        // 注意：显式指定 PolyPathD 类型，避免编译器将其识别为 object
        foreach (PolyPathD outerNode in polyTree)
        {
            // 使用 .Polygon 获取路径数据
            PathD outerPath = Clipper.SimplifyPath(outerNode.Polygon, 0.2);
            if (Math.Abs(Clipper.Area(outerPath)) < 0.1) continue;

            // 3. 检查是否有孔洞 (子节点)
            if (outerNode.Count > 0)
            {
                List<PathD> holes = new List<PathD>();
                // 同样显式指定孔洞节点的类型
                foreach (PolyPathD holeNode in outerNode)
                {
                    PathD simplifiedHole = Clipper.SimplifyPath(holeNode.Polygon, 0.2);
                    if (Math.Abs(Clipper.Area(simplifiedHole)) > 0.1)
                    {
                        holes.Add(simplifiedHole);
                    }
                }
                // 缝合外壳与孔洞
                result.Add(CreateBridgePathNew(outerPath, holes));
            }
            else
            {
                result.Add(ConvertToVector3Array(outerPath));
            }
        }
        return result;
    }
    private Vector3[] CreateBridgePathNew(PathD outer, List<PathD> holes)
    {
        // 将初始外壳转换为可动态修改的列表
        List<Vector3> currentPolygon = ConvertToVector3List(outer);

        // 依次处理每个孔洞，将其“缝合”进主多边形
        foreach (var hole in holes)
        {
            List<Vector3> holePath = ConvertToVector3List(hole);

            int bestOuterIdx = 0;
            int bestHoleIdx = 0;
            float minDistanceSqr = float.MaxValue;

            // 寻找“最薄的墙”：遍历外壳和孔洞的所有点，找最近的一对
            for (int i = 0; i < currentPolygon.Count; i++)
            {
                for (int j = 0; j < holePath.Count; j++)
                {
                    float distSqr = (currentPolygon[i] - holePath[j]).sqrMagnitude;
                    if (distSqr < minDistanceSqr)
                    {
                        minDistanceSqr = distSqr;
                        bestOuterIdx = i;
                        bestHoleIdx = j;
                    }
                }
            }

            // 执行缝合：在外壳最近点处插入孔洞路径
            // 路径顺序：外壳点 -> 孔洞点[n] -> ... -> 孔洞点[0..n] -> 回到外壳点
            List<Vector3> newBridge = new List<Vector3>();

            // 1. 插入到连接点之前的外壳部分
            for (int i = 0; i <= bestOuterIdx; i++) newBridge.Add(currentPolygon[i]);

            // 2. 插入孔洞路径（从最近点开始绕一圈回到最近点）
            for (int i = 0; i < holePath.Count; i++)
            {
                int idx = (bestHoleIdx + i) % holePath.Count;
                newBridge.Add(holePath[idx]);
            }
            newBridge.Add(holePath[bestHoleIdx]); // 闭合孔洞

            // 3. 回到外壳连接点，并完成剩余的外壳部分
            for (int i = bestOuterIdx; i < currentPolygon.Count; i++) newBridge.Add(currentPolygon[i]);

            currentPolygon = newBridge;
        }

        return currentPolygon.ToArray();
    }
    private Vector3[] ConvertToVector3Array(PathD path)
    {
        Vector3[] pts = new Vector3[path.Count];
        for (int i = 0; i < path.Count; i++)
            pts[i] = new Vector3((float)path[i].x, (float)path[i].y, 0);
        return pts;
    }
    private List<Vector3> ConvertToVector3List(PathD path)
    {
        List<Vector3> pts = new List<Vector3>(path.Count);
        foreach (var p in path) pts.Add(new Vector3((float)p.x, (float)p.y, 0));
        return pts;
    }

    #endregion
    #region 生成ShadowCaster
    private IEnumerator SyncShadowsCoroutine()
    {
        bool_isProcessing = true;
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
            bool_isProcessing = false;
            syncCoroutine = null;
        }
    }
    public void StopAllSyncOperations()
    {
        if (syncCoroutine != null)
        {
            StopCoroutine(syncCoroutine);
            syncCoroutine = null;
        }
        bool_isProcessing = false;
    }

    /// <summary>
    /// 分帧创建新的阴影
    /// </summary>
    private IEnumerator CreateNewShadowsCoroutine()
    {
        Transform temp = bool_useShadowGroup0 ? trans_ShadowGroup_0 : trans_ShadowGroup_1;

        // 1. 采集数据 (必须在主线程)
        List<List<Vector2>> inputPolygons = PreparePolygons();

        // 2. 异步计算 (多线程执行，不会卡顿)
        Task<List<Vector3[]>> mergeTask = Task.Run(() => DoMergePolygons(inputPolygons));
        while (!mergeTask.IsCompleted) yield return null;
        List<Vector3[]> mergedResults = mergeTask.Result;
        int updatedThisFrame = 0;
        for (int i = 0; i < mergedResults.Count; i++)
        {
            // 创建阴影
            CreateSingleShadowCaster(i, mergedResults[i], mergedResults[i].Length, temp);
            updatedThisFrame++;
            // 控制每帧创建的数量
            if (updatedThisFrame >= int_MaxShadowsPerFrame)
            {
                updatedThisFrame = 0;
                yield return null; // 等待一帧
            }
        }
        // 强制等待一帧，确保所有对象都创建完成
        yield return null;
    }
    /// <summary>
    /// 创建单个阴影投射器
    /// </summary>
    private void CreateSingleShadowCaster(int index, Vector3[] pathPoints, int pointCount, Transform shadowGroup)
    {
        if (shadowGroup == null)
            return;

        GameObject shadowObj = new GameObject($"ShadowPath_{index}");
        shadowObj.transform.SetParent(shadowGroup, false);

        ShadowCaster2D caster = shadowObj.AddComponent<ShadowCaster2D>();
        caster.useRendererSilhouette = false;
        caster.selfShadows = bool_SelfShadows;


        // 转换顶点数据
        Vector3[] pathPointsV3 = new Vector3[pointCount];
        for (int j = 0; j < pointCount; j++)
        {
            pathPointsV3[j] = pathPoints[j];
        }
        // 设置ShadowCaster2D的路径
        shapeField?.SetValue(caster, pathPointsV3);
        hashField?.SetValue(caster, UnityEngine.Random.Range(1, 99999));
        targetLayerField?.SetValue(caster, shadowTargetLayer);
        // 激活ShadowCaster2D
        onEnableMethod?.Invoke(caster, null);
    }
    private IEnumerator ClearOldShadowsCoroutine()
    {
        Transform temp = bool_useShadowGroup0 ? trans_ShadowGroup_1 : trans_ShadowGroup_0;
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
    #endregion
    #region 调试
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan; // 设置调试线条颜色

        // 检查两个组里的所有 ShadowCaster2D
        DrawGroupGizmos(trans_ShadowGroup_0);
        DrawGroupGizmos(trans_ShadowGroup_1);
    }
    private void DrawGroupGizmos(Transform group)
    {
        foreach (Transform child in group)
        {
            ShadowCaster2D caster = child.GetComponent<ShadowCaster2D>();
            if (caster == null) continue;

            // 通过反射获取你设置进去的路径数据
            Vector3[] path = (Vector3[])shapeField?.GetValue(caster);

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
