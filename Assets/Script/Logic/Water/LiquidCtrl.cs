using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
/// <summary>/// 
/// �ο����£�
///     https://web.archive.org/web/20160418004149/http://freespace.virgin.net/hugo.elias/graphics/x_water.htm
/// 
/// Velocity(x, y) = -Buffer2(x, y)
/// Smoothed(x, y) = (Buffer1(x - 1, y) + Buffer1(x + 1, y) + Buffer1(x, y - 1) + Buffer1(x, y + 1)) / 4;
/// NewHeight(x, y) = Smoothed(x, y) * 2 + Velocity(x, y)
/// 
/// ˥����ʽ1��
/// NewHeight(x,y) = NewHeight(x,y) * damping
/// 
/// ˥����ʽ2��
/// NewHeight(x,y) = NewHeight(x,y) - (NewHeight(x,y)/n)
/// nΪ2����
/// </summary>
public class LiquidCtrl : MonoBehaviour
{
    [Header("======��������=======")]
    public bool isEditor = false;
    public bool isMouseOperate = false;

    [Header("========��������=========")]
    public Material waveMat;
    public Material addMat;
    public Material liquidMat;

    [Header("======Wave��������=======")]
    public Vector2Int texSize = new Vector2Int(1024, 1024);
    public float damping = 0.99f;
    public float off = 0.001f;
    public float speed = 1f;
    public TextureWrapMode mode;

    [Header("======Wave��������=======")]
    public bool update = false;
    public float updateTime = 0.5f;
    private float timer;

    [Header("======Ĭ��Mask����=======")]
    public Texture2D defaultMask;
    public Vector2 defaultMaskSize = Vector2.one;

    /// <summary>
    /// ��һ֡
    /// </summary>
    private RenderTexture Hp; 
    private Vector4 HpTrans;
    /// <summary>
    /// ��ǰ֡
    /// </summary>
    private RenderTexture Hc;   
    private Vector4 HcTrans;
    /// <summary>
    /// ����
    /// </summary>
    private RenderTexture Temp;     // ����

    private SpriteRenderer spRenderer;
    private Material renderMat;

    #region ��ʼ���뷴��ʼ��
    void Start()
    {
        Init();
    }

    private void OnDestroy()
    {
        UnInit();
    }

    private void Init()
    {
        // ��ʼ�� RenderTexture
        Hc = CreateRenderTexture(texSize.x, texSize.y);
        Hp = CreateRenderTexture(texSize.x, texSize.y);
        Temp = CreateRenderTexture(texSize.x, texSize.y);

        liquidMat.SetTexture("_DistortTex", Hc);

        //����ˮ����
        spRenderer = GetComponent<SpriteRenderer>();
        renderMat = spRenderer.sharedMaterial;
    }

    private void UnInit()
    {
        liquidMat.SetTexture("_DistortTex", null);
        renderMat.SetTexture("_BlendTex", null);

        ReleaseRenderTexture(ref Hc);
        ReleaseRenderTexture(ref Hp);
        ReleaseRenderTexture(ref Temp);
    }

    private RenderTexture CreateRenderTexture(int width, int height)
    {
        RenderTexture rt = new RenderTexture(width, height, 0);
        rt.wrapMode = mode;
        rt.enableRandomWrite = true;
        rt.Create();
        return rt;
    }

    private void ReleaseRenderTexture(ref RenderTexture rt)
    {
        if (rt != null)
        {
            rt.Release();
            Destroy(rt);
            rt = null;
        }
    }
    #endregion

    #region ����
    private void Update()
    {
        //��⿪���༭
        UpdateEditor();
        //������
        UpdateMouse();

        if (update)
        {
            timer += Time.deltaTime;
            if (timer > updateTime)
            {
                UpdateFrameShader();
                timer -= updateTime;
            }
        }
    }

    /// <summary>
    /// ����֡
    /// </summary>
    void UpdateFrameShader()
    {
        Vector2 wPos = transform.position;
        Vector2 size = transform.localScale;
        Vector4 HoTrans = new Vector4(
            wPos.x,
            wPos.y,
            size.x,
            size.y);

        waveMat.SetTexture("_Hp", Hp);
        waveMat.SetVector("_HpTrans", HpTrans);
        waveMat.SetTexture("_Hc", Hc);
        waveMat.SetVector("_HcTrans", HcTrans);
        waveMat.SetVector("_HoTrans", HoTrans);
        waveMat.SetFloat("_Damping", damping);
        waveMat.SetFloat("_Off", off);
        waveMat.SetFloat("_Speed", speed);
        Graphics.Blit(null, Temp, waveMat);

        // �������Ⱦ����Ļ
        Graphics.Blit(Hc, Hp);
        Graphics.Blit(Temp, Hc);

        HpTrans = HcTrans;
        HcTrans = HoTrans;

        // ����ˮ����Ⱦ����
        liquidMat.SetTexture("_DistortTex", Hc);
        Vector4 waveTrans = new Vector4(
            wPos.x,
            wPos.y,
            1 / size.x,
            1 / size.y);
        liquidMat.SetVector("_DistortTran", waveTrans);
    }
    #endregion


    #region ������
    /// <summary>
    /// �༭��������
    /// </summary>
    private void UpdateEditor()
    {
        if (isEditor)
        {
            spRenderer.color = new Color(1, 1, 1, 1);
            spRenderer.enabled = true;
            renderMat.SetTexture("_BlendTex", Hc);
        }
        else
        {
            spRenderer.color = new Color(0, 0, 0, 0);
            spRenderer.enabled = false;
            renderMat.SetTexture("_BlendTex", null);
        }
    }

    private void UpdateMouse()
    {
        if (!isMouseOperate)
            return;
        if (Input.GetMouseButton(0))
        {
            AddWave(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
        //if (Input.GetMouseButton(0))
        //{
        //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //    RaycastHit hit;
        //    if (Physics.Raycast(ray, out hit))
        //    {
        //        if (hit.collider == null)
        //            return;
        //        var comp = hit.collider.gameObject.GetComponent<LiquidCtrl>();
        //        if (comp == null)
        //            return;
        //        Vector2 hitPos = hit.point;
        //        //��Ӳ�
        //        AddWave(hitPos);
        //    }
        //}
    }
    #endregion


    #region ����ӿ�
    /// <summary>
    /// ��Ӳ�
    /// </summary>
    /// <param name="wPos">��Ӳ�����������λ��</param>
    /// <param name="mask">��������</param>
    /// <param name="maskSize">���Ĵ�С</param>
    public void AddWave(Vector2 wPos, Texture2D mask = null, Vector2 maskSize = default)
    {
        mask = mask == null ? defaultMask : mask;
        maskSize = maskSize == default ? defaultMaskSize : maskSize;
        if (maskSize.x * maskSize.y == 0) return;

        //���λ��
        Vector2 relatePos = (wPos - (Vector2)transform.position);
        relatePos -= maskSize * 0.5f;

        //mask���uv����
        Vector2 waveTxSize = transform.localScale;
        Vector2 maskUVPos = relatePos / waveTxSize + Vector2.one * 0.5f;

        //mask������ųߴ�
        Vector2 maskUVScale = defaultMaskSize / waveTxSize;

        //���Mask
        addMat.SetTexture("_MainTex", Hc);
        addMat.SetTexture("_AddTex", mask);
        addMat.SetVector("_AddTexTrans", new Vector4(
            maskUVPos.x,
            maskUVPos.y,
            1 / maskUVScale.x,
            1 / maskUVScale.y));
        Graphics.Blit(Hc, Temp, addMat);
        Graphics.Blit(Temp, Hc);                      
    }
    #endregion
}
