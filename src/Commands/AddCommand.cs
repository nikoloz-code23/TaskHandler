using System;
using TaskTracker.Types;
using TaskTracker.Enums;
using TaskTracker.Interfaces;
using TaskTracker.Utilities;
using TaskTracker.Factory;

namespace TaskTracker.Commands;

public class AddCommand : ICommand {
    public TodoTaskFactory? Factory { get; set; }
    public string Description { get; set; } = string.Empty;

    public AddCommand(string description, TodoTaskFactory factory)
    {
        Description = description.Trim();
        Factory = factory;
    }

    public void Execute(string filePath, JsonUtilities jsonUtilities)
    {
        if (Description == string.Empty)
        {
            Console.WriteLine("Add a description for the task.");
            return;
        }

        if(Factory == null)
            throw new Exception("Factory not detected. Something has went terribly wrong! Aborting.");

        TodoTask todoTask = Factory.CreateTodoTask(Description.Trim(), TodoTaskStatus.TODO);
        jsonUtilities.AddElementToArray<TodoTask>(filePath, todoTask);
    }
}