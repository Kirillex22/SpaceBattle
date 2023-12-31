using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib;

public class MacroLongOperation : ICommand
{
    private string _dependency;
    private IUObject _obj;

    public MacroLongOperation(string dependency, IUObject obj)
    {
        _dependency = dependency;
        _obj = obj;
    }

    public void Execute()
    {
        var mc = IoC.Resolve<ICommand>("Create.LongMacro", _dependency, _obj);
        mc.Execute();

        var LongOperatin = IoC.Resolve<ICommand>("Game.Command.LongOperation", mc);
        LongOperatin.Execute();
        
        var QueueCommand = IoC.Resolve<ICommand>("Game.Queue.Push", LongOperatin);
        QueueCommand.Execute();
    }
}

