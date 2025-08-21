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
    public Vector3Int vector3Int_LastPos;
    public Vector3Int vector3Int_CurPos;
    private Vector2 vector2_CurPos;
    /// <summary>
    /// ����ʱ��
    /// </summary>
    private float time_Frezze = 0;
    private List<BuildingTile> buildingTiles_NearbyRecord = new List<BuildingTile>();
    private List<BuildingTile> buildingTiles_NearbyTemp = new List<BuildingTile>();
    /// <summary>
    /// ��鵱ǰλ��
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
    /// ������
    /// </summary>
    public void CheckDistance()
    {
        if (actorManager.actorAuthority.isState && actorManager.actorAuthority.isPlayer)
        {
            float distance = actorManager.transform.position.magnitude;
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
        if (time_Frezze > 0) { time_Frezze -= dt; return; }
        if (groundTile_TargetTile)
        {
            Vector2 temp = Vector2.zero;
            /*�Ѿ���Ŀ��ؿ�*/
            if (vector3Int_CurPos == groundTile_TargetTile.tilePos)
            {
                /*��һ��У׼λ��*/
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
            }
            else
            {
                /*��δ����·���㣬ǰ��·����*/
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
            float commonSpeed = actorManager.actorNetManager.Net_SpeedCommon * 0.1f;
            Vector2 velocity = new Vector2(temp.x * commonSpeed, temp.y * commonSpeed);
            Vector3 newPos = actorManager.transform.position + new UnityEngine.Vector3(velocity.x * dt, velocity.y * dt, 0);
            actorManager.actorNetManager.State_UpdateNetworkRigidbody(newPos, velocity.magnitude);
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
    /// �ƶ�
    /// </summary>
    /// <param name="targetPos"></param>
    /// <returns></returns>
    public bool State_MovePostion(Vector3Int targetPos,int maxStep = 100)
    {
        groundTiles_TargetPath.Clear();
        List<GroundTile> temp = MapManager.Instance.navManager.FindPath(targetPos, vector3Int_CurPos, maxStep);
        if (temp.Count > 0)
        {
            groundTiles_TargetPath = temp;
            State_UpdateTargetTile(groundTiles_TargetPath[0]);
            groundTiles_TargetPath.RemoveAt(0);
            return true;
        }
        else
        {
            return false;
        }
    }
    /// <summary>
    /// ͣ��
    /// </summary>
    /// <param name="time">��������</param>
    public void State_StandDown(float time)
    {
        time_Frezze = time;
        groundTiles_TargetPath.Clear();
        groundTile_TargetTile = null;
    }
    #endregion
}
