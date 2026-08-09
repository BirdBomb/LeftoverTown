using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class GameUI_MiniMap : SingleTon<GameUI_MiniMap>, ISingleTon
{
    /// <summary>
    /// 用于显示的image
    /// </summary>
    public Image image_MiniMap;
    private Texture2D texture2D_Temp;
    private int int_Texture2D_Width;
    private int int_Texture2D_Height;
    private Rect rect_Texture2D;
    private Vector2 pivot_Texture2D;
    [Header("缩放条")]
    public Scrollbar scrollbar_Scaling;
    public Button btn_ZoomIn;
    public Button btn_ZoomOut;
    public TextMeshProUGUI text_Postion;
    private int int_MinMapHeight = 32;
    private int int_MaxMapHeight = 96;
    private int int_MapHeight = 32;
    private Vector2Int vector2_MapCenter = new Vector2Int(0, 0);

    // 新增：用于跟踪已修改的像素，避免重复创建sprite
    private bool isTextureDirty = false;
    private Sprite currentSprite;
    public void Init()
    {
        Bind();
        int_Texture2D_Width = image_MiniMap.sprite.texture.width;
        int_Texture2D_Height = image_MiniMap.sprite.texture.height;
        texture2D_Temp = new Texture2D(int_Texture2D_Width, int_Texture2D_Height, image_MiniMap.sprite.texture.format, false);
        texture2D_Temp.filterMode = FilterMode.Point;
        texture2D_Temp.wrapMode = TextureWrapMode.Repeat; // 关键设置：平铺时重复纹理

        // 复制原始纹理像素
        texture2D_Temp.Apply();
        pivot_Texture2D = new Vector2(0.5f, 0.5f);

        Color[] blackPixels = new Color[int_Texture2D_Width * int_Texture2D_Height];
        for (int i = 0; i < blackPixels.Length; i++)
        {
            blackPixels[i] = Color.black;
        }
        texture2D_Temp.SetPixels(blackPixels);

        // 初始化显示
        UpdateRect(vector2_MapCenter, int_MapHeight);
    }
    private void Bind()
    {
        btn_ZoomIn.onClick.AddListener(ZoomIn);
        btn_ZoomOut.onClick.AddListener(ZoomOut);
    }
    /// <summary>
    /// 放大
    /// </summary>
    private void ZoomIn()
    {
        if (int_MapHeight > int_MinMapHeight)
        {
            int_MapHeight = Mathf.Max(int_MinMapHeight, int_MapHeight - 10);
        }
        UpdateRect(vector2_MapCenter, int_MapHeight);
        DrawSprite();
    }
    /// <summary>
    /// 缩小
    /// </summary>
    private void ZoomOut()
    {
        if (int_MapHeight < int_MaxMapHeight)
        {
            int_MapHeight = Mathf.Min(int_MaxMapHeight, int_MapHeight + 10);
        }
        UpdateRect(vector2_MapCenter, int_MapHeight);
        DrawSprite();
    }
    /// <summary>
    /// 修改玩家位置
    /// </summary>
    /// <param name="pos"></param>
    public void ChangePlayerPos(Vector2Int pos)
    {
        vector2_MapCenter = pos;
        text_Postion.text = $"{pos}";
        UpdateRect(vector2_MapCenter, int_MapHeight);
        DrawSprite();
    }
    /// <summary>
    /// 更新绘制范围
    /// </summary>
    /// <param name="scaling">缩放0-1</param>
    /// <param name="center">中心</param>
    private void UpdateRect(Vector2 center,int h)
    {
        // 计算显示区域
        int width = h * int_Texture2D_Width / int_Texture2D_Height;
        int height = h;

        // 计算中心偏移
        float halfWidth = width / 2f;
        float halfHeight = height / 2f;
        float centerX = center.x + int_Texture2D_Width / 2f;
        float centerY = center.y + int_Texture2D_Height / 2f;

        // 计算矩形位置
        float x = Mathf.Clamp(centerX - halfWidth, 0, int_Texture2D_Width - width);
        float y = Mathf.Clamp(centerY - halfHeight, 0, int_Texture2D_Height - height);

        rect_Texture2D = new Rect(x, y, width, height);
        isTextureDirty = true;
    }
    public void ChangeGroundOnTex(Vector3Int pos, int id)
    {
        // 坐标转换到纹理空间
        Vector3Int texturePos = pos + new Vector3Int(int_Texture2D_Width / 2, int_Texture2D_Height / 2, 0);

        // 边界检查
        if (texturePos.x < 0 || texturePos.x >= int_Texture2D_Width ||
            texturePos.y < 0 || texturePos.y >= int_Texture2D_Height)
        {
            Debug.LogWarning($"绘制位置越界: {texturePos}");
            return;
        }

        GroundConfig config = GroundConfigData.GetFloorConfig(id);
        texture2D_Temp.SetPixel(texturePos.x, texturePos.y, (Color)config.Ground_Color);
        isTextureDirty = true;
    }
    public void DrawSprite()
    {
        if (isTextureDirty)
        {
            isTextureDirty = false;
            texture2D_Temp.Apply();
            if (currentSprite != null) Destroy(currentSprite);
            currentSprite = Sprite.Create(texture2D_Temp, rect_Texture2D, pivot_Texture2D);
            image_MiniMap.sprite = currentSprite;
        }
    }
}
