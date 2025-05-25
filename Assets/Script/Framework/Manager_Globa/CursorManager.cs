using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CursorManager : SingleTon<CursorManager>, ISingleTon
{
    public Texture2D texture_CommonCursor;
    public Texture2D texture_BuildCursor;
    public Texture2D texture_AimCursor;
    public Texture2D texture_WeaponCursor;
    public Texture2D texture_ToolCursor;
    public Transform transform_Follow;
    public Image image_Aim;
    public float float_moveSpeed;

    private Vector3 vector3_ref;
    private Vector3 vector3_targetPos;
    private Vector3 vector3_curPos;
    public CursorMode cursorMode = CursorMode.Auto;
    private List<CursorType> cursorStateList = new List<CursorType>();

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
        
    }
    private void FollowCursor()
    {
        if (transform_Follow != null)
        {
            // 计算目标位置
            vector3_targetPos = Input.mousePosition;
            vector3_curPos = Vector3.SmoothDamp(transform_Follow.position, vector3_targetPos, ref vector3_ref, float_moveSpeed);
            transform_Follow.position = vector3_curPos;
        }

    }
    public void AddCursor(CursorType cursorType)
    {
        if (!cursorStateList.Contains(cursorType))
        {
            cursorStateList.Add(cursorType);
        }
        ChangeCursor(cursorType);
    }
    public void SubCursor(CursorType cursorType)
    {
        if (cursorStateList.Contains(cursorType))
        {
            cursorStateList.Remove(cursorType); 
        }
        if (cursorStateList.Count > 0)
        {
            ChangeCursor(cursorStateList[cursorStateList.Count - 1]);
        }
        else
        {
            ChangeCursor(CursorType.Common);
        }
    }
    private void ChangeCursor(CursorType cursorType)
    {
        image_Aim.gameObject.SetActive(false);
        switch (cursorType)
        {
            case CursorType.Common:
                Cursor.SetCursor(texture_CommonCursor, Vector2.zero, cursorMode);
                break;
            case CursorType.Build:
                Cursor.SetCursor(texture_BuildCursor, Vector2.zero, cursorMode);
                break;
            case CursorType.Aim:
                Cursor.SetCursor(texture_AimCursor, Vector2.zero, cursorMode);
                image_Aim.gameObject.SetActive(true);
                break;
            case CursorType.Weapon:
                Cursor.SetCursor(texture_WeaponCursor, Vector2.zero, cursorMode);
                break;
            case CursorType.Tool:
                Cursor.SetCursor(texture_ToolCursor, Vector2.zero, cursorMode);
                break;
        }
    }
}
