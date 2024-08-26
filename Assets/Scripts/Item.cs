using System.IO;
using System.Reflection;
using UnityEngine;


[System.Serializable]
public class Item
{
    [SerializeField]
    public string Title;
    [SerializeField]
    public string Author;
    [SerializeField]
    public string Subject;
    [SerializeField]
    public int SystemNumber;
    [SerializeField]
    public int CallNumber;
    [SerializeField]
    public enum status { AVAILABLE, LIMITED, RESTRICTED, NOT_AVAILABLE }


    public static string directory = "Assets/JSONs/";

    public static Item ReadItemJSON(string fileName)
    {
        string jsonString = File.ReadAllText(directory + fileName + ".json");
        return JsonUtility.FromJson<Item>(jsonString);
    }

    public static void WriteItemJSON(Item item)
    {
        string jsonString = JsonUtility.ToJson(item);
        string filePath = directory + item.Title + ".json";
        File.WriteAllText(filePath, jsonString);
    }

    public void Load()
    {
        var content = File.ReadAllText(directory);
        var book = JsonUtility.FromJson<Item>(content);

        Title = book.Title;
        Author = book.Author;
        Subject = book.Subject;
        SystemNumber = book.SystemNumber;
        CallNumber = book.CallNumber;
    }
}
