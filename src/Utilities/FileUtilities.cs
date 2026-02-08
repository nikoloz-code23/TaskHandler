using System.IO;
using System.Threading.Tasks;

namespace TaskTracker.Utilities;

public static class FileUtilities
{
    public static async Task CreateFileAsync(string filePath, string defaultString = "")
    {
        using (File.Create(filePath)) {};
        if (defaultString.Equals("")) return;
        await File.WriteAllTextAsync(filePath, defaultString);
    }
}