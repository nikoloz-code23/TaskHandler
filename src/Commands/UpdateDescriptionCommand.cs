using System;
using TaskTracker.Types;
using TaskTracker.Interfaces;
using TaskTracker.Utilities;

namespace TaskTracker.Commands;

public class UpdateDescriptionCommand : ICommand {
    public object? Id { get; set; }
    public string NewDescription { get; set; } = string.Empty;
    public string DescriptionProperty { get; set; } = string.Empty;

    public UpdateDescriptionCommand(object id, string newDesc, string descriptionProp)
    {
        Id = id;
        NewDescription = newDesc.Trim();
        DescriptionProperty = descriptionProp.Trim();
    }

    public void Execute(string filePath, JsonUtilities jsonUtilities)
    {
        if (Id == null)
        {
            Console.WriteLine("Specify the id of the element to remove.");
            return;
        }

        if (NewDescription == string.Empty)
        {
            Console.WriteLine("Specify the new description that you want to update your element with.");
            return;
        }

        if (DescriptionProperty == string.Empty)
        {
            Console.WriteLine("Specify what is the property name of Description.");
            return;
        }

        jsonUtilities.UpdateElementInArray<TodoTask>(filePath, Id, NewDescription, DescriptionProperty);
    }
}