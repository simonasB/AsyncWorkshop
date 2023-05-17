using System.Dynamic;

namespace AsyncAwaitUnderTheHood;

public class ExecutionContextDemo
{
    // Try all three cases and notice the difference
    // For 2nd and 3rd case you will need to remove .Value to compile the code
    
    private static readonly AsyncLocal<string> UserId = new();
    //private static string UserId = "";
    //[ThreadStatic] private static string UserId = "none";
    private readonly ServiceA _serviceA = new();

    public static string GetUserId() => UserId.Value;
    public static string SetUserId(string userId) => UserId.Value = userId;
    
    public async Task ExecuteAsync(string userId)
    {
        SetUserId(userId);
        await _serviceA.ExecuteOperationAsync();
    }
}

public class ServiceA
{
    private readonly ServiceB _serviceB = new ServiceB();

    public async Task ExecuteOperationAsync()
    {
        await Task.Delay(1000);
        await _serviceB.ExecuteOperationAsync();
    }
}


public class ServiceB
{
    public async Task ExecuteOperationAsync()
    {
        await Task.Delay(1000);
        Console.WriteLine(ExecutionContextDemo.GetUserId());
    }
}