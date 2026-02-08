using System.Threading.Tasks;
using TaskTracker.Interfaces;
using TaskTracker.Utilities;

namespace TaskTracker.Commands;

public class ListCommand : ICommand {
    public async Task Execute(string filePath, JsonUtilities jsonUtilities)
    {
        await jsonUtilities.ListElementInArrayAsync(filePath);
    }
}