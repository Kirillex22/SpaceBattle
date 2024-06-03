using Hwdtech;
using System;

namespace SpaceBattle.Lib;

public class CreateTurnCommandStrategy
{
    public object Run(params object[] args)
    {
        var uobj = (IUObject)args[0];
        var cmd = new Turn(IoC.Resolve<ITurnable>("Game.UObject.Adapter.Create", uobj, typeof(ITurnable)));

        return cmd;
    }
}

