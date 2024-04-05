using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UniRx;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using static UnityEditor.PlayerSettings;
using System.Threading.Tasks;
using System;

public class MapNetManager : NetworkBehaviour
{
    [SerializeField]
    public MapManager mapManager;
    public MapData mapData = null;
    private bool mapDataAlready = false;
    void Start()
    {
        MessageBroker.Default.Receive<MapEvent.MapEvent_DamageTile_Upload>().Subscribe(_ =>
        {
            TryDamageTile(_.tilePos, _.damageValue);
        }).AddTo(this);
        MessageBroker.Default.Receive<MapEvent.MapEvent_RequestMapData>().Subscribe(_ =>
        {
            TryRequestMapData(_.pos, _.player);
        }).AddTo(this);
    }
    #region//�ؿ����
    private void TryDamageTile(Vector3Int pos, int damage)
    {
        RPC_DamageTile(pos, damage);
    }
    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_DamageTile(Vector3Int pos, int damage)
    {
        MessageBroker.Default.Publish(new MapEvent.MapEvent_DamageTile()
        {
            tilePos = pos,
            damageValue = damage
        });
    }

    private void TryChangeTile(Vector2 pos)
    {

    }
    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_ChangeTile(Vector2 pos)
    {

    }
    #endregion
    #region//�����뷵�ص�ͼ����
    private void TryRequestMapData(Vector3Int pos,PlayerRef player)
    {
        Debug.Log("���������������ͼ��Ϣ/λ������" + pos);
        RPC_RequestMapData(pos, player);
    }
    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    private async void RPC_RequestMapData(Vector3Int pos, PlayerRef player)
    {
        if (Object.HasStateAuthority)
        {
            Debug.Log("���������Է��͵�ͼ����");
            if (!mapDataAlready)
            {
                Debug.Log("��������ͼδ��ʼ��,�ȳ�ʼ��");
                mapDataAlready = true;
                await LoadMap();
                await TryInitMapForSomeone(pos, 19, player);
            }
            else
            {
                Debug.Log("��������ͼ�Ѿ���ʼ��");
                await TryInitMapForSomeone(pos, 19, player);
            }
        }
    }
    private async Task LoadMap()
    {
        mapData = GameDataManager.Instance.LoadMap();
        await Task.Delay(100);
        Debug.Log("��������ͼ��ʼ���ɹ�");
    }
    /// <summary>
    /// Ϊĳ�˳�ʼ����ͼ
    /// </summary>
    /// <param name="center">����</param>
    /// <param name="center">�ߴ�</param>
    /// <param name="player">Ŀ�����</param>
    /// <returns></returns>
    private async Task TryInitMapForSomeone(Vector3Int center,int size, PlayerRef player)
    {
        for(int i = 0; i < 9; i++)
        {
            Vector3Int tempPos = new Vector3Int();
            if (i == 0) { tempPos = center; }
            if (i == 1) { tempPos = center + new Vector3Int(size, 0, 0); }
            if (i == 2) { tempPos = center + new Vector3Int(-size, 0, 0); }
            if (i == 3) { tempPos = center + new Vector3Int(0, size, 0); }
            if (i == 4) { tempPos = center + new Vector3Int(0, -size, 0); }
            if (i == 5) { tempPos = center + new Vector3Int(size, size, 0); }
            if (i == 6) { tempPos = center + new Vector3Int(-size, size, 0); }
            if (i == 7) { tempPos = center + new Vector3Int(-size, -size, 0); }
            if (i == 8) { tempPos = center + new Vector3Int(size, -size, 0); }
            TrySendMapData(tempPos, size, size, player);
            await Task.Delay(1000);
        }
    }
    /// <summary>
    /// ����������Ϣ����
    /// </summary>
    /// <param name="center">��������</param>
    /// <param name="width">�����</param>
    /// <param name="height">�����</param>
    /// <param name="player">Ŀ��ͻ���</param>
    private void TrySendMapData(Vector3Int center, int width, int height, PlayerRef player)
    {
        string[] tempArray = new string[width * height];
        int index = 0;
        for (int x = -(width - 1) / 2; x <= (width - 1) / 2; x++)
        {
            for (int y = -(height - 1) / 2; y <= (height - 1) / 2; y++)
            {
                string info = mapData.tiles[(center.x + x).ToString() + "," + (center.y + y).ToString()];
                tempArray[index] = info;
                index++;
            }
        }
        RPC_SendMapData(player, tempArray, center, width, height);
    }
    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    private void RPC_SendMapData([RpcTarget] PlayerRef target, string[] tileList, Vector3Int center, int width, int height)
    {
        Debug.Log("��ȡ��������ͼ����:����" + center + "�ߴ�" + width + "/" + height);
        int index = 0;
        for (int x = -(width - 1) / 2; x <= (width - 1) / 2; x++)
        {
            for (int y = -(height - 1) / 2; y <= (height - 1) / 2; y++)
            {
                mapManager.LoadTile(new Vector3Int(center.x + x, center.y + y, 0), tileList[index]);
                index++;
            }
        }
    }
    #endregion
}
