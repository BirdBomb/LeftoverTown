using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquidManager : SingleTon<LiquidManager>, ISingleTon
{
    [Header("======开启调试=======")]
    public bool isEditor = false;
    public bool isMouseIn = false;
    private Material material_Editor;
    private SpriteRenderer spRenderer;

    [Header("扰动演算材质")]
    public Material material_Distort;
    [Header("扰动混合材质")]
    public Material material_Blend;
    [Header("扰动输出目标材质")]
    public Material material_Target;


    [Header("======扰动参数配置=======")]
    public Vector2Int texSize = new Vector2Int(1024, 1024);
    public float damping = 0.99f;
    public float off = 0.001f;
    public float speed = 1f;
    public TextureWrapMode mode;
    [Header("======扰动更新配置=======")]
    public bool update = false;
    public float updateTime = 0.5f;
    private float timer;
    [Header("======扰动样式配置=======")]
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

    public void Init()
    {
        // 初始化 RenderTexture
        Hc = CreateRenderTexture(texSize.x, texSize.y);
        Hp = CreateRenderTexture(texSize.x, texSize.y);
        Temp = CreateRenderTexture(texSize.x, texSize.y);

        material_Target.SetTexture("_DistortTex", Hc);
        //调试水波用
        spRenderer = GetComponent<SpriteRenderer>();
        material_Editor = spRenderer.sharedMaterial;

    }
    public void OnDestroy()
    {
        material_Target.SetTexture("_DistortTex", null);
        material_Editor.SetTexture("_BlendTex", null);

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

        material_Distort.SetTexture("_Hp", Hp);
        material_Distort.SetVector("_HpTrans", HpTrans);
        material_Distort.SetTexture("_Hc", Hc);
        material_Distort.SetVector("_HcTrans", HcTrans);
        material_Distort.SetVector("_HoTrans", HoTrans);
        material_Distort.SetFloat("_Damping", damping);
        material_Distort.SetFloat("_Off", off);
        material_Distort.SetFloat("_Speed", speed);
        Graphics.Blit(null, Temp, material_Distort);

        // 将结果渲染到屏幕
        Graphics.Blit(Hc, Hp);
        Graphics.Blit(Temp, Hc);

        HpTrans = HcTrans;
        HcTrans = HoTrans;

        // 设置水波渲染纹理
        material_Target.SetTexture("_DistortTex", Hc);
        Vector4 waveTrans = new Vector4(
            wPos.x,
            wPos.y,
            1 / size.x,
            1 / size.y);
        material_Target.SetVector("_DistortTran", waveTrans);
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
            material_Editor.SetTexture("_BlendTex", Hc);
        }
        else
        {
            spRenderer.color = new Color(0, 0, 0, 0);
            spRenderer.enabled = false;
            material_Editor.SetTexture("_BlendTex", null);
        }
    }

    private void UpdateMouse()
    {
        if (!isMouseIn)
            return;
        if (Input.GetMouseButton(0))
        {
            AddWave(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
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
        material_Blend.SetTexture("_MainTex", Hc);
        material_Blend.SetTexture("_AddTex", mask);
        material_Blend.SetVector("_AddTexTrans", new Vector4(
            maskUVPos.x,
            maskUVPos.y,
            1 / maskUVScale.x,
            1 / maskUVScale.y));
        Graphics.Blit(Hc, Temp, material_Blend);
        Graphics.Blit(Temp, Hc);
    }
    #endregion

}
