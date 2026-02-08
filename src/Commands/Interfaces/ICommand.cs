using System.Threading.Tasks;
using TaskTracker.Utilities;

namespace TaskTracker.Interfaces;

public interface ICommand
{
    public Task Execute(string filePath, JsonUtilities jsonUtilities);
}