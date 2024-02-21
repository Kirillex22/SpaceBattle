using Hwdtech;

namespace SpaceBattle.Lib;

public class SoftStopCommand : ICommand
{
    private ServerThread _thread;
    private Action _exitAction;
    public SoftStopCommand(ServerThread thread, Action exitAction)
    {
        _thread = thread;
        _exitAction = exitAction;
    }

    public void Execute()
    {
        var threadQueue = _thread.ThreadQueue;

        _thread.ChangeBehaviour(() =>
        {
            if (threadQueue.Count == 0)
            {
                _thread.Stop();
                _exitAction();
            }

            var cmd = threadQueue.Take();

            try
            {
                cmd.Execute();
            }
            catch (Exception e)
            {
                IoC.Resolve<ICommand>("Exception.Handler", cmd, e).Execute();
            }
        });
    }
}

