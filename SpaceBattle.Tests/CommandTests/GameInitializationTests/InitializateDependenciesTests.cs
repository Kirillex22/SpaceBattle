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
    public void InitializateDependenciesTestsSuccessful()
    {
        var gameQueue = new Queue<SpaceBattle.Lib.ICommand>();
        var dependencies = new Dictionary<string, IStrategy>();
        dependencies.Add("Turn", new CreateTurnCommandStrategy());

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.Thread.Game.Register.Dependencies", (object[] args) => new RegisterDependenciesStrategy().Run(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Dependencies.Initialization", (object[] args) => new InitializateDependenciesStrategy().Run(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Queue.Push", (object[] args) => new ActionCommand(() => { gameQueue.Enqueue((SpaceBattle.Lib.ICommand)args[1]); })).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Register.Commands", (object[] args) => new RegisterCommandsStrategy().Run(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Create.MacroCommand", (object[] args) => new CreateMacroCommandStrategy().Run(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Dependencies.Get", (object[] args) => dependencies).Execute();
        
        var t = new RegisterDependenciesStrategy().Run(123);

        Assert.True(gameQueue.Count == 1);
        gameQueue.Dequeue().Execute();

        var moqTurn = new Mock<ITurnable>();
        IoC.Resolve<ICommand>("IoC.Register","Game.UObject.Adapter.Create", (object[] args) => moqTurn.Object).Execute();

        var moqUobj = new Mock<IUObject>();

        var cmd = IoC.Resolve<ICommand>("Game.Command.Turn", moqUobj.Object);

        Assert.True(typeof(Turn) == cmd.GetType());
    }

}

