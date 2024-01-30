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
    }
    public void WriteFile(string name,string json)
    {
        CheckPath();
        string dataPath = Application.dataPath + "/SaveData/" + name + ".json";
        using (StreamWriter writer = File.CreateText(dataPath))//���ù��߽�������Ϣд��
        {
            writer.Write(json);
        }
    }
    public string ReadFile(string name)
    {
        CheckPath();
        string dataPath = Application.dataPath + "/SaveData/" + name + ".json";
        if (!File.Exists(dataPath))//��û�л�ȡ���ļ�
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
}
