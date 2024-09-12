using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;
using System;
using UniRx;

public class NavManager : MonoBehaviour
{
    public Tilemap tilemap;
    public Grid grid;
    //private int maxStep = 16;
    /// <summary>
    /// δѡ���б�
    /// </summary>
    private List<MyTile> openList = new List<MyTile>();
    /// <summary>
    /// ѡ���б�
    /// </summary>
    private List<MyTile> closeList = new List<MyTile>();
    public MyTile FindTileByPos(Vector2 pos)
    {
        return (MyTile)tilemap.GetTile(grid.WorldToCell(pos));
    }
    public List<MyTile> FindPath(MyTile to, MyTile from)
    {
        if (to.passType == TilePassType.PassStop)
        {
            Debug.Log("�޷�ǰ��Ŀ���");
            from.ResetTilePathInfo();
            return new List<MyTile> { from, from };
        }
        if (to._posInWorld == from._posInWorld)
        {
            Debug.Log("ԭ��ǰ��");
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
                /*������Χ�ĸ���*/
                if (openList.Contains(surroundTile))
                {
                    /*��������Ѿ�����ӵ�δѡ���б�����·������*/
                    float newPathG = CalcG(surroundTile, minTile);
                    if (newPathG < surroundTile._temp_DistanceToFrom)
                    {
                        surroundTile._temp_DistanceToFrom = newPathG;
                        surroundTile._temp_DistanceMain = surroundTile._temp_DistanceToFrom + surroundTile.passOffset + surroundTile._temp_DistanceToTarget;
                        surroundTile._temp_fatherTile = minTile;
                    }
                }
                else
                {
                    /*������ӻ�δ����ӵ�δѡ���б���ӵ�ѡ���б�*/
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
    /// ��δѡ���б���������ĸ���
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
    /// ���Ŀ����Χ����
    /// </summary>
    /// <param name="from"></param>
    /// <returns></returns>
    private List<MyTile> FindSurroundTile(MyTile from)
    {
        List<MyTile> list = new List<MyTile>();

        MyTile up , down , right , left,
            rightUp , leftUp , rightDown , leftDown;

        up = (MyTile)tilemap.GetTile(from._posInCell + Vector3Int.up);
        down = (MyTile)tilemap.GetTile(from._posInCell + Vector3Int.down);
        right = (MyTile)tilemap.GetTile(from._posInCell + Vector3Int.right);
        left  = (MyTile)tilemap.GetTile(from._posInCell + Vector3Int.left);
        rightUp = (MyTile)tilemap.GetTile(from._posInCell + Vector3Int.right + Vector3Int.up);
        leftUp = (MyTile)tilemap.GetTile(from._posInCell + Vector3Int.left + Vector3Int.up);
        rightDown = (MyTile)tilemap.GetTile(from._posInCell + Vector3Int.right + Vector3Int.down);
        leftDown = (MyTile)tilemap.GetTile(from._posInCell + Vector3Int.left + Vector3Int.down);

        if (up != null)
        {
            if (up.passType != TilePassType.PassStop)
            {
                list.Add(up);
            }
        }
        if (down != null)
        {
            if (down.passType != TilePassType.PassStop)
            {
                list.Add(down);
            }
        }
        if (right != null)
        {
            if (right.passType != TilePassType.PassStop)
            {
                list.Add(right);
            }
        }
        if (left != null)
        {
            if(left.passType != TilePassType.PassStop)
            {
                list.Add(left);
            }
        }
        if (rightUp != null && right != null && up != null)
        {
            if (rightUp.passType != TilePassType.PassStop && right.passType != TilePassType.PassStop && up.passType != TilePassType.PassStop)
            {
                list.Add(rightUp);
            }
        }
        if (leftUp != null && left != null && up != null)
        {
            if (leftUp.passType != TilePassType.PassStop && left.passType != TilePassType.PassStop && up.passType != TilePassType.PassStop)
            {
                list.Add(leftUp);
            }
        }
        if (rightDown != null && right != null && down != null)
        {
            if (rightDown.passType != TilePassType.PassStop && right.passType != TilePassType.PassStop && down.passType != TilePassType.PassStop)
            {
                list.Add(rightDown);
            }
        }
        if (leftDown != null && left != null && down != null)
        {
            if (leftDown.passType != TilePassType.PassStop && left.passType != TilePassType.PassStop && down.passType != TilePassType.PassStop)
            {
                list.Add(leftDown);
            }
        }

        return list;
    }
    /// <summary>
    /// ���Ѿ�����ѡ���б�ĸ����Ƴ���Χ����
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
    /// ������СԤ��ֵG
    /// </summary>
    /// <param name="surround"></param>
    /// <param name="from"></param>
    /// <returns></returns>
    private float CalcG(MyTile surround,MyTile from)
    {
        return from._temp_DistanceToFrom /*+ from.passOffset*/ + Vector3.Distance(surround._posInCell, from._posInCell);
    }
    /// <summary>
    /// ����õ㵽�յ�F
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
        float f = g + h + now.passOffset;
        now._temp_DistanceMain = f;
        now._temp_DistanceToFrom = g;
        now._temp_DistanceToTarget = h;
    }
}
