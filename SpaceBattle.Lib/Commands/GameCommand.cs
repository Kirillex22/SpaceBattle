using Hwdtech;
using System.Diagnostics;

namespace SpaceBattle.Lib;

public class GameCommand : ICommand
{
    private object _scope;
    public GameCommand(object scope)
    {
        _scope = scope;
    }

    public void Execute()
    {
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", _scope).Execute();
        var queue = IoC.Resolve<Queue<ICommand>>("Game.Queue");
        var timeQuant = long.Parse(IoC.Resolve<string>("Game.TimeQuant"));
        var timer = new Stopwatch();

        while ((timer.ElapsedMilliseconds < timeQuant) && (queue.Count > 0))
        {
            timer.Start();
            var cmd = queue.Dequeue();
            try
            {
                cmd.Execute();
            }
            catch (Exception exc)
            {
                IoC.Resolve<ICommand>("Game.ExceptionHandler.Handle", cmd, exc).Execute();
            }
            timer.Stop();
        }
    }
}

