using Hwdtech;
using Hwdtech.Ioc;
using System.Collections;

namespace SpaceBattle.Lib;

public class InterpretatorCommand: ICommand
{
    IMessage _message;

    public InterpretatorCommand(IMessage message)
    {
        _message = message;
    }

    public void Execute()
    {
        var iterpretatorCommand = IoC.Resolve<ICommand>("Game.Command.Create", _message);
        IoC.Resolve<ICommand>("Game.Queue.Push", _message.GameId, iterpretatorCommand).Execute();
    }
}

