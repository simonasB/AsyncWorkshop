namespace AsyncAwaitUnderTheHood;

public class GenerateNumberSequenceStateMachine
{
    // /////////////////////////////////////
    private int _state;
    
    // Fields that need survive every MoveNext (local variables, method parameters)
    
    public GenerateNumberSequenceStateMachine(int state)
    {
        _state = state;
    }
    
    // //////////////////////////////////////

    public bool MoveNext()
    {
        switch (_state)
        {
            case 0:
                // Do something until first yield/await
                break;
            case 1:
            case 2:
            case 3:
                // based on the which yield it is, jump to correct logic or set the correct state
                break;
        }

        // Main method logic

        return false;
    }
    
    // //////////////////////////////////////

    public static GenerateNumberSequenceStateMachine Create()
    {
        return new GenerateNumberSequenceStateMachine(-1);
    }
    
    // //////////////////////////////////////
}