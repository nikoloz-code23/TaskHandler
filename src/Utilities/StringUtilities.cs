namespace TaskTracker.Utilities;

public static class StringUtilities
{
    public static string FindSubstringInBetweenChar(ref string input, char character)
    {
        int firstIndex = input.IndexOf(character); 
        int secondIndex = input.LastIndexOf(character);
        if (firstIndex == -1 || secondIndex == -1) return "";

        string substring = input.Substring(firstIndex + 1, secondIndex - firstIndex - 1); 
        input = input.Remove(firstIndex, secondIndex - firstIndex + 1);
        
        return substring;
    }
}