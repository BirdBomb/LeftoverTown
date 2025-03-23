using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
    public void WriteFile(string name,string json)
    {
        CheckPath();
        string dataPath = Application.dataPath + "/SaveData/" + name + ".json";
        using (StreamWriter writer = File.CreateText(dataPath))//利用工具将配置信息写入
        {
            writer.Write(json);
        }
    }
    public string ReadFile(string name)
    {
        CheckPath();
        string dataPath = Application.dataPath + "/SaveData/" + name + ".json";
        if (!File.Exists(dataPath))//若没有获取到文件
        {
            return "";
        }
        else
        {
            using (StreamReader reader = File.OpenText(dataPath))
            {
                return reader.ReadToEnd();
            }
        }
    }
    public void DeleteFile(string name)
    {
        string dataPath = Application.dataPath + "/SaveData/" + name + ".json";
        if (File.Exists(dataPath))
        {
            Debug.Log(dataPath);
            File.Delete(dataPath);
        }
    }
}
