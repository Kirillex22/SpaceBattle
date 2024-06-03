using System;
using Hwdtech;

namespace SpaceBattle.Lib;

public class InitializateDependenciesStrategy
{
    public object Run(params object[] args)
    {
        var registerCmd = IoC.Resolve<ICommand>("Game.Register.Commands");

        var registerOperation = new ActionCommand(() => {
            IoC.Resolve<ICommand>("IoC.Register", "Game.Macro.Create.FromDependencies", 
            (object[] args) => new CreateMacroCommandStrategy().Run(args));});

        return new ActionCommand( () => {
            registerCmd.Execute();
            registerOperation.Execute();});
    }
}

