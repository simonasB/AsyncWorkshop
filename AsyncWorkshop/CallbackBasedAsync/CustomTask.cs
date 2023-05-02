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
            if (_completed)
            {
                ThreadPool.QueueUserWorkItem(_ => action(this));
            }
            else if (_continuation is not null)
            {
                throw new InvalidOperationException("Unlike Task, this implementation only supports a single continuation.");
            }
            else
            {
                _continuation = action;
            }
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

    public static CustomTask WhenAll(CustomTask t1, CustomTask t2)
    {
        var t = new CustomTask();

        int remaining = 2;
        Exception? e = null;

        void Continuation(CustomTask completed)
        {
            e ??= completed._error; // just store a single exception for simplicity
            if (Interlocked.Decrement(ref remaining) == 0)
            {
                if (e is not null)
                    t.SetException(e);
                else
                    t.SetResult();
            }
        }

        t1.ContinueWith(Continuation);
        t2.ContinueWith(Continuation);

        return t;
    }
    
    public static CustomTask Run(Action action)
    {
        var t = new CustomTask();

        ThreadPool.QueueUserWorkItem(_ =>
        {
            try
            {
                action();
                t.SetResult();
            }
            catch (Exception e)
            {
                t.SetException(e);
            }
        });

        return t;
    }
    
    public static CustomTask Delay(TimeSpan delay)
    {
        var t = new CustomTask();

        var timer = new Timer(_ => t.SetResult());
        timer.Change(delay, Timeout.InfiniteTimeSpan);

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

            _error = error;
            _completed = true;

            if (_continuation is not null)
            {
                ThreadPool.QueueUserWorkItem(_ =>
                {
                    _continuation(this);
                });
            }
        }
    }
}