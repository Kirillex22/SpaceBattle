using Hwdtech;

namespace SpaceBattle.Lib;

public class CreateEndMoveCommandStrategy : IStrategy
{
    public object Run(params object[] args)
    {
        var uobj = (IUObject)args[0];
        var cmd = EndMoveCommand(IoC.Resolve<IMoveCommandStopable>("Game.UObject.Adapter.Create", uobj, typeof(IMoveCommandStopable)));

        return cmd;
    }
}