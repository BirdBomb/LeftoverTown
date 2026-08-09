using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public class FileManager :SingleTon<FileManager>,ISingleTon
{
    public void Init()
    {
        
    }
    public void CheckPath()
    {
        if (!Directory.Exists(Application.dataPath + "/SaveData"))
            Directory.CreateDirectory(Application.dataPath + "/SaveData");
        if (!Directory.Exists(Application.dataPath + "/SaveData/MapData"))
            Directory.CreateDirectory(Application.dataPath + "/SaveData/MapData");
        if (!Directory.Exists(Application.dataPath + "/SaveData/PlayerData"))
            Directory.CreateDirectory(Application.dataPath + "/SaveData/PlayerData");
    }

    public PlayerData ReadPlayerData(string name)
    {
        CheckPath();
        PlayerData data = new PlayerData();
        string dataPath = $"{Application.dataPath}/SaveData/{name}.json";
        if (!File.Exists(dataPath))
        {
            data = null;
        }
        else
        {
            using (StreamReader reader = File.OpenText(dataPath))
            {
                data = new PlayerData();
                data = JsonConvert.DeserializeObject<PlayerData>(reader.ReadToEnd()); 
            }
        }
        return data;
    }
    public MapTileTypeData ReadTileTypeData(string name)
    {
        CheckPath();
        MapTileTypeData data = new MapTileTypeData();
        string dataPath = $"{Application.dataPath}/SaveData/{name}.json";
        if (!File.Exists(dataPath)) 
        {
            data = null;
        }
        else
        {
            byte[] fileData = File.ReadAllBytes(dataPath);
            using var stream = new MemoryStream(fileData);
            using var reader = new BinaryReader(stream);

            int count = reader.ReadInt32();
            if (count > 0)
            {
                var result = new Dictionary<int, short>(count);
                for (int i = 0; i < count; i++)
                {
                    int key = reader.ReadInt32();
                    result[key] = reader.ReadInt16();
                }
                data.tileDic = result;
            }
        }
        return data;
    }
    public MapTileInfoData ReadTileInfoData(string name)
    {
        CheckPath();
        MapTileInfoData data = new MapTileInfoData();
        string dataPath = $"{Application.dataPath}/SaveData/{name}.json";
        if (!File.Exists(dataPath))
        {
            data = null;
        }
        else
        {
            byte[] fileData = File.ReadAllBytes(dataPath);
            using var stream = new MemoryStream(fileData);
            using var reader = new BinaryReader(stream);

            int count = reader.ReadInt32();
            if (count > 0)
            {
                var result = new Dictionary<int, byte[]>(count);
                for (int i = 0; i < count; i++)
                {
                    int key = reader.ReadInt32();
                    int length = reader.ReadInt32();
                    byte[] value = reader.ReadBytes(length);
                    result[key] = value;
                }
                data.mapData = result;
            }
        }
        return data;
    }
    public MapInfoData ReadMapInfoData(string name)
    {
        CheckPath();
        MapInfoData data = new MapInfoData();
        string dataPath = $"{Application.dataPath}/SaveData/{name}.json";
        if (!File.Exists(dataPath))
        {
            data = null;
        }
        else
        {
            byte[] fileData = File.ReadAllBytes(dataPath);
            using var stream = new MemoryStream(fileData);
            using var reader = new BinaryReader(stream);


            data.name = reader.ReadString();
            data.seed = reader.ReadString();
            data.distance = reader.ReadInt16();
            data.date = reader.ReadInt16();
            data.hour = reader.ReadInt16();
        }
        return data;
    }

    public async Task WriteBytes(string name, PlayerData data)
    {
        CheckPath();
        string dataPath = Path.Combine(Application.dataPath, "SaveData", $"{name}.json");
        Debug.Log(dataPath);
        string json = JsonConvert.SerializeObject(data);
        await File.WriteAllTextAsync(dataPath, json);
    }
    public async Task WriteBytes(string name, MapTileTypeData data)
    {
        CheckPath();
        string dataPath = Path.Combine(Application.dataPath, "SaveData", $"{name}.json");
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream);
        // 写入字典数量
        writer.Write(data.tileDic.Count);
        foreach (var kvp in data.tileDic)
        {
            writer.Write(kvp.Key);                    // key (int)
            writer.Write(kvp.Value);                  // byte[] 数据
        }
        await File.WriteAllBytesAsync(dataPath, stream.ToArray());
    }
    public async Task WriteBytes(string name, MapTileInfoData data)
    {
        CheckPath();
        string dataPath = Path.Combine(Application.dataPath, "SaveData", $"{name}.json");
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream);

        // 写入字典数量
        writer.Write(data.mapData.Count);
        foreach (var kvp in data.mapData)
        {
            writer.Write(kvp.Key);                    // key (int)
            writer.Write(kvp.Value.Length);           // byte[] 长度
            writer.Write(kvp.Value);                  // byte[] 数据
        }

        await File.WriteAllBytesAsync(dataPath, stream.ToArray());
    }
    public async Task WriteBytes(string name, MapInfoData data)
    {
        CheckPath();
        string dataPath = Path.Combine(Application.dataPath, "SaveData", $"{name}.json");
        using var stream = new MemoryStream();
        using var writer = new BinaryWriter(stream);

        // 写入字典数量
        writer.Write(data.name);
        writer.Write(data.seed);
        writer.Write(data.distance);
        writer.Write(data.date);
        writer.Write(data.hour);

        await File.WriteAllBytesAsync(dataPath, stream.ToArray());
    }

    public void DeleteFile(string name)
    {
        string dataPath = Path.Combine(Application.dataPath, "SaveData", $"{name}.json");
        if (File.Exists(dataPath))
        {
            File.Delete(dataPath);
        }
        else
        {
            Debug.Log("未找到" + dataPath);
        }
    }
}
