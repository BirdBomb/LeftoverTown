using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(LineRenderer))]
public class SkillIndicators : MonoBehaviour
{
    [SerializeField, Header("����Ⱦ��")]
    LineRenderer lineRenderer_Line;
    [SerializeField, Header("����")]
    private Transform transform_CenterTrans;
    [SerializeField, Header("����")]
    private Transform transform_DirTrans;
    [SerializeField, Header("ͼ��")]
    private LayerMask layerMask;
    private void Start()
    {
        lineRenderer_Line = gameObject.GetComponent<LineRenderer>();
        lineRenderer_Line.positionCount = 20;
        lineRenderer_Line.useWorldSpace = false;//��ʹ����������
    }
    #region//����ָʾ������
    /// <summary>
    /// ����ָʾ������
    /// </summary>
    /// <param name="dir">����</param>
    /// <param name="radiu">�뾶</param>
    /// <param name="angle">�Ƕ�</param>
    /// <param name="alpha">͸����</param>
    public void Draw_SkillIndicators(Vector2 dir, float radiu, float angle, float alpha)
    {
        CreatePoints(radiu, angle, dir);
        lineRenderer_Line.startColor = new Color(1, 1, 1, alpha);
        lineRenderer_Line.endColor = new Color(1, 1, 1, alpha);
    }
    #endregion
    #region//���ָʾ������
    public void Checkout_SkillIndicators(Vector2 dir, float radiu, float angle, out Transform[] targets)
    {
        CheckAround(dir, radiu, angle, 4, out targets);
    }
    public void Checkout_SkillIndicators(Vector2 dir, float radiu, float angle, out Collider2D[] colliders)
    {
        CheckCollider(dir, radiu, angle, 4, out colliders);
    }
    /// <summary>
    /// ���μ��
    /// </summary>
    /// <param name="dir">����</param>
    /// <param name="radiu">�뾶</param>
    /// <param name="angle">�Ƕ�</param>
    /// <param name="acc">����</param>
    /// <param name="targets">����ֵ</param>
    private void CheckAround(Vector2 dir, float radiu, float angle, float acc, out Transform[] targets)
    {
        var targetList = new List<Transform>();
        float subAngle = ((angle * 0.5f) / acc);
        for (int i = 0; i <= acc; i++)
        {
            Vector2 tempDirL = Quaternion.Euler(0, 0, -1f * subAngle * i) * (dir);
            RaycastHit2D[] hitsL = Physics2D.RaycastAll(transform_CenterTrans.position, tempDirL, radiu, layerMask);
            for (int j = 0; j < hitsL.Length; j++)
            {
                if (!targetList.Contains(hitsL[j].transform))
                {
                    targetList.Add(hitsL[j].transform);
                }
            }
        }
        for (int i = 0; i < acc; i++)
        {
            Vector2 tempDirR = Quaternion.Euler(0, 0, 1f * subAngle * (i + 1f)) * (dir);
            RaycastHit2D[] hitsR = Physics2D.RaycastAll(transform_CenterTrans.position, tempDirR, radiu, layerMask);
            for (int j = 0; j < hitsR.Length; j++)
            {
                if (!targetList.Contains(hitsR[j].transform))
                {
                    targetList.Add(hitsR[j].transform);
                }
            }
        }
        targets = targetList.ToArray();
    }
    /// <summary>
    /// ���μ��
    /// </summary>
    /// <param name="dir">����</param>
    /// <param name="radiu">�뾶</param>
    /// <param name="angle">�Ƕ�</param>
    /// <param name="acc">����</param>
    /// <param name="targets">����ֵ</param>
    private void CheckCollider(Vector2 dir, float radiu, float angle, float acc, out Collider2D[] colliders)
    {
        var targetList = new List<Collider2D>();
        float subAngle = ((angle * 0.5f) / acc);
        for (int i = 0; i <= acc; i++)
        {
            Vector2 tempDirL = Quaternion.Euler(0, 0, -1f * subAngle * i) * (dir);
            RaycastHit2D[] hitsL = Physics2D.RaycastAll(transform_CenterTrans.position, tempDirL, radiu, layerMask);
            for (int j = 0; j < hitsL.Length; j++)
            {
                if (!targetList.Contains(hitsL[j].collider))
                {
                    targetList.Add(hitsL[j].collider);
                }
            }
        }
        for (int i = 0; i < acc; i++)
        {
            Vector2 tempDirR = Quaternion.Euler(0, 0, 1f * subAngle * (i + 1f)) * (dir);
            RaycastHit2D[] hitsR = Physics2D.RaycastAll(transform_CenterTrans.position, tempDirR, radiu, layerMask);
            for (int j = 0; j < hitsR.Length; j++)
            {
                if (!targetList.Contains(hitsR[j].collider))
                {
                    targetList.Add(hitsR[j].collider);
                }
            }
        }
        colliders = targetList.ToArray();
    }
    #endregion
    #region//ͼ����ʾ
    void CreatePoints(float radius, float angle, Vector3 dir)//����Բ
    {
        if (angle < 1) { lineRenderer_Line.enabled = false; return; }
        else
        {
            lineRenderer_Line.enabled = true;
        }
        /*�������*/
        int pointCoint = (int)(angle * 0.1f + 5);

        angle += 1;
        float startAngle;

        //if (dir.x >= 0) { startAngle = Vector2.Angle(dir, Vector2.up); }
        //else { startAngle = Vector2.Angle(dir, Vector2.down) + 180; }
        startAngle = Vector2.Angle(dir, Vector2.up);

        startAngle += angle * 0.5f;

        float angleUp = 0;

        lineRenderer_Line.positionCount = pointCoint;
        for (int i = 0; i < pointCoint; i++)
        {
            Vector3 tempUp = new Vector3();
            tempUp.x = Mathf.Sin(Mathf.Deg2Rad * (startAngle + angleUp)) * radius;
            tempUp.y = Mathf.Cos(Mathf.Deg2Rad * (startAngle + angleUp)) * radius;
            angleUp -= (angle / pointCoint);
            lineRenderer_Line.SetPosition(i, tempUp);
        }
    }
    /// <summary>
    /// ҡ��ָʾ��
    /// </summary>
    /// <param name="dir"></param>
    /// <param name="duraction"></param>
    public void Shake_SkillIndicators(Vector3 dir, float duraction)
    {
        transform.DOKill();
        transform.localScale = Vector3.one;
        transform.DOPunchScale(dir, duraction);
    }
    #endregion
}
