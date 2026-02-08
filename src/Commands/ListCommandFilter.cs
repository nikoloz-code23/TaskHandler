using System;
using System.Threading.Tasks;
using TaskTracker.Enums;
using TaskTracker.Interfaces;
using TaskTracker.Utilities;

namespace TaskTracker.Commands;

public class ListCommandFilter : ICommand {
    public string? StatusFilter { get; set; } = null;

    public ListCommandFilter(string statusFilter)
    {
        StatusFilter = statusFilter.ToLower().Trim();
    }

    public async Task Execute(string filePath, JsonUtilities jsonUtilities)
    {
        if (StatusFilter == null)
            throw new Exception("Status filter is not being passed. Aborting!");

        switch (StatusFilter)
        {
            case "done":        await jsonUtilities.ListElementInArray(filePath, "Status", TodoTaskStatus.DONE);        break;
            case "todo":        await jsonUtilities.ListElementInArray(filePath, "Status", TodoTaskStatus.TODO);        break;
            case "in-progress": await jsonUtilities.ListElementInArray(filePath, "Status", TodoTaskStatus.IN_PROGRESS); break;
            default:            Console.WriteLine("The status does not exist. Try again or double check.");             break;
        }
    }
}