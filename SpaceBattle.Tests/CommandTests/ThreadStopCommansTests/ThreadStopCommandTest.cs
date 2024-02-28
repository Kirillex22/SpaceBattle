using System.Collections.Concurrent;
using Hwdtech;
using Moq;
using Hwdtech.Ioc;
using SpaceBattle.Lib;
using System.Threading;

namespace SpaceBattle.Tests;

public class ThreadStopCommandsTest
{
    public ThreadStopCommandsTest()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Exception.Handler", (object[] args) =>
        {
            return new EmptyCommand();

        }).Execute();

        var cmd = new Mock<SpaceBattle.Lib.ICommand>();
        cmd.Setup(c => c.Execute()).Verifiable();

        new CreateThreadList().Call();
        new CreateAndStartThread().Call();
        new SendCommand().Call();
        new HardStopThread().Call();
        new SoftStopThread().Call();
    }

    [Fact]
    public void SuccesfulHardStop()
    {
        var bar = new Barrier(3);

        var threadList = IoC.Resolve<Dictionary<int, ServerThread>>("Game.Struct.ServerThread.List");
        var queueList = IoC.Resolve<Dictionary<int, BlockingCollection<SpaceBattle.Lib.ICommand>>>("Game.Struct.ServerThreadQueue.List");
        var currentScope = IoC.Resolve<object>("Scopes.Current");
        IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Struct.ServerThread.CreateAndStart", 1, currentScope, () => { }).Execute();
        IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Struct.ServerThread.CreateAndStart", 2, currentScope, () => { }).Execute();

        var cmds = new List<SpaceBattle.Lib.ICommand>(){
            new EmptyCommand(),
            new EmptyCommand(),
            new EmptyCommand(),
            new EmptyCommand()
        };

        cmds.ForEach(c => IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Struct.ServerThread.SendCommand", 1, c).Execute());
        cmds.ForEach(c => IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Struct.ServerThread.SendCommand", 2, c).Execute());

        var hs1 = IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Struct.ServerThread.HardStop", 1, () => { bar.SignalAndWait(); });
        var hs2 = IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Struct.ServerThread.HardStop", 2, () => { bar.SignalAndWait(); });

        hs1.Execute();
        hs2.Execute();

        bar.SignalAndWait();

        bar.Dispose();

        Assert.False(threadList[1].Status());
        Assert.False(threadList[2].Status());
        threadList.Remove(1);
        threadList.Remove(2);
        queueList.Remove(1);
        queueList.Remove(2);
    }

    [Fact]
    public void SuccesfulSoftStopThread()
    {
        var bar = new Barrier(3);

        var currentScope = IoC.Resolve<object>("Scopes.Current");
        var threadList = IoC.Resolve<Dictionary<int, ServerThread>>("Game.Struct.ServerThread.List");
        var queueList = IoC.Resolve<Dictionary<int, BlockingCollection<SpaceBattle.Lib.ICommand>>>("Game.Struct.ServerThreadQueue.List");

        var q1 = new BlockingCollection<SpaceBattle.Lib.ICommand>();
        var st1 = new ServerThread(q1, currentScope, () => { });
        threadList.Add(11, st1);
        queueList.Add(11, q1);

        var q2 = new BlockingCollection<SpaceBattle.Lib.ICommand>();
        var st2 = new ServerThread(q2, currentScope, () => { });
        threadList.Add(23, st2);
        queueList.Add(23, q2);

        var c1 = new Mock<SpaceBattle.Lib.ICommand>();
        var c2 = new Mock<SpaceBattle.Lib.ICommand>();
        var c3 = new Mock<SpaceBattle.Lib.ICommand>();
        var c11 = new Mock<SpaceBattle.Lib.ICommand>();
        var c21 = new Mock<SpaceBattle.Lib.ICommand>();
        var c31 = new Mock<SpaceBattle.Lib.ICommand>();

        c1.Setup(c => c.Execute()).Verifiable();
        c2.Setup(c => c.Execute()).Verifiable();
        c3.Setup(c => c.Execute()).Verifiable();

        IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Struct.ServerThread.SendCommand", 11, c1.Object).Execute();
        IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Struct.ServerThread.SoftStop", 11, () =>
        {
            bar.SignalAndWait();
        }).Execute();

        IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Struct.ServerThread.SendCommand", 11, c2.Object).Execute();
        IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Struct.ServerThread.SendCommand", 11, c3.Object).Execute();

        st1.Start();

        c11.Setup(c => c.Execute()).Verifiable();
        c21.Setup(c => c.Execute()).Verifiable();
        c31.Setup(c => c.Execute()).Verifiable();

        IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Struct.ServerThread.SendCommand", 23, c11.Object).Execute();
        IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Struct.ServerThread.SoftStop", 23, () =>
        {
            bar.SignalAndWait();
        }).Execute();

        IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Struct.ServerThread.SendCommand", 23, c21.Object).Execute();
        IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Struct.ServerThread.SendCommand", 23, c31.Object).Execute();

        st2.Start();

        bar.SignalAndWait();
        bar.Dispose();

        Assert.False(st1.Status());
        Assert.False(st2.Status());

        Mock.Verify(c1, c2, c3, c11, c21, c31);

        threadList.Remove(11);
        threadList.Remove(23);
        queueList.Remove(11);
        queueList.Remove(23);
    }
}

