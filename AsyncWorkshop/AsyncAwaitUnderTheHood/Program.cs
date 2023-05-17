// See https://aka.ms/new-console-template for more information


using System.Diagnostics;
using AsyncAwaitUnderTheHood;

// ExecutionContextDemo();
// await CustomAwaitableDemo(2000);

Console.ReadLine();


// async Task ExecutionContextDemo()
// {
//     var executionContextDemo = new ExecutionContextDemo();
//
//     var task = executionContextDemo.ExecuteAsync("1");
//     var task2 = executionContextDemo.ExecuteAsync("2");
//     var task3 = executionContextDemo.ExecuteAsync("3");
//
//     Console.WriteLine("All tasks started");
//
//     await Task.WhenAll(task, task2, task3);
// }

// async Task CustomAwaitableDemo(double interval)
// {
//     var stopwatch = Stopwatch.StartNew();
//     await new TimerAwaitable(interval);
//     Console.WriteLine(stopwatch.ElapsedMilliseconds);
// }

async Task CreateFileCopyAsync(string fromFilePath, string toFilePath)
{
    if (!File.Exists(fromFilePath))
        throw new Exception($"File '{fromFilePath}' does not exist");

    var fileLines = await File.ReadAllLinesAsync(fromFilePath);
    
    foreach (var fileLine in fileLines)
    {
        await File.AppendAllTextAsync(toFilePath, fileLine);
    }
}