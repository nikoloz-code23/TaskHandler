using TaskTracker.Application;

namespace TaskTracker;

public static class Program 
{ 
    public static void Main() 
    { 
        App app = new App("task-cli");
        app.FilePath = "./todo.json";

        app.Run('"');
    } 
} 