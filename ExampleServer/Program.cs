using ExampleServer.Data;
using ExampleServer.Server;

TaskModel.TotalTasks = 0;
System.Console.WriteLine(TaskModel.TotalTasks);

// Insstance of our class
TaskModel task1 = new TaskModel("Task 1", "This is the first task.");
// task1.TotalTasks; // Cannot be referenced through an instance
task1.WriteTotalTasks();

TaskModel task2 =new("Task 2", "The second task.");
task1.WriteTotalTasks();
task2.WriteTotalTasks();

System.Console.WriteLine(task2.Id);

// Implicit Types
// var assumes the type from the righthand side of the expression
var task3 = new TaskModel("Task 3", "The third task!");

// Targated-type new
// Implicit new will assume the type from the lefthad side
TaskModel task4 = new("Task 4", "May the fourth be with you.");

TaskModel task5 = new("Task 5", "What??!?!");

task1.IsComplte = true;
task4.IsComplte = true;

TaskRepository taskRepo = new TaskRepository();
// TaskRepository repo = new();
// var repo = new TaskRepository();

taskRepo.AddTask(task1);
taskRepo.AddTask(task2);
taskRepo.AddTask(task3);
taskRepo.AddTask(task4);
taskRepo.AddTask(task5);

taskRepo.DeleteTaskById(5);

// tasks gets its type from the GetTasks() return type
var tasks = taskRepo.GetTasks();
foreach (var task in tasks)
{
    System.Console.WriteLine(task.Description);
}

var completedTasks = taskRepo.GetTaskByStatus(false);
foreach (var task in completedTasks)
{
    System.Console.WriteLine(task.Description);
}

// Initialize a WebServer instance
// Passing in a repository instance, and a URL to listen on
WebServer server = new WebServer(taskRepo, "http://localhost:8000/");
server.Run(); // Public method in the class WebServer