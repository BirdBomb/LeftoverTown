using Fusion.Addons.Physics;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSimulation : MonoBehaviour
{
    [Header("网络根节点")]
    public Transform transform_NetRoot;
    [Header("移动预测启用")]
    public bool bool_On = true;
    private CircleCollider2D circleCollider2D;
    /// <summary>
    /// 是否正在模拟预测
    /// </summary>
    private bool bool_Simulation = false;
    /// <summary>
    /// 移动预测最大距离
    /// </summary>
    private float float_SimulationDistance = 1f;
    /// <summary>
    /// 移动预测速度倍数
    /// </summary>
    private float float_SimulationSpeedOffset = 0.5f;
    /// <summary>
    /// 模拟预测时间
    /// </summary>
    private float float_SimlationTime;
    /// <summary>
    /// 模拟位置
    /// </summary>
    private Vector2 vector2_SimulationPos;
    /// <summary>
    /// 模拟方向
    /// </summary>
    private Vector2 vector2_SimulationDir;
    /// <summary>
    /// 模拟速度
    /// </summary>
    private float float_SimulationSpeed;
    /// <summary>
    /// 实际方向
    /// </summary>
    private Vector2 vector2_NetDir;
    /// <summary>
    /// 实际速度
    /// </summary>
    private float float_NetSpeed;
    /// <summary>
    /// 实际位置(当前)
    /// </summary>
    private Vector2 vector2_NetPosCur;
    /// <summary>
    /// 实际位置
    /// </summary>
    private Vector2 vector2_NetPosLast;
    /// <summary>
    /// 进入碰撞
    /// </summary>
    private bool bool_InCollision = false;
    /// <summary>
    /// 离开碰撞
    /// </summary>
    private bool bool_OutCollision = false;
    /// <summary>
    /// 碰撞阻力
    /// 
    /// </summary>
    private float float_CollisionDrag = 0.6f;
    private void Awake()
    {
        circleCollider2D = GetComponent<CircleCollider2D>();
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (bool_Simulation && bool_On && !collision.isTrigger)
        {
            bool_OutCollision = false;
            bool_InCollision = true;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (bool_Simulation && bool_On && !collision.isTrigger)
        {
            bool_OutCollision = false; 
            bool_InCollision = true;
            // 计算排斥方向
            Vector2 direction = (Vector2)transform.position - collision.ClosestPoint(transform.position)/*collision.transform.position*/;
            // 计算排斥距离
            float distance = direction.magnitude;

            //我的尺寸
            float mySize = GetColliderSize(circleCollider2D);
            //碰撞物尺寸
            float otherSize = GetColliderSize(collision);

            // 计算应该保持的最小距离
            float minDistance = (mySize + otherSize) * 0.5f;

            if (distance < minDistance && distance > 0)
            {
                // 计算穿透深度
                float penetration = minDistance - distance;
                Vector2 separation = direction.normalized * penetration * 0.025f;

                // 移动物体分离
                vector2_SimulationPos += separation;
                transform.position = vector2_SimulationPos;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (bool_Simulation && bool_On && !collision.isTrigger)
        {
            bool_OutCollision = true;
        }
    }
    private float GetColliderSize(Collider2D collider)
    {
        if (collider is CircleCollider2D circle)
            return circle.radius * 2;
        else if (collider is BoxCollider2D box)
            return Mathf.Max(box.size.x, box.size.y);
        else
            return 1f; // 默认值
    }
    /// <summary>
    /// 模拟
    /// </summary>
    /// <param name="dt"></param>
    private void Simulation(float dt)
    {
        CheckNetState(dt);

        if (vector2_SimulationDir != Vector2.zero)
        {
            /*本地端移动*/
            if (Vector2.Distance(vector2_NetDir, vector2_SimulationDir) < 0.1f)
            {
                /*本地端与网络端移动方向一致*/
                if (bool_InCollision)
                {
                    vector2_SimulationPos += vector2_SimulationDir * float_SimulationSpeed * float_CollisionDrag * dt;
                }
                else
                {
                    vector2_SimulationPos += vector2_SimulationDir * float_SimulationSpeed * dt;
                }
            }
            else
            {
                /*本地端与网络端移动方向不一致*/
                if (bool_InCollision)
                {
                    vector2_SimulationPos += vector2_SimulationDir * float_SimulationSpeed * float_CollisionDrag * dt;
                }
                else
                {
                    vector2_SimulationPos += vector2_SimulationDir * float_SimulationSpeed * dt;
                }

                if (float_SimlationTime <= 1)
                {
                    /*开始增加模拟时间*/
                    float_SimlationTime += dt;
                }
                /*别差太远*/
                if (Vector2.Distance(vector2_SimulationPos, transform_NetRoot.position) > float_SimulationDistance)
                {
                    /*超过最大距离,复位*/
                    Sync();
                }
                else
                {
                    /*没超过最大距离*/
                    transform.position = vector2_SimulationPos;
                }
            }
        }
        else
        {
            /*本地端静止*/
            if (float_SimlationTime >= 0)
            {
                /*开始减少模拟时间*/
                float_SimlationTime -= dt;
            }
            if (Vector2.Distance(vector2_NetDir, vector2_SimulationDir) < 0.1f)
            {
                /*本地端与网络端都静止*/
                if (float_SimlationTime < 0)
                {
                    /*模拟结束,复位*/
                    Sync();
                }
            }
            else
            {
                /*本地端静止与网络端仍在移动*/
                transform.position = vector2_SimulationPos;
            }
        }
        if (bool_OutCollision)
        {
            bool_OutCollision = false;
            bool_InCollision = false;
        }
    }
    /// <summary>
    /// 对齐网络状态
    /// </summary>
    private void Sync()
    {
        transform.position = transform_NetRoot.position;
        vector2_SimulationPos = transform.position;
        float_SimlationTime = 0;
    }
    /// <summary>
    /// 检查网络状态
    /// </summary>
    /// <param name="dt"></param>
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
