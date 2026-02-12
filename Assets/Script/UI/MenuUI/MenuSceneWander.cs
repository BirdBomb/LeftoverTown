using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSceneWander : MonoBehaviour
{
    [Header("漫游设置")]
    [SerializeField] private float wanderSpeed = 2f;           // 移动速度
    [SerializeField] private float areaRadius = 5f;            // 漫游区域半径
    [SerializeField] private float changeTargetDistance = 0.5f; // 切换目标的距离阈值

    [Header("随机目标设置")]
    [SerializeField] private float minWaitTime = 1f;           // 最小停留时间
    [SerializeField] private float maxWaitTime = 3f;           // 最大停留时间
    [SerializeField] private float minTargetDistance = 1f;     // 最小目标距离
    [SerializeField] private float maxTargetDistance = 3f;     // 最大目标距离

    [Header("平滑设置")]
    [SerializeField] private bool useSmoothMovement = true;    // 使用平滑移动
    [SerializeField] private float smoothTime = 0.5f;          // 平滑时间

    private Vector3 originalPosition;  // 原始位置（中心点）
    private Vector3 targetPosition;    // 当前目标位置
    private Vector3 velocity;          // 用于SmoothDamp的速度缓存
    private float waitTimer;           // 停留计时器
    private bool isWaiting;            // 是否在等待

    void Start()
    {
        originalPosition = transform.position;
        GenerateNewTarget();

        // 初始不等待，直接开始移动
        isWaiting = false;
        waitTimer = 0f;
    }

    void Update()
    {
        if (isWaiting)
        {
            // 停留等待
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0f)
            {
                isWaiting = false;
                GenerateNewTarget();
            }
        }
        else
        {
            // 向目标移动
            MoveToTarget();

            // 检查是否到达目标
            if (Vector3.Distance(transform.position, targetPosition) < changeTargetDistance)
            {
                StartWaiting();
            }
        }
    }

    void MoveToTarget()
    {
        if (useSmoothMovement)
        {
            // 使用平滑阻尼移动
            transform.position = Vector3.SmoothDamp(
                transform.position,
                targetPosition,
                ref velocity,
                smoothTime,
                wanderSpeed
            );
        }
        else
        {
            // 使用线性插值移动
            transform.position = Vector3.Lerp(
                transform.position,
                targetPosition,
                wanderSpeed * Time.deltaTime
            );
        }
    }

    void GenerateNewTarget()
    {
        // 在圆形区域内生成随机目标
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        float randomDistance = Random.Range(minTargetDistance, maxTargetDistance);

        // 限制在区域内
        randomDistance = Mathf.Min(randomDistance, areaRadius);

        // 计算目标位置（只使用XY轴）
        targetPosition = originalPosition +
                        new Vector3(randomDirection.x, randomDirection.y, 0) * randomDistance;

        // 重置速度缓存
        velocity = Vector3.zero;
    }

    void StartWaiting()
    {
        isWaiting = true;
        waitTimer = Random.Range(minWaitTime, maxWaitTime);
    }

    // 在Scene视图中显示漫游区域
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(originalPosition, areaRadius);

        // 绘制当前目标
        if (Application.isPlaying)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(targetPosition, 0.2f);
            Gizmos.DrawLine(transform.position, targetPosition);
        }
    }

    // 公共方法：重新设置漫游中心
    public void SetCenterPosition(Vector3 newCenter)
    {
        originalPosition = newCenter;
    }

    // 公共方法：立即切换到新目标
    public void ForceNewTarget()
    {
        GenerateNewTarget();
        isWaiting = false;
    }
}
