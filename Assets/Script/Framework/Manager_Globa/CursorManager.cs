using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CursorManager : SingleTon<CursorManager>, ISingleTon
{
    [Header("Cursor Textures")]
    public Texture2D texture_CommonCursor;
    public Texture2D texture_BuildCursor;
    public Texture2D texture_AimCursor;
    public Texture2D texture_WeaponCursor;
    public Texture2D texture_ToolCursor;
    [Header("UI Elements")]
    public Transform transform_Follow;
    public Image image_Aim;
    [Header("Settings")]
    public float float_moveSpeed;
    public CursorMode cursorMode = CursorMode.Auto;

    private Vector3 vector3_ref;
    private Stack<CursorType> cursorStateStack = new Stack<CursorType>();
    private CursorType currentCursorType = CursorType.Common;
    public enum CursorType
    {
        Common,
        Build,
        Aim,
        Weapon,
        Tool,
    }
    public void Update()
    {
        FollowCursor();
    }
    public void Init()
    {
        if (image_Aim != null)
            image_Aim.gameObject.SetActive(false);

        ChangeCursor(CursorType.Common);
    }
    private void FollowCursor()
    {
        if (transform_Follow == null) return;

        // 只有瞄准镜激活时才需要跟随
        if (image_Aim != null && image_Aim.gameObject.activeSelf)
        {
            transform_Follow.position = Vector3.SmoothDamp(
                transform_Follow.position,
                Input.mousePosition,
                ref vector3_ref,
                float_moveSpeed
            );
        }
    }
    public void AddCursor(CursorType cursorType)
    {
        if (cursorStateStack.Count == 0 || cursorStateStack.Peek() != cursorType)
        {
            cursorStateStack.Push(cursorType);
            ChangeCursor(cursorType);
        }
    }
    public void SubCursor(CursorType cursorType)
    {
        if (cursorStateStack.Count > 0 && cursorStateStack.Peek() == cursorType)
        {
            cursorStateStack.Pop();
            ChangeCursor(cursorStateStack.Count > 0 ? cursorStateStack.Peek() : CursorType.Common);
        }
    }
    private void ChangeCursor(CursorType cursorType)
    {
        if (currentCursorType == cursorType) return;

        currentCursorType = cursorType;

        // 隐藏瞄准镜UI
        if (image_Aim != null)
            image_Aim.gameObject.SetActive(cursorType == CursorType.Aim);

        // 设置系统光标
        Texture2D cursorTexture = GetCursorTexture(cursorType);
        Vector2 hotspot = GetCursorHotspot(cursorType);
        Cursor.SetCursor(cursorTexture, hotspot, cursorMode);
    }
    private Texture2D GetCursorTexture(CursorType cursorType)
    {
        switch (cursorType)
        {
            case CursorType.Common: return texture_CommonCursor;
            case CursorType.Build: return texture_BuildCursor;
            case CursorType.Aim: return texture_AimCursor;
            case CursorType.Weapon: return texture_WeaponCursor;
            case CursorType.Tool: return texture_ToolCursor;
            default: return null;
        }
    }
    private Vector2 GetCursorHotspot(CursorType cursorType)
    {
        return Vector2.zero; // 默认左上角
    }
}
