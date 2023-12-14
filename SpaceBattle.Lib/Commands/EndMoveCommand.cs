using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib;

public class EndMoveCommand: ICommand
{
    private IMoveCommandStopable _end;

    public EndMoveCommand(IMoveCommandStopable end)
    {
        _end = end;
    } 

    public void Execute()
    {
        var Empty = IoC.Resolve<ICommand>("Command.Empty");
        _end.Properties.ToList().ForEach(pair => IoC.Resolve<ICommand>("DeleteProperty", _end.Uobject, pair).Execute());
        var command = IoC.Resolve<IBridgeCommand>("GetProperty", _end.Uobject, _end.NameCommand);

        IoC.Resolve<IBridgeCommand>("Command.Inject", command, Empty);
    }
}

