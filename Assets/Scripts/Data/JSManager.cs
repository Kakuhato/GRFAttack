using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JSManager : RegulatorSingleton<JSManager>
{
    private string dataSavePath;
    
    public void SaveData(object data, string fileName)
    {
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(dataSavePath + "/" + fileName + ".json", json);
    }
    
    public T LoadData<T>(string fileName) where T : new()
    {
        string filePath = dataSavePath + "/" + fileName + ".json";
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            return JsonUtility.FromJson<T>(json);
        }
        else
        {
            Debug.LogWarning($"File {fileName} not found at {dataSavePath}");
            // 如果文件不存在，查找是否有默认值
            T defaultData = new T();
            SaveData(defaultData, fileName); // 保存默认值
            return defaultData;
        }
    }

    protected override void InitialSingleton()
    {
        base.InitialSingleton();
        dataSavePath = Application.persistentDataPath;
    }
}
