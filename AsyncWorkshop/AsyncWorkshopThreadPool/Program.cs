using System.Net;
using System.Text;
using System.Text.Json;

/*
 * 1. Implement the GetThreadPoolStatistics() method.
 * 2. Implement the SimulateStarvationWithTasks() method.
 * 3. Implement the SimulateStarvationWithThreadPool() method.
 * 4. Implement the CreateThreadForCustomHttpListener() method.
 * 5. Use it before app.Run() and update Index.cshtml to use http://localhost:5005/ for statistics (line 122), observe change in UI.
 * 6. Try ThreadPool.SetMinThreads (e.g., 50, compare the request creation rate).
 * 7. Revert MinThreads
 * 8. Implement the SimulateNonBlockingWithTasks()
 * 9. Implement the SimulateNonBlockingWithThreadPool()
 * 10. Implement CustomThreadPool class
 */

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.MapRazorPages();

app.MapGet("/api/threadpool", GetThreadPoolStatistics);

app.MapGet("/api/simulateStarvationTasks", SimulateStarvationWithTasks);
app.MapGet("/api/simulateStarvationThreadPool", SimulateStarvationWithThreadPool);
app.MapGet("/api/simulateNonBlockingWithThreadPool", SimulateNonBlockingWithThreadPool);
app.MapGet("/api/simulateNonBlockingWithTasks", SimulateNonBlockingWithTasks);

// CreateThreadForCustomHttpListener();

app.Run();

ThreadPoolStatistics GetThreadPoolStatistics()
{
    // Use ThreadPool class methods, they all have a decent documentation.
    int usedWorkersThreads = 0;
    int usedCompletionPortThreads = 0;
    long pendingWorkItemCount = 0;
    int minWorkerThreads = 0;
    int minCompletionPortThreads = 0;

    return new ThreadPoolStatistics(usedWorkersThreads, usedCompletionPortThreads, pendingWorkItemCount, minWorkerThreads, minCompletionPortThreads);
}

void SimulateStarvationWithTasks()
{
    for (int i = 0; i < 100; i++)
    {
        // Simulate a situation to spin up and block 30 tasks to make a thread starvation. Use Tasks.
    }
}

void SimulateStarvationWithThreadPool()
{
    for (int i = 0; i < 30; i++)
    {
        // Simulate a situation to spin up and block 30 actions to make thread starvation. Use ThreadPool directly.
        // Pay attention the difference in UI responsiveness.
    }
}

void SimulateNonBlockingWithThreadPool()
{
    for (int i = 0; i < 100; i++)
    {
        // Use async/await callback, Task.Delay and ThreadPool.QueueUserWorkItem instead
        // Simulate some CPU work additionally, you can use for cycle with 100000000 iterations for example.
    }
}

void SimulateNonBlockingWithTasks()
{
    for (int i = 0; i < 100; i++)
    {
        // Use async/await callback, Task.Delay and Task.Run instead
        // Simulate some CPU work additionally, you can use for cycle with 100000000 iterations for example.
    }
}

void CreateThreadForCustomHttpListener()
{
    // Start a new thread using the Thread class
    // Pass StartCustomHttpListener to Thread
}

void StartCustomHttpListener()
{
    HttpListener listener = new HttpListener();
    listener.Prefixes.Add("http://localhost:5005/");
    listener.Start();

    while (true)
    {
        HttpListenerContext context = listener.GetContext();
        
        HttpListenerResponse response = context.Response;

        var statistics = GetThreadPoolStatistics();
        var bytes = JsonSerializer.SerializeToUtf8Bytes(statistics, new JsonSerializerOptions() {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        
        response.AddHeader("Content-Type", "application/json; charset=utf-8");
        response.AddHeader("Transfer-Encoding", "chunked");
        response.AddHeader("Access-Control-Allow-Origin", "*");
        response.ContentEncoding = Encoding.UTF8;

        response.SendChunked = true;
        response.StatusCode = 200;
        response.OutputStream.Write(bytes);
        response.Close();
    }
}

record ThreadPoolStatistics(int UserWorkerThreads, int UsedCompletionPortThreads, long PendingWorkItemCount, int MinWorkerThreadCount, int MinCompletionPortThreadCount);