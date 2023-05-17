using System.Runtime.CompilerServices;

namespace AsyncAwaitUnderTheHood;

public class TimerAwaitable
{
    private readonly double _interval;

    public TimerAwaitable(double interval)
    {
        _interval = interval;
    }

    public TimerAwaiter GetAwaiter() => new TimerAwaiter(_interval);

    public class TimerAwaiter : INotifyCompletion
    {
        private readonly System.Timers.Timer _timer;
        private Action _continuation;

        public TimerAwaiter(double interval)
        {
            _timer = new System.Timers.Timer(interval);
            _timer.Elapsed += (sender, e) => { IsCompleted = true; _continuation?.Invoke(); };
            _timer.AutoReset = false;
        }

        public bool IsCompleted { get; private set; }

        public void OnCompleted(Action continuation)
        {
            _continuation = continuation;
            _timer.Start();
        }

        public void GetResult() { }
    }
}