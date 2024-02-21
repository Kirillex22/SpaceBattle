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
        _thread.Stop();
        _exitAction();
    }
}