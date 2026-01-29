namespace TaskTracker;

public class Program 
{ 
    public static void Main() 
    { 
        App app = new App("task-cli");

        app.Run('"');
    } 
} 