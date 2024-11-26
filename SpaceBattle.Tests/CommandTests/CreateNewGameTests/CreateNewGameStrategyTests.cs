using System;
using Moq;
using Hwdtech;
using Hwdtech.Ioc;
using System.Collections.Concurrent;

namespace SpaceBattle.Lib.Test;

public class CreateNewGameStrategyTests
{
    public CreateNewGameStrategyTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
    }

    [Fact]
    public void CreateNewGameStrategySuccessful()
    {
        var moqCmd = new Mock<Lib.ICommand>();
        moqCmd.Setup(i => i.Execute()).Verifiable();
        var gameMap = new Dictionary<int, ICommand>();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Create", (object[] args) => new CreateNewGameStrategy().Run(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Scope.Create", (object[] args) => (object)0).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Queue.Create", (object[] args) => new Queue<ICommand>()).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command", (object[] args) => moqCmd.Object).Execute();


        var queue = new BlockingCollection<Lib.ICommand>();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.Thread.Queue", (object[] args) => queue).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command.Macro", (object[] args) => new MacroCommandForTest((List<ICommand>)args[0])).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command.Inject", (object[] args) => new BridgeCommand((ICommand)args[0])).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command.Repeat", (object[] args) => new RepeatGameCommand((ICommand)args[0])).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Map", (object[] args) => gameMap).Execute();


        Assert.Empty(gameMap);

        int gameId = 123;
        var newGame = IoC.Resolve<ICommand>("Game.Create", gameId, 300d, IoC.Resolve<object>("Scopes.Current"));

        Assert.Single(gameMap);
        Assert.Equal(typeof(BridgeCommand), gameMap[gameId].GetType());
        Assert.Equal(typeof(BridgeCommand), newGame.GetType());
        Assert.Equal(newGame, gameMap[gameId]);

        Assert.Empty(queue);
        queue.Add(newGame);
        Assert.Single(queue);

        queue.Take().Execute();
        queue.Take().Execute();
        queue.Take().Execute();
        moqCmd.Verify(x => x.Execute(), Times.Exactly(3));

        Assert.Single(queue);
        queue.Take();
        Assert.Empty(queue);
    }
}


public class MacroCommandForTest : ICommand
{
    private IEnumerable<ICommand> _commands;

    public MacroCommandForTest(IEnumerable<ICommand> commands) => _commands = commands;

    public void Execute() => _commands.ToList().ForEach(
        command => command.Execute()
    );
}
