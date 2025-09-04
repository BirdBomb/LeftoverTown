using Fusion.Addons.Physics;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSimulation : MonoBehaviour
{
    [Header("������ڵ�")]
    public Transform transform_NetRoot;
    [Header("�ƶ�Ԥ������")]
    public bool bool_On = true;
    private CircleCollider2D circleCollider2D;
    /// <summary>
    /// �Ƿ�����ģ��Ԥ��
    /// </summary>
    private bool bool_Simulation = false;
    /// <summary>
    /// �ƶ�Ԥ��������
    /// </summary>
    private float float_SimulationDistance = 1f;
    /// <summary>
    /// �ƶ�Ԥ���ٶȱ���
    /// </summary>
    private float float_SimulationSpeedOffset = 0.5f;
    /// <summary>
    /// ģ��Ԥ��ʱ��
    /// </summary>
    private float float_SimlationTime;
    /// <summary>
    /// ģ��λ��
    /// </summary>
    private Vector2 vector2_SimulationPos;
    /// <summary>
    /// ģ�ⷽ��
    /// </summary>
    private Vector2 vector2_SimulationDir;
    /// <summary>
    /// ģ���ٶ�
    /// </summary>
    private float float_SimulationSpeed;
    /// <summary>
    /// ʵ�ʷ���
    /// </summary>
    private Vector2 vector2_NetDir;
    /// <summary>
    /// ʵ���ٶ�
    /// </summary>
    private float float_NetSpeed;
    /// <summary>
    /// ʵ��λ��(��ǰ)
    /// </summary>
    private Vector2 vector2_NetPosCur;
    /// <summary>
    /// ʵ��λ��
    /// </summary>
    private Vector2 vector2_NetPosLast;
    /// <summary>
    /// ������ײ
    /// </summary>
    private bool bool_InCollision = false;
    /// <summary>
    /// �뿪��ײ
    /// </summary>
    private bool bool_OutCollision = false;
    /// <summary>
    /// ��ײ����
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
            // �����ųⷽ��
            Vector2 direction = (Vector2)transform.position - collision.ClosestPoint(transform.position)/*collision.transform.position*/;
            // �����ų����
            float distance = direction.magnitude;

            //�ҵĳߴ�
            float mySize = GetColliderSize(circleCollider2D);
            //��ײ��ߴ�
            float otherSize = GetColliderSize(collision);

            // ����Ӧ�ñ��ֵ���С����
            float minDistance = (mySize + otherSize) * 0.5f;

            if (distance < minDistance && distance > 0)
            {
                // ���㴩͸���
                float penetration = minDistance - distance;
                Vector2 separation = direction.normalized * penetration * 0.025f;

                // �ƶ��������
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
            return 1f; // Ĭ��ֵ
    }
    /// <summary>
    /// ģ��
    /// </summary>
    /// <param name="dt"></param>
    private void Simulation(float dt)
    {
        CheckNetState(dt);

        if (vector2_SimulationDir != Vector2.zero)
        {
            /*���ض��ƶ�*/
            if (Vector2.Distance(vector2_NetDir, vector2_SimulationDir) < 0.1f)
            {
                /*���ض���������ƶ�����һ��*/
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
                /*���ض���������ƶ�����һ��*/
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
                    /*��ʼ����ģ��ʱ��*/
                    float_SimlationTime += dt;
                }
                /*���̫Զ*/
                if (Vector2.Distance(vector2_SimulationPos, transform_NetRoot.position) > float_SimulationDistance)
                {
                    /*����������,��λ*/
                    Sync();
                }
                else
                {
                    /*û����������*/
                    transform.position = vector2_SimulationPos;
                }
            }
        }
        else
        {
            /*���ض˾�ֹ*/
            if (float_SimlationTime >= 0)
            {
                /*��ʼ����ģ��ʱ��*/
                float_SimlationTime -= dt;
            }
            if (Vector2.Distance(vector2_NetDir, vector2_SimulationDir) < 0.1f)
            {
                /*���ض�������˶���ֹ*/
                if (float_SimlationTime < 0)
                {
                    /*ģ�����,��λ*/
                    Sync();
                }
            }
            else
            {
                /*���ض˾�ֹ������������ƶ�*/
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
    /// ��������״̬
    /// </summary>
    private void Sync()
    {
        transform.position = transform_NetRoot.position;
        vector2_SimulationPos = transform.position;
        float_SimlationTime = 0;
    }
    /// <summary>
    /// �������״̬
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
