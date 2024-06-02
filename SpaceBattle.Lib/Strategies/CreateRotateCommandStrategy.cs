using Hwdtech;
using System;

namespace SpaceBattle.Lib;

public class CreateRotateCommandStrategy
{
    public object Run(params object[] args)
    {
        var uobj = (IUObject)args[0];
        var cmd = RotateCommand(IoC.Resolve<IRotatable>("Game.UObject.Adapter.Create", uobj, typeof(IRotatable)));

        return cmd;
    }
}

