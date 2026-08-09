using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public class ActorPathManager 
{
    private ActorManager actorManager;
    public void Bind(ActorManager actorManager)
    {
        this.actorManager = actorManager;
    }
    public Vector3Int vector3Int_LastPos;
    public Vector3Int vector3Int_CurPos;
    
    private GroundTile groundTile_StandOn;
    private BuildingTile buildingTile_StandOn;

    private float float_StandingInSameTileTimer;//站立不动计时器
    private const float float_StandingInSameTileMaxTime = 1;//站立不动重设路径时间
    private float float_GroundSpeedOffset = 1;//地板速度系数
    private List<BuildingTile> buildingTiles_NearbyRecord = new List<BuildingTile>();
    /// <summary>
    /// 检查当前位置
    /// </summary>
    public void ForAll_CheckTile(float dt)
    {
        vector3Int_CurPos = MapManager.Instance.grid_Ground.WorldToCell(actorManager.transform.position);
        if (vector3Int_LastPos != vector3Int_CurPos)
        {
            vector3Int_LastPos = vector3Int_CurPos;
            float_StandingInSameTileTimer = 0;
            ForAll_UpdateNearbyBuildings();
            ForAll_UpdateStandBuilding();
            ForAll_UpdateStandGround();
            actorManager.ForAll_Listen_MoveMyself(vector3Int_CurPos);
            MessageBroker.Default.Publish(new GameEvent.GameEvent_AllClient_SomeoneMove
            {
                moveActor = actorManager,
                movePos = vector3Int_CurPos
            });
        }
        else
        {
            float_StandingInSameTileTimer += dt;
        }
    }
    public void ForState_CheckTile()
    {
        if (float_StandingInSameTileTimer > float_StandingInSameTileMaxTime)
        {
            float_StandingInSameTileTimer = 0;
            State_ClearPath();
        }
    }
    public float ForAll_GetSpeedOffset()
    {
        return float_GroundSpeedOffset;
    }
    public void ForAll_UpdateStandGround()
    {
        if (MapManager.Instance.GetGround(vector3Int_CurPos, out groundTile_StandOn))
        {
            groundTile_StandOn.StandOnTileByActor(actorManager);
            float_GroundSpeedOffset = groundTile_StandOn.GetSpeedOffset();
        }
        else 
        {
            if(!actorManager.actorAuthority.isPlayer) actorManager.actionManager.Despawn();
        }
    }
    public void ForAll_UpdateStandBuilding()
    {
        if (MapManager.Instance.GetBuilding(vector3Int_CurPos,out buildingTile_StandOn))
        {
            buildingTile_StandOn.StandOnTileByActor(actorManager);
        }
    }
    public void ForAll_UpdateNearbyBuildings()
    {
        // 获取当前周围地块
        var currentNearby = MapManager.Instance.GetNearbyBuildings(vector3Int_CurPos, Vector3Int.zero, DirectionType.Four);
        // 移除离开的地块
        var toRemove = buildingTiles_NearbyRecord.Except(currentNearby).ToList();
        foreach (var tile in toRemove)
        {
            tile.FarawayTileByActor(actorManager);
            buildingTiles_NearbyRecord.Remove(tile);
        }

        // 添加新进入的地块
        foreach (var tile in currentNearby)
        {
            if (tile == null || buildingTiles_NearbyRecord.Contains(tile)) continue;

            if (tile.NearbyTileByActor(actorManager))
                buildingTiles_NearbyRecord.Add(tile);
        }
    }
    #region//寻路
    private float float_PathFrezzeTime = 0;

    /// <summary>
    /// 目标路径(主机)
    /// </summary>
    private Queue<Vector3Int> State_Path = new Queue<Vector3Int>();
    /// <summary>
    /// 路径终点(主机)
    /// </summary>
    private Vector3Int State_TargetPos = new Vector3Int(int.MaxValue, int.MaxValue, 0);
    /// <summary>
    /// 目标地块(主机)
    /// </summary>
    private Vector3Int State_NextPos = new Vector3Int(int.MaxValue, int.MaxValue, 0);
    /// <summary>
    /// 目标地块可用
    /// </summary>
    private bool State_NextPosIsValue = false;
    /// <summary>
    /// 路径(总长度)
    /// </summary>
    public int State_PathLenght;
    /// <summary>
    /// 路径(已完成)
    /// </summary>
    public int State_PathCompleted;
    /// <summary>
    /// 完成回调
    /// </summary>
    public Action State_ArriveCallBack;
    /// <summary>
    /// 检查剩余路径百分比(主机)
    /// </summary>
    /// <returns>剩余百分比</returns>
    public float State_CheckRemainingPathProportion()
    {
        if(State_PathLenght == 0) { return 1; }
        else
        {
            return (float)(State_PathCompleted + 1) / (float)State_PathLenght;
        }
    }
    /// <summary>
    /// 检查剩余路径数量(主机)
    /// </summary>
    /// <returns>剩余数量</returns>
    public int State_CheckRemainingPathCount()
    {
        if (State_PathLenght == 0) { return 0; }
        else
        {
            return State_PathLenght - State_PathCompleted;
        }
    }
    public bool State_CheckTargetPos(Vector3Int pos)
    {
        return State_TargetPos.Equals(pos);
    }
    /// <summary>
    /// 执行路径(主机)
    /// </summary>
    /// <param name="dt"></param>
    public void State_RunningPath(float dt)
    {
        if (float_PathFrezzeTime > 0) 
        {
            float_PathFrezzeTime -= dt; 
            return; 
        }
        if (State_NextPosIsValue)
        {
            Vector2 temp = Vector2.zero;
            /*已经在目标地块*/
            if (vector3Int_CurPos == State_NextPos)
            {
                /*进一步校准位置*/
                if (actorManager.transform.position.x > vector3Int_CurPos.x + 0.2f + 0.5f)
                {
                    temp += new Vector2(-1, 0);
                }
                else if (actorManager.transform.position.x < vector3Int_CurPos.x - 0.2f + 0.5f)
                {
                    temp += new Vector2(1, 0);
                }
                if (actorManager.transform.position.y > vector3Int_CurPos.y + 0.2f + 0.5f)
                {
                    temp += new Vector2(0, -1);
                }
                else if (actorManager.transform.position.y < vector3Int_CurPos.y - 0.2f + 0.5f)
                {
                    temp += new Vector2(0, 1);
                }
                if (temp == Vector2.zero)
                {
                    /*到达路径点，检查*/
                    if (State_Path.Count > 0)
                    {
                        /*路径还未结束*/
                        State_GoToNext(false);
                    }
                    else
                    {
                        /*路径已经结束*/
                        State_GoToNext(true);
                    }
                    //return;
                }
            }
            else
            {
                /*还未到达路径点，前往路径点*/
                if (vector3Int_CurPos.x > State_NextPos.x)
                {
                    temp += new Vector2(-1, 0);
                }
                else if (vector3Int_CurPos.x < State_NextPos.x)
                {
                    temp += new Vector2(1, 0);
                }
                if (vector3Int_CurPos.y > State_NextPos.y)
                {
                    temp += new Vector2(0, -1);
                }
                else if (vector3Int_CurPos.y < State_NextPos.y)
                {
                    temp += new Vector2(0, 1);
                }
            }
            actorManager.actorNetManager.State_MoveNetworkRigidbody(temp, dt);
        }
    }
    /// <summary>
    /// 设置路径(主机)
    /// </summary>
    public void State_SettingPath(List<Vector3Int> path, Action callBack)
    {
        State_ArriveCallBack = callBack;
        path.RemoveAt(0);
        foreach (Vector3Int pos in path)
        {
            State_Path.Enqueue(pos);
        }
        State_TargetPos = path.Count > 0 ? path[path.Count - 1] : new Vector3Int(int.MaxValue, int.MaxValue, 0);
        State_PathLenght = State_Path.Count;
        State_PathCompleted = -1;
        State_GoToNext(false);
    }
    public void State_ClearPath()
    {
        State_Path.Clear();
        State_NextPos = new Vector3Int(int.MaxValue, int.MaxValue, 0);
        State_TargetPos = new Vector3Int(int.MaxValue, int.MaxValue, 0);
        State_PathLenght = 0;
        State_PathCompleted = -1;
        State_ClearTargetTile();
    }
    /// <summary>
    /// 前往下一个目标点(主机)
    /// </summary>
    /// <param name="ending">抵达终点</param>
    public void State_GoToNext(bool ending)
    {
        if (ending)
        { 
            State_PathCompleted = State_PathLenght;
            State_ClearTargetTile();
            if (State_ArriveCallBack != null) 
            {
                State_ArriveCallBack.Invoke(); 
            }
        }
        else
        {
            State_PathCompleted++;
            Vector3Int pos = State_Path.Dequeue();
            State_UpdateTargetTile(pos);
        }
    }
    /// <summary>
    /// 更新目标地块(主机)
    /// </summary>
    public void State_UpdateTargetTile(Vector3Int groundTile)
    {
        State_NextPos = groundTile;
        State_NextPosIsValue = true;
    }
    /// <summary>
    /// 清除目标地块(主机)
    /// </summary>
    public void State_ClearTargetTile()
    {
        State_NextPosIsValue = false;
    }
    /// <summary>
    /// 移动(短途)
    /// </summary>
    /// <param name="targetPos">目标位置</param>
    /// <param name="callBack">抵达回调</param>
    /// <param name="maxStep">最大步数</param>
    /// <returns></returns>
    public bool State_MoveShort(Vector3Int targetPos, Action callBack)
    {
        if (State_CheckTargetPos(targetPos)) { return true; }//已经在路上
        List<Vector3Int> temp = NavManager.Instance.FindPath(targetPos, vector3Int_CurPos, 200);
        if (temp.Count > 0)
        {
            State_ClearPath();
            State_SettingPath(temp, callBack);
            NavManager.Instance.ReturnListToPool(temp);
            return true;
        }
        else
        {
            return false;
        }
    }
    /// <summary>
    /// 移动(长途)
    /// </summary>
    /// <param name="targetPos">目标地点</param>
    /// <param name="range">目标区域范围</param>
    /// <param name="callBack">抵达回调</param>
    /// <returns></returns>
    public bool State_MoveLong(Vector3Int targetPos, int range, Action callBack = null)
    {
        if (State_CheckTargetPos(targetPos)) { return true; }//已经在路上
        float distance = (targetPos - vector3Int_CurPos).sqrMagnitude;
        if (distance < 100)//我离目标点够近,直接去
        {
            if (State_MoveShort(targetPos, callBack)) return true;
        }

        while ((targetPos - vector3Int_CurPos).sqrMagnitude > 100)
        {
            targetPos = (targetPos + vector3Int_CurPos) / 2;
        }
        Vector3Int offset = Vector3Int.zero;
        for (int x = -range; x < range; x++)
        {
            for (int y = -range; y < range; y++)
            {
                offset.x = x; offset.y = y;
                if (State_MoveShort(targetPos + offset, callBack))
                {
                    return true;
                }
            }
        }
        return false;
    }
    public void State_Stop(float time)
    {
        float_PathFrezzeTime = time;
        State_ClearPath();
    }
    public void State_Continue(float time = 0)
    {
        float_PathFrezzeTime = time;
        State_ClearPath();
    }
    #endregion
}
