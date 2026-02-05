using System;
using TaskTracker.Types;
using TaskTracker.Interfaces;
using TaskTracker.Utilities;

namespace TaskTracker.Commands;

public class UpdateDescriptionCommand : ICommand {
    public object? Id { get; set; }
    public object? NewDescription { get; set; }
    public string? DescriptionProperty { get; set; }

    public UpdateDescriptionCommand(object id, object newDesc, string descriptionProp)
    {
        Id = id;
        NewDescription = newDesc;
        DescriptionProperty = descriptionProp;
    }

    public void Execute(string filePath, JsonUtilities jsonUtilities)
    {
        if (Id == null)
        {
            Console.WriteLine("Specify the id of the element to remove.");
            return;
        }

        if (NewDescription == null)
        {
            Console.WriteLine("Specify the new description that you want to update your element with.");
            return;
        }

        if (DescriptionProperty == null)
        {
            Console.WriteLine("Specify what is the property name of Description.");
            return;
        }

        jsonUtilities.UpdateElementInArray<TodoTask>(filePath, Id, NewDescription, DescriptionProperty);
    }
}