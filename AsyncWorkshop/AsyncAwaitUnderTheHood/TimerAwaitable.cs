using System.Runtime.CompilerServices;

namespace AsyncAwaitUnderTheHood;

public class TimerAwaitable
{
    private readonly double _interval;

    public TimerAwaitable(double interval)
    {
        _interval = interval;
    }
    
    // Add GetAwaiter method

    // Implement INotifyCompletion
    public class TimerAwaiter
    {
        private readonly System.Timers.Timer _timer;
        private Action _continuation;

        public TimerAwaiter(double interval)
        {
            _timer = new System.Timers.Timer(interval);
            _timer.AutoReset = false;
            // When _timer elapses
            // Set awaiter to completed
            // Invoke continuation if exists
        }

        public bool IsCompleted { get; private set; }

        public void OnCompleted(Action continuation)
        {
            // Save continuation
            // Start timer
        }

        public void GetResult() { }
    }
}