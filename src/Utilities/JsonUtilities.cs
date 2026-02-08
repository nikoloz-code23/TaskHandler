using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace TaskTracker.Utilities;

public class JsonUtilities
{
    public JsonSerializerOptions JsonOptions { get; set; } = new() 
    {
        WriteIndented = true,
    };

    public string IdPropertyName {get; set;} = "";
    public string UpdatePropertyName {get; set; } = "";

    public async Task<T?> GetLastPropertyValueAsync<T>(string filePath, string propertyName)
    {
        if(!File.Exists(filePath)) return default;
        
        using FileStream fileData = File.OpenRead(filePath);

        JsonNode? jsonNode;

        try
        {
            jsonNode = await JsonNode.ParseAsync(fileData);
        }
        catch (JsonException e)
        {
            Console.WriteLine($"The JSON is invalid: {e}");
            throw;
        }

        var jsonArray = jsonNode!.AsArray();   

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
    
    public async Task<JsonNode?> SerializeAndReturnJsonNodeAsync(object obj)
    {
        using (MemoryStream memoryStream = new())
        {
            await JsonSerializer.SerializeAsync(memoryStream, obj);
            memoryStream.Position = 0;
            return await JsonNode.ParseAsync(memoryStream);
        }
    }

    public async Task WriteNewDataToFileAsync(string filePath, JsonNode jsonData)
    {
        string newJsonContentsString = jsonData.ToJsonString(JsonOptions);
        await File.WriteAllTextAsync(filePath, newJsonContentsString);
    } 

    public async Task AddElementToArrayAsync<T>(string filePath, object obj)
    {
        if (!File.Exists(filePath))
        {
            await FileUtilities.CreateFileAsync(filePath, "[]");
        }

        JsonNode? oldJsonContent;

        try
        {
            using FileStream fs = File.OpenRead(filePath);
            oldJsonContent = await JsonNode.ParseAsync(fs);
        }
        catch (JsonException e)
        {
            Console.WriteLine($"The JSON is invalid: {e}");
            throw;
        }
        catch (FileNotFoundException e)
        {
            Console.WriteLine($"File doesn't exist! {e}");
            return;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
        JsonArray jsonArray;
        
        try
        {
            jsonArray = oldJsonContent!.AsArray();   
        }
        catch (InvalidOperationException e)
        {
            Console.WriteLine($"The JSON does not contain an array: {e}");
            throw;
        }

        JsonNode? newElement = await SerializeAndReturnJsonNodeAsync(obj);

        if(newElement == null)
            throw new NoNullAllowedException("Something went wrong. Aborting!");
        
        jsonArray.Add(newElement);
        await WriteNewDataToFileAsync(filePath, jsonArray);
        Console.WriteLine("New element added succesfully!");    
    }

    public async Task UpdateElementInArrayAsync<T>(string filePath, object idSearch, object newData, string propertyName)
    {
        IList<T>? elementsInJson;    

        try
        {
            using FileStream fs = File.OpenRead(filePath);
            elementsInJson = await JsonSerializer.DeserializeAsync<IList<T>>(fs);
        }
        catch (FileNotFoundException e)
        {
            Console.WriteLine($"File doesn't exist! {e}");
            return;
        }
        catch (JsonException e)
        {
            Console.WriteLine($"The JSON is invalid: {e}");
            throw;
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


        JsonNode? newJsonContents = await SerializeAndReturnJsonNodeAsync(elementsInJson);

        if(newJsonContents == null)
            throw new NoNullAllowedException("Something went clearly wrong during Parsing process. Aborting!");

        await WriteNewDataToFileAsync(filePath, newJsonContents);
    }

    public async Task RemoveElementInArrayAsync<T>(string filePath, object idSearch)
    {
        IList<T>? elementsInJson;    

        try
        {
            using FileStream fs = File.OpenRead(filePath);
            elementsInJson = await JsonSerializer.DeserializeAsync<IList<T>>(fs);
        }
        catch (FileNotFoundException e)
        {
            Console.WriteLine($"File doesn't exist! {e}");
            return;
        }
        catch (JsonException e)
        {
            Console.WriteLine($"The JSON is invalid: {e}");
            throw;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        if (elementsInJson == null || elementsInJson.Count == 0)
        {
            Console.WriteLine("There are no elements to remove.");
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

            await Task.Delay(100);
            Console.WriteLine($"Element with Id {idSearch} has been deleted succesfully!");
        }

        JsonNode? newJsonContents = await SerializeAndReturnJsonNodeAsync(elementsInJson);

        if(newJsonContents == null)
            throw new NoNullAllowedException("Something went clearly wrong during Parsing process. Aborting!");

        await WriteNewDataToFileAsync(filePath, newJsonContents);
    }

    public async Task ListElementInArrayAsync(string filePath)
    {
        JsonNode? oldJsonContent;

        try
        {
            using FileStream fs = File.OpenRead(filePath);
            oldJsonContent = await JsonNode.ParseAsync(fs);
        }
        catch (FileNotFoundException e)
        {
            Console.WriteLine($"File doesn't exist! {e}");
            return;
        }
        catch (JsonException e)
        {
            Console.WriteLine($"The JSON is invalid: {e}");
            throw;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        if(oldJsonContent == null)
        {
            Console.WriteLine("There are no elements to list.");
            return;
        }
        
        JsonArray jsonArray = oldJsonContent.AsArray();

        foreach(var element in jsonArray)
        {
            if (element == null) continue;
            Console.WriteLine(element.ToString());
        }
    }

    public async Task ListElementInArrayAsync(string filePath, string propertyName, object valueToFilterWith)
    {
        JsonNode? oldJsonContent;

        try
        {
            using FileStream fs = File.OpenRead(filePath);
            oldJsonContent = await JsonNode.ParseAsync(fs);
        }
        catch (FileNotFoundException e)
        {
            Console.WriteLine($"File doesn't exist! {e}");
            return;
        }
        catch (JsonException e)
        {
            Console.WriteLine($"The JSON is invalid: {e}");
            throw;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        if(oldJsonContent == null)
        {
            Console.WriteLine("There are no elements to list.");
            return;
        }
        
        JsonArray jsonArray = oldJsonContent.AsArray();

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