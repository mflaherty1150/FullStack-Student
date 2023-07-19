// TaskModel is our POCO (plain old C# object)
// It is going to represent the data object

namespace ExampleServer.Data;

// Task or ToDo is something we want to done
// A TaskModel instance respresents a single task
// Identifier, Title, Description, a completion status
public class TaskModel
{
    public static int TotalTasks = 0;

    public TaskModel(string title, string description)
    {
        TotalTasks++;
        Id = TotalTasks;

        Title = title;
        Description = description;
    }

    // Properties
    public int Id { get; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsComplte { get; set; }

    public void WriteTotalTasks()
    {
        System.Console.WriteLine($"Task {Id}/{TotalTasks}");
    }
}