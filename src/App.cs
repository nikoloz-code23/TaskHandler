using System;
using System.Collections.Generic;
using TaskTracker.Handlers;

namespace TaskTracker;

public class App
{
    public string AppName { get; set; }
    public char AppNameSuffix { get; set; }

    public App()
    {
        AppName = "AppName";
        AppNameSuffix = '>';
    }

    public App(string name)
    {
        AppName = name;
        AppNameSuffix = '>';
    }

    public App(string name, char nameSuffix)
    {
        AppName = name;
        AppNameSuffix = nameSuffix;
    }

    public void Run(char textMark = '"')
    {
        string appPrefix = AppName + AppNameSuffix + ' '; 

        InputHandler inputHandler = new InputHandler();
        inputHandler.TextMark = textMark;

        while(true)
        {
            Console.Write(appPrefix);
            
            string? userInput = inputHandler.GetUserInput();
            if (userInput == null) continue;

            List<string> inputList = [..userInput.Split(' ')];

            inputHandler.HandleInput(inputList);

            foreach(var element in inputList)
            {
                Console.WriteLine(element);
            }
        }
    }
}