using Hwdtech;

namespace SpaceBattle.Lib;

public class HardStopCommand : ICommand
{
    private ServerThread _thread;
    private Action _exitAction;
    public HardStopCommand(ServerThread thread, Action exitAction)
    {
        _thread = thread;
        _exitAction = exitAction;
    }

    public void Execute()
    {
        if (!_thread.Equals(Thread.CurrentThread))
        {
            throw new Exception("Trying to stop thread from another thread");
        }
        _thread.Stop();
        _exitAction();
    }
}