using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;
using System;
using UniRx;

public class NavManager : MonoBehaviour
{
    public MapManager mapManager;
    /// <summary>
    /// 未选中列表
    /// </summary>
    private List<GroundTile> groundTiles_OpenList = new List<GroundTile>();
    /// <summary>
    /// 选中列表
    /// </summary>
    private List<GroundTile> groundTiles_CloseList = new List<GroundTile>();
    public List<GroundTile> FindPath(Vector3Int toPos, Vector3Int fromPos, int maxStep)
    {
        if (mapManager.GetGround(toPos, out GroundTile to)) to.ResetTilePathInfo();
        if (mapManager.GetGround(fromPos, out GroundTile from)) from.ResetTilePathInfo();
        if (to == null || !to.offset_Pass)
        {
            //Debug.Log("终点阻塞");
            return new List<GroundTile> { };
        }
        if (from == null || !from.offset_Pass)
        {
            Debug.Log("起点阻塞");
            return new List<GroundTile> { };
        }
        if (to.tilePos == from.tilePos)
        {
            //Debug.Log("已经在终点");
            return new List<GroundTile> { };
        }
        groundTiles_OpenList.Clear();
        groundTiles_CloseList.Clear();
        groundTiles_OpenList.Add(to);
        while (groundTiles_OpenList.Count > 0)
        {
            if (groundTiles_CloseList.Count > maxStep) 
            { 
                //Debug.Log("达到最大步数"); 
                return CreatePath(from); 
            }
            GroundTile minTile = FindMinTile(groundTiles_OpenList);

            groundTiles_OpenList.Remove(minTile);
            groundTiles_CloseList.Add(minTile);

            List<GroundTile> surroundTiles = FindSurroundTile(minTile);
            SurroundTileFilter(surroundTiles, groundTiles_CloseList);
            foreach (GroundTile surroundTile in surroundTiles)
            {
                /*遍历周围的格子*/
                if (groundTiles_OpenList.Contains(surroundTile))
                {
                    /*这个格子已经被添加到未选择列表，更新路径数据*/
                    float newPathG = CalcG(surroundTile, minTile);
                    if (newPathG < surroundTile._temp_DistanceToFrom)
                    {
                        surroundTile._temp_DistanceToFrom = newPathG;
                        surroundTile._temp_DistanceMain = surroundTile._temp_DistanceToFrom + surroundTile.offset_Drag + surroundTile._temp_DistanceToTarget;
                        surroundTile._temp_fatherTile = minTile;
                    }
                }
                else
                {
                    /*这个格子还未被添加到未选择列表，添加到选择列表*/
                    surroundTile.ResetTilePathInfo();
                    surroundTile._temp_fatherTile = minTile;
                    CalcF(surroundTile, from);
                    groundTiles_OpenList.Add(surroundTile);
                }
            }
            if (groundTiles_OpenList.IndexOf(from) > -1)
            {
                break;
            }
        }
        return CreatePath(from);
    }
    private List<GroundTile> CreatePath(GroundTile from)
    {
        List<GroundTile> pathList = new List<GroundTile>();
        GroundTile temp = from;
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
        foreach (GroundTile tile in pathList)
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
    private GroundTile FindMinTile(List<GroundTile> tiles)
    {
        float f = float.MaxValue;
        GroundTile temp = null;
        foreach (GroundTile tile in tiles)
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
    private List<GroundTile> FindSurroundTile(GroundTile from)
    {
        List<GroundTile> list = new List<GroundTile>();

        GroundTile up, down, right, left, rightUp, leftUp, rightDown, leftDown;
        mapManager.GetGround(from.tilePos + Vector3Int.up, out up);
        mapManager.GetGround(from.tilePos + Vector3Int.down, out down);
        mapManager.GetGround(from.tilePos + Vector3Int.right, out right);
        mapManager.GetGround(from.tilePos + Vector3Int.left, out left);
        mapManager.GetGround(from.tilePos + Vector3Int.right + Vector3Int.up, out rightUp);
        mapManager.GetGround(from.tilePos + Vector3Int.left + Vector3Int.up, out leftUp);
        mapManager.GetGround(from.tilePos + Vector3Int.right + Vector3Int.down, out rightDown);
        mapManager.GetGround(from.tilePos + Vector3Int.left + Vector3Int.down, out leftDown);
        if (up != null)
        {
            if (up.offset_Pass) list.Add(up);
        }
        if (down != null)
        {
            if (down.offset_Pass) list.Add(down);
        }
        if (right != null)
        {
            if (right.offset_Pass) list.Add(right);
        }
        if (left != null)
        {
            if (left.offset_Pass) list.Add(left);
        }
        if (rightUp != null && right != null && up != null)
        {
            if (rightUp.offset_Pass && right.offset_Pass && up.offset_Pass)
            {
                list.Add(rightUp);
            }
        }
        if (leftUp != null && left != null && up != null)
        {
            if (leftUp.offset_Pass && left.offset_Pass && up.offset_Pass)
            {
                list.Add(leftUp);
            }
        }
        if (rightDown != null && right != null && down != null)
        {
            if (rightDown.offset_Pass && right.offset_Pass && down.offset_Pass)
            {
                list.Add(rightDown);
            }
        }
        if (leftDown != null && left != null && down != null)
        {
            if (leftDown.offset_Pass && left.offset_Pass && down.offset_Pass)
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
    private void SurroundTileFilter(List<GroundTile> surroundTiles, List<GroundTile> closeTiles)
    {
        foreach (GroundTile closeTile in closeTiles)
        {
            if (surroundTiles.Contains(closeTile))
            {
                surroundTiles.Remove(closeTile);
            }
        }
    }
    /// <summary>
    /// 计算该点到终点F
    /// </summary>
    /// <param name="now"></param>
    /// <param name="to"></param>
    /// <returns></returns>
    private void CalcF(GroundTile now, GroundTile end)
    {
        float h = MathF.Abs(end.tilePos.x - now.tilePos.x) + MathF.Abs(end.tilePos.y - now.tilePos.y);
        float g;
        if (now._temp_fatherTile == null)
        {
            g = 0;
        }
        else
        {
            g = Vector3.Distance(now.tilePos, now._temp_fatherTile.tilePos) + now._temp_DistanceToFrom;
        }
        float f = g + h + now.offset_Drag;
        now._temp_DistanceMain = f;
        now._temp_DistanceToFrom = g;
        now._temp_DistanceToTarget = h;
    }
    /// <summary>
    /// 计算最小预估值G
    /// </summary>
    /// <param name="surround"></param>
    /// <param name="from"></param>
    /// <returns></returns>
    private float CalcG(GroundTile surround, GroundTile from)
    {
        return from._temp_DistanceToFrom + Vector3.Distance(surround.tilePos, from.tilePos);
    }
}
