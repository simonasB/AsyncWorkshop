using System.Collections;

namespace CallbackBasedAsync;

public class GenerateNumberSequenceStateMachine : IEnumerable<int>, IEnumerable, IEnumerator<int>, IEnumerator, IDisposable
{
    private int _state;
    private int _current;

    private int _number;
    private readonly int _count;

    public GenerateNumberSequenceStateMachine(int state, int count)
    {
        _state = state;
        _count = count;
    }
    
    public IEnumerator<int> GetEnumerator()
    {
        throw new NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public bool MoveNext()
    {
        switch (_state)
        {
            // state management
            // need to check for count < 0
            // increment number
            default:
                return false;
        }

        // Logic inside loop

        return false;
    }

    public void Reset()
    {
        throw new NotImplementedException();
    }

    public int Current => _current;

    object IEnumerator.Current => Current;

    public void Dispose()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<int> GenerateNumberSequence(int count)
    {
        return new GenerateNumberSequenceStateMachine(0, count);
    }
}