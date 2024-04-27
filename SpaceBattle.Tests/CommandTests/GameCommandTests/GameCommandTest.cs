using System.Collections;
using System.Drawing.Printing;
using Hwdtech;
using Hwdtech.Ioc;
using Moq;
using SpaceBattle.Lib;

namespace SpaceBattle.Tests;

internal class ActionCommand : SpaceBattle.Lib.ICommand
{
    private Action _act;
    public ActionCommand(Action act)
    {
        _act = act;
    }

    public void Execute()
    {
        _act();
    }
}

public class GameCommandTests
{
    public GameCommandTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "Scopes.Current.Set",
             IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))
        ).Execute();
    }

    [Fact]
    public void SuccesfulCmdsExecutingWithCmdsWhoWasLate()
    {
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.TimeQuant",
            (object[] args) => "33"
        ).Execute();

        var q = new Queue<SpaceBattle.Lib.ICommand>();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Queue",
            (object[] args) => q
        ).Execute();

        new ExceptionHandler().Call();

        var scope = IoC.Resolve<object>("Scopes.Current");
        var gameCmd = new GameCommand(scope);

        var inGameCmd = new Mock<SpaceBattle.Lib.ICommand>();
        inGameCmd.Setup(c => c.Execute()).Callback(() => Thread.Sleep(33));

        q.Enqueue(inGameCmd.Object);
        q.Enqueue(inGameCmd.Object);

        var barrier = new Barrier(2);

        var th = new Thread(() =>
        {
            new ActionCommand(() =>
            {
                gameCmd.Execute();
                barrier.SignalAndWait();

            }).Execute();
        });

        th.Start();
        barrier.SignalAndWait();
        Assert.True(q.Count == 1);
    }

    [Fact]
    public void SuccesfulExecutingWithDefaultExceptionStrategy()
    {
        var resolveCmd = new Mock<SpaceBattle.Lib.ICommand>();
        resolveCmd.Setup(c => c.Execute()).Verifiable(Times.Once);

        var resolveCmd1 = new Mock<SpaceBattle.Lib.ICommand>();
        resolveCmd1.Setup(c => c.Execute()).Verifiable(Times.Once);

        var concreteTree = new Hashtable(){
                {"SpaceBattle.Lib.MacroCommand", new Hashtable(){{"System.Exception", resolveCmd.Object}}},
                {"default", new Hashtable(){{"default", resolveCmd1.Object}}}
        };

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Struct.ExceptionTree",
            (object[] args) => concreteTree
        ).Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.TimeQuant",
            (object[] args) => "33"
        ).Execute();

        var q = new Queue<SpaceBattle.Lib.ICommand>();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Queue",
            (object[] args) => q
        ).Execute();

        new ExceptionHandler().Call();

        var scope = IoC.Resolve<object>("Scopes.Current");
        var gameCmd = new GameCommand(scope);

        var inGameCmd = new Mock<SpaceBattle.Lib.ICommand>();
        inGameCmd.Setup(c => c.Execute()).Throws(new Exception());

        var inGameCmd1 = new Mock<SpaceBattle.Lib.ICommand>();
        inGameCmd1.Setup(c => c.Execute()).Throws(new Exception());
        var mc = new MacroCommand(new SpaceBattle.Lib.ICommand[] { inGameCmd1.Object });

        q.Enqueue(inGameCmd.Object);
        q.Enqueue(mc);

        var barrier = new Barrier(2);

        var th = new Thread(() =>
        {
            new ActionCommand(() =>
            {
                gameCmd.Execute();
                barrier.SignalAndWait();

            }).Execute();
        });

        th.Start();
        barrier.SignalAndWait();
        Mock.Verify(resolveCmd, resolveCmd1);
    }
}

