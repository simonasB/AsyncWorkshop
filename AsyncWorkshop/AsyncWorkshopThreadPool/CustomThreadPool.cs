namespace AsyncWorkshopThreadPool;

// Use thread-safe collections, maybe primitive synchronization mechanisms like lock, resetevent
// Don't try to premature optimization, make it work correctly first, optimize later.
public class CustomThreadPool
{
    private readonly int _minThreads;
    private readonly int _maxThreads;
    
    // Need a collection for threads
    // Need a queue for actions

    public CustomThreadPool(int minThreads, int maxThreads)
    {
        if (minThreads <= 0 || maxThreads <= 0 || minThreads > maxThreads)
            throw new ArgumentException("Invalid minimum or maximum thread count.");

        _minThreads = minThreads;
        _maxThreads = maxThreads;

        for (int i = 0; i < minThreads; i++)
        {
            CreateWorkerThread();
        }
    }

    public void EnqueueTask(Action task)
    {
        // Add task to queue
        // Notify the threads that queue is not empty
        // If there are not enough threads add new one if it does not exceed max thread count
    }

    private void CreateWorkerThread()
    {
        // Use Thread class, Set IsBackground = true
        // Use WorkerThreadLoop()
    }

    private void WorkerThreadLoop()
    {
        // The main logic for each thread
        // Need loop through actions
        // If some threads are not used anymore, you can remove them from the queue and terminate (respect min thread count)
        // This method can react to notification when some action is ready.
    }

    public void Dispose()
    {
        // Shutdown pool that no more action can be queue
        // Notify threads they can shutdown
        // Use Thread.Join() to finish all the threads gracefully.
    }
}