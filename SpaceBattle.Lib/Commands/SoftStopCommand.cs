using Hwdtech;
using System.Collections.Concurrent;

namespace SpaceBattle.Lib;

public class SoftStopCommand : ICommand
{
    private ServerThread _thread;
    private BlockingCollection<ICommand> _queue;
    private Action _exitAction;
    public SoftStopCommand(ServerThread thread, BlockingCollection<ICommand> queue, Action exitAction)
    {
        _thread = thread;
        _queue = queue;
        _exitAction = exitAction;
    }

    public void Execute()
    {
        _thread.ChangeBehaviour(() =>
        {
            if (_queue.Count == 0)
            {
                _thread.Stop();
                _exitAction();
            }

            else
            {
                var cmd = _queue.Take();

                try
                {
                    cmd.Execute();
                }
                catch (Exception e)
                {
                    IoC.Resolve<ICommand>("Exception.Handler", cmd, e).Execute();
                }
            }

        });
    }
}

