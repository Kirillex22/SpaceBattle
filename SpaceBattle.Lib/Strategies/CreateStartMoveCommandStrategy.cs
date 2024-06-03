using Hwdtech;

namespace SpaceBattle.Lib;

public class CreateStartMoveCommandStrategy : IStrategy
{
    public object Run(params object[] args)
    {
        var uobj = (IUObject)args[0];
        var cmd = new StartMoveCommand(IoC.Resolve<IMoveStartable>("Game.UObject.Adapter.Create", uobj, typeof(IMoveStartable)));

        return cmd; 
    }
}

