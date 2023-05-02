using System.Text;
using CallbackBasedAsync;

// 1. Analyze NativeFilerHelper code, around 2 minutes.
// 2. Let's discuss how files are written using OS API.
// 3. Implement CustomTask.Run method.
// 4. Implement CustomTask.Complete method.
// 5. Implement CustomTask.ContinueWith method.
// 6. Implement CustomTask.Wait method.
// 7. Implement NativeFileHelper.WriteAsyncUsingCustomTask
// 8. Implement CustomTask.WhenAll for 2 arguments
// 9. Implement CustomTask.WhenAll for any tasks
// 10. Implement CustomTask.Delay method
// 10. Implement IterateAsync method.


byte[] dataToWrite = Encoding.UTF8.GetBytes("Hello, World!");
NativeFileHelper.WriteAsyncUsingCallback("MyFile.txt", dataToWrite, success =>
{
    Console.WriteLine(success ? "Success" : "Fail"); 
});

// Uncomment when implemented
// NativeFileHelper.WriteAsyncUsingCustomTask("MyFile.txt", dataToWrite).ContinueWith((task) =>
// {
//     task.Wait();
//     
//     Console.WriteLine("Success");
// });

// Uncomment when implemented
//var task = NativeFileHelper.WriteAsyncUsingCustomTask("MyFile.txt", dataToWrite);

// Uncomment when implemented
// var task2 = IterateAsync(WriteToMultipleFiles());
// task2.Wait();

Console.WriteLine("Blocking main thread.");
Console.ReadLine();

static CustomTask IterateAsync(IEnumerable<CustomTask> tasks)
{
    var task = new CustomTask();

    // Use GetEnumerator, MoveNext, ContinueWith, SetResult, SetException methods
    // The goal is to chain tasks one after another.
    
    IEnumerator<CustomTask> e = tasks.GetEnumerator();

    return task;
}

static IEnumerable<CustomTask> WriteToMultipleFiles()
{
    byte[] dataToWrite = Encoding.UTF8.GetBytes("Hello, World!");
    
    var firstTask = NativeFileHelper.WriteAsyncUsingCustomTask("MyFile.txt", dataToWrite);
    yield return firstTask;
    
    Console.WriteLine("First success.");
    
    var secondTask = NativeFileHelper.WriteAsyncUsingCustomTask("MyFile2.txt", dataToWrite);
    yield return secondTask;
    
    Console.WriteLine("Second success.");
}

static IEnumerable<int> GenerateNumberSequence(int count)
{
    if (count < 0)
    {
        throw new ArgumentOutOfRangeException(nameof(count), "The count must be a non-negative integer.");
    }

    for (int i = 1; i <= count; i++)
    {
        if (i % 3 == 0)
        {
            yield return i * i; // Multiples of 3: return the square of the number
        }
        else if (i % 5 == 0)
        {
            yield return i * 2; // Multiples of 5: return the double of the number
        }
        else
        {
            yield return i; // All other numbers: return the number itself
        }
    }
}