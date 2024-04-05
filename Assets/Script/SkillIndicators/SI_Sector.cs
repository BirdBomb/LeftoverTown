using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SI_Sector : MonoBehaviour
{
    [SerializeField, Header("ͼ��")]
    private SpriteRenderer _renderer;

    [SerializeField, Header("����")]
    private Transform _centerTrans;
    public Vector2 CenterPos { set { _centerPos = value; } get { return _centerTrans.position; } }
    private Vector2 _centerPos;
    [SerializeField, Header("����")]
    private Transform _dirTrans;
    public Vector2 Dir { set { _dir = value; } get { return _dir; } }
    private Vector2 _dir;
    [SerializeField, Range(1, 359), Header("�Ƕ�")]
    private float _angles;
    [SerializeField, Range(0.1f, 15), Header("�뾶")]
    private float _radius;

    [SerializeField]
    private Color _viewColor = Color.red;

    private float _scaleVal;
    private void Start()
    {
        _scaleVal = 1f / transform.localScale.x;
    }
    private void Update()
    {
        
    }


    public void Init()
    {
        
    }
    public void MyDestroy()
    {

    }



    #region//����ָʾ������
    public void Update_SIsector(Vector2 dir,float radiu,float angle)
    {
        _dir = dir;
        _angles = angle;
        _radius = radiu * _scaleVal;
        if(_radius > 1) { _radius = 1; }
        RefreshView();
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
    [ContextMenu("ˢ�����������չʾ")]
    private void RefreshView()
    {
        CreateSprite(_radius, _angles, _viewColor);
    }
    public void CreateSprite(float radius, float angle, Color color)
    {
        if (radius > 0)
        {
            /*���γߴ�*/
            var size = (int)(radius * 2 * 100);
            /*ʵ�ʰ뾶*/
            var actualRadius = size / 2;
            /*���*/
            var halfAngle = angle / 2;
            Texture2D texture2D = new Texture2D(size, size);
            Vector2 centerPixel = Vector2.one * size / 2;

            // ����
            var emptyColor = Color.clear;
            Vector2 tempPixel;
            float tempAngle;
            float tempDisSqr;
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    tempPixel.x = x - centerPixel.x;
                    tempPixel.y = y - centerPixel.y;

                    tempDisSqr = tempPixel.sqrMagnitude;
                    if (tempDisSqr <= actualRadius * actualRadius)
                    {
                        tempAngle = Vector2.Angle(Dir, tempPixel);
                        if (tempAngle < halfAngle || tempAngle > 360 - halfAngle)
                        {
                            //��������ɫֵ
                            texture2D.SetPixel(x, y, color);
                            continue;
                        }
                    }
                    texture2D.SetPixel(x, y, emptyColor);
                }
            }
            texture2D.Apply();
            _renderer.enabled = true;
            _renderer.sprite = Sprite.Create(texture2D, new Rect(0, 0, size, size), Vector2.one * 0.5f);
        }
        else
        {
            _renderer.enabled = false;
        }
    }
    #endregion
}

