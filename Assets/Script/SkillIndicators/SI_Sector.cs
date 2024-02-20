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

    private void Start()
    {
       
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
        _radius = radiu;
        if(_radius > 1) { _radius = 1; }
        RefreshView();
    }
    #endregion
    #region//���ָʾ������
    public void Checkout_SIsector(LayerMask targetLayer, out Transform[] targets)
    {
        CanFindTargets(targetLayer, out targets);
    }
    private bool CanFindTarget(LayerMask targetLayer, out Transform target)
    {
        var ret = CanFindTargets(targetLayer, out Transform[] targets);
        target = ret ? targets[0] : null;
        return ret;
    }
    private bool CanFindTargets(LayerMask targetLayer, out Transform[] targets)
    {
        var ret = false;
        var targetList = new List<Transform>();
        var cols = Physics2D.OverlapCircleAll(CenterPos, _radius, targetLayer);
        Debug.Log("���뾶" + _radius);
        Debug.Log("���Ƕ�" + _angles);
        Debug.Log("���㼶" + targetLayer);
        if (cols != null)
        {
            foreach (var col in cols)
            {
                Vector2 pos = col.transform.position;
                Debug.Log(col.transform.name);
                if (IsInArea(pos))
                {
                    targetList.Add(col.transform);
                }
            }
        }

        ret = targetList.Count > 0;
        targets = targetList.ToArray();

        return ret;
    }
    public bool IsInArea(Vector2 pos)
    {
        Debug.Log(pos);
        Debug.Log(CenterPos);
        var ret = false;
        if (Vector2.Distance(CenterPos, pos) < _radius)
        {
            var halfAngle = _angles / 2;
            var dir = (pos - CenterPos).normalized;
            var curAngle = Vector2.Angle(Dir, dir);
            if (curAngle < halfAngle || curAngle > 360 - halfAngle)
            {
                ret = true;
            }
        }
        return ret;
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
        for (int x = 0; x < size; x ++)
        {
            for (int y = 0; y < size; y ++)
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
        _renderer.sprite = Sprite.Create(texture2D, new Rect(0, 0, size, size), Vector2.one * 0.5f);
    }
    #endregion
}

