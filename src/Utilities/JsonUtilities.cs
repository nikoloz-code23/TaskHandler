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

    public static void AddElement<T>(string filePath, T obj)
    {
        if (!File.Exists(filePath))
        {
            FileUtilities.CreateFile(filePath, JsonSerializer.Serialize("[]"));
        }

        JsonNode? jsonNode = null;

        try
        {
            jsonNode = JsonNode.Parse(File.ReadAllText(filePath));
            jsonNode ??= JsonNode.Parse("");  
        }
        catch (JsonException e)
        {
            Console.WriteLine($"Invalid JSON String encountered: {e}");
            throw;
        }
        
        JsonArray? jsonArray = jsonNode?.AsArray();
        jsonArray ??= [];
        
        string serializedObj = JsonSerializer.Serialize(obj);
        jsonArray.Add(JsonNode.Parse(serializedObj));

        var jsonString = jsonArray.ToJsonString(JsonOptions);
        File.WriteAllText(filePath, jsonString);
    }
}