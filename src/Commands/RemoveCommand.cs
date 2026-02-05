using System;
using TaskTracker.Types;
using TaskTracker.Interfaces;
using TaskTracker.Utilities;

namespace TaskTracker.Commands;

public class RemoveCommand : ICommand {
    public object? Id { get; set; }

    public RemoveCommand(object id)
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

        jsonUtilities.RemoveElementInArray<TodoTask>(filePath, Id);
    }
}