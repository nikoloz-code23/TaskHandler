using System;
using System.Collections.Generic;
using TaskTracker.Utilities;

namespace TaskTracker.Handlers;

public class InputHandler
{
    public char TextMark { get; set; }
    public char[] CharactersToIgnore { get; set; } = ['\n', '\t'];

    public InputHandler() { }

    private bool ContainsTextChar(string stringToCheck)
    {
        if (TextMark.Equals('\0')) throw new Exception("TextMark is not defined.");
        return stringToCheck.Contains(TextMark);
    }

    public string GetUserInput()
    {
        return string.Join("", Console.ReadLine()!.Split(CharactersToIgnore)); 
    }

    public string BuildTextArg(List<string> inputList, int firstIndex, int secondIndex)
    {
        string argString = string.Empty;

        for(int i = 0; i < inputList.Count; i++)
        {
            if (i >= firstIndex && i <= secondIndex)
            {
                if (argString == string.Empty) argString = inputList[i];
                else argString = string.Join(' ', argString, inputList[i]);
            }
        }

        return argString;
    }

    public List<string> HandleInput(List<string> inputList)
    {
        int firstIndex = -1, secondIndex = -1;
        int inputListSize = inputList.Count;

        firstIndex = ListUtilities.IterateUntilPredicate(inputList, ContainsTextChar);
        secondIndex = ListUtilities.IterateUntilPredicateReverse(inputList, ContainsTextChar);

        if (!firstIndex.Equals(-1) || !secondIndex.Equals(-1))
        {
            string argString = BuildTextArg(inputList, firstIndex, secondIndex);
            inputList.RemoveRange(firstIndex, secondIndex - firstIndex + 1);
            inputList.Insert(firstIndex, argString);
        }
    
        return inputList;
    }
}