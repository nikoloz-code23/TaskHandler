using System;
using TaskTracker.Types;
using TaskTracker.Enums;
using TaskTracker.Utilities;

namespace TaskTracker.Factory;

public class TodoTaskFactory
{
    public static int Id { get; set; }

    public TodoTaskFactory(string filePath, JsonUtilities jsonUtilities, string idPropertyName)
    {
        int? IdFromJson = jsonUtilities.GetLastPropertyValue<int>(filePath, idPropertyName);
        
        if (IdFromJson.Equals(null) || IdFromJson.Equals(default(int))) 
            Id = 0;
        else 
            Id = (int)++IdFromJson;
    }

    public TodoTask CreateTodoTask(string description, TodoTaskStatus status)
    {
        TodoTask task = new();
        task.Id = Id;
        task.Description = description;
        task.Status = status;
        task.CreatedAt = DateTime.Now;
        task.UpdatedAt = null;

        Id++;

        return task;
    }
}