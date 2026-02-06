using System;
using TaskTracker.Types;
using TaskTracker.Interfaces;
using TaskTracker.Utilities;
using TaskTracker.Enums;

namespace TaskTracker.Commands;

public class MarkInProgressCommand : ICommand {
    public int? Id { get; set; } = null; 

    public MarkInProgressCommand(int id)
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

        jsonUtilities.UpdateElementInArray<TodoTask>(filePath, Id, TodoTaskStatus.IN_PROGRESS, "Status");
    }
}