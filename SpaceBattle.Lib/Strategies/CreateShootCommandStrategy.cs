using System;
using Hwdtech;

namespace SpaceBattle.Lib;

public class CreateShootCommandStrategy
{
    public object Run(params object[] args)
    {
        var uobj = (IUObject)args[0];
        var cmd = ShootCommand(IoC.Resolve<IShootable>("Game.UObject.Adapter.Create", uobj, typeof(IShootable)));

        return cmd
    }
}

