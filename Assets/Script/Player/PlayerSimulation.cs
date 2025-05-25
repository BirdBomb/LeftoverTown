using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSimulation : MonoBehaviour
{
    [Header("网络根节点")]
    public Transform transform_NetRoot;
    [Header("移动预测启用")]
    public bool bool_On = true;
    private bool bool_Simulation;
    [Header("移动预测最大距离")]
    public float float_SimulationDistance = 1f;
    [Header("移动预测速度倍数")]
    public float float_SimulationSpeedOffset = 0.5f;
    /// <summary>
    /// 模拟预测时间
    /// </summary>
    private float float_SimlationTime;
    /// <summary>
    /// 模拟位置
    /// </summary>
    public Vector2 vector2_SimulationPos;
    /// <summary>
    /// 模拟方向
    /// </summary>
    public Vector2 vector2_SimulationDir;
    /// <summary>
    /// 模拟速度
    /// </summary>
    public float float_SimulationSpeed;
    /// <summary>
    /// 实际方向
    /// </summary>
    public Vector2 vector2_NetDir;
    /// <summary>
    /// 实际速度
    /// </summary>
    public float float_NetSpeed;
    /// <summary>
    /// 实际位置(当前)
    /// </summary>
    private Vector2 vector2_NetPosCur;
    /// <summary>
    /// 实际位置
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
            /*本地端移动*/
            if (Vector2.Distance(vector2_NetDir, vector2_SimulationDir) < 0.1f)
            {
                /*本地端与网络端移动方向一致*/
                vector2_SimulationPos += vector2_SimulationDir * float_SimulationSpeed * dt;
            }
            else
            {
                float_SimlationTime += dt;
                /*本地端与网络端移动方向不一致*/
                vector2_SimulationPos += vector2_SimulationDir * float_SimulationSpeed * dt;
                /*别差太远*/
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
            /*本地端静止*/
            if (Vector2.Distance(vector2_NetDir, vector2_SimulationDir) < 0.1f)
            {
                /*本地端与网络端都静止*/
                /*复位*/
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
                /*本地端静止与网络端仍在移动*/
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
