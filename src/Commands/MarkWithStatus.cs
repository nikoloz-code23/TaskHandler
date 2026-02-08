using System;
using System.Threading.Tasks;
using TaskTracker.Types;
using TaskTracker.Interfaces;
using TaskTracker.Utilities;
using TaskTracker.Enums;

namespace TaskTracker.Commands;

public class MarkWithStatus : ICommand {
    public int? Id { get; set; } = null;
    public TodoTaskStatus NewStatus { get; set; }

    public MarkWithStatus(int id, TodoTaskStatus status)
    {
        Id = id;
        NewStatus = status;
    }

    public async Task Execute(string filePath, JsonUtilities jsonUtilities)
    {
        if (Id == null)
        {
            Console.WriteLine("Specify the id of the element to change status of.");
            return;
        }

        await jsonUtilities.UpdateElementInArrayAsync<TodoTask>(filePath, Id, NewStatus, "Status");
    }
}