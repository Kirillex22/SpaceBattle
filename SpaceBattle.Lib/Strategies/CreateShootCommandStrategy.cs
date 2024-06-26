using System;
using Hwdtech;

namespace SpaceBattle.Lib;

public class CreateShootCommandStrategy : IStrategy
{
    public object Run(params object[] args)
    {
        var uobj = (IUObject)args[0];
        var cmd = new ShootCommand(IoC.Resolve<IShootable>("Game.UObject.Adapter.Create", uobj, typeof(IShootable)));

        return cmd;
    }
}

