using System.IO;

namespace TaskTracker.Utilities;

public static class FileUtilities
{
    public static void CreateFile(string filePath, string defaultString = "")
    {
        using (File.Create(filePath)) {};
        if (defaultString.Equals("")) return;
        File.WriteAllText(filePath, defaultString);
    }
}