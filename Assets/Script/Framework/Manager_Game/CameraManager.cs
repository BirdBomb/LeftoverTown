using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class CameraManager : SingleTon<CameraManager>, ISingleTon
{
    [Header("�����")]
    public Transform tran_Camera;
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
    public void Init()
    {
        
    }
    public void Shake(float time, float strength)
    {
        tran_Camera.DOKill();
        tran_Camera.localPosition = Vector3.zero;
        tran_Camera.DOShakePosition(time, strength);
    }
    void LateUpdate()
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
}
