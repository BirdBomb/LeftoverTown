using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class ActorPathManager 
{
    private ActorManager actorManager;
    public void Bind(ActorManager actorManager)
    {
        this.actorManager = actorManager;
    }
    private GroundTile groundTile_LastRecord = null;
    private GroundTile groundTile_CurRecord = null;
    private BuildingTile buildingTile_LastRecord = null;
    private BuildingTile buildingTile_CurRecord = null;

    public Vector3Int vector3Int_LastPos;
    public Vector3Int vector3Int_CurPos;

    private List<BuildingTile> buildingTiles_NearbyRecord = new List<BuildingTile>();
    private List<BuildingTile> buildingTiles_NearbyTemp = new List<BuildingTile>();
    /// <summary>
    /// ��õ�ǰ�ؿ�
    /// </summary>
    public BuildingTile GetBuildingTile()
    {
        if (buildingTile_CurRecord)
        {
            return buildingTile_CurRecord;
        }
        else
        {
            MapManager.Instance.GetBuilding(actorManager.transform.position,out buildingTile_CurRecord);
            return buildingTile_CurRecord;
        }
    }
    /// <summary>
    /// ��鵱ǰλ��
    /// </summary>
    public void AllClient_CheckTile()
    {
        vector3Int_CurPos = MapManager.Instance.grid_Ground.WorldToCell(actorManager.transform.position);
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
    /// ������
    /// </summary>
    public void CheckDistance()
    {
        if (actorManager.actorAuthority.isState && actorManager.actorAuthority.isPlayer)
        {
            float distance = actorManager.transform.position.magnitude;
            if (distance > WorldManager.Instance.int_Distance)
            {
                if (!actorManager.buffManager.CheckBuff(200))
                {
                    actorManager.actorNetManager.RPC_LocalInput_AddBuff(200, "");
                }
            }
            else
            {
                if (actorManager.buffManager.CheckBuff(200))
                {
                    actorManager.actorNetManager.RPC_LocalInput_SubBuff(200);
                }
            }
        }
    }
    /// <summary>
    /// ���¸����ĵؿ�
    /// </summary>
    public void UpdateNearbyBuilding() 
    {
        /*��Χ�ؿ�*/
        buildingTiles_NearbyTemp = MapManager.Instance.GetNearbyBuildings_FourSide(vector3Int_CurPos, Vector3Int.zero);
        if (MapManager.Instance.GetBuilding(vector3Int_CurPos,out BuildingTile buildingTile))
        {
            buildingTile.StandOnTileByActor(actorManager);
        }
        /*�޳��ϴμ��ĵؿ�*/
        for (int i = 0; i < buildingTiles_NearbyRecord.Count; i++)
        {
            if (buildingTiles_NearbyRecord[i] == null) { continue; }
            if (!buildingTiles_NearbyTemp.Contains(buildingTiles_NearbyRecord[i]))
            {
                buildingTiles_NearbyRecord[i].FarawayTileByActor(actorManager);
                buildingTiles_NearbyRecord.RemoveAt(i);
            }
        }
        /*��ӱ��μ���ĵؿ�*/
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
    #region//Ѱ·
    /// <summary>
    /// Ŀ��·��(����)
    /// </summary>
    private List<GroundTile> groundTiles_TargetPath = new List<GroundTile>();
    /// <summary>
    /// Ŀ��ؿ�(����)
    /// </summary>
    private GroundTile groundTile_TargetTile = null;
    /// <summary>
    /// ִ��·��(����)
    /// </summary>
    /// <param name="dt"></param>
    public void State_RunningPath(float dt)
    {
        if (groundTile_TargetTile)
        {
            if (Vector2.Distance(groundTile_TargetTile.tileWorldPos, actorManager.transform.position) <= 0.1f)
            {
                /*����·���㣬���*/
                if (groundTiles_TargetPath.Count > 0)
                {
                    /*·����δ����*/
                    State_UpdateTargetTile(groundTiles_TargetPath[0]);
                    groundTiles_TargetPath.RemoveAt(0);
                }
                else
                {
                    /*·���Ѿ�����*/
                    State_UpdateTargetTile(null);
                }
                return;
            }
            else
            {
                /*��δ����·���㣬ǰ��·����*/
                Vector2 temp = Vector2.zero;
                if (actorManager.transform.position.x > groundTile_TargetTile.tileWorldPos.x)
                {
                    if ((actorManager.transform.position.x - groundTile_TargetTile.tileWorldPos.x) > 0.05f)
                    {
                        temp += new Vector2(-1, 0);
                    }
                }
                else
                {
                    if ((actorManager.transform.position.x - groundTile_TargetTile.tileWorldPos.x) < -0.05f)
                    {
                        temp += new Vector2(1, 0);
                    }
                }
                if (actorManager.transform.position.y > groundTile_TargetTile.tileWorldPos.y)
                {
                    if ((actorManager.transform.position.y - groundTile_TargetTile.tileWorldPos.y) > 0.05f)
                    {
                        temp += new Vector2(0, -1);
                    }
                }
                else
                {
                    if ((actorManager.transform.position.y - groundTile_TargetTile.tileWorldPos.y) < -0.05f)
                    {
                        temp += new Vector2(0, 1);
                    }
                }
                temp = temp.normalized;

                float commonSpeed = actorManager.actorNetManager.Net_SpeedCommon / 10f;

                Vector2 velocity = new Vector2(temp.x * commonSpeed, temp.y * commonSpeed);
                Vector3 newPos = actorManager.transform.position + new UnityEngine.Vector3(velocity.x * dt, velocity.y * dt, 0);
                actorManager.actorNetManager.OnlyState_UpdateNetworkRigidbody(newPos, velocity.magnitude);
            }
        }
    }
    /// <summary>
    /// ����Ŀ��ؿ�(����)
    /// </summary>
    public void State_UpdateTargetTile(GroundTile groundTile)
    {
        groundTile_TargetTile = groundTile;
    }
    /// <summary>
    /// �����ƶ�
    /// </summary>
    /// <param name="targetPos"></param>
    /// <returns></returns>
    public bool State_MoveTo(Vector2 targetPos)
    {
        return State_MoveTo(MapManager.Instance.grid_Ground.WorldToCell(targetPos));
    }
    public bool State_MoveTo(Vector3Int targetPos)
    {
        groundTiles_TargetPath.Clear();
        if (MapManager.Instance.GetGround(targetPos, out GroundTile groundTile))
        {
            List<GroundTile> temp = MapManager.Instance.navManager.FindPath(groundTile, groundTile_CurRecord);
            if (temp.Count > 0)
            {
                groundTiles_TargetPath = temp;
                State_UpdateTargetTile(groundTiles_TargetPath[0]);
                groundTiles_TargetPath.RemoveAt(0);
                return true;
            }
            else
            {
                Debug.Log(temp);
                return false;
            }

        }
        else
        {
            Debug.Log("����");
            return false;
        }
    }
    #endregion
}
