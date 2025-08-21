using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class GameUI_MiniMap : SingleTon<GameUI_MiniMap>, ISingleTon
{
    /// <summary>
    /// 用于显示的image
    /// </summary>
    public Image image_MiniMap;
    /// <summary>
    /// 地图纹理
    /// </summary>
    private Texture2D texture2D_Temp;
    /// <summary>
    /// 纹理原始宽度
    /// </summary>
    private int int_Texture2D_Width;
    /// <summary>
    /// 纹理原始高度
    /// </summary>
    private int int_Texture2D_Height;
    private Rect rect_Texture2D;
    private Vector2 pivot_Texture2D;
    [Header("缩放条")]
    public Scrollbar scrollbar_Scaling;
    private int int_MinMapHeight = 32;
    private int int_MaxMapHeight = 96;
    private int int_MapHeight = 32;
    private Vector2Int vector2_MapCenter = new Vector2Int(0, 0);    
    public void Init()
    {
        Bind();
        int_Texture2D_Width = image_MiniMap.sprite.texture.width;
        int_Texture2D_Height = image_MiniMap.sprite.texture.height;
        texture2D_Temp = new Texture2D(int_Texture2D_Width, int_Texture2D_Height, image_MiniMap.sprite.texture.format, false);
        texture2D_Temp.filterMode = FilterMode.Point;
        texture2D_Temp.wrapMode = TextureWrapMode.Repeat; // 关键设置：平铺时重复纹理
        pivot_Texture2D = new Vector2(0.5f, 0.5f);
        //Debug.Log(texture2D_Temp.height+"/"+texture2D_Temp.width);
    }
    private void Bind()
    {
        scrollbar_Scaling.onValueChanged.AddListener(ChangeScaling);
    }
    /// <summary>
    /// 更改地图地板像素
    /// </summary>
    /// <param name="id"></param>
    /// <param name="pos"></param>
    public void ChangeGroundInMap(int id, Vector3Int pos)
    {
        _ = DrawGroundOnTex(pos, id);
    }
    /// <summary>
    /// 更改地图建筑像素
    /// </summary>
    /// <param name="id"></param>
    /// <param name="pos"></param>
    public void ChangeBuildingInMap(int id, Vector3Int pos)
    {

    }
    /// <summary>
    /// 修改地图缩放
    /// </summary>
    /// <param name="pos"></param>
    private void ChangeScaling(float val)
    {
        int_MapHeight = (int)Mathf.Lerp(int_MinMapHeight, int_MaxMapHeight, val);
        UpdateRect(vector2_MapCenter, int_MapHeight);
    }
    /// <summary>
    /// 修改玩家位置
    /// </summary>
    /// <param name="pos"></param>
    public void ChangePlayerPos(Vector2Int pos)
    {
        vector2_MapCenter = pos;
        //_ = DrawPlayerOnTex((Vector3Int)vector2_MapCenter, Color.red);
        UpdateRect(vector2_MapCenter, int_MapHeight);
    }
    /// <summary>
    /// 更新绘制范围
    /// </summary>
    /// <param name="scaling">缩放0-1</param>
    /// <param name="center">中心</param>
    private void UpdateRect(Vector2 center,int h)
    {
        int width = int_MapHeight * int_Texture2D_Width / int_Texture2D_Height;
        int height = int_MapHeight;
        Vector2 pos = center + new Vector2(int_Texture2D_Width / 2, int_Texture2D_Height / 2) - new Vector2(width / 2, height / 2);
        rect_Texture2D = new Rect(pos.x, pos.y, width, height);
        if (Mathf.Abs(pos.x) + width < int_Texture2D_Width && Mathf.Abs(pos.y) + height < int_Texture2D_Height)
        {
            texture2D_Temp.Apply();
            image_MiniMap.sprite = Sprite.Create(texture2D_Temp, rect_Texture2D, pivot_Texture2D);
        }
        else
        {
            Debug.Log("越界");
        }
    }
    public async Task DrawGroundOnTex(Vector3Int pos, int id)
    {
        await Task.Delay(20);
        pos = pos + new Vector3Int(int_Texture2D_Width / 2, int_Texture2D_Height / 2, 0);
        GroundConfig config = GroundConfigData.GetFloorConfig(id);
        texture2D_Temp.SetPixel(pos.x, pos.y, (Color)config.Ground_Color);
    }
    public async Task DrawPlayerOnTex(Vector3Int pos, Color color)
    {
        await Task.Delay(20);
        pos = pos + new Vector3Int(int_Texture2D_Width / 2, int_Texture2D_Height / 2, 0);
        texture2D_Temp.SetPixel(pos.x, pos.y, color);
    }
}
