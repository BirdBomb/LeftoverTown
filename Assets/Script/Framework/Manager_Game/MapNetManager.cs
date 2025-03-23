using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UniRx;
using System.Threading.Tasks;
using System.Numerics;
using System.Text;
using System;
using System.Drawing;

public class MapNetManager : NetworkBehaviour
{
    public MapInfoData bind_MapInfoData = null;
    public MapTileTypeData bind_BuildingTileTypeData = null;
    public MapTileInfoData bind_BuildingTileInfoData = null;
    public MapTileTypeData bind_GroundTileTypeData = null;
    private bool mapDataAlready = false;
    private int mapSeed;
    void Start()
    {
        MessageBroker.Default.Receive<MapEvent.MapEvent_LocalTile_RequestMapData>().Subscribe(_ =>
        {
            Local_RequestMapData(_.playerPos, _.mapSize, Runner.LocalPlayer);
        }).AddTo(this);
        MessageBroker.Default.Receive<MapEvent.MapEvent_LocalTile_SaveMapData>().Subscribe(_ =>
        {
            if (Object.HasStateAuthority)
            {
                SaveMap();
            }
        }).AddTo(this);
        MessageBroker.Default.Receive<MapEvent.MapEvent_LocalTile_TakeDamage>().Subscribe(_ =>
        {
            RPC_LocalInput_TileTakeDamage(_.pos, _.damage, Runner.LocalPlayer);
        }).AddTo(this);
        MessageBroker.Default.Receive<MapEvent.MapEvent_LocalTile_UpdateBuildingInfo>().Subscribe(_ =>
        {
            RPC_LocalInput_TileUpdateInfo(_.pos, _.info);
        }).AddTo(this);
        MessageBroker.Default.Receive<MapEvent.MapEvent_LocalTile_ChangeBuildingArea>().Subscribe(_ =>
        {
            RPC_LocalInput_BuildingAreaChange(_.buildingPos, _.buildingID, (int)_.areaSize);
        }).AddTo(this);
        MessageBroker.Default.Receive<MapEvent.MapEvent_LocalTile_ChangeGround>().Subscribe(_ =>
        {
            RPC_LocalInput_FloorChange(_.groundPos, _.groundID);
        }).AddTo(this);
        MessageBroker.Default.Receive<MapEvent.MapEvent_LocalTile_ChangeSunLight>().Subscribe(_ =>
        {
            RPC_LocalInput_ChangeSun(_.distance);
        }).AddTo(this);
        MessageBroker.Default.Receive<GameEvent.GameEvent_State_ChangeTime>().Subscribe(_ => 
        {
            ChangeTime(_.hour);
        }).AddTo(this);
    }
    public override void Spawned()
    {
        base.Spawned();
    }


    #region//����Ͷ�ȡ
    private async Task LoadMap()
    {
        GameDataManager.Instance.LoadMap(out MapInfoData mapInfoData, out MapTileTypeData buildingTypeData, out MapTileInfoData buildingInfoData, out MapTileTypeData floorTypeData);
        bind_MapInfoData = mapInfoData;
        bind_BuildingTileTypeData = buildingTypeData;
        bind_BuildingTileInfoData = buildingInfoData;
        bind_GroundTileTypeData = floorTypeData;
        await Task.Delay(100);
        Debug.Log("��������ͼ��ʼ���ɹ�");
        InitMap();
    }
    private void SaveMap()
    {
        if (bind_BuildingTileTypeData != null && bind_BuildingTileInfoData != null && bind_GroundTileTypeData != null)
        {
            GameDataManager.Instance.SaveMap(bind_MapInfoData, bind_BuildingTileTypeData, bind_BuildingTileInfoData, bind_GroundTileTypeData);
            Debug.Log("��������ͼ����ɹ�");
        }
    }
    private void InitMap()
    {
        InitLoop();
        InitSeed();
    }
    private void InitLoop()
    {
        Debug.Log("��ʼ��������ʱ��");
        Hour = bind_MapInfoData.hour;
        Date = bind_MapInfoData.date;
        InvokeRepeating("UpdateTime", 120, 120);
    }
    private void InitSeed()
    {
        string str = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        int seedInt = 0;
        StringBuilder seedStr = new StringBuilder(bind_MapInfoData.seed);
        for (int i = 0; i < seedStr.Length; i++)
        {
            char c = seedStr[i];
            int temp = str.IndexOf(c);
            seedInt += temp * (int)Mathf.Pow(10, i);
        }
        mapSeed = seedInt;
    }
    #endregion
    #region//ʱ������
    public void UpdateTime()
    {
        if (Object.HasStateAuthority)
        {
            if (Hour < 10)
            {
                Hour += 1;
            }
            else
            {
                Date += 1;
                Hour = 0;
            }
            bind_MapInfoData.hour = Hour;
            bind_MapInfoData.date = Date;
        }
    }
    public void ChangeTime(int val)
    {
        if (Object.HasStateAuthority)
        {
            Hour = val;
            bind_MapInfoData.hour = Hour;
            bind_MapInfoData.date = Date;
            CancelInvoke("UpdateTime");
            InvokeRepeating("UpdateTime", 120, 120);
        }
    }
    #endregion
    #region//���ض�
    /// <summary>
    /// ���ض������ͼ����
    /// </summary>
    /// <param name="center">��������(ֻ����20�ı���)</param>
    /// <param name="player"></param>
    private void Local_RequestMapData(Vector3Int center,int size, PlayerRef player)
    {
        Debug.Log("�����������ͼ��Ϣ/λ������" + center);
        RPC_LocalInput_RequestMapData(center, size, player);
    }
    /// <summary>
    /// ���ض˸��µ�ͼ����
    /// </summary>
    /// <param name="center"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    private void Local_UpdateMapCenter(PlayerRef player, Vector3Int center, int width, int height, int seed)
    {
        //Debug.Log("�յ���������ͼ��Ϣ(��ͼ��������)/����" + center + "�ߴ�" + width + "/" + height);
        MapManager.Instance.UpdatePlayerCenterInMap(player, center, width, height, seed);
    }
    /// <summary>
    /// ���ض˻�õ�ͼ����(�������)
    /// </summary>
    /// <param name="tileList"></param>
    /// <param name="center"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    private void Local_ReceiveBuildTypeData(int[] tileTypeList, string[] tileInfoList, Vector3Int center, int width, int height)
    {
        //Debug.Log("�յ���������ͼ��Ϣ(�������)/����" + center + "�ߴ�" + width + "/" + height);
        if(MapManager.Instance.AddBuildingAreaInMap(center))
        {
            int index = 0;
            for (int x = -width / 2; x < width / 2; x++)
            {
                for (int y = -height / 2; y < height / 2; y++)
                {
                    if (tileTypeList[index] > 0)
                    {
                        MapManager.Instance.CreateBuilding(tileTypeList[index], new Vector3Int(center.x + x, center.y + y, 0), out BuildingTile buildingTile);
                        if (tileInfoList[index].Length > 0)
                        {
                            buildingTile.tileObj.UpdateInfo(tileInfoList[index]);
                        }
                    }
                    index++;
                }
            }
            MapManager.Instance.DrawBuilding(center, width + 2, height + 2);
        }
    }
    /// <summary>
    /// ���ض˻�õ�ͼ����(�ؿ����)
    /// </summary>
    /// <param name="tileList"></param>
    /// <param name="center"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    private void Local_ReceiveGroundTypeData(int[] tileList, Vector3Int center, int width, int height)
    {
        //Debug.Log("�յ���������ͼ��Ϣ(�ؿ����)/����" + center + "�ߴ�" + width + "/" + height);
        if(MapManager.Instance.AddGroundAreaInMap(center))
        {
            int index = 0;
            for (int x = -width / 2; x < width / 2; x++)
            {
                for (int y = -height / 2; y < height / 2; y++)
                {
                    int x1 = center.x + x;
                    int y1 = center.y + y;
                    MapManager.Instance.CreateGround(tileList[index], new Vector3Int(x1, y1, 0));
                    index++;
                }
            }
            MapManager.Instance.DrawGround(center, width + 2, height + 2);
        }
    }
    #endregion
    #region//�����
    /// <summary>
    /// Ϊĳ�˳�ʼ����ͼ
    /// </summary>
    /// <param name="center">����</param>
    /// <param name="center">�ߴ�</param>
    /// <param name="player">Ŀ�����</param>
    /// <returns></returns>
    private async Task State_TryInitMapForSomeone(Vector3Int center, int size, PlayerRef player)
    {
        State_TrySendMapCenter(center, size, size, player, mapSeed);
        for (int i = 0; i < 25; i++)
        {
            Vector3Int tempPos = new Vector3Int();
            switch (i)
            {
                case 0:
                    tempPos = center;
                    break;
                case 1:
                    tempPos = center + new Vector3Int(size, 0, 0);
                    break;
                case 2:
                    tempPos = center + new Vector3Int(size, size, 0);
                    break;
                case 3:
                    tempPos = center + new Vector3Int(0, size, 0);
                    break;
                case 4:
                    tempPos = center + new Vector3Int(-size, size, 0);
                    break;
                case 5:
                    tempPos = center + new Vector3Int(-size, 0, 0);
                    break;
                case 6:
                    tempPos = center + new Vector3Int(-size, -size, 0);
                    break;
                case 7:
                    tempPos = center + new Vector3Int(0, -size, 0);
                    break;
                case 8:
                    tempPos = center + new Vector3Int(size, -size, 0);
                    break;
                case 9:
                    tempPos = center + new Vector3Int(size * 2, -size * 2, 0);
                    break;
                case 10:
                    tempPos = center + new Vector3Int(size * 2, -size, 0);
                    break;
                case 11:
                    tempPos = center + new Vector3Int(size * 2, 0, 0);
                    break;
                case 12:
                    tempPos = center + new Vector3Int(size * 2, size, 0);
                    break;
                case 13:
                    tempPos = center + new Vector3Int(size * 2, size * 2, 0);
                    break;
                case 14:
                    tempPos = center + new Vector3Int(size, size * 2, 0);
                    break;
                case 15:
                    tempPos = center + new Vector3Int(0, size * 2, 0);
                    break;
                case 16:
                    tempPos = center + new Vector3Int(-size, size * 2, 0);
                    break;
                case 17:
                    tempPos = center + new Vector3Int(-size * 2, size * 2, 0);
                    break;
                case 18:
                    tempPos = center + new Vector3Int(-size * 2, size, 0);
                    break;
                case 19:
                    tempPos = center + new Vector3Int(-size * 2, 0, 0);
                    break;
                case 20:
                    tempPos = center + new Vector3Int(-size * 2, -size, 0);
                    break;
                case 21:
                    tempPos = center + new Vector3Int(-size * 2, -size * 2, 0);
                    break;
                case 22:
                    tempPos = center + new Vector3Int(-size, -size * 2, 0);
                    break;
                case 23:
                    tempPos = center + new Vector3Int(0, -size * 2, 0);
                    break;
                case 24:
                    tempPos = center + new Vector3Int(size, -size * 2, 0);
                    break;
            }
            await Task.Delay(20);
            State_TrySendGroundTileTypeData(tempPos, size, size, player);
            State_TrySendBuildingTileTypeData(tempPos, size, size, player);
        }
    }
    private void State_TrySendMapCenter(Vector3Int center, int width, int height, PlayerRef player, int seed)
    {
        RPC_StateCall_UpdateMapCenter(player, center, width, height, seed);
    }
    private string tempKey;
    /// <summary>
    /// ���ͽ����ؿ���������
    /// </summary>
    /// <param name="center">��������</param>
    /// <param name="width">�����</param>
    /// <param name="height">�����</param>
    /// <param name="player">Ŀ��ͻ���</param>
    private void State_TrySendBuildingTileTypeData(Vector3Int center, int width, int height, PlayerRef player)
    {
        int[] tempTileTypeArray = new int[width * height];
        string[] tempTileInfoArray = new string[width * height];
        int index = 0;
        for (int x = -width / 2; x < width / 2; x++)
        {
            for (int y = -height / 2; y < height / 2; y++)
            {
                int tempX = center.x + x + 30000;
                int tempY = center.y + y + 30000;
                int tempIndex;
                if (tempY > tempX)
                {
                    tempIndex = tempY * tempY + tempY + tempY - tempX;
                }
                else
                {
                    tempIndex = tempX * tempX + tempY;
                }

                if (bind_BuildingTileTypeData.tileDic.ContainsKey(tempIndex))
                {
                    int tileType = bind_BuildingTileTypeData.tileDic[tempIndex];
                    tempTileTypeArray[index] = tileType;
                }
                else
                {
                    //bind_BuildingTileTypeData.tileDic.Add(tempIndex, 0);
                    tempTileTypeArray[index] = 0;
                }

                if (bind_BuildingTileInfoData.tileDic.ContainsKey(tempIndex))
                {
                    string tileInfo = bind_BuildingTileInfoData.tileDic[tempIndex];
                    tempTileInfoArray[index] = tileInfo;
                }
                else
                {
                    //bind_BuildingTileInfoData.tileDic.Add(tempIndex, "");
                    tempTileInfoArray[index] = "";
                }
                index++;
            }
        }
        RPC_StateCall_SendBuildingTileTypeData(player, tempTileTypeArray, tempTileInfoArray, center, width, height);
    }
    /// <summary>
    /// ���͵���ؿ���������
    /// </summary>
    /// <param name="center"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="player"></param>
    private void State_TrySendGroundTileTypeData(Vector3Int center, int width, int height, PlayerRef player)
    {
        int[] tempTileArray = new int[width * height];
        int index = 0;
        for (int x = -width / 2; x < width / 2; x++)
        {
            for (int y = -height / 2; y < height / 2; y++)
            {
                int tempX = center.x + x + 30000;
                int tempY = center.y + y + 30000;
                int tempIndex;
                if (tempY > tempX)
                {
                    tempIndex = tempY * tempY + tempY + tempY - tempX;
                }
                else
                {
                    tempIndex = tempX * tempX + tempY;
                }

                if (bind_GroundTileTypeData.tileDic.ContainsKey(tempIndex))
                {
                    int tileType = bind_GroundTileTypeData.tileDic[tempIndex];
                    tempTileArray[index] = tileType;
                }
                else
                {
                    //bind_GroundTileTypeData.tileDic.Add(tempIndex, 1001);
                    tempTileArray[index] = 1001;
                }
                index++;
            }
        }
        RPC_StateCall_SendGroundTileTypeData(player, tempTileArray, center, width, height);
    }
    #endregion

    #region//RPC
    /// <summary>
    /// �ͻ��������ͼ����
    /// </summary>
    /// <param name="center"></param>
    /// <param name="player"></param>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    private async void RPC_LocalInput_RequestMapData(Vector3Int center,int size,PlayerRef player)
    {
        if (Object.HasStateAuthority)
        {
            Debug.Log("���������Է��͵�ͼ����");
            if (!mapDataAlready)
            {
                Debug.Log("��������ͼδ��ʼ��,�ȳ�ʼ��");
                mapDataAlready = true;
                await LoadMap();
            }
            else
            {
                Debug.Log("��������ͼ�Ѿ���ʼ��");
            }
            await State_TryInitMapForSomeone(center, size, player);
        }
    }
    /// <summary>
    /// �ͻ�������Եؿ�Ĺ���
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="damage"></param>
    /// <param name="player"></param>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    private void RPC_LocalInput_TileTakeDamage(Vector3Int pos, int damage, PlayerRef player)
    {
        if (Object.HasStateAuthority)
        {
            if (MapManager.Instance.GetBuilding(pos, out BuildingTile buildingTile))
            {
                RPC_StateCall_TileUpdateHp(pos, damage, player);
            }
            else
            {
                Debug.Log("δ�ҵ�Ŀ��tile(" + pos + ")");
            }
        }
    }
    /// <summary>
    /// �ͻ�������Եؿ�ĸ���
    /// </summary>
    /// <param name="pos"></param>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    private void RPC_LocalInput_TileUpdateInfo(Vector3Int pos, string info)
    {
        if (Object.HasStateAuthority)
        {
            RPC_StateCall_BuildingUpdateInfo(pos, info);
            int tempX = pos.x + 30000;
            int tempY = pos.y + 30000;
            int tempIndex;
            if (tempY > tempX)
            {
                tempIndex = tempY * tempY + tempY + tempY - tempX;
            }
            else
            {
                tempIndex = tempX * tempX + tempY;
            }
            bind_BuildingTileInfoData.tileDic[tempIndex] = info;
        }
    }
    /// <summary>
    /// �ͻ������������ĸı�
    /// </summary>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    private void RPC_LocalInput_BuildingAreaChange(Vector3Int pos, short id, int size)
    {
        if (Object.HasStateAuthority)
        {
            OnlyState_BuildingAreaChange(pos, id, (AreaSize)size);
        }
    }
    /// <summary>
    /// �������ı�һ������Ľ���
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="id"></param>
    /// <param name="size"></param>
    private void OnlyState_BuildingAreaChange(Vector3Int pos, short id, AreaSize size)
    {
        OnlyState_BuildingChange(pos, id);
        switch (size)
        {
            case AreaSize._1X1:
                break;
            case AreaSize._1X2:
                OnlyState_BuildingChange(pos + Vector3Int.up, 99);
                break;
            case AreaSize._2X1:
                OnlyState_BuildingChange(pos + Vector3Int.right, 99);
                break;
            case AreaSize._2X2:
                OnlyState_BuildingChange(pos + Vector3Int.up, 99);
                OnlyState_BuildingChange(pos + Vector3Int.right, 99);
                OnlyState_BuildingChange(pos + Vector3Int.up + Vector3Int.right, 99);
                break;
            case AreaSize._3X3:
                OnlyState_BuildingChange(pos + Vector3Int.up, 99);
                OnlyState_BuildingChange(pos + Vector3Int.down, 99);
                OnlyState_BuildingChange(pos + Vector3Int.left, 99);
                OnlyState_BuildingChange(pos + Vector3Int.right, 99);
                OnlyState_BuildingChange(pos + Vector3Int.up + Vector3Int.left, 99);
                OnlyState_BuildingChange(pos + Vector3Int.up + Vector3Int.right, 99);
                OnlyState_BuildingChange(pos + Vector3Int.down + Vector3Int.left, 99);
                OnlyState_BuildingChange(pos + Vector3Int.down + Vector3Int.right, 99);
                break;
        }
    }
    /// <summary>
    /// �������ı䵥������
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="id"></param>
    private void OnlyState_BuildingChange(Vector3Int pos, short id)
    {
        MapManager.Instance.GetBuilding(pos,out BuildingTile buildingTile);
        if (buildingTile != null)
        {
            AreaSize size = BuildingConfigData.GetBuildingConfig(buildingTile.tileID).Building_Size;
            switch (size)
            {
                case AreaSize._1X1:
                    break;
                case AreaSize._1X2:
                    OnlyState_BuildingChange(pos + Vector3Int.up, 0);
                    break;
                case AreaSize._2X1:
                    OnlyState_BuildingChange(pos + Vector3Int.right, 0);
                    break;
                case AreaSize._2X2:
                    OnlyState_BuildingChange(pos + Vector3Int.up, 0);
                    OnlyState_BuildingChange(pos + Vector3Int.right, 0);
                    OnlyState_BuildingChange(pos + Vector3Int.up + Vector3Int.right, 0);
                    break;
                case AreaSize._3X3:
                    OnlyState_BuildingChange(pos + Vector3Int.up, 0);
                    OnlyState_BuildingChange(pos + Vector3Int.down, 0);
                    OnlyState_BuildingChange(pos + Vector3Int.left, 0);
                    OnlyState_BuildingChange(pos + Vector3Int.right, 0);
                    OnlyState_BuildingChange(pos + Vector3Int.up + Vector3Int.right, 0);
                    OnlyState_BuildingChange(pos + Vector3Int.up + Vector3Int.left, 0);
                    OnlyState_BuildingChange(pos + Vector3Int.down + Vector3Int.right, 0);
                    OnlyState_BuildingChange(pos + Vector3Int.down + Vector3Int.left, 0);
                    break;
            }

        }
        int tempX = pos.x + 30000;
        int tempY = pos.y + 30000;
        int tempIndex;
        if (tempY > tempX)
        {
            tempIndex = tempY * tempY + tempY + tempY - tempX;
        }
        else
        {
            tempIndex = tempX * tempX + tempY;
        }
        RPC_StateCall_BuildingChange(pos, id);
        if (id > 0)
        {
            bind_BuildingTileTypeData.tileDic[tempIndex] = id;
        }
        else
        {
            bind_BuildingTileTypeData.tileDic.Remove(tempIndex);
        }
    }
    /// <summary>
    /// �ͻ�������Եؿ�ĸı�
    /// </summary>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    private void RPC_LocalInput_FloorChange(Vector3Int pos, short id)
    {
        if (Object.HasStateAuthority)
        {
            RPC_StateCall_GroundChange(pos, id);

            int tempX = pos.x + 30000;
            int tempY = pos.y + 30000;
            int tempIndex;
            if (tempY > tempX)
            {
                tempIndex = tempY * tempY + tempY + tempY - tempX;
            }
            else
            {
                tempIndex = tempX * tempX + tempY;
            }

            bind_GroundTileTypeData.tileDic[tempIndex] = id;
        }
    }

    /// <summary>
    /// ����˸�ĳ�˷��͵�ͼ����
    /// </summary>
    /// <param name="target"></param>
    /// <param name="center"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    private void RPC_StateCall_UpdateMapCenter(/*[RpcTarget]*/PlayerRef target,Vector3Int center, int width, int height,int seed)
    {
        Local_UpdateMapCenter(target, center, width, height, seed);
    }
    /// <summary>
    /// ����˷��ͽ����������
    /// </summary>
    /// <param name="target"></param>
    /// <param name="tileList"></param>
    /// <param name="center"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    private void RPC_StateCall_SendBuildingTileTypeData(/*[RpcTarget]*/PlayerRef target, int[] tileTypeList, string[] tileInfoList, Vector3Int center, int width, int height)
    {
        Local_ReceiveBuildTypeData(tileTypeList, tileInfoList, center, width, height);
    }
    /// <summary>
    /// ����˷��͵����������
    /// </summary>
    /// <param name="target"></param>
    /// <param name="tileList"></param>
    /// <param name="center"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    private void RPC_StateCall_SendGroundTileTypeData(/*[RpcTarget]*/ PlayerRef target, int[] tileList, Vector3Int center, int width, int height)
    {
        Local_ReceiveGroundTypeData(tileList, center, width, height);
    }
    /// <summary>
    /// ������֪ͨ���µؿ�����ֵ
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="hp"></param>
    /// <param name="player"></param>
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    private void RPC_StateCall_TileUpdateHp(Vector3Int pos, int hp, PlayerRef player)
    {
        if(MapManager.Instance.GetBuilding(pos,out BuildingTile buildingTile))
        {
            buildingTile.tileObj.UpdateHP(hp);
        }
    }
    /// <summary>
    /// ������֪ͨ�ı佨��
    /// </summary>
    /// <param name="pos"></param>
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    private void RPC_StateCall_BuildingChange(Vector3Int pos, int id)
    {
        MapManager.Instance.CreateBuilding(id, pos, out _);
        MapManager.Instance.DrawBuilding(pos, 2, 2);
    }
    /// <summary>
    /// ������֪ͨ���½�����Ϣ
    /// </summary>
    /// <param name="pos"></param>
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    private void RPC_StateCall_BuildingUpdateInfo(Vector3Int pos, string info)
    {
        MapManager.Instance.GetBuilding(pos, out BuildingTile buildingTile);
        buildingTile.tileObj.UpdateInfo(info);
    }
    /// <summary>
    /// ������֪ͨ�ı�ذ�
    /// </summary>
    /// <param name="pos"></param>
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    private void RPC_StateCall_GroundChange(Vector3Int pos, int id)
    {
        MapManager.Instance.CreateGround(id, pos);
        MapManager.Instance.DrawGround(pos, 2, 2);
    }
    /// <summary>
    /// �ͻ��˸ı�̫��
    /// </summary>
    /// <param name="distance"></param>
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    private void RPC_LocalInput_ChangeSun(int distance)
    {
        Distance = distance;
    }
    #endregion
    #region//Network
    [Networked, OnChangedRender(nameof(OnHourChange)), HideInInspector]
    public int Hour { get; set; }
    [Networked, HideInInspector]
    public int Date { get; set; }
    [Networked, OnChangedRender(nameof(OnDistance)), HideInInspector]
    public int Distance { get; set; }
    public void OnHourChange()
    {
        WorldManager.Instance.UpdateTime(Hour, Date);
    }
    public void OnDistance()
    {
        WorldManager.Instance.UpdateDistance(Distance);
    }
    #endregion
}
