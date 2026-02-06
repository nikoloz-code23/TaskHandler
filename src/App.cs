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

        TodoTaskFactory factory = new(FilePath, jsonUtilities);

        while(true)
        {
            Console.Write(appPrefix);
            
            string? userInput = inputHandler.GetUserInput();
            if (userInput == null) continue;

            List<string> inputList = [..userInput.Split(' ')];

            inputHandler.HandleInput(inputList);

            string command = inputList[0].ToLower();

            switch(command)
            {
                case "add":
                    if(inputList.Count < 2)
                    {
                        Console.WriteLine("Add command requires an argument for description!");
                        continue;    
                    }

                    AddCommand addCommand = new(inputList[1], factory);
                    addCommand.Execute(FilePath, jsonUtilities);
                break;

                case "update":
                    if(inputList.Count < 3)
                    {
                        Console.WriteLine("Update command requires an argument for an id and a new description!");
                        continue;    
                    }

                    UpdateDescriptionCommand updateDescriptionCommand = new(int.Parse(inputList[1]), inputList[2], "Description");
                    updateDescriptionCommand.Execute(FilePath, jsonUtilities);
                break;

                case "mark-in-progress":
                    if(inputList.Count < 2)
                    {
                        Console.WriteLine("Update command requires an argument for an id!");
                        continue;    
                    }

                    MarkInProgressCommand markInProgressCommand = new(int.Parse(inputList[1]));
                    markInProgressCommand.Execute(FilePath, jsonUtilities);
                break;

                case "mark-done":
                    if(inputList.Count < 2)
                    {
                        Console.WriteLine("Update command requires an argument for an id!");
                        continue;    
                    }

                    MarkDone markDoneCommand = new(int.Parse(inputList[1]));
                    markDoneCommand.Execute(FilePath, jsonUtilities);
                break;

                case "delete":
                    if(inputList.Count < 2)
                    {
                        Console.WriteLine("Update command requires an argument for an id!");
                        continue;    
                    }

                    RemoveCommand removeCommand = new(int.Parse(inputList[1]));
                    removeCommand.Execute(FilePath, jsonUtilities);
                break;

                case "help":
                    Console.WriteLine(
                    """
                    =======    Task-Cli     ========
                    - add -> Adds a new task! Takes a description of your task for adding.
                    - update -> Updates an already existing task! Takes an id and a new description.
                    - mark-in-proress -> Updates the task status to "In-Progress". Takes an id.
                    - mark-done -> Updates the task status to "Done". Takes an id.
                    - delete -> Deletes a task from the list. Takes an id.
                    - list -> Lists all tasks. If you add "done", "todo" or "in-progress", will list by status.
                    ======= Enjoy using it! ========
                    """
                    );
                break;

                default:
                    Console.WriteLine("Command doesn't exist! Use 'help' to check available commands.");
                break;
            }
        }
        
    }
}