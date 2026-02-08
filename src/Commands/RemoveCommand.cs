using System;
using System.Threading.Tasks;
using TaskTracker.Types;
using TaskTracker.Interfaces;
using TaskTracker.Utilities;

namespace TaskTracker.Commands;

public class RemoveCommand : ICommand {
    public int? Id { get; set; } = null;

    public RemoveCommand(int id)
    {
        Id = id;
    }
    public async Task Execute(string filePath, JsonUtilities jsonUtilities)
    {
        if (Id == null)
        {
            Console.WriteLine("Specify the id of the element to remove.");
            return;
        }

        await jsonUtilities.RemoveElementInArray<TodoTask>(filePath, Id);
    }
}