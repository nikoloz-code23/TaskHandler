using System;
using System.Linq;
using System.Threading.Tasks;
using TaskTracker.Application;

namespace TaskTracker;

public static class Program 
{ 
    public static async Task Main() 
    {
        App app = new App("task-cli");
        app.FilePath = "./todo.json";

        await app.Run();
    } 
} 