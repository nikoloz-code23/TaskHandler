using System;
using System.Collections.Generic;
using TaskTracker.Handlers;
using TaskTracker.Commands;
using TaskTracker.Utilities;
using TaskTracker.Factory;

namespace TaskTracker.Application;

public class App
{
    public string FilePath { get; set; } = "./todo.json";
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

        JsonUtilities jsonUtilities = new();
        jsonUtilities.IdPropertyName = "Id";
        jsonUtilities.UpdatePropertyName = "UpdatedAt";

        TodoTaskFactory factory = new(FilePath, jsonUtilities, "Id");

        while(true)
        {
            Console.Write(appPrefix);
            
            string? userInput = inputHandler.GetUserInput();
            if (userInput == null) continue;

            List<string> inputList = [..userInput.Split(' ')];

            inputHandler.HandleInput(inputList);

            string command = inputList[0].ToLower();

            for(int i = 0; i < inputList.Count; i++)
            {
                Console.WriteLine($"{i}: {inputList[i]}");
            }

            switch(command)
            {
                case "add":
                    AddCommand addCommand = new(inputList[1], factory);
                    addCommand.Execute(FilePath, jsonUtilities);
                break;

                case "update":
                    UpdateDescriptionCommand updateDescriptionCommand = new(int.Parse(inputList[1]), inputList[2], "Description");
                    updateDescriptionCommand.Execute(FilePath, jsonUtilities);
                break;

                case "mark-in-progress":
                    MarkInProgressCommand markInProgressCommand = new(int.Parse(inputList[1]));
                    markInProgressCommand.Execute(FilePath, jsonUtilities);
                break;

                case "mark-done":
                    MarkDone markDoneCommand = new(int.Parse(inputList[1]));
                    markDoneCommand.Execute(FilePath, jsonUtilities);
                break;

                case "delete":
                    RemoveCommand removeCommand = new(int.Parse(inputList[1]));
                    removeCommand.Execute(FilePath, jsonUtilities);
                break;
            }
        }
        
    }
}