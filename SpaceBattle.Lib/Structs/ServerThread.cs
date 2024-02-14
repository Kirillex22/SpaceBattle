using System.Collections.Concurrent;
using Hwdtech;

namespace SpaceBattle.Lib;

public class ServerThread
{
    private Action _behaviour;
    private bool _stop;
    private BlockingCollection<ICommand> _threadQueue;
    private Thread _thread;

    public ServerThread(BlockingCollection<ICommand> threadQueue)
    {
        _stop = false;
        _threadQueue = threadQueue;

        _behaviour = () =>
        {
            var cmd = _threadQueue.Take();
            try
            {
                cmd.Execute();
            }
            catch (Exception e)
            {
                IoC.Resolve<ICommand>("Exception.Handle", cmd, e).Execute();
            }
        };

        _thread = new Thread(() =>
        {
            while (!_stop)
            {
                _behaviour();
            }
        });
    }

    public void Start()
    {
        _thread.Start();
    }

    internal void Stop()
    {
        _stop = true;
    }

    internal void ChangeBehaviour(Action newBeh)
    {
        _behaviour = newBeh;
    }
}