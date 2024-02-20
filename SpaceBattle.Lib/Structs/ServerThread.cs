using System.Collections.Concurrent;
using Hwdtech;

namespace SpaceBattle.Lib;

public class ServerThread
{
    private Action _behaviour;
    private bool _stop;
    public BlockingCollection<ICommand> ThreadQueue { get; }
    private Thread _thread;

    public ServerThread(BlockingCollection<ICommand> threadQueue)
    {
        _stop = false;
        ThreadQueue = threadQueue;

        _behaviour = () =>
        {
            var cmd = ThreadQueue.Take();
            try
            {
                cmd.Execute();
            }
            catch (Exception e)
            {
                IoC.Resolve<ICommand>("Exception.Handler", cmd, e).Execute();
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
        Console.Write("SUCCESFUL STOPPED");
        _stop = true;
    }

    internal void ChangeBehaviour(Action newBeh)
    {
        _behaviour = newBeh;
    }

    public bool Status()
    {
        return _stop;
    }
}