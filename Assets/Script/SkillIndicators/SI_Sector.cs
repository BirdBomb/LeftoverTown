using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(LineRenderer))]
public class SI_Sector : MonoBehaviour
{
    [SerializeField, Header("����Ⱦ��")]
    LineRenderer _line;
    [SerializeField, Header("����")]
    private Transform _centerTrans;
    public Vector2 CenterPos { set { _centerPos = value; } get { return _centerTrans.position; } }
    private Vector2 _centerPos;
    [SerializeField, Header("����")]
    private Transform _dirTrans;

    private void Start()
    {
        _line = gameObject.GetComponent<LineRenderer>();
        _line.positionCount = 20;
        _line.useWorldSpace = false;//��ʹ����������
    }
    #region//����ָʾ������
    public void Update_SIsector(Vector2 dir,float radiu,float angle)
    {
        CreatePoints(radiu, angle, dir);
    }
    #endregion
    #region//���ָʾ������
    public void Checkout_SIsector(Vector2 dir, float radiu, float angle, out Transform[] targets)
    {
        CheckAround(dir, radiu, angle, 4, out targets);
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
        for(int i = 0; i <= acc; i++)
        {
            Vector2 tempDirL = Quaternion.Euler(0, 0, -1f * subAngle * i) * (dir);
            RaycastHit2D[] hitsL = Physics2D.RaycastAll(CenterPos, tempDirL, radiu);
            for (int j = 0; j < hitsL.Length; j++)
            {
                if (!targetList.Contains(hitsL[j].transform))
                {
                    targetList.Add(hitsL[j].transform);
                }
            }
        }
        for(int i = 0; i < acc; i++)
        {
            Vector2 tempDirR = Quaternion.Euler(0, 0, 1f * subAngle * (i + 1f)) * (dir);
            RaycastHit2D[] hitsR = Physics2D.RaycastAll(CenterPos, tempDirR, radiu);
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


    #endregion
    #region//ͼ����ʾ
    void CreatePoints(float radius, float angle, Vector3 dir)//����Բ
    {
        if (angle < 1) { _line.enabled = false; return; }
        else
        {
            _line.enabled = true;
        }
        /*�������*/
        int pointCoint = (int)(angle * 0.1f + 5);

        angle += 1;
        float startAngle;
        if (dir.x >= 0) { startAngle = Vector2.Angle(dir, Vector2.up); }
        else { startAngle = Vector2.Angle(dir, Vector2.down) + 180; }

        startAngle += angle * 0.5f;

        float angleUp = 0;

        _line.positionCount = pointCoint;
        for (int i = 0; i < pointCoint; i++)
        {
            Vector3 tempUp = new Vector3();
            tempUp.x = Mathf.Sin(Mathf.Deg2Rad * (startAngle + angleUp)) * radius;
            tempUp.y = Mathf.Cos(Mathf.Deg2Rad * (startAngle + angleUp)) * radius;
            angleUp -= (angle / pointCoint);
            _line.SetPosition(i, tempUp);
        }
    }
    #endregion
}

