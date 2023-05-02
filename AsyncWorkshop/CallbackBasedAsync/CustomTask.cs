using System.Runtime.ExceptionServices;

namespace CallbackBasedAsync;

public class CustomTask
{
    private bool _completed;
    private Exception? _error;
    private Action<CustomTask>? _continuation;

    public void ContinueWith(Action<CustomTask> action)
    {
        lock (this)
        {
            if (_continuation is not null)
            {
                throw new InvalidOperationException("Only one continuation supported.");
            }
            
            
            // If task is complete, queue continuation to ThreadPool
            // Otherwise, save continuation for later invocation.
        }
    }
    
    public void SetResult() => Complete(null);

    public void SetException(Exception error) => Complete(error);

    public void Wait()
    {
        ManualResetEventSlim? mres = null;
        lock (this)
        {
            if (!_completed)
            {
                mres = new ManualResetEventSlim();
                ContinueWith(_ => mres.Set());
            }
        }

        mres?.Wait();
        if (_error is not null)
        {
            ExceptionDispatchInfo.Throw(_error);
        }
    }

    public static CustomTask WhenAll(params CustomTask[] tasks)
    {
        var t = new CustomTask();

        int remaining = tasks.Length;
        Exception? e = null; // store only single exception for learning purposes

        // Create a common continuation function for very task:
        // When tasks is completed, reduce remaining count.
        // If count is 0 and there are no exceptions, call SetResult()
        // Otherwise, call SetException()

        // Attach continuation for every task

        return t;
    }
    
    public static CustomTask Run(Action action)
    {
        var t = new CustomTask();
        
        // Queue action to the ThreadPool
        // If action succeeded, call SetResult()
        // If not, call SetException()

        return t;
    }
    
    public static CustomTask Delay(TimeSpan delay)
    {
        var t = new CustomTask();

        // Use Timer class and call SetResult()

        return t;
    }
    
    private void Complete(Exception? error)
    {
        lock (this)
        {
            if (_completed)
            {
                throw new InvalidOperationException("Already completed");
            }

            // Mark task as completed and if continuation exists, run it on the ThreadPool.
        }
    }
}