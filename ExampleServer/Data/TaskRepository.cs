// TaskRepository is responsible for storing and manipulating our collection of data, in this case TaskModels

namespace ExampleServer.Data;

public class TaskRepository
{
    // Data Storage (All of our Tasks)
    private readonly List<TaskModel> _taskList = new List<TaskModel>();

    // Create method
    // NOT creating an instance, we're creating an entry in the list
    public void AddTask(TaskModel task)
    {
        _taskList.Add(task);
        // _taskList.Contains(task); // could be used to check if it was added successfully
    }

    // Read method
    public List<TaskModel> GetTasks()
    {
        return new List<TaskModel>(_taskList);
    }

    // public List<TaskModel> GetTasks() => new List<TaskModel>(_taskList);

    public List<TaskModel> GetTaskByStatus(bool isComplete)
    {
        // Start a new list
        List<TaskModel> tasks = new();

        // Iterate through all tasks and check the status
        foreach (var task in _taskList)
        {
            // Add a task to the new list if its status matches the parameter
            if (task.IsComplte == isComplete)
            {
                tasks.Add(task);
            }
        }

        // Return the new list
        return tasks;
    }

    // Delete method
    public bool DeleteTaskById(int id)
    {
        // Loop through each task
        foreach (TaskModel task in _taskList)
        {
            // Check the task Id against our parament
            if (task.Id == id)
            {
                // If we find it, remove the task and return true/false
                return _taskList.Remove(task);
            }
        }

        // Return false if we don't find the Id in the loop
        return false;
    }
}