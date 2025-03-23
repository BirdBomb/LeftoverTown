using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : SingleTon<CursorManager>, ISingleTon
{
    public Texture2D texture_CommonCursor;
    public Texture2D texture_BuildCursor;
    public CursorMode cursorMode = CursorMode.Auto;
    public enum CursorType
    {
        Common,
        Build,
    } 
    public void Init()
    {
        
    }
    public void ChangeCursor(CursorType cursorType)
    {
        switch (cursorType)
        {
            case CursorType.Common:
                Cursor.SetCursor(texture_CommonCursor, Vector2.zero, cursorMode);
                break;
            case CursorType.Build:
                Cursor.SetCursor(texture_BuildCursor, Vector2.zero, cursorMode);
                break;
        }
    }
}
