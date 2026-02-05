using System;
using TaskTracker.Types;
using TaskTracker.Interfaces;
using TaskTracker.Utilities;
using TaskTracker.Enums;

namespace TaskTracker.Commands;

public class MarkDone : ICommand {
    public object? Id { get; set; }

    public MarkDone(object id)
    {
        Id = id;
    }

    public void Execute(string filePath, JsonUtilities jsonUtilities)
    {
        if (Id == null)
        {
            Console.WriteLine("Specify the id of the element to remove.");
            return;
        }

        jsonUtilities.UpdateElementInArray<TodoTask>(filePath, Id, TodoTaskStatus.DONE, "Status");
    }
}