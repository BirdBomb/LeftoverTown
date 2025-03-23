using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
/// <summary>/// 
/// 参考文章：
///     https://web.archive.org/web/20160418004149/http://freespace.virgin.net/hugo.elias/graphics/x_water.htm
/// 
/// Velocity(x, y) = -Buffer2(x, y)
/// Smoothed(x, y) = (Buffer1(x - 1, y) + Buffer1(x + 1, y) + Buffer1(x, y - 1) + Buffer1(x, y + 1)) / 4;
/// NewHeight(x, y) = Smoothed(x, y) * 2 + Velocity(x, y)
/// 
/// 衰减方式1：
/// NewHeight(x,y) = NewHeight(x,y) * damping
/// 
/// 衰减方式2：
/// NewHeight(x,y) = NewHeight(x,y) - (NewHeight(x,y)/n)
/// n为2的幂
/// </summary>
public class LiquidCtrl : MonoBehaviour
{
    [Header("======开启调试=======")]
    public bool isEditor = false;
    public bool isMouseOperate = false;

    [Header("========基础材质=========")]
    public Material waveMat;
    public Material addMat;
    public Material liquidMat;

    [Header("======Wave参数配置=======")]
    public Vector2Int texSize = new Vector2Int(1024, 1024);
    public float damping = 0.99f;
    public float off = 0.001f;
    public float speed = 1f;
    public TextureWrapMode mode;

    [Header("======Wave更新配置=======")]
    public bool update = false;
    public float updateTime = 0.5f;
    private float timer;

    [Header("======默认Mask配置=======")]
    public Texture2D defaultMask;
    public Vector2 defaultMaskSize = Vector2.one;

    /// <summary>
    /// 上一帧
    /// </summary>
    private RenderTexture Hp; 
    private Vector4 HpTrans;
    /// <summary>
    /// 当前帧
    /// </summary>
    private RenderTexture Hc;   
    private Vector4 HcTrans;
    /// <summary>
    /// 缓存
    /// </summary>
    private RenderTexture Temp;     // 缓存

    private SpriteRenderer spRenderer;
    private Material renderMat;

    #region 初始化与反初始化
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
        // 初始化 RenderTexture
        Hc = CreateRenderTexture(texSize.x, texSize.y);
        Hp = CreateRenderTexture(texSize.x, texSize.y);
        Temp = CreateRenderTexture(texSize.x, texSize.y);

        liquidMat.SetTexture("_DistortTex", Hc);

        //调试水波用
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

    #region 更新
    private void Update()
    {
        //检测开启编辑
        UpdateEditor();
        //检测鼠标
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
    /// 更新帧
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

        // 将结果渲染到屏幕
        Graphics.Blit(Hc, Hp);
        Graphics.Blit(Temp, Hc);

        HpTrans = HcTrans;
        HcTrans = HoTrans;

        // 设置水波渲染纹理
        liquidMat.SetTexture("_DistortTex", Hc);
        Vector4 waveTrans = new Vector4(
            wPos.x,
            wPos.y,
            1 / size.x,
            1 / size.y);
        liquidMat.SetVector("_DistortTran", waveTrans);
    }
    #endregion


    #region 调试用
    /// <summary>
    /// 编辑器调试用
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
        //        //添加波
        //        AddWave(hitPos);
        //    }
        //}
    }
    #endregion


    #region 对外接口
    /// <summary>
    /// 添加波
    /// </summary>
    /// <param name="wPos">添加波的世界坐标位置</param>
    /// <param name="mask">波的遮罩</param>
    /// <param name="maskSize">波的大小</param>
    public void AddWave(Vector2 wPos, Texture2D mask = null, Vector2 maskSize = default)
    {
        mask = mask == null ? defaultMask : mask;
        maskSize = maskSize == default ? defaultMaskSize : maskSize;
        if (maskSize.x * maskSize.y == 0) return;

        //相对位置
        Vector2 relatePos = (wPos - (Vector2)transform.position);
        relatePos -= maskSize * 0.5f;

        //mask相对uv坐标
        Vector2 waveTxSize = transform.localScale;
        Vector2 maskUVPos = relatePos / waveTxSize + Vector2.one * 0.5f;

        //mask相对缩放尺寸
        Vector2 maskUVScale = defaultMaskSize / waveTxSize;

        //混合Mask
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
