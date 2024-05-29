using System;
using Moq;
using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib.Test;

public class DeleteGameStrategyTests
{
    public DeleteGameStrategyTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();        
    }
    [Fact]
    public void DeleteGameStrategySuccessful()
    {
        var gameMap = new Dictionary<int, IBridgeCommand>();
        var scopeMap = new Dictionary<int, object>();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Delete", (object[] args) => new DeleteGameStrategy().Run(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Map", (object[] args) => gameMap).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.EmptyCommand", (object[] args) => new EmptyCommand()).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Scope.Map", (object[] args) => scopeMap).Execute();

        var moqCmd = new Mock<IBridgeCommand>();
        var gameId = 123;
        var scope = "TestScope";

        Assert.Empty(gameMap);
        Assert.Empty(scopeMap);

        gameMap.Add(gameId, moqCmd.Object);
        scopeMap.Add(gameId, scope);

        Assert.Single(gameMap);
        Assert.Single(scopeMap);

        IoC.Resolve<ICommand>("Game.Delete", gameId).Execute();

        Assert.Single(gameMap);
        Assert.Empty(scopeMap);
    }
}

