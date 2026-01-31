using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace TaskTracker.Utilities;

public static class JsonUtilities
{
    public static JsonSerializerOptions JsonOptions { get; set; } = new() 
    {
        WriteIndented = true
    };

    public static void AddElementToArray<T>(string filePath, T obj)
    {
        if (!File.Exists(filePath))
        {
            FileUtilities.CreateFile(filePath, "[]");
        }

        JsonNode? jsonNode;

        try
        {
            jsonNode = JsonNode.Parse(File.ReadAllText(filePath));
        }
        catch (JsonException)
        {
            Console.WriteLine("The JSON is invalid.");
            return;
        }
        
        JsonArray jsonArray;
        
        try
        {
            jsonArray = jsonNode!.AsArray();   
        }
        catch (InvalidOperationException)
        {
            Console.WriteLine("The JSON does not contain an array.");
            return;
        }
        
        string serializedObj = JsonSerializer.Serialize(obj);
        jsonArray.Add(JsonNode.Parse(serializedObj));

        string jsonString = jsonArray!.ToJsonString(JsonOptions);
        File.WriteAllText(filePath, jsonString);
    }
}