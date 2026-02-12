using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSceneFollow : MonoBehaviour
{
    [Header("跟随设置")]
    [SerializeField] private float smoothTime = 0.3f;     // 平滑时间
    [SerializeField] private float maxSpeed = 10f;        // 最大跟随速度
    [SerializeField] private float maxOffset = 3f;        // 最大偏移量

    [Header("鼠标设置")]
    [SerializeField] private float mouseSensitivity = 1.5f;
    [SerializeField] private bool useScreenPercentage = true; // 使用屏幕百分比
    [SerializeField][Range(0.1f, 0.5f)] private float screenEdgePercent = 0.3f; // 屏幕边缘百分比

    [Header("边界限制")]
    [SerializeField] private bool useBoundary = true;
    [SerializeField] private Vector2 boundaryMin = new Vector2(-10, -5);
    [SerializeField] private Vector2 boundaryMax = new Vector2(10, 5);

    private Vector3 velocity = Vector3.zero;  // 当前速度
    private Vector3 initialPosition;          // 初始位置
    private Vector3 targetPosition;           // 目标位置

    void Start()
    {
        initialPosition = transform.position;
        targetPosition = initialPosition;

        // 鼠标设置
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    void Update()
    {
        UpdateTargetPosition();
        SmoothFollow();
    }

    void UpdateTargetPosition()
    {
        // 方法A：基于鼠标移动量
        if (!useScreenPercentage)
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            Vector3 mouseOffset = new Vector3(mouseX, mouseY, 0) * mouseSensitivity;
            targetPosition += mouseOffset;
        }
        // 方法B：基于鼠标在屏幕上的位置（类似RTS游戏）
        else
        {
            Vector3 mouseViewport = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            Vector3 mouseOffset = Vector3.zero;

            // 检查屏幕边缘
            if (mouseViewport.x < screenEdgePercent)        // 左边缘
                mouseOffset.x = -(1 - mouseViewport.x / screenEdgePercent);
            else if (mouseViewport.x > 1 - screenEdgePercent) // 右边缘
                mouseOffset.x = (mouseViewport.x - (1 - screenEdgePercent)) / screenEdgePercent;

            if (mouseViewport.y < screenEdgePercent)        // 下边缘
                mouseOffset.y = -(1 - mouseViewport.y / screenEdgePercent);
            else if (mouseViewport.y > 1 - screenEdgePercent) // 上边缘
                mouseOffset.y = (mouseViewport.y - (1 - screenEdgePercent)) / screenEdgePercent;

            targetPosition = initialPosition + mouseOffset * maxOffset;
        }

        // 应用最大偏移限制
        ApplyMaxOffset();

        // 应用边界限制（如果需要）
        if (useBoundary)
        {
            ApplyBoundary();
        }
    }

    void SmoothFollow()
    {
        // 使用SmoothDamp实现更自然的阻尼效果
        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPosition,
            ref velocity,
            smoothTime,
            maxSpeed
        );
    }

    void ApplyMaxOffset()
    {
        // 限制在最大偏移范围内
        Vector3 offset = targetPosition - initialPosition;

        if (offset.magnitude > maxOffset)
        {
            offset = offset.normalized * maxOffset;
            targetPosition = initialPosition + offset;
        }
    }

    void ApplyBoundary()
    {
        // 限制在设定的边界内
        targetPosition.x = Mathf.Clamp(targetPosition.x, boundaryMin.x, boundaryMax.x);
        targetPosition.y = Mathf.Clamp(targetPosition.y, boundaryMin.y, boundaryMax.y);
    }

    void OnDisable()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // 调试用：在Scene视图中显示边界
    void OnDrawGizmosSelected()
    {
        if (useBoundary)
        {
            Gizmos.color = Color.yellow;
            Vector3 center = new Vector3(
                (boundaryMin.x + boundaryMax.x) * 0.5f,
                (boundaryMin.y + boundaryMax.y) * 0.5f,
                transform.position.z
            );
            Vector3 size = new Vector3(
                boundaryMax.x - boundaryMin.x,
                boundaryMax.y - boundaryMin.y,
                0.1f
            );
            Gizmos.DrawWireCube(center, size);
        }
    }
}
