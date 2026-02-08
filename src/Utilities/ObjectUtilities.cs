using System;
using System.Reflection;

namespace TaskTracker.Utilities;

public static class ObjectUtilities
{
    public static object? ReturnValueFromProperty(Type objectType, object obj, string propertyName)
    {
        PropertyInfo? property = objectType.GetProperty(propertyName);
        if(property == null)
        {
            Console.WriteLine($"{propertyName} does not exist.");
            return null;
        }

        return property.GetValue(obj);
    }

    public static void SetValueInProperty(Type objectType, object obj, string propertyName, object newValue)
    {
        PropertyInfo? property = objectType.GetProperty(propertyName);
        if (property == null)
        {
            Console.WriteLine($"{propertyName} does not exist! Aborting.");
            return;
        }

        property.SetValue(obj, newValue);
    } 
}