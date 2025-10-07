using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    [SerializeField] private List<Transform> _objectsToSave;

    private string _filePath;

    private void Awake()
    {
        _filePath = Path.Combine(Application.persistentDataPath, "objectPositions.json");
    }

    public void Save()
    {
        List<ObjectData> dataList = new List<ObjectData>();

        foreach (var obj in _objectsToSave)
        {
            ObjectData data = new ObjectData
            {
                ObjectID = obj.GetComponent<ObjectUniqueID>().ID,
                Position = obj.position
            };
            dataList.Add(data);
        }

        string json = JsonUtility.ToJson(new ObjectDataList { Objects = dataList }, true);
        File.WriteAllText(_filePath, json);
        Debug.Log($"Saved {dataList.Count} objects to {_filePath}");
    }

    public void Load()
    {
        if (!File.Exists(_filePath))
        {
            Debug.LogWarning("Saved file not found");
            return;
        }

        string json = File.ReadAllText(_filePath);
        ObjectDataList loadedData = JsonUtility.FromJson<ObjectDataList>(json);

        foreach (var data in loadedData.Objects)
        {
            Transform obj = _objectsToSave.Find(o => o.GetComponent<ObjectUniqueID>().ID == data.ObjectID);
            if (obj != null)
            {
                obj.position = data.Position;
            }
        }

        Debug.Log("Loaded succesfuly");
    }

  
    [System.Serializable]
    public class ObjectData
    {
        public string ObjectID;
        public Vector3 Position;
    }

    [System.Serializable]
    public class ObjectDataList
    {
        public List<ObjectData> Objects;
    }

}
