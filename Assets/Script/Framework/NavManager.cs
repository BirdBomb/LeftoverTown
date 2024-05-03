using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;
using UnityEngine.WSA;
using Unity.VisualScripting;
using System;
using UniRx;
using static UnityEditor.Progress;
using UnityEditor.Rendering;

public class NavManager : MonoBehaviour
{
    public Tilemap tilemap;
    public Grid grid;
    public int listCount;
    [HideInInspector]
    public List<MyTile> openList = new List<MyTile>();
    [HideInInspector]
    public List<MyTile> closeList = new List<MyTile>();

    public MyTile FindTileByPos(Vector2 pos)
    {
        return (MyTile)tilemap.GetTile(grid.WorldToCell(pos));
    }
    public List<MyTile> FindPath(MyTile to, MyTile from)
    {
        if (to.passType == TilePassType.PassStop)
        {
            Debug.Log("无法前往目标点");
            return new List<MyTile> { from, from };
        }
        if (to.posInWorld == from.posInWorld)
        {
            Debug.Log("原地前进");
            return new List<MyTile> { from, from };
        }
        openList.Clear();
        closeList.Clear();

        openList.Add(to);

        to.ResetTilePathInfo();
        from.ResetTilePathInfo();

        while (openList.Count > 0)
        {
            MyTile minTile = FindMinTile(openList);

            openList.Remove(minTile);
            closeList.Add(minTile);

            List<MyTile> surroundTiles = FindSurroundTile(minTile);
            SurroundTileFilter(surroundTiles, closeList);

            foreach(MyTile surroundTile in surroundTiles)
            {
                /*如果在openlist里有周围的格子*/
                if (openList.Contains(surroundTile))
                {
                    float newPathG = CalcG(surroundTile, minTile);
                    if (newPathG < surroundTile._temp_DistanceToFrom)
                    {
                        surroundTile._temp_DistanceToFrom = newPathG;
                        surroundTile._temp_DistanceMain = surroundTile._temp_DistanceToFrom + surroundTile.passOffset + surroundTile._temp_DistanceToTarget;
                        surroundTile._temp_fatherTile = minTile;
                    }
                }
                /*如果在openlist没有周围的格子*/
                else
                {
                    surroundTile.ResetTilePathInfo();
                    surroundTile._temp_fatherTile = minTile;
                    CalcF(surroundTile, from);
                    openList.Add(surroundTile);
                }
            }
            if (openList.IndexOf(from)>-1)
            {
                break;
            }

        }
        return ShowPath(to,from);
    }
    /// <summary>
    /// 在列表里找预计值最小的格子
    /// </summary>
    /// <param name="tiles"></param>
    /// <returns></returns>
    private MyTile FindMinTile(List<MyTile> tiles)
    {
        float f = float.MaxValue;
        MyTile temp = null;
        foreach(MyTile tile in tiles)
        {
            if (tile._temp_DistanceMain < f)
            {
                temp = tile;
                f = temp._temp_DistanceMain;
            }
        }
        return temp;
    }
    /// <summary>
    /// 获得目标周围格子
    /// </summary>
    /// <param name="from"></param>
    /// <returns></returns>
    private List<MyTile> FindSurroundTile(MyTile from)
    {
        List<MyTile> list = new List<MyTile>();

        MyTile up = null, down = null, right = null, left = null,
            rightUp = null, leftUp = null, rightDown = null, leftDown = null;

        up = (MyTile)tilemap.GetTile(from.posInCell + Vector3Int.up);
        down = (MyTile)tilemap.GetTile(from.posInCell + Vector3Int.down);
        right = (MyTile)tilemap.GetTile(from.posInCell + Vector3Int.right);
        left  = (MyTile)tilemap.GetTile(from.posInCell + Vector3Int.left);
        rightUp = (MyTile)tilemap.GetTile(from.posInCell + Vector3Int.right + Vector3Int.up);
        leftUp = (MyTile)tilemap.GetTile(from.posInCell + Vector3Int.left + Vector3Int.up);
        rightDown = (MyTile)tilemap.GetTile(from.posInCell + Vector3Int.right + Vector3Int.down);
        leftDown = (MyTile)tilemap.GetTile(from.posInCell + Vector3Int.left + Vector3Int.down);

        if (up != null)
        {
            list.Add(up);
        }
        if (down != null)
        {
            list.Add(down);
        }
        if (right != null)
        {
            list.Add(right);
        }
        if (left != null)
        {
            list.Add(left);
        }
        if (rightUp != null)
        {
            list.Add(rightUp);
        }
        if (leftUp != null)
        {
            list.Add(leftUp);
        }
        if (rightDown != null)
        {
            list.Add(rightDown);
        }
        if (leftDown != null)
        {
            list.Add(leftDown);
        }

        return list;
    }
    /// <summary>
    /// 将已经进入关闭列表的格子移出周围格子
    /// </summary>
    /// <param name="surroundTiles"></param>
    /// <param name="closeTiles"></param>
    private void SurroundTileFilter(List<MyTile> surroundTiles,List<MyTile> closeTiles)
    {
        foreach(MyTile closeTile in closeTiles)
        {
            if (surroundTiles.Contains(closeTile))
            {
                surroundTiles.Remove(closeTile);
            }
        }
    }
    /// <summary>
    /// 计算最小预估值G
    /// </summary>
    /// <param name="surround"></param>
    /// <param name="from"></param>
    /// <returns></returns>
    private float CalcG(MyTile surround,MyTile from)
    {
        return from._temp_DistanceToFrom /*+ from.passOffset*/ + Vector3.Distance(surround.posInCell, from.posInCell);
    }
    /// <summary>
    /// 计算该点到终点F
    /// </summary>
    /// <param name="now"></param>
    /// <param name="to"></param>
    /// <returns></returns>
    private void CalcF(MyTile now,MyTile end)
    {
        float h = MathF.Abs(end.posInWorld.x - now.posInWorld.x) + MathF.Abs(end.posInWorld.y - now.posInWorld.y);
        float g = 0;
        if (now._temp_fatherTile == null)
        {
            g = 0;
        }
        else
        {
            g = Vector2.Distance(now.posInWorld, now._temp_fatherTile.posInWorld) + now._temp_DistanceToFrom;
        }
        float f = g + h + now.passOffset;
        now._temp_DistanceMain = f;
        now._temp_DistanceToFrom = g;
        now._temp_DistanceToTarget = h;
    }
    List<MyTile> path = new List<MyTile>();
    private List<MyTile> ShowPath(MyTile start,MyTile end)
    {
        path.Clear();
        MyTile temp = end;
        while(true)
        {
            path.Add(temp);
            if(temp._temp_fatherTile == null)
            {
                break;
            }
            temp = temp._temp_fatherTile;
        }
        return path;
    }
}
