using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Unity.VisualScripting;

public class CameraManager : SingleTon<CameraManager>, ISingleTon
{
    [Header("主摄像机")]
    public Camera camera_Main;
    [Header("主摄像机位置")]
    public Transform tran_Camera;
    [Header("地下摄像机")]
    public Camera camera_UnderGround;
    [Header("地下摄像机位置")]
    public Transform tran_UnderGround;
    [Header("地下摄像机纹理")]
    public RenderTexture renderTexture_UnderGround;
    [Header("光线贴图")]
    public SpriteRenderer spriteRenderer_LightShake;
    [Header("跟踪目标")]
    public Transform tran_target = null;
    [Header("跟踪速度")]
    public float float_moveSpeed;
    [Header("跟踪位移")]
    public Vector3 vector3_offset;
    [HideInInspector]
    public Vector3 vector3_ref;
    [HideInInspector]
    public Vector3 vector3_targetPos;
    [HideInInspector]
    public Vector3 vector3_curPos;
    /// <summary>
    /// 当前屏幕宽度
    /// </summary>
    private int currentScreenWidth;
    /// <summary>
    /// 当前屏幕高度
    /// </summary>
    private int currentScreenHeight;
    private void Update()
    {
        if (currentScreenWidth != Screen.width || currentScreenHeight != Screen.height)
        {
            currentScreenWidth = Screen.width;
            currentScreenHeight = Screen.height;
            UpdateScreen();
        }
    }
    private void LateUpdate()
    {
        if (tran_target != null)
        {
            // 计算目标位置
            vector3_targetPos = tran_target.position + vector3_offset;
            vector3_curPos = Vector3.SmoothDamp(transform.position, vector3_targetPos, ref vector3_ref, float_moveSpeed);
            vector3_curPos.z = -10f;
            transform.position = vector3_curPos;
        }
    }

    public void Init()
    {
        LightBreath();
    }
    public void FollowTarget(Transform followTo)
    {
        tran_target = followTo;
    }
    public void Shake(float time, float strength)
    {
        tran_Camera.DOKill();
        tran_Camera.localPosition = Vector3.zero;
        tran_Camera.DOShakePosition(time, strength);
    }
    public void LightBreath()
    {
        spriteRenderer_LightShake.DOKill();
        spriteRenderer_LightShake.color = new Color(1, 1, 1, 0.4f);
        spriteRenderer_LightShake.DOFade(0.1f, 8f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
    }
    private void UpdateScreen()
    {
        UpdateRenderTexture(); // 屏幕变化时，重新创建RT
        SyncCameras();
    }
    /// <summary>
    /// 同步相机
    /// </summary>
    void SyncCameras()
    {
        // 1. 同步视野 (FOV)
        // 这对于透视相机（Perspective）至关重要
        camera_UnderGround.fieldOfView = camera_Main.fieldOfView;

        // 2. 同步近/远裁剪平面
        camera_UnderGround.nearClipPlane = camera_Main.nearClipPlane;
        camera_UnderGround.farClipPlane = camera_Main.farClipPlane;

        // 3. 同步正交大小 (Orthographic Size)
        // 如果你的相机是正交模式（Orthographic），这个很重要
        camera_UnderGround.orthographic = camera_Main.orthographic;
        if (camera_UnderGround.orthographic)
        {
            camera_UnderGround.orthographicSize = camera_Main.orthographicSize;
        }

        // 4. 同步视口矩形 (Viewport Rect)
        // 确保渲染到屏幕的比例一致
        camera_UnderGround.rect = camera_Main.rect;

    }
    /// <summary>
    /// 更新纹理尺寸                                                                                                                                                          
    /// </summary>
    void UpdateRenderTexture()
    {
        if (renderTexture_UnderGround != null)
        {
            // 释放原有的RT
            renderTexture_UnderGround.Release();
            // 创建新的RT，尺寸可以是屏幕的1/2或1/4以提高性能
            renderTexture_UnderGround.width = Mathf.Max(1, currentScreenWidth );
            renderTexture_UnderGround.height = Mathf.Max(1, currentScreenHeight );
        }
    }
}
