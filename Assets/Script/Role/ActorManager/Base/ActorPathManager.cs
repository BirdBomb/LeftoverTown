using System;
using System.Collections;
using System.Collections.Generic;
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
    
    private Vector2 vector2_CurPos;
    /// <summary>
    /// 冻结时间
    /// </summary>
    private float time_Frezze = 0;
    private List<BuildingTile> buildingTiles_NearbyRecord = new List<BuildingTile>();
    private List<BuildingTile> buildingTiles_NearbyTemp = new List<BuildingTile>();
    /// <summary>
    /// 检查当前位置
    /// </summary>
    public void Local_CheckTile()
    {
        vector3Int_CurPos = MapManager.Instance.grid_Ground.WorldToCell(actorManager.transform.position);
        vector2_CurPos = MapManager.Instance.grid_Ground.CellToWorld(vector3Int_CurPos);
        if (!MapManager.Instance.GetGround(vector3Int_CurPos, out _)) 
        {
            actorManager.actionManager.Despawn(); 
        }
        if (vector3Int_LastPos != vector3Int_CurPos)
        {
            vector3Int_LastPos = vector3Int_CurPos;
            actorManager.AllClient_Listen_MoveMyself(vector3Int_CurPos);
            if(actorManager.actorAuthority.isState) actorManager.State_Listen_MoveMyself(vector3Int_CurPos);
            MessageBroker.Default.Publish(new GameEvent.GameEvent_AllClient_SomeoneMove
            {
                moveActor = actorManager,
                movePos = vector3Int_CurPos
            });
        }
    }
    /// <summary>
    /// 检查距离
    /// </summary>
    public void CheckDistance()
    {
        if (actorManager.actorAuthority.isState && actorManager.actorAuthority.isPlayer)
        {
            float distance = actorManager.transform.position.magnitude;
        }
    }
    /// <summary>
    /// 更新附近的地块
    /// </summary>
    public void UpdateNearbyBuilding() 
    {
        /*周围地块*/
        buildingTiles_NearbyTemp = MapManager.Instance.GetNearbyBuildings_FourSide(vector3Int_CurPos, Vector3Int.zero);
        if (MapManager.Instance.GetBuilding(vector3Int_CurPos,out BuildingTile buildingTile))
        {
            buildingTile.StandOnTileByActor(actorManager);
        }
        if (MapManager.Instance.GetGround(vector3Int_CurPos, out GroundTile groundTile))
        { 

            groundTile.StandOnTileByActor(actorManager);
        }
        /*剔除上次检测的地块*/
        for (int i = 0; i < buildingTiles_NearbyRecord.Count; i++)
        {
            if (buildingTiles_NearbyRecord[i] == null) { continue; }
            if (!buildingTiles_NearbyTemp.Contains(buildingTiles_NearbyRecord[i]))
            {
                buildingTiles_NearbyRecord[i].FarawayTileByActor(actorManager);
                buildingTiles_NearbyRecord.RemoveAt(i);
            }
        }
        /*添加本次加入的地块*/
        for (int i = 0; i < buildingTiles_NearbyTemp.Count; i++)
        {
            if (buildingTiles_NearbyTemp[i] == null) { continue; }
            if (buildingTiles_NearbyTemp[i].NearbyTileByActor(actorManager))
            {
                if (!buildingTiles_NearbyRecord.Contains(buildingTiles_NearbyTemp[i]))
                {
                    buildingTiles_NearbyRecord.Add(buildingTiles_NearbyTemp[i]);
                }
            }
        }
    }
    #region//寻路
    /// <summary>
    /// 目标路径(主机)
    /// </summary>
    private List<GroundTile> groundTiles_TargetPath = new List<GroundTile>();
    /// <summary>
    /// 目标地块(主机)
    /// </summary>
    private GroundTile groundTile_TargetTile = null;
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
    /// <summary>
    /// 执行路径(主机)
    /// </summary>
    /// <param name="dt"></param>
    public void State_RunningPath(float dt)
    {
        if (time_Frezze > 0) { time_Frezze -= dt; return; }
        if (groundTile_TargetTile)
        {
            Vector2 temp = Vector2.zero;
            /*已经在目标地块*/
            if (vector3Int_CurPos == groundTile_TargetTile.tilePos)
            {
                /*进一步校准位置*/
                if (actorManager.transform.position.x > vector2_CurPos.x + 0.2f + 0.5f)
                {
                    temp += new Vector2(-1, 0);
                }
                else if (actorManager.transform.position.x < vector2_CurPos.x - 0.2f + 0.5f)
                {
                    temp += new Vector2(1, 0);
                }
                if (actorManager.transform.position.y > vector2_CurPos.y + 0.2f + 0.5f)
                {
                    temp += new Vector2(0, -1);
                }
                else if (actorManager.transform.position.y < vector2_CurPos.y - 0.2f + 0.5f)
                {
                    temp += new Vector2(0, 1);
                }
                if (temp == Vector2.zero)
                {
                    /*到达路径点，检查*/
                    if (groundTiles_TargetPath.Count > 0)
                    {
                        /*路径还未结束*/
                        State_GoToNext(false);
                    }
                    else
                    {
                        /*路径已经结束*/
                        State_GoToNext(true);
                    }
                    return;
                }
            }
            else
            {
                /*还未到达路径点，前往路径点*/
                if (vector3Int_CurPos.x > groundTile_TargetTile.tilePos.x)
                {
                    temp += new Vector2(-1, 0);
                }
                else if (vector3Int_CurPos.x < groundTile_TargetTile.tilePos.x)
                {
                    temp += new Vector2(1, 0);
                }
                if (vector3Int_CurPos.y > groundTile_TargetTile.tilePos.y)
                {
                    temp += new Vector2(0, -1);
                }
                else if (vector3Int_CurPos.y < groundTile_TargetTile.tilePos.y)
                {
                    temp += new Vector2(0, 1);
                }
            }
            temp = temp.normalized;
            float commonSpeed = actorManager.actionManager.Client_GetSpeed();
            Vector2 velocity = new Vector2(temp.x * commonSpeed, temp.y * commonSpeed);
            Vector3 newPos = actorManager.transform.position + new UnityEngine.Vector3(velocity.x * dt, velocity.y * dt, 0);
            actorManager.actorNetManager.State_UpdateNetworkRigidbody(newPos, velocity.magnitude, dt);
        }
    }
    /// <summary>
    /// 设置路径(主机)
    /// </summary>
    public void State_SettingPath(List<GroundTile> path, Action callBack)
    {
        State_ArriveCallBack = callBack;
        groundTiles_TargetPath = path;
        State_PathLenght = groundTiles_TargetPath.Count;
        State_PathCompleted = -1;
        State_GoToNext(false);
    }
    public void State_ClearPath()
    {
        groundTiles_TargetPath.Clear();
        groundTile_TargetTile = null;
        State_PathLenght = 0;
        State_PathCompleted = -1;
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
            State_UpdateTargetTile(null);
            if (State_ArriveCallBack != null) 
            {
                State_ArriveCallBack.Invoke(); 
            }
        }
        else
        {
            State_PathCompleted++;
            State_UpdateTargetTile(groundTiles_TargetPath[0]);
            groundTiles_TargetPath.RemoveAt(0);
        }
    }
    /// <summary>
    /// 更新目标地块(主机)
    /// </summary>
    public void State_UpdateTargetTile(GroundTile groundTile)
    {
        groundTile_TargetTile = groundTile;
    }
    /// <summary>
    /// 移动至(坐标)
    /// </summary>
    /// <param name="targetPos"></param>
    /// <returns></returns>
    public bool State_MovePostion(Vector3Int targetPos,int maxStep = 100)
    {
        List<GroundTile> temp = NavManager.Instance.FindPath(targetPos, vector3Int_CurPos, maxStep);
        if (temp.Count > 0)
        {
            State_ClearPath();
            State_SettingPath(temp, null);
            return true;
        }
        else
        {
            return false;
        }
    }
    /// <summary>
    /// 移动至(坐标)
    /// </summary>
    /// <param name="targetPos">目标位置</param>
    /// <param name="callBack">抵达回调</param>
    /// <param name="maxStep">最大步数</param>
    /// <returns></returns>
    public bool State_MovePostion(Vector3Int targetPos, Action callBack, int maxStep = 100)
    {
        List<GroundTile> temp = NavManager.Instance.FindPath(targetPos, vector3Int_CurPos, maxStep);
        if (temp.Count > 0)
        {
            State_ClearPath();
            State_SettingPath(temp, callBack);
            return true;
        }
        else
        {
            return false;
        }
    }
    /// <summary>
    /// 移动至(方向)
    /// </summary>
    /// <param name="startPos">起点</param>
    /// <param name="targetDir">方向</param>
    /// <param name="minDistance">移动最小距离</param>
    /// <param name="maxDistance">移动最大距离</param>
    /// <returns></returns>
    public bool State_MoveDiraction(Vector3Int startPos, Vector3Int targetDir, int minDistance, int maxDistance, Action callBack = null)
    {
        int minDistanceNew = minDistance;
        for (int i = minDistance; i < maxDistance; i++)
        {
            if (MapManager.Instance.GetGround(startPos + targetDir * i, out GroundTile groundTile))
            {
                //目标地块存在
                if (groundTile.offset_Pass)
                {
                    //目标地块可通过
                    //更新最小移动距离
                    minDistanceNew = i;
                }
                else
                {
                    break;
                }
            }
            else { break; }
        }
        for (int i = minDistanceNew; i < maxDistance; i++)
        {
            if (State_MovePostion(startPos + targetDir * i, callBack))
            {
                return true;
            }
        }
        return false;
    }
    /// <summary>
    /// 移动至(区域)
    /// </summary>
    /// <param name="centerPos"></param>
    /// <param name="radio"></param>
    /// <param name=""></param>
    /// <returns></returns>
    public bool State_MoveArea(Vector3Int centerPos, int size, int dir_x, int dir_y, Action callBack = null)
    {
        Vector3Int offset = Vector3Int.zero;
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                offset.x = x * dir_x;
                offset.y = y * dir_y;
                if (State_MovePostion(centerPos + offset, callBack))
                {
                    return true;
                }
            }
        }
        return false;
    }
    /// <summary>
    /// 设置冻结时间
    /// </summary>
    /// <param name="time">冻结秒数</param>
    public void State_SetFrezzeTime(float time)
    {
        time_Frezze = time;
    }
    #endregion
}
