using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public class WorldEventManager : SingleTon<WorldEventManager>, ISingleTon
{
    private System.Random random = new System.Random();
    private List<Vector3Int> cachedPositions = new List<Vector3Int>();
    public void Init()
    {

    }
    public void StartMorning()
    {

    }
    public void InMorning()
    {

    }
    public void StartForenoon()
    {

    }
    public void InForenoon()
    {

    }
    public void StartHighnoon()
    {

    }
    public void InHighnoon()
    {

    }
    public void StartAfternoon()
    {

    }
    public void InAfternoon()
    {

    }
    public void StartDusk()
    {

    }
    public void InDusk()
    {

    }
    public void StartEvening() 
    {
        Event_ZombieComing();
    }
    public void InEvening()
    {
        
    }
    private void Event_ZombieComing()
    {
        if (!WorldManager.Instance.gameNetManager.Object.HasStateAuthority)
            return;

        var actorsList = WorldActorManager.Instance.GetActorList().ToList();
        foreach (var actor in actorsList)
        {
            if (!actor.actorAuthority.isPlayer)
                continue;

            // 尝试在玩家周围找位置生成僵尸
            if (FindSpawnPositionAround(actor.pathManager.vector3Int_CurPos, 13, 15, out Vector3Int spawnPos))
            {
                SpawnActor("Actor/Zombie_Runner", spawnPos, actor.pathManager.vector3Int_CurPos);
            }
        }
    }
    private void SpawnActor(string ActorName,Vector3Int spawnPos, Vector3Int targetPos)
    {
        MessageBroker.Default.Publish(new GameEvent.GameEvent_State_SpawnActor()
        {
            name = ActorName,
            pos = spawnPos,
            callBack = (actor) =>
            {
                actor.GetComponent<ActorManager>().brainManager.ForState_SetActivityPos(targetPos);
            }
        });
    }
    public bool GetEnemyPos(Vector3Int targetPos, int distance, out Vector3Int enemyPos)
    {
        List<Vector3Int> postions = GetExactRadiusPoints(targetPos, distance);
        for (int k = 0; k < postions.Count; k++)
        {
            if (!MapManager.Instance.GetBuilding(postions[k], out _) && MapManager.Instance.GetGround(postions[k], out GroundTile ground))
            {
                if (ground.tileID < 2000)
                {
                    enemyPos = postions[k];
                    return true;
                }
            }
        }
        enemyPos = Vector3Int.zero;
        return false;
    }
    List<Vector3Int> GetExactRadiusPoints(Vector3Int center, int radius)
    {
        List<Vector3Int> points = new List<Vector3Int>();
        int radiusSquared = radius * radius;

        for (int x = -radius; x <= radius; x++)
        {
            int remaining = radiusSquared - x * x;

            if (remaining >= 0)
            {
                int y = (int)Mathf.Sqrt(remaining);

                if (y * y == remaining && y != 0)
                {
                    points.Add(new Vector3Int(center.x + x, center.y + y));
                    points.Add(new Vector3Int(center.x + x, center.y - y));

                    if (x != 0)
                    {
                        points.Add(new Vector3Int(center.x + y, center.y + x));
                        points.Add(new Vector3Int(center.x - y, center.y + x));
                    }
                }
            }
        }

        return points;
    }
    private bool FindSpawnPositionAround(Vector3Int center, int minRadius, int maxRadius, out Vector3Int result)
    {
        // 从近到远搜索（跳过最小半径）
        for (int r = minRadius; r <= maxRadius; r++)
        {
            if (TryGetRingPositions(center, r, cachedPositions))
            {
                // 随机打乱顺序，避免每次都生成在同一方向
                ShuffleList(cachedPositions);

                foreach (var pos in cachedPositions)
                {
                    if (IsValidSpawnPosition(pos))
                    {
                        result = pos;
                        return true;
                    }
                }
            }
        }

        result = Vector3Int.zero;
        return false;
    }
    /// <summary>
    /// 获取网格上的圆环位置（整数坐标）
    /// </summary>
    private bool TryGetRingPositions(Vector3Int center, int radius, List<Vector3Int> output)
    {
        output.Clear();

        // 使用 Bresenham 圆算法获取所有在圆环上的整数点
        int x = 0;
        int y = radius;
        int d = 3 - 2 * radius;

        while (y >= x)
        {
            // 八个对称点
            AddRingPoint(center.x + x, center.y + y, output);
            AddRingPoint(center.x - x, center.y + y, output);
            AddRingPoint(center.x + x, center.y - y, output);
            AddRingPoint(center.x - x, center.y - y, output);
            AddRingPoint(center.x + y, center.y + x, output);
            AddRingPoint(center.x - y, center.y + x, output);
            AddRingPoint(center.x + y, center.y - x, output);
            AddRingPoint(center.x - y, center.y - x, output);

            x++;

            if (d > 0)
            {
                y--;
                d = d + 4 * (x - y) + 10;
            }
            else
            {
                d = d + 4 * x + 6;
            }
        }

        return output.Count > 0;
    }
    private void AddRingPoint(int x, int y, List<Vector3Int> list)
    {
        var pos = new Vector3Int(x, y);
        // 简单去重（Bresenham可能会有重复点）
        if (!list.Contains(pos))
        {
            list.Add(pos);
        }
    }
    /// <summary>
    /// 随机打乱列表
    /// </summary>
    private void ShuffleList<T>(List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            (list[k], list[n]) = (list[n], list[k]);
        }
    }
    /// <summary>
    /// 是否是可用地块
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    private bool IsValidSpawnPosition(Vector3Int pos)
    {
        // 不能有建筑
        if (MapManager.Instance.GetBuilding(pos, out _))
            return false;

        // 必须有地面，且是可走的地面类型
        if (!MapManager.Instance.GetGround(pos, out GroundTile ground))
            return false;

        return ground.tileID < 2000;
    }
}
