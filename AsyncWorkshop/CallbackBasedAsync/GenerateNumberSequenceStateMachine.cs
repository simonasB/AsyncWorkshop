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
            case 0:
                _state = -1;
                if (_count < 0)
                {
                    throw new ArgumentOutOfRangeException("_count", "The count must be a non-negative integer.");
                }

                _number = 1;
                break;
            case 1:
            case 2:
            case 3:
                _state = -1;
                _number++;
                break;
            default:
                return false;
        }

        if (_number <= _count)
        {
            if (_number % 3 == 0)
            {
                _current = _number + _number;
                _state = 1;
                return true;
            }

            if (_number % 5 == 0)
            {
                _current = _number * 2;
                _state = 2;
                return true;
            }

            _current = _number;
            _state = 3;
            return true;
        }

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