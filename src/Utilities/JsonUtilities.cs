using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace TaskTracker.Utilities;

public class JsonUtilities
{
    public JsonSerializerOptions JsonOptions { get; set; } = new() 
    {
        WriteIndented = true
    };

    public string IdPropertyName {get; set;} = "";
    public string UpdatePropertyName {get; set; } = "";

    public T? GetLastPropertyValue<T>(string filePath, string propertyName)
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

    public void AddElementToArray<T>(string filePath, object obj)
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

    public void UpdateElementInArray<T>(string filePath, object idSearch, object newData, string propertyName)
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
            ObjectUtilities.SetValueInProperty(type, element, UpdatePropertyName, DateTime.Now.ToString());

            Console.WriteLine($"Element {idValue} updated succesfully!");            
        }

        string newJsonString = JsonSerializer.Serialize(elementsInJson, JsonOptions);
        File.WriteAllText(filePath, newJsonString);
    }

    public void RemoveElementInArray<T>(string filePath, object idSearch)
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
        
        for(int i = elementsInJson.Count - 1; i >= 0; i--)
        {
            object? element = elementsInJson[i];
            if(element == null) continue;
        
            Type type = element.GetType();
            object? idValue = ObjectUtilities.ReturnValueFromProperty(type, element, IdPropertyName);

            if (!Equals(idValue, idSearch))
                continue;

            elementsInJson.RemoveAt(i);

            Console.WriteLine($"Element with Id {idSearch} has been deleted succesfully!");
        }

        string newJsonString = JsonSerializer.Serialize(elementsInJson, JsonOptions);
        File.WriteAllText(filePath, newJsonString);
    }

    public void ListElementInArray(string filePath)
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

        if (fileData == null)
            throw new ArgumentNullException("Can't access file data");

        JsonArray jsonArray = JsonNode.Parse(fileData)!.AsArray();

        foreach(var element in jsonArray)
        {
            if (element == null) continue;
            Console.WriteLine(element.ToString());
        }
    }

    public void ListElementInArray(string filePath, string propertyName, object valueToFilterWith)
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

        if (fileData == null)
            throw new ArgumentNullException("Can't access file data");

        JsonArray jsonArray = JsonNode.Parse(fileData)!.AsArray();

        // The solution here is specifically for int based statuses in a JSON.
        // TODO: Find a way to generalize the solution here so we can list with more filters.
        int checkValue = (int)valueToFilterWith;
        foreach(var element in jsonArray)
        {
            if (element == null || element[propertyName] == null) continue;
            
            int elementValue = (int)element[propertyName]!;
            if(elementValue != checkValue) continue;
            Console.WriteLine(element.ToString());
        }
    }
}