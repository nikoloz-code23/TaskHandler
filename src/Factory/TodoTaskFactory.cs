using System;
using System.Threading.Tasks;
using TaskTracker.Types;
using TaskTracker.Enums;
using TaskTracker.Utilities;

namespace TaskTracker.Factory;

public class TodoTaskFactory
{
    public static int Id { get; set; } = 0;

    public TodoTaskFactory() { }

    public async Task InitializeFactory(string filePath, JsonUtilities jsonUtilities)
    {
        int? IdFromJson = await jsonUtilities.GetLastPropertyValue<int?>(filePath, jsonUtilities.IdPropertyName);
        
        if (IdFromJson != null)
        {
            Id = (int)IdFromJson;
            Id++;
        }
    }

    public TodoTask CreateTodoTask(string description, TodoTaskStatus status)
    {
        TodoTask task = new();
        task.Id = Id;
        task.Description = description;
        task.Status = status;
        task.CreatedAt = DateTime.Now.ToString();
        task.UpdatedAt = null;

        Id++;

        return task;
    }
}