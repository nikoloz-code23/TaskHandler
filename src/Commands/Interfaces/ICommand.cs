using TaskTracker.Utilities;
using TaskTracker.Factory;

namespace TaskTracker.Interfaces;

public interface ICommand
{
    public void Execute(string filePath, JsonUtilities jsonUtilities);
}