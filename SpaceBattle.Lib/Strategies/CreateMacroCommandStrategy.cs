using System;
using Hwdtech;

namespace SpaceBattle.Lib;


public class CreateMacroCommandStrategy : IStrategy
{
    public object Run(params object[] args)
    {
        var uobj = (IUObject)args[0];
        var type = (string)args[1];

        var dependencies = IoC.Resolve<IEnumerable<string>>("Game.Dependencies.Get.Macro." + type);
        var commands = dependencies.ToList().Select(x => IoC.Resolve<ICommand>("Game.Command." + x, uobj)).ToList();

        return IoC.Resolve<ICommand>("Game.Command.Macro", commands);
    }
}

