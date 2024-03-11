using System.Collections.Concurrent;
using Hwdtech;
using Moq;
using Hwdtech.Ioc;
using SpaceBattle.Lib;

namespace SpaceBattle.Tests;

public class ThreadStopCommandsTest
{
    public ThreadStopCommandsTest()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        new CreateThreadList().Call();
        new CreateAndStartThread().Call();
        new SendCommand().Call();
        new HardStopThread().Call();
        new SoftStopThread().Call();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Test.ClearDicts", (object[] args) =>
        {
            var threadList = IoC.Resolve<Dictionary<int, ServerThread>>("Game.Struct.ServerThread.List");
            var queueList = IoC.Resolve<Dictionary<int, BlockingCollection<SpaceBattle.Lib.ICommand>>>("Game.Struct.ServerThreadQueue.List");
            threadList.Remove((int)args[0]);
            threadList.Remove((int)args[0]);
            queueList.Remove((int)args[1]);
            queueList.Remove((int)args[1]);
            return new object();

        }).Execute();
    }

    [Fact]
    public void SuccesfulHardStopThreadsWithCommandThrowsException()
    {
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Exception.Handler", (object[] args) =>
        {
            return new EmptyCommand();
        }).Execute();

        var bar = new Barrier(3);
        var threadList = IoC.Resolve<Dictionary<int, ServerThread>>("Game.Struct.ServerThread.List");
        var queueList = IoC.Resolve<Dictionary<int, BlockingCollection<SpaceBattle.Lib.ICommand>>>("Game.Struct.ServerThreadQueue.List");
        var currentScope = IoC.Resolve<object>("Scopes.Current");
        var c = new Mock<SpaceBattle.Lib.ICommand>();
        var c1 = new Mock<SpaceBattle.Lib.ICommand>();

        IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Struct.ServerThread.CreateAndStart", 1, () =>
        {
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", currentScope).Execute();
        }).Execute();
        IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Struct.ServerThread.CreateAndStart", 2, () =>
        {
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", currentScope).Execute();
        }).Execute();

        c.Setup(c => c.Execute()).Throws(new NotImplementedException()).Verifiable(Times.AtLeast(2));
        c1.Setup(c => c.Execute()).Verifiable(Times.Never());

        IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Struct.ServerThread.SendCommand", 1, c.Object).Execute();
        IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Struct.ServerThread.SendCommand", 2, c.Object).Execute();

        IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Struct.ServerThread.HardStop", 1, () => { bar.SignalAndWait(); }).Execute();
        IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Struct.ServerThread.HardStop", 2, () => { bar.SignalAndWait(); }).Execute();

        IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Struct.ServerThread.SendCommand", 1, c.Object).Execute();
        IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Struct.ServerThread.SendCommand", 2, c.Object).Execute();

        bar.SignalAndWait();
        bar.Dispose();
        Assert.True(threadList[1].Status());
        Assert.True(threadList[2].Status());
        Mock.Verify(c, c1);
        IoC.Resolve<object>("Test.ClearDicts", 1, 2);
    }

    [Fact]
    public void SuccesfulHardStopThreads()
    {
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Exception.Handler", (object[] args) =>
        {
            return new EmptyCommand();
        }).Execute();

        var bar = new Barrier(3);
        var threadList = IoC.Resolve<Dictionary<int, ServerThread>>("Game.Struct.ServerThread.List");
        var queueList = IoC.Resolve<Dictionary<int, BlockingCollection<SpaceBattle.Lib.ICommand>>>("Game.Struct.ServerThreadQueue.List");
        var currentScope = IoC.Resolve<object>("Scopes.Current");
        var c = new Mock<SpaceBattle.Lib.ICommand>();
        var c1 = new Mock<SpaceBattle.Lib.ICommand>();

        IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Struct.ServerThread.CreateAndStart", 1, () =>
        {
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", currentScope).Execute();
        }).Execute();
        IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Struct.ServerThread.CreateAndStart", 2, () =>
        {
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", currentScope).Execute();
        }).Execute();

        c.Setup(c => c.Execute()).Verifiable(Times.AtLeast(2));
        c1.Setup(c => c.Execute()).Verifiable(Times.Never());

        IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Struct.ServerThread.SendCommand", 1, c.Object).Execute();
        IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Struct.ServerThread.SendCommand", 2, c.Object).Execute();

        IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Struct.ServerThread.HardStop", 1, () => { bar.SignalAndWait(); }).Execute();
        IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Struct.ServerThread.HardStop", 2, () => { bar.SignalAndWait(); }).Execute();

        IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Struct.ServerThread.SendCommand", 1, c.Object).Execute();
        IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Struct.ServerThread.SendCommand", 2, c.Object).Execute();
        bar.SignalAndWait();

        bar.Dispose();
        Assert.True(threadList[1].Status());
        Assert.True(threadList[2].Status());
        Mock.Verify(c, c1);
        IoC.Resolve<object>("Test.ClearDicts", 1, 2);
    }

    [Fact]
    public void SuccesfulSoftStopThreadsWithCommandThrowsException()
    {
        var bar = new Barrier(3);
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Exception.Handler", (object[] args) =>
        {
            return new EmptyCommand();
        }).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Test.CreateSoftStopConfiguration", (object[] args) =>
        {
            IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Struct.ServerThread.SendCommand", (int)args[0], (SpaceBattle.Lib.ICommand)args[1]).Execute();
            IoC.Resolve<SpaceBattle.Lib.ICommand>(
                "Game.Struct.ServerThread.SoftStop",
                (int)args[0],
                () => { bar.SignalAndWait(); }
            ).Execute();
            IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Struct.ServerThread.SendCommand", (int)args[0], (SpaceBattle.Lib.ICommand)args[2]).Execute();
            return new object();
        }).Execute();

        var currentScope = IoC.Resolve<object>("Scopes.Current");
        var threadList = IoC.Resolve<Dictionary<int, ServerThread>>("Game.Struct.ServerThread.List");
        var queueList = IoC.Resolve<Dictionary<int, BlockingCollection<SpaceBattle.Lib.ICommand>>>("Game.Struct.ServerThreadQueue.List");
        var q1 = new BlockingCollection<SpaceBattle.Lib.ICommand>();
        var st1 = new ServerThread(q1, () =>
        {
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", currentScope).Execute();
        });
        var q2 = new BlockingCollection<SpaceBattle.Lib.ICommand>();
        var st2 = new ServerThread(q2, () =>
        {
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", currentScope).Execute();
        });
        var c1 = new Mock<SpaceBattle.Lib.ICommand>();
        var c2 = new Mock<SpaceBattle.Lib.ICommand>();

        threadList.Add(11, st1);
        queueList.Add(11, q1);
        threadList.Add(23, st2);
        queueList.Add(23, q2);

        c1.Setup(c => c.Execute()).Throws(new NotImplementedException()).Verifiable(Times.AtLeast(2));
        c2.Setup(c => c.Execute()).Verifiable(Times.AtLeast(2));

        IoC.Resolve<object>("Test.CreateSoftStopConfiguration", 11, c2.Object, c1.Object);
        IoC.Resolve<object>("Test.CreateSoftStopConfiguration", 23, c2.Object, c1.Object);

        var q = IoC.Resolve<Dictionary<int, BlockingCollection<SpaceBattle.Lib.ICommand>>>("Game.Struct.ServerThreadQueue.List")[11];
        st1.Start();
        st2.Start();

        bar.SignalAndWait();
        bar.Dispose();
        Assert.True(st1.Status());
        Assert.True(st2.Status());
        Mock.Verify(c1, c2);
        IoC.Resolve<object>("Test.ClearDicts", 11, 23);
    }

    [Fact]
    public void SuccesfulSoftStopThreadsWithoutExceptions()
    {
        var bar = new Barrier(3);
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Exception.Handler", (object[] args) =>
        {
            return new EmptyCommand();
        }).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Test.CreateSoftStopConfiguration", (object[] args) =>
        {
            IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Struct.ServerThread.SendCommand", (int)args[0], (SpaceBattle.Lib.ICommand)args[1]).Execute();
            IoC.Resolve<SpaceBattle.Lib.ICommand>(
                "Game.Struct.ServerThread.SoftStop",
                (int)args[0],
                () => { bar.SignalAndWait(); }
            ).Execute();
            IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Struct.ServerThread.SendCommand", (int)args[0], (SpaceBattle.Lib.ICommand)args[2]).Execute();
            return new object();
        }).Execute();

        var currentScope = IoC.Resolve<object>("Scopes.Current");
        var threadList = IoC.Resolve<Dictionary<int, ServerThread>>("Game.Struct.ServerThread.List");
        var queueList = IoC.Resolve<Dictionary<int, BlockingCollection<SpaceBattle.Lib.ICommand>>>("Game.Struct.ServerThreadQueue.List");
        var q1 = new BlockingCollection<SpaceBattle.Lib.ICommand>();
        var st1 = new ServerThread(q1, () =>
        {
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", currentScope).Execute();
        });
        var q2 = new BlockingCollection<SpaceBattle.Lib.ICommand>();
        var st2 = new ServerThread(q2, () =>
        {
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", currentScope).Execute();
        });
        var c1 = new Mock<SpaceBattle.Lib.ICommand>();
        var c2 = new Mock<SpaceBattle.Lib.ICommand>();

        threadList.Add(11, st1);
        queueList.Add(11, q1);
        threadList.Add(23, st2);
        queueList.Add(23, q2);

        c1.Setup(c => c.Execute()).Verifiable(Times.AtLeast(2));
        c2.Setup(c => c.Execute()).Verifiable(Times.AtLeast(2));

        IoC.Resolve<object>("Test.CreateSoftStopConfiguration", 11, c1.Object, c2.Object);
        IoC.Resolve<object>("Test.CreateSoftStopConfiguration", 23, c1.Object, c2.Object);

        st1.Start();
        st2.Start();

        bar.SignalAndWait();
        bar.Dispose();
        Assert.True(st1.Status());
        Assert.True(st2.Status());
        Mock.Verify(c1, c2);
        IoC.Resolve<object>("Test.ClearDicts", 11, 23);
    }

    [Fact]
    public void HardStopThreadFromAnotherThread()
    {
        var bar = new Barrier(2);
        var currentScope = IoC.Resolve<object>("Scopes.Current");
        IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Struct.ServerThread.CreateAndStart", 0, () =>
        {
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", currentScope).Execute();
        }).Execute();
        var th = IoC.Resolve<Dictionary<int, ServerThread>>("Game.Struct.ServerThread.List")[0];
        var hsThread = new HardStopCommand(th, () => { });
        Assert.Throws<Exception>(() => hsThread.Execute());
        IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Struct.ServerThread.HardStop", 0, () => { bar.SignalAndWait(); }).Execute();
        bar.SignalAndWait();
    }

    [Fact]
    public void SoftStopThreadFromAnotherThread()
    {
        var bar = new Barrier(2);
        var currentScope = IoC.Resolve<object>("Scopes.Current");
        IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Struct.ServerThread.CreateAndStart", 0, () =>
        {
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", currentScope).Execute();
        }).Execute();
        var queue = IoC.Resolve<Dictionary<int, BlockingCollection<SpaceBattle.Lib.ICommand>>>("Game.Struct.ServerThreadQueue.List")[0];
        var th = IoC.Resolve<Dictionary<int, ServerThread>>("Game.Struct.ServerThread.List")[0];
        var ssThread = new SoftStopCommand(th, queue, () => { });
        Assert.Throws<Exception>(() => ssThread.Execute());
        IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Struct.ServerThread.HardStop", 0, () => { bar.SignalAndWait(); }).Execute();
        bar.SignalAndWait();
    }

    [Fact]
    public void NullThreadComparation()
    {
        var currentScope = IoC.Resolve<object>("Scopes.Current");
        IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Struct.ServerThread.CreateAndStart", 0, () =>
        {
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", currentScope).Execute();
        }).Execute();
        var th = IoC.Resolve<Dictionary<int, ServerThread>>("Game.Struct.ServerThread.List")[0];
        th.GetHashCode();
        Assert.False(th.Equals(null));
        IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Struct.ServerThread.HardStop", 0, () => { }).Execute();
    }
}

