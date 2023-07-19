// Define our actual class that serves and handles web requests
// Listen to HTTP requests, handle back and forth with our client
// Clicent being our web page (HTML, CSS, JS)

using System.Net;
using System.Text;
using System.Text.Json;
using ExampleServer.Data;
using ExampleServer.Models;

namespace ExampleServer.Server;

public class WebServer
{
    // Private field
    private readonly TaskRepository _taskRepository;
    private readonly HttpListener _httpListener = new();

    public WebServer(TaskRepository repository, string url)
    {
        _taskRepository = repository;
        _httpListener.Prefixes.Add(url);
    }

    public void Run()
    {
        while (true)
        {
            // Start the server (http listener)
            _httpListener.Start();

            // Add some debeg feedback (console writeline)
            System.Console.WriteLine($"Listening for connections on {_httpListener.Prefixes.First()}");

            // Handle our incoming connections/requests
            // Bulk of our logic
            HandleIncomingRequests();

            // Stop the server
            _httpListener.Stop();
        }
    }

    private void HandleIncomingRequests()
    {
        // Have server sit and wait for connection request
        // Once there is a connection request, it will rerturn the context
        HttpListenerContext context = _httpListener.GetContext();

        // Get Request and Response objects from the context
        HttpListenerRequest request = context.Request;
        HttpListenerResponse response = context.Response;

        System.Console.WriteLine($"{request.HttpMethod} {request.Url}");

        switch (request.HttpMethod)
        {
            case "GET":
                // Handle GET Requests
                HandleGetRequest(request, response);
                break;
            case "POST":
                // Handle POST requests
                System.Console.WriteLine(request.HasEntityBody);
                HandlePostRequest(request, response); 
                break;
            default:
                break;
        }

    }

    private void HandleGetRequest(HttpListenerRequest request, HttpListenerResponse response)
    {
        if (request.Url?.AbsolutePath == "/")
        {
            var tasks = _taskRepository.GetTasks();
            SendResponse(response, HttpStatusCode.OK, tasks);
        }
        else
        {
            SendResponse(response, HttpStatusCode.NotFound, null);
        }
    }

    private void HandlePostRequest(HttpListenerRequest request, HttpListenerResponse response)
    {
        // Check that the request has a body
        if (request.HasEntityBody)
        {
            // Deserialize our request body into the C# request type
            TaskCreateRequest? body = JsonSerializer.Deserialize<TaskCreateRequest>(request.InputStream);

            // Check to make sure it is not null
            if (body != null)
            {
                // Create the new taskModel
                TaskModel newTask = new TaskModel(body.Title ?? "Title", body.Description ?? "");

                // Add that task to our repository
                _taskRepository.AddTask(newTask);

                // Create a response message
                string logOutput = $"Added new item: # {newTask.Id}: {newTask.Title}";
                System.Console.WriteLine(logOutput);

                // Send that response
                SendResponse(response, HttpStatusCode.Created, newTask);
            }
        }
        else
        {
            // If our POST request does not have a body...
            string errorMessage = "Failed ot add task as there was no request body.";
            System.Console.WriteLine(errorMessage);

            ErrorResponse error = new ErrorResponse(errorMessage);
            SendResponse(response, HttpStatusCode.BadRequest, error);
        }
    }

    private void SendResponse(HttpListenerResponse response, HttpStatusCode statusCode, object? data)
    {
        // Convert our C# object to JSON, which allow our browser ot understand it
        // We need to also tell our response the content is JSON
        string json = JsonSerializer.Serialize(data);
        response.ContentType = "Application/json";

        // Convert our JSON ot a byte[] -> basic numbers we can send over the internet
        // Breaking down JSON to a stream of numbers
        byte[] buffer = Encoding.UTF8.GetBytes(json);
        // We need to tell the response how much content to listen for
        // Tells the recipient (browswer) how much of the data stream is the content
        response.ContentLength64 = buffer.Length;

        // Setting our response status code (OK, Bad, Good, etc...)
        // Casting our statusCode variable from the type enum to type int
        response.StatusCode = (int)statusCode;

        // Simply put this here because CORS sucks
        response.AddHeader("Access-Control-Allow-Origin", "*");

        // Writing or sending our response
        response.OutputStream.Write(buffer, 0, buffer.Length);
        response.Close();
    }

}