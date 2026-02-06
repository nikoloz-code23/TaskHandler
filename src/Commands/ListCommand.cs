using TaskTracker.Interfaces;
using TaskTracker.Utilities;

namespace TaskTracker.Commands;

public class ListCommand : ICommand {
    public void Execute(string filePath, JsonUtilities jsonUtilities)
    {
        jsonUtilities.ListElementInArray(filePath);
    }
}