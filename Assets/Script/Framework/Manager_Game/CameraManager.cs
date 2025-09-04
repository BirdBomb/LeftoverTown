using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Unity.VisualScripting;

public class CameraManager : SingleTon<CameraManager>, ISingleTon
{
    [Header("�������")]
    public Camera camera_Main;
    [Header("�������λ��")]
    public Transform tran_Camera;
    [Header("���������")]
    public Camera camera_UnderGround;
    [Header("���������λ��")]
    public Transform tran_UnderGround;
    [Header("�������������")]
    public RenderTexture renderTexture_UnderGround;
    [Header("������ͼ")]
    public SpriteRenderer spriteRenderer_LightShake;
    [Header("����Ŀ��")]
    public Transform tran_target = null;
    [Header("�����ٶ�")]
    public float float_moveSpeed;
    [Header("����λ��")]
    public Vector3 vector3_offset;
    [HideInInspector]
    public Vector3 vector3_ref;
    [HideInInspector]
    public Vector3 vector3_targetPos;
    [HideInInspector]
    public Vector3 vector3_curPos;
    /// <summary>
    /// ��ǰ��Ļ���
    /// </summary>
    private int currentScreenWidth;
    /// <summary>
    /// ��ǰ��Ļ�߶�
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
            // ����Ŀ��λ��
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
        UpdateRenderTexture(); // ��Ļ�仯ʱ�����´���RT
        SyncCameras();
    }
    /// <summary>
    /// ͬ�����
    /// </summary>
    void SyncCameras()
    {
        // 1. ͬ����Ұ (FOV)
        // �����͸�������Perspective��������Ҫ
        camera_UnderGround.fieldOfView = camera_Main.fieldOfView;

        // 2. ͬ����/Զ�ü�ƽ��
        camera_UnderGround.nearClipPlane = camera_Main.nearClipPlane;
        camera_UnderGround.farClipPlane = camera_Main.farClipPlane;

        // 3. ͬ��������С (Orthographic Size)
        // ���������������ģʽ��Orthographic�����������Ҫ
        camera_UnderGround.orthographic = camera_Main.orthographic;
        if (camera_UnderGround.orthographic)
        {
            camera_UnderGround.orthographicSize = camera_Main.orthographicSize;
        }

        // 4. ͬ���ӿھ��� (Viewport Rect)
        // ȷ����Ⱦ����Ļ�ı���һ��
        camera_UnderGround.rect = camera_Main.rect;

    }
    /// <summary>
    /// ��������ߴ�                                                                                                                                                          
    /// </summary>
    void UpdateRenderTexture()
    {
        if (renderTexture_UnderGround != null)
        {
            // �ͷ�ԭ�е�RT
            renderTexture_UnderGround.Release();
            // �����µ�RT���ߴ��������Ļ��1/2��1/4���������
            renderTexture_UnderGround.width = Mathf.Max(1, currentScreenWidth );
            renderTexture_UnderGround.height = Mathf.Max(1, currentScreenHeight );
        }
    }
}
