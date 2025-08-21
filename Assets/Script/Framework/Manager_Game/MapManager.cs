using DG.Tweening;
using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Lifetime;
using UniRx;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;
using UnityEngine.U2D;
using UnityEngine.UIElements;

public class MapManager : SingleTon<MapManager>,ISingleTon
{
    [Header("����map")]
    public Tilemap tilemap_Ground;
    [Header("����Grid")]
    public Grid grid_Ground;
    [Header("����map")]
    public Tilemap tilemap_Building;
    [Header("����grid")]
    public Grid grid_Building;
    [Header("����pool")]
    public Transform transform_BuildingPool;
    [Header("Ѱ·")]
    public NavManager navManager;
    [Header("�������")]
    public MapNetManager mapNetManager;

    [HideInInspector]
    public int mapSeed;
    /// <summary>
    /// ������ҵ�λ��
    /// </summary>
    private Dictionary<PlayerRef,Vector3Int> playerCenterDic = new Dictionary<PlayerRef,Vector3Int>();
    /// <summary>
    /// ��������
    /// </summary>
    private List<ActorManager> actorManagers = new List<ActorManager>();
    /// <summary>
    /// �������
    /// </summary>
    private List<PlayerRef> playerRefs = new List<PlayerRef>();
    /// <summary>
    /// �Ѿ����صĽ�������
    /// </summary>
    private List<Vector3Int> areaList_Building = new List<Vector3Int>();
    /// <summary>
    /// �Ѿ����صĵؿ�����
    /// </summary>
    private List<Vector3Int> areaList_Ground = new List<Vector3Int>();
    private Dictionary<int, GroundTile> GroundTilePool = new Dictionary<int, GroundTile>();
    private Dictionary<int, BuildingTile> BuildingTilePool = new Dictionary<int, BuildingTile>();
    /// <summary>
    /// �����Ұ����
    /// </summary>
    private const float config_MaxView = 200f;
    public void Init()
    {
    }
    /*�����������*/
    #region
    /// <summary>
    /// ���µ�ͼ����������
    /// </summary>
    public void UpdatePlayerCenterInMap(PlayerRef player, Vector3Int center, int width, int height, int seed)
    {
        mapSeed = seed;
        if (playerCenterDic.ContainsKey(player)) 
        { 
            ChangePlayerCenter(player, center); 
        }
        else
        {
            AddPlayerCenter(player, center);
        }
        CheckPlayerCenter(width, height);
    }
    /// <summary>
    /// ���һ���������
    /// </summary>
    /// <param name="player"></param>
    /// <param name="center"></param>
    private void AddPlayerCenter(PlayerRef player,Vector3Int center)
    {
        Debug.Log("��ͼ�����һ���������" + player + "pos" + center);
        playerCenterDic.Add(player, center);
        playerRefs.Add(player);
    }
    /// <summary>
    /// �޸�һ���������
    /// </summary>
    /// <param name="player"></param>
    /// <param name="center"></param>
    private void ChangePlayerCenter(PlayerRef player, Vector3Int center)
    {
        playerCenterDic[player] = center;
    }
    /// <summary>
    /// ��������������
    /// </summary>
    /// <param name="player"></param>
    /// <param name="center"></param>
    private void CheckPlayerCenter(int width, int height)
    {
        Debug.Log("-----------------------" + width + "/" + height);
        List<Vector3Int> cullingList = new List<Vector3Int>();
        /*���������Ѿ����Ƶ����򲢼�¼�������κ���ҵ�����*/
        for (int i = 0; i < areaList_Ground.Count; i++)
        {
            bool culling = true;
            for (int j = 0; j < playerRefs.Count; j++)
            {
                if (Vector3Int.Distance(areaList_Ground[i], playerCenterDic[playerRefs[j]]) <= config_MaxView)
                {
                    culling = false;    
                }
                else
                {
                    Debug.Log("������Ұ");
                }
            }
            if (culling)
            {
                Vector3Int temp = areaList_Building[i];
                SubAreaInMap(temp, width, height);
                cullingList.Add(temp);
            }
        }
        /*���������Ѿ����Ƶ������޳��Ѿ���¼������*/
        for (int i = 0; i < cullingList.Count; i++)
        {
            areaList_Ground.Remove(cullingList[i]);
            areaList_Building.Remove(cullingList[i]);
        }
    }
    #endregion
    /*ȥ��һ������*/
    #region
    /// <summary>
    /// ȥ��һ�����ƺõ�����
    /// </summary>
    /// <param name="center"></param>
    /// <param name="widht"></param>
    /// <param name="height"></param>
    public void SubAreaInMap(Vector3Int center, int widht, int height)
    {
        int index = 0;
        for (int x = -widht / 2; x < widht / 2; x++)
        {
            for (int y = -height / 2; y < height / 2; y++)
            {
                Vector3Int vector3 = new Vector3Int(center.x + x, center.y + y, 0);
                DeleteBuilding(vector3);
                DeleteGround(vector3);
                index++;
            }
        }
    }

    #endregion
    /*����һ������*/
    #region
    /// <summary>
    /// ��������һ��δ���Ƶ�����
    /// </summary>
    /// <param name="center"></param>
    /// <returns>���Ի���</returns>
    public bool AddBuildingAreaInMap(Vector3Int center)
    {
        if (!areaList_Building.Contains(center))
        {
            areaList_Building.Add(center);
            return true;
        }
        return false;
    }
    /// <summary>
    /// ��������һ��δ���Ƶ�����
    /// </summary>
    /// <param name="center"></param>
    /// <returns>���Ի���</returns>
    public bool AddGroundAreaInMap(Vector3Int center)
    {
        if (!areaList_Ground.Contains(center))
        {
            areaList_Ground.Add(center);
            return true;
        }
        return false;
    }
    #endregion
    #region//����
    /// <summary>
    /// ���ɵ���
    /// </summary>
    public void CreateGround(int id, Vector3Int pos)
    {
        GroundTile groundTile = ScriptableObject.CreateInstance<GroundTile>();
        groundTile.InitData(GetGroundConfig(id), pos, id);
        if (tilemap_Ground.HasTile(pos)) DeleteGround(pos);
        tilemap_Ground.SetTile(pos, groundTile);
        groundTile.BindObj(GetGroundObj(pos));

        GameUI_MiniMap.Instance.ChangeGroundInMap(id, pos);
    }
    /// <summary>
    /// ɾ������
    /// </summary>
    /// <param name="tilePos"></param>
    public void DeleteGround(Vector3Int tilePos)
    {
        GroundTile groundTile = tilemap_Ground.GetTile<GroundTile>(tilePos);
        if (groundTile)
        {
            Destroy(tilemap_Ground.GetInstantiatedObject(tilePos));
            Destroy(groundTile);
        }
        tilemap_Ground.SetTile(tilePos, null);
    }
    /// <summary>
    /// ���Ƶ���
    /// </summary>
    /// <param name="center"></param>
    /// <param name="widht"></param>
    /// <param name="height"></param>
    public void DrawGround(Vector3Int center, int widht, int height)
    {
        for (int x = -(widht / 2); x <= (widht / 2); x++)
        {
            for (int y = -(height / 2); y <= (height / 2); y++)
            {
                int posX = center.x + x;
                int posY = center.y + y;
                if (GetGround(new Vector3Int(posX, posY, 0), out GroundTile groundTile))
                {
                    groundTile.tileObj.Draw();
                }
            }
        }
    }
    /// <summary>
    /// ��õ���
    /// </summary>
    /// <param name="tilePos"></param>
    /// <returns></returns>
    public bool GetGround(Vector3Int tilePos, out GroundTile groundTile)
    {
        groundTile = tilemap_Ground.GetTile<GroundTile>(tilePos);
        if (groundTile)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    /// <summary>
    /// ��õ���ʵ��
    /// </summary>
    /// <param name="tilePos"></param>
    /// <returns></returns>
    public GameObject GetGroundObj(Vector3Int tilePos)
    {
        return tilemap_Ground.GetInstantiatedObject(tilePos);
    }
    /// <summary>
    /// ��õ�������
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    private GroundTile GetGroundConfig(int id)
    {
        if (GroundTilePool.ContainsKey(id)) { return GroundTilePool[id]; }
        else
        {
            GroundTile config = Resources.Load<GroundTile>("TileScript/Ground/" + id);
            if (config != null)
            {
                GroundTilePool.Add(id, config);
            }
            return config;
        }
    }
    /// <summary>
    /// �����Χ
    /// </summary>
    /// <param name="id"></param>
    /// <param name="pos"></param>
    /// <returns></returns>
    public Around CheckGround(int id, Vector3Int pos)
    {
        Around around = new Around();
        GroundTile ground;
        around.C = (GetGround(pos, out ground) && ground.tileID == id) ? true : false;
        around.U = (GetGround(pos + Vector3Int.up, out ground) && ground.tileID == id) ? true : false;
        around.D = (GetGround(pos + Vector3Int.down, out ground) && ground.tileID == id) ? true : false;
        around.L = (GetGround(pos + Vector3Int.left, out ground) && ground.tileID == id) ? true : false;
        around.R = (GetGround(pos + Vector3Int.right, out ground) && ground.tileID == id) ? true : false;
        around.UR = (GetGround(pos + Vector3Int.up + Vector3Int.right, out ground) && ground.tileID == id) ? true : false;
        around.UL = (GetGround(pos + Vector3Int.up + Vector3Int.left, out ground) && ground.tileID == id) ? true : false;
        around.DL = (GetGround(pos + Vector3Int.down + Vector3Int.left, out ground) && ground.tileID == id) ? true : false;
        around.DR = (GetGround(pos + Vector3Int.down + Vector3Int.right, out ground) && ground.tileID == id) ? true : false;
        return around;
    }
    #endregion
    #region//����
    /// <summary>
    /// ���ɽ���
    /// </summary>
    /// <param name="tilePos"></param>
    /// <param name="tileName"></param>
    public void CreateBuilding(int id, Vector3Int tilePos, out BuildingTile buildingTile)
    {
        if (tilemap_Building.HasTile(tilePos)) DeleteBuilding(tilePos);
        if (id > 0)
        {
            buildingTile = ScriptableObject.CreateInstance<BuildingTile>();
            buildingTile.InitData(GetBuildingConfig(id), tilePos, id);
            tilemap_Building.SetTile(tilePos, buildingTile);
            UpdateGroundDrag(tilePos, buildingTile.config_Pass, buildingTile.config_Drag);
            buildingTile.BindObj(GetBuildingObj(tilePos));
        }
        else
        {
            buildingTile = null;
        }
    }
    private void UpdateGroundDrag(Vector3Int pos,bool pass,int drag)
    {
        if (GetGround(pos, out GroundTile groundTile))
        {
            groundTile.offset_Pass = pass && groundTile.config_Pass;
            groundTile.offset_Drag = drag + groundTile.config_Drag;  
        }
    }
    /// <summary>
    /// ɾ������
    /// </summary>
    /// <param name="tilePos"></param>
    public void DeleteBuilding(Vector3Int tilePos)
    {
        BuildingTile buildingTile = tilemap_Building.GetTile<BuildingTile>(tilePos);
        if (buildingTile)
        {
            Destroy(tilemap_Building.GetInstantiatedObject(tilePos));
            Destroy(buildingTile);
            DeleteBuildingPlaceHolding(tilePos, BuildingConfigData.GetBuildingConfig(buildingTile.tileID).Building_Size);
        }
        UpdateGroundDrag(tilePos, true, 0);
        tilemap_Building.SetTile(tilePos, null);
    }
    /// <summary>
    /// ɾ������ռλ
    /// </summary>
    /// <param name="tilePos"></param>
    /// <param name="size"></param>
    public void DeleteBuildingPlaceHolding(Vector3Int tilePos, AreaSize size)
    {
        switch (size)
        {
            case AreaSize._1X1:
                break;
            case AreaSize._1X2:
                DeleteBuilding(tilePos + Vector3Int.up);
                break;
            case AreaSize._2X1:
                DeleteBuilding(tilePos + Vector3Int.right);
                break;
            case AreaSize._2X2:
                DeleteBuilding(tilePos + Vector3Int.up);
                DeleteBuilding(tilePos + Vector3Int.right);
                DeleteBuilding(tilePos + Vector3Int.up + Vector3Int.right);
                break;
            case AreaSize._3X3:
                DeleteBuilding(tilePos + Vector3Int.up);
                DeleteBuilding(tilePos + Vector3Int.down);
                DeleteBuilding(tilePos + Vector3Int.right);
                DeleteBuilding(tilePos + Vector3Int.left);
                DeleteBuilding(tilePos + Vector3Int.up + Vector3Int.right);
                DeleteBuilding(tilePos + Vector3Int.up + Vector3Int.left);
                DeleteBuilding(tilePos + Vector3Int.down + Vector3Int.right);
                DeleteBuilding(tilePos + Vector3Int.down + Vector3Int.left);
                break;
        }
    }
    /// <summary>
    /// ���ƽ���
    /// </summary>
    /// <param name="center"></param>
    /// <param name="widht"></param>
    /// <param name="height"></param>
    public void DrawBuilding(Vector3Int center, int widht, int height)
    {
        for (int x = -widht / 2; x <= widht / 2; x++)
        {
            for (int y = -height / 2; y <= height / 2; y++)
            {
                int posX = center.x + x;
                int posY = center.y + y;

                if (GetBuilding(new Vector3Int(posX, posY, 0), out BuildingTile buildingTile))
                {
                    buildingTile.tileObj.All_Draw();
                }
            }
        }
    }
    /// <summary>
    /// ��ý���
    /// </summary>
    /// <param name="tilePos"></param>
    /// <returns></returns>
    public bool GetBuilding(Vector3Int tilePos, out BuildingTile buildingTile)
    {
        buildingTile = tilemap_Building.GetTile<BuildingTile>(tilePos);
        if (buildingTile)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool GetBuilding(Vector3 tilePos, out BuildingTile buildingTile)
    {
        Vector3Int vector3Int = grid_Building.WorldToCell(tilePos);
        return GetBuilding(vector3Int, out buildingTile);
    }
    /// <summary>
    /// ��ý���ʵ��
    /// </summary>
    /// <param name="tilePos"></param>
    /// <returns></returns>
    public GameObject GetBuildingObj(Vector3Int tilePos)
    {
        return tilemap_Building.GetInstantiatedObject(tilePos);
    }
    /// <summary>
    /// ��ý�������
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    private BuildingTile GetBuildingConfig(int id)
    {
        if (BuildingTilePool.ContainsKey(id)) { return BuildingTilePool[id]; }
        else
        {
            BuildingTile config = Resources.Load<BuildingTile>("TileScript/Building/" + id);
            if (config != null)
            {
                BuildingTilePool.Add(id, config);
            }
            return config;
        }
    }
    /// <summary>
    /// �����Χ
    /// </summary>
    /// <returns></returns>
    public Around CheckBuilding_EightSide(int id, Vector3Int pos)
    {
        Around around = new Around();
        BuildingTile building;
        around.C = (GetBuilding(pos, out building) && building.tileID == id) ? true : false;
        around.U = (GetBuilding(pos + Vector3Int.up, out building) && building.tileID == id) ? true : false;
        around.D = (GetBuilding(pos + Vector3Int.down, out building) && building.tileID == id) ? true : false;
        around.L = (GetBuilding(pos + Vector3Int.left, out building) && building.tileID == id) ? true : false;
        around.R = (GetBuilding(pos + Vector3Int.right, out building) && building.tileID == id) ? true : false;
        around.UR = (GetBuilding(pos + Vector3Int.up + Vector3Int.right, out building) && building.tileID == id) ? true : false;
        around.UL = (GetBuilding(pos + Vector3Int.up + Vector3Int.left, out building) && building.tileID == id) ? true : false;
        around.DL = (GetBuilding(pos + Vector3Int.down + Vector3Int.left, out building) && building.tileID == id) ? true : false;
        around.DR = (GetBuilding(pos + Vector3Int.down + Vector3Int.right, out building) && building.tileID == id) ? true : false;
        return around;
    }
    
    public Around CheckBuilding_FourSide(int id, Vector3Int pos)
    {
        Around around = new Around();
        BuildingTile building;
        around.L = (GetBuilding(pos + Vector3Int.left, out building) && building.tileID == id) ? true : false;
        around.R = (GetBuilding(pos + Vector3Int.right, out building) && building.tileID == id) ? true : false;
        around.U = (GetBuilding(pos + Vector3Int.up, out building) && building.tileID == id) ? true : false;
        around.D = (GetBuilding(pos + Vector3Int.down, out building) && building.tileID == id) ? true : false;
        return around;
    }
    public Around CheckBuilding_FourSide(Func<int, bool> func, Vector3Int pos)
    {
        Around around = new Around();
        BuildingTile building;
        around.L = (GetBuilding(pos + Vector3Int.left, out building) && func(building.tileID)) ? true : false;
        around.R = (GetBuilding(pos + Vector3Int.right, out building) && func(building.tileID)) ? true : false;
        around.U = (GetBuilding(pos + Vector3Int.up, out building) && func(building.tileID)) ? true : false;
        around.D = (GetBuilding(pos + Vector3Int.down, out building) && func(building.tileID)) ? true : false;
        return around;
    }
    public Around CheckBuilding_TwoSide(int id, Vector3Int pos)
    {
        Around around = new Around();
        BuildingTile building;
        around.L = (GetBuilding(pos + Vector3Int.left, out building) && building.tileID == id) ? true : false;
        around.R = (GetBuilding(pos + Vector3Int.right, out building) && building.tileID == id) ? true : false;
        return around;
    }
    public Around CheckBuilding_TwoSide(Func<int, bool> func, Vector3Int pos)
    {
        Around around = new Around();
        BuildingTile building;
        around.L = (GetBuilding(pos + Vector3Int.left, out building) && func(building.tileID)) ? true : false;
        around.R = (GetBuilding(pos + Vector3Int.right, out building) && func(building.tileID)) ? true : false;
        return around;
    }

    /// <summary>
    /// ����Ƿ��н�����λ
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="size"></param>
    /// <returns></returns>
    public bool CheckBuildingEmpty(Vector3 pos, AreaSize size)
    {
        bool result = true;
        Vector3Int vector3Int = grid_Building.WorldToCell(pos);
        switch (size)
        {
            case AreaSize._1X1:
                if (tilemap_Building.GetTile(vector3Int)) result = false;
                break;
            case AreaSize._1X2:
                if (tilemap_Building.GetTile(vector3Int)) result = false;
                if (tilemap_Building.GetTile(vector3Int + Vector3Int.right)) result = false;
                break;
            case AreaSize._2X1:
                if (tilemap_Building.GetTile(vector3Int)) result = false;
                if (tilemap_Building.GetTile(vector3Int + Vector3Int.up)) result = false;
                break;
            case AreaSize._2X2:
                if (tilemap_Building.GetTile(vector3Int)) result = false;
                if (tilemap_Building.GetTile(vector3Int + Vector3Int.up)) result = false;
                if (tilemap_Building.GetTile(vector3Int + Vector3Int.right)) result = false;
                if (tilemap_Building.GetTile(vector3Int + Vector3Int.right + Vector3Int.up)) result = false;
                break;
            case AreaSize._3X3:
                if (tilemap_Building.GetTile(vector3Int)) result = false;
                if (tilemap_Building.GetTile(vector3Int + Vector3Int.up)) result = false;
                if (tilemap_Building.GetTile(vector3Int + Vector3Int.right)) result = false;
                if (tilemap_Building.GetTile(vector3Int + Vector3Int.right + Vector3Int.up)) result = false;
                if (tilemap_Building.GetTile(vector3Int + Vector3Int.up + Vector3Int.up)) result = false;
                if (tilemap_Building.GetTile(vector3Int + Vector3Int.right + Vector3Int.right)) result = false;
                if (tilemap_Building.GetTile(vector3Int + 2 * Vector3Int.up + Vector3Int.right)) result = false;
                if (tilemap_Building.GetTile(vector3Int + 2 * Vector3Int.right + Vector3Int.up)) result = false;
                if (tilemap_Building.GetTile(vector3Int + 2 * Vector3Int.right + 2 * Vector3Int.up)) result = false;
                break;
        }
        return result;
    }
    List<BuildingTile> buildingTiles_Temp = new List<BuildingTile>();
    public List<BuildingTile> GetNearbyBuildings_EightSide(Vector3Int postion, Vector3Int offset)
    {
        buildingTiles_Temp.Clear();
        Vector3Int pos = postion + offset;
        buildingTiles_Temp.Add(tilemap_Building.GetTile<BuildingTile>(pos));
        buildingTiles_Temp.Add(tilemap_Building.GetTile<BuildingTile>(pos + Vector3Int.up));
        buildingTiles_Temp.Add(tilemap_Building.GetTile<BuildingTile>(pos + Vector3Int.up + Vector3Int.right));
        buildingTiles_Temp.Add(tilemap_Building.GetTile<BuildingTile>(pos + Vector3Int.right));
        buildingTiles_Temp.Add(tilemap_Building.GetTile<BuildingTile>(pos + Vector3Int.right + Vector3Int.down));
        buildingTiles_Temp.Add(tilemap_Building.GetTile<BuildingTile>(pos + Vector3Int.down));
        buildingTiles_Temp.Add(tilemap_Building.GetTile<BuildingTile>(pos + Vector3Int.down + Vector3Int.left));
        buildingTiles_Temp.Add(tilemap_Building.GetTile<BuildingTile>(pos + Vector3Int.left));
        buildingTiles_Temp.Add(tilemap_Building.GetTile<BuildingTile>(pos + Vector3Int.left + Vector3Int.up));
        return buildingTiles_Temp;
    }
    public List<BuildingTile> GetNearbyBuildings_FourSide(Vector3Int postion, Vector3Int offset)
    {
        buildingTiles_Temp.Clear();
        Vector3Int pos = postion + offset;
        buildingTiles_Temp.Add(tilemap_Building.GetTile<BuildingTile>(pos));
        buildingTiles_Temp.Add(tilemap_Building.GetTile<BuildingTile>(pos + Vector3Int.up));
        buildingTiles_Temp.Add(tilemap_Building.GetTile<BuildingTile>(pos + Vector3Int.right));
        buildingTiles_Temp.Add(tilemap_Building.GetTile<BuildingTile>(pos + Vector3Int.down));
        buildingTiles_Temp.Add(tilemap_Building.GetTile<BuildingTile>(pos + Vector3Int.left));
        return buildingTiles_Temp;
    }
    #endregion
}
public struct Around
{
    public bool C;
    public bool U;
    public bool D;
    public bool L;
    public bool R;
    public bool UR;
    public bool UL;
    public bool DL;
    public bool DR;
}