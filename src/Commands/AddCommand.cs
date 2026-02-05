using System;
using TaskTracker.Types;
using TaskTracker.Enums;
using TaskTracker.Interfaces;
using TaskTracker.Utilities;
using TaskTracker.Factory;

namespace TaskTracker.Commands;

public class AddCommand : ICommand {
    public TodoTaskFactory? Factory { get; set; }
    public string Description { get; set; } = "";

    public AddCommand(string description, TodoTaskFactory factory)
    {
        Description = description;
        Factory = factory;
    }

    public void Execute(string filePath, JsonUtilities jsonUtilities)
    {
        if (Description.Equals(""))
        {
            Console.WriteLine("Add a description!");
            return;
        }

        if(Factory == null)
        {
            Console.WriteLine("Factory not detected. Please pass the factory.");
            return;
        }

        TodoTask todoTask = Factory.CreateTodoTask(Description, TodoTaskStatus.TODO);
        jsonUtilities.AddElementToArray<TodoTask>(filePath, todoTask);
    }
}