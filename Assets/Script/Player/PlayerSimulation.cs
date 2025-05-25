using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSimulation : MonoBehaviour
{
    [Header("������ڵ�")]
    public Transform transform_NetRoot;
    [Header("�ƶ�Ԥ������")]
    public bool bool_On = true;
    private bool bool_Simulation;
    [Header("�ƶ�Ԥ��������")]
    public float float_SimulationDistance = 1f;
    [Header("�ƶ�Ԥ���ٶȱ���")]
    public float float_SimulationSpeedOffset = 0.5f;
    /// <summary>
    /// ģ��Ԥ��ʱ��
    /// </summary>
    private float float_SimlationTime;
    /// <summary>
    /// ģ��λ��
    /// </summary>
    public Vector2 vector2_SimulationPos;
    /// <summary>
    /// ģ�ⷽ��
    /// </summary>
    public Vector2 vector2_SimulationDir;
    /// <summary>
    /// ģ���ٶ�
    /// </summary>
    public float float_SimulationSpeed;
    /// <summary>
    /// ʵ�ʷ���
    /// </summary>
    public Vector2 vector2_NetDir;
    /// <summary>
    /// ʵ���ٶ�
    /// </summary>
    public float float_NetSpeed;
    /// <summary>
    /// ʵ��λ��(��ǰ)
    /// </summary>
    private Vector2 vector2_NetPosCur;
    /// <summary>
    /// ʵ��λ��
    /// </summary>
    private Vector2 vector2_NetPosLast;
    private void Update()
    {
        
    }
    private void LateUpdate()
    {
        if (bool_Simulation && bool_On)
        {
            Simulation(Time.deltaTime);
        }
    }
    public void SetSimulation(Vector2 dir, float speed)
    {
        bool_Simulation = true;
        vector2_SimulationDir = dir;
        float_SimulationSpeed = speed * float_SimulationSpeedOffset;
    }
    private void Simulation(float dt)
    {
        CheckNetState(dt);

        if (vector2_SimulationDir != Vector2.zero)
        {
            /*���ض��ƶ�*/
            if (Vector2.Distance(vector2_NetDir, vector2_SimulationDir) < 0.1f)
            {
                /*���ض���������ƶ�����һ��*/
                vector2_SimulationPos += vector2_SimulationDir * float_SimulationSpeed * dt;
            }
            else
            {
                float_SimlationTime += dt;
                /*���ض���������ƶ�����һ��*/
                vector2_SimulationPos += vector2_SimulationDir * float_SimulationSpeed * dt;
                /*���̫Զ*/
                if (Vector2.Distance(vector2_SimulationPos, transform_NetRoot.position) > float_SimulationDistance)
                {
                    transform.position = transform_NetRoot.position + ((Vector3)vector2_SimulationPos - transform_NetRoot.position).normalized * float_SimulationDistance;
                    //vector2_SimulationPos = transform.position;
                }
                else
                {
                    transform.position = vector2_SimulationPos;
                }
            }
        }
        else
        {
            if (float_SimlationTime > 0)
            {
                float_SimlationTime -= dt;
            }
            /*���ض˾�ֹ*/
            if (Vector2.Distance(vector2_NetDir, vector2_SimulationDir) < 0.1f)
            {
                /*���ض�������˶���ֹ*/
                /*��λ*/
                if (float_SimlationTime <= 0)
                {
                    float_SimlationTime = 0;
                    if (transform.localPosition.magnitude > 0.05f)
                    {
                        transform.localPosition -= transform.localPosition.normalized * float_SimulationSpeed * dt;
                    }
                    vector2_SimulationPos = transform.position;
                }
            }
            else
            {
                /*���ض˾�ֹ������������ƶ�*/
                if (transform.localPosition.magnitude > 0.05f)
                {
                    if (Vector2.Distance(vector2_SimulationPos, transform_NetRoot.position) > float_SimulationDistance)
                    {
                        transform.position = transform_NetRoot.position + ((Vector3)vector2_SimulationPos - transform_NetRoot.position).normalized * float_SimulationDistance;
                    }
                    else
                    {
                        transform.position = vector2_SimulationPos;
                    }
                }

            }
        }
    }
    public void CheckNetState(float dt)
    {
        vector2_NetPosCur = transform_NetRoot.position;
        float distance = Vector2.Distance(vector2_NetPosLast, vector2_NetPosCur);
        float_NetSpeed = distance / dt;
        if (float_NetSpeed < 1)
        {
            vector2_NetDir = Vector2.zero;
        }
        else
        {
            vector2_NetDir = (vector2_NetPosCur - vector2_NetPosLast).normalized;
        }
        vector2_NetPosLast = vector2_NetPosCur;
    }
}
