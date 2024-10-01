using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;
using System;
using UniRx;

public class NavManager : MonoBehaviour
{
    [Header("建筑地图")]
    public Tilemap tilemap_Building;
    [Header("地块地图")]
    public Tilemap tilemap_Floor;
    public Grid grid;
    /// <summary>
    /// 未选中列表
    /// </summary>
    private List<MyTile> openList = new List<MyTile>();
    /// <summary>
    /// 选中列表
    /// </summary>
    private List<MyTile> closeList = new List<MyTile>();
    public MyTile FindTileByPos(Vector2 pos)
    {
        return (MyTile)tilemap_Building.GetTile(grid.WorldToCell(pos));
    }
    public List<MyTile> FindPath(MyTile to, MyTile from)
    {
        if (to.config_passType == TilePassType.PassStop)
        {
            //Debug.Log("无法前往目标点");
            from.ResetTilePathInfo();
            return new List<MyTile> { from, from };
        }
        if (to._posInWorld == from._posInWorld)
        {
            from.ResetTilePathInfo();
            return new List<MyTile> { from, from };
        }
        openList.Clear();
        closeList.Clear();
        openList.Add(to);

        to.ResetTilePathInfo();
        from.ResetTilePathInfo();

        while (openList.Count > 0 /*&& closeList.Count < maxStep*/)
        {
            MyTile minTile = FindMinTile(openList);

            openList.Remove(minTile);
            closeList.Add(minTile);

            List<MyTile> surroundTiles = FindSurroundTile(minTile);
            SurroundTileFilter(surroundTiles, closeList);
            foreach(MyTile surroundTile in surroundTiles)
            {
                /*遍历周围的格子*/
                if (openList.Contains(surroundTile))
                {
                    /*这个格子已经被添加到未选择列表，更新路径数据*/
                    float newPathG = CalcG(surroundTile, minTile);
                    if (newPathG < surroundTile._temp_DistanceToFrom)
                    {
                        surroundTile._temp_DistanceToFrom = newPathG;
                        surroundTile._temp_DistanceMain = surroundTile._temp_DistanceToFrom + surroundTile.temp_PassDrag + surroundTile._temp_DistanceToTarget;
                        surroundTile._temp_fatherTile = minTile;
                    }
                }
                else
                {
                    /*这个格子还未被添加到未选择列表，添加到选择列表*/
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
        return CreatePath(to,from);
    }
    private List<MyTile> CreatePath(MyTile start, MyTile end)
    {
        List<MyTile> pathList = new List<MyTile>();
        MyTile temp = end;
        while (true)
        {
            if (temp._temp_fatherTile == null)
            {
                break;
            }
            else
            {
                pathList.Add(temp._temp_fatherTile);
                temp = temp._temp_fatherTile;
            }
        }
        foreach (MyTile tile in pathList)
        {
            tile.ResetTilePathInfo();
        }
        return pathList;
    }

    /// <summary>
    /// 在未选中列表里找最近的格子
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

        MyTile up , down , right , left,
            rightUp , leftUp , rightDown , leftDown;

        up = (MyTile)tilemap_Building.GetTile(from._posInCell + Vector3Int.up);
        down = (MyTile)tilemap_Building.GetTile(from._posInCell + Vector3Int.down);
        right = (MyTile)tilemap_Building.GetTile(from._posInCell + Vector3Int.right);
        left  = (MyTile)tilemap_Building.GetTile(from._posInCell + Vector3Int.left);
        rightUp = (MyTile)tilemap_Building.GetTile(from._posInCell + Vector3Int.right + Vector3Int.up);
        leftUp = (MyTile)tilemap_Building.GetTile(from._posInCell + Vector3Int.left + Vector3Int.up);
        rightDown = (MyTile)tilemap_Building.GetTile(from._posInCell + Vector3Int.right + Vector3Int.down);
        leftDown = (MyTile)tilemap_Building.GetTile(from._posInCell + Vector3Int.left + Vector3Int.down);

        if (up != null)
        {
            if (up.config_passType != TilePassType.PassStop)
            {
                list.Add(up);
            }
        }
        if (down != null)
        {
            if (down.config_passType != TilePassType.PassStop)
            {
                list.Add(down);
            }
        }
        if (right != null)
        {
            if (right.config_passType != TilePassType.PassStop)
            {
                list.Add(right);
            }
        }
        if (left != null)
        {
            if(left.config_passType != TilePassType.PassStop)
            {
                list.Add(left);
            }
        }
        if (rightUp != null && right != null && up != null)
        {
            if (rightUp.config_passType != TilePassType.PassStop && right.config_passType != TilePassType.PassStop && up.config_passType != TilePassType.PassStop)
            {
                list.Add(rightUp);
            }
        }
        if (leftUp != null && left != null && up != null)
        {
            if (leftUp.config_passType != TilePassType.PassStop && left.config_passType != TilePassType.PassStop && up.config_passType != TilePassType.PassStop)
            {
                list.Add(leftUp);
            }
        }
        if (rightDown != null && right != null && down != null)
        {
            if (rightDown.config_passType != TilePassType.PassStop && right.config_passType != TilePassType.PassStop && down.config_passType != TilePassType.PassStop)
            {
                list.Add(rightDown);
            }
        }
        if (leftDown != null && left != null && down != null)
        {
            if (leftDown.config_passType != TilePassType.PassStop && left.config_passType != TilePassType.PassStop && down.config_passType != TilePassType.PassStop)
            {
                list.Add(leftDown);
            }
        }

        return list;
    }
    /// <summary>
    /// 将已经进入选中列表的格子移出周围格子
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
        return from._temp_DistanceToFrom + Vector3.Distance(surround._posInCell, from._posInCell);
    }
    /// <summary>
    /// 计算该点到终点F
    /// </summary>
    /// <param name="now"></param>
    /// <param name="to"></param>
    /// <returns></returns>
    private void CalcF(MyTile now,MyTile end)
    {
        float h = MathF.Abs(end._posInWorld.x - now._posInWorld.x) + MathF.Abs(end._posInWorld.y - now._posInWorld.y);
        float g = 0;
        if (now._temp_fatherTile == null)
        {
            g = 0;
        }
        else
        {
            g = Vector2.Distance(now._posInWorld, now._temp_fatherTile._posInWorld) + now._temp_DistanceToFrom;
        }
        float f = g + h + now.temp_PassDrag;
        now._temp_DistanceMain = f;
        now._temp_DistanceToFrom = g;
        now._temp_DistanceToTarget = h;
    }
}
