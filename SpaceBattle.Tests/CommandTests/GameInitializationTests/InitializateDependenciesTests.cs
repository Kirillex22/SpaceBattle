using System;
using Hwdtech;
using Hwdtech.Ioc;
using Moq;

namespace SpaceBattle.Lib.Test;

public class InitializateDependenciesTests
{
    public InitializateDependenciesTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();        
     }

    [Fact]
    public void InitializateDependenciesTestsException()
    {
        var gameQueue = new Queue<SpaceBattle.Lib.ICommand>();

        var dependencies = new Dictionary<string, IStrategy>();
        dependencies.Add("Turn", new CreateTurnCommandStrategy());

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.Thread.Game.Register.Dependencies", (object[] args) => new RegisterDependenciesStrategy().Run(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Dependencies.Initialization", (object[] args) => new ActionCommand(() => {throw new Exception();})).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Queue.Push", (object[] args) => new ActionCommand(() => { gameQueue.Enqueue((SpaceBattle.Lib.ICommand)args[1]); })).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Register.Commands", (object[] args) => new RegisterCommandsStrategy().Run(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Macro.Create.FromDependencies", (object[] args) => new CreateMacroCommandStrategy().Run(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Dependencies.Get",(object[] args) => dependencies).Execute();

        IoC.Resolve<SpaceBattle.Lib.ICommand>("Server.Thread.Game.Register.Dependencies", 1).Execute();

        Assert.True(gameQueue.Count == 1);

        Assert.Throws<Exception>(() => {gameQueue.Dequeue().Execute();});
    }
}

