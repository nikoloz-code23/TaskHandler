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

    public static string IdPropertyName {get; set;} = "Id";
    public static string UpdatePropertyName {get; set; } = "UpdatedAt";

    public static T? GetLastPropertyValue<T>(string filePath, string propertyName)
    {
        if(!File.Exists(filePath)) return default;

        string? fileData = File.ReadAllText(filePath);

        JsonNode? jsonNode;

        try
        {
            jsonNode = JsonNode.Parse(fileData);
        }
        catch (JsonException e)
        {
            Console.WriteLine($"The JSON is invalid: {e}");
            throw;
        }

        JsonArray jsonArray;
        
        try
        {
            jsonArray = jsonNode!.AsArray();   
        }
        catch (InvalidOperationException e)
        {
            Console.WriteLine($"The JSON does not contain an array: {e}");
            throw;
        }

        JsonObject? jsonObj;

        if (jsonArray.Count > 0)
            jsonObj = jsonArray[jsonArray.Count - 1]?.AsObject();
        else
            return default;
        
        if (jsonObj == null) return default;

        T? value = jsonObj[propertyName]!.GetValue<T>();
        if (value == null) throw new InvalidOperationException("No property of that name exists.");

        return value;
    }

    public static void AddElementToArray<T>(string filePath, object obj)
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
        catch (JsonException e)
        {
            Console.WriteLine($"The JSON is invalid: {e}");
            throw;
        }
        
        JsonArray jsonArray;
        
        try
        {
            jsonArray = jsonNode!.AsArray();   
        }
        catch (InvalidOperationException e)
        {
            Console.WriteLine($"The JSON does not contain an array: {e}");
            throw;
        }
        
        string serializedObj = JsonSerializer.Serialize(obj);
        jsonArray.Add(JsonNode.Parse(serializedObj));

        string jsonString = jsonArray!.ToJsonString(JsonOptions);
        File.WriteAllText(filePath, jsonString);

        Console.WriteLine("New element added succesfully!");
    }

    public static void UpdateElementInArray<T>(string filePath, object idSearch, object newData, string propertyName)
    {
        string? fileData = null;

        try
        {
            fileData = File.ReadAllText(filePath);
        }
        catch (FileNotFoundException e)
        {
            Console.WriteLine(e);
        }

        IList<T>? elementsInJson;                
        
        if (fileData == null)
            throw new ArgumentNullException("Can't access file data");

        try
        {
            elementsInJson = JsonSerializer.Deserialize<IList<T>>(fileData);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        if (elementsInJson == null || elementsInJson.Count == 0)
        {
            Console.WriteLine("There are no elements to update.");
            return;
        }
        
        foreach(object? element in elementsInJson)
        {
            if (element == null) continue;

            Type type = element.GetType();
            object? idValue = ObjectUtilities.ReturnValueFromProperty(type, element, IdPropertyName);

            if (!Equals(idValue, idSearch))
                continue;

            ObjectUtilities.SetValueInProperty(type, element, propertyName, newData);
            ObjectUtilities.SetValueInProperty(type, element, UpdatePropertyName, DateTime.Now);

            Console.WriteLine($"Element {idValue} updated succesfully!");            
        }

        string newJsonString = JsonSerializer.Serialize(elementsInJson, JsonOptions);
        File.WriteAllText(filePath, newJsonString);
    }
}