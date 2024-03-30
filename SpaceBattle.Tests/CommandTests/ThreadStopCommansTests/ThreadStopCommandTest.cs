using System.Collections.Concurrent;
using Hwdtech;
using Moq;
using Hwdtech.Ioc;
using SpaceBattle.Lib;

namespace SpaceBattle.Tests;

public class ThreadStopCommandsTest
{
    private ConcurrentDictionary<Guid, ServerThread> threadList = new ConcurrentDictionary<Guid, ServerThread>();

    public ThreadStopCommandsTest()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "ServerThreadContainer.Find", (object[] args) => 
        {
            return threadList[(Guid)args[0]];
        }).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "ServerThreadContainer.Add", (object[] args) =>
        {   
            var act = () => 
            {
                threadList.TryAdd((Guid)args[0], (ServerThread)args[1]);
            };

            return new ActionCommand(act);       
        }).Execute();

        new CreateAndStartThread().Call();
        new SendCommand().Call();
        new HardStopThread().Call();
        new SoftStopThread().Call();
    }

    [Fact]
    public void SuccesfulHardStopThreadsWithCommandThrowsException()
    {
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Exception.Handler", (object[] args) =>
        {
            return new EmptyCommand();
        }).Execute();

        var bar = new Barrier(3);
        var currentScope = IoC.Resolve<object>("Scopes.Current");

        var c = new Mock<SpaceBattle.Lib.ICommand>();
        var c1 = new Mock<SpaceBattle.Lib.ICommand>();

        var id1 = Guid.NewGuid();
        var id2 = Guid.NewGuid();

        IoC.Resolve<SpaceBattle.Lib.ICommand>(
            "Game.Struct.ServerThread.CreateAndStart",
            id1, 
            () => {IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", currentScope).Execute();},
            () => {bar.SignalAndWait();}
        ).Execute();

        IoC.Resolve<SpaceBattle.Lib.ICommand>(
            "Game.Struct.ServerThread.CreateAndStart", 
            id2,
            () => {IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", currentScope).Execute();},
            () => {bar.SignalAndWait();}
        ).Execute();

        c.Setup(c => c.Execute()).Throws(new NotImplementedException()).Verifiable(Times.AtLeast(2));
        c1.Setup(c => c.Execute()).Verifiable(Times.Never());

        IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Struct.ServerThread.SendCommand", id1, c.Object).Execute();
        IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Struct.ServerThread.SendCommand", id2, c.Object).Execute();

        IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Struct.ServerThread.HardStop", id1, () => {}).Execute();
        IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Struct.ServerThread.HardStop", id2, () => {}).Execute();

        IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Struct.ServerThread.SendCommand", id1, c1.Object).Execute();
        IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Struct.ServerThread.SendCommand", id2, c1.Object).Execute();

        bar.SignalAndWait();
        bar.Dispose();

        threadList[id1].Wait(100);
        threadList[id2].Wait(100);
      
        Assert.False(threadList[id1].Status());
        Assert.False(threadList[id2].Status());

        Mock.Verify(c, c1);
    } 

    [Fact]
    public void SuccesfulHardStopThreads()
    {
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Exception.Handler", (object[] args) =>
        {
            return new EmptyCommand();
        }).Execute();

        var bar = new Barrier(3);
        var currentScope = IoC.Resolve<object>("Scopes.Current");

        var c = new Mock<SpaceBattle.Lib.ICommand>();
        var c1 = new Mock<SpaceBattle.Lib.ICommand>();

        var id1 = Guid.NewGuid();
        var id2 = Guid.NewGuid();

        IoC.Resolve<SpaceBattle.Lib.ICommand>(
            "Game.Struct.ServerThread.CreateAndStart",
            id1, 
            () => {IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", currentScope).Execute();},
            () => {bar.SignalAndWait();}
        ).Execute();

        IoC.Resolve<SpaceBattle.Lib.ICommand>(
            "Game.Struct.ServerThread.CreateAndStart",
            id2,
            () => {IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", currentScope).Execute();},
            () => {bar.SignalAndWait();}
        ).Execute();

        c.Setup(c => c.Execute()).Verifiable(Times.AtLeast(2));
        c1.Setup(c => c.Execute()).Verifiable(Times.Never());

        IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Struct.ServerThread.SendCommand", id1, c.Object).Execute();
        IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Struct.ServerThread.SendCommand", id2, c.Object).Execute();

        IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Struct.ServerThread.HardStop", id1, () => {}).Execute();
        IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Struct.ServerThread.HardStop", id2, () => {}).Execute();

        IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Struct.ServerThread.SendCommand", id1, c1.Object).Execute();
        IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Struct.ServerThread.SendCommand", id2, c1.Object).Execute();
        
        bar.SignalAndWait();
        bar.Dispose();

        threadList[id1].Wait(100);
        threadList[id2].Wait(100);

        Assert.False(threadList[id1].Status());
        Assert.False(threadList[id2].Status());
        Mock.Verify(c, c1);
    }

    [Fact]
    public void SuccesfulSoftStopThreadsWithCommandThrowsException()
    {
        var bar = new Barrier(3);

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Exception.Handler", (object[] args) =>
        {
            return new EmptyCommand();
        }).Execute();

        var id1 = Guid.NewGuid();
        var id2 = Guid.NewGuid();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Test.CreateSoftStopConfiguration", (object[] args) =>
        {
            var act = () => {
                IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Struct.ServerThread.SendCommand", (Guid)args[0], (SpaceBattle.Lib.ICommand)args[1]).Execute();
                IoC.Resolve<SpaceBattle.Lib.ICommand>(
                    "Game.Struct.ServerThread.SoftStop",
                    (Guid)args[0],
                    () => {}
                ).Execute();
                IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Struct.ServerThread.SendCommand", (Guid)args[0], (SpaceBattle.Lib.ICommand)args[2]).Execute();
            };
            return new ActionCommand(act);
        }).Execute();

        var currentScope = IoC.Resolve<object>("Scopes.Current");

        var q1 = new BlockingCollection<SpaceBattle.Lib.ICommand>();
        var st1 = new ServerThread(
            q1,
            () => {IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", currentScope).Execute();},
            () => {bar.SignalAndWait();}
        );

        var q2 = new BlockingCollection<SpaceBattle.Lib.ICommand>();
        var st2 = new ServerThread(
            q2,
            () => {IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", currentScope).Execute();},
            () => {bar.SignalAndWait();}
        );

        var c1 = new Mock<SpaceBattle.Lib.ICommand>();
        var c2 = new Mock<SpaceBattle.Lib.ICommand>();

        threadList.TryAdd(id1, st1);
        threadList.TryAdd(id2, st2);

        c1.Setup(c => c.Execute()).Throws(new NotImplementedException()).Verifiable(Times.AtLeast(2));
        c2.Setup(c => c.Execute()).Verifiable(Times.AtLeast(2));

        IoC.Resolve<SpaceBattle.Lib.ICommand>("Test.CreateSoftStopConfiguration", id1, c2.Object, c1.Object).Execute();
        IoC.Resolve<SpaceBattle.Lib.ICommand>("Test.CreateSoftStopConfiguration", id2, c2.Object, c1.Object).Execute();

        st1.Start();
        st2.Start();

        bar.SignalAndWait();
        bar.Dispose();

        threadList[id1].Wait(100);
        threadList[id2].Wait(100);

        Assert.False(st1.Status());
        Assert.False(st2.Status());

        Mock.Verify(c1, c2);
    }

    [Fact]
    public void SuccesfulSoftStopThreadsWithoutExceptions()
    {
        var bar = new Barrier(3);
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Exception.Handler", (object[] args) =>
        {
            return new EmptyCommand();
        }).Execute();

        var id1 = Guid.NewGuid();
        var id2 = Guid.NewGuid();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Test.CreateSoftStopConfiguration", (object[] args) =>
        {
            var act = () => {
                IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Struct.ServerThread.SendCommand", (Guid)args[0], (SpaceBattle.Lib.ICommand)args[1]).Execute();
                IoC.Resolve<SpaceBattle.Lib.ICommand>(
                    "Game.Struct.ServerThread.SoftStop",
                    (Guid)args[0],
                    () => {}
                ).Execute();
                IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Struct.ServerThread.SendCommand", (Guid)args[0], (SpaceBattle.Lib.ICommand)args[2]).Execute();
            }; 
            return new ActionCommand(act);
        }).Execute();

        var currentScope = IoC.Resolve<object>("Scopes.Current");

        var q1 = new BlockingCollection<SpaceBattle.Lib.ICommand>();
        var st1 = new ServerThread(
            q1,
            () => {IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", currentScope).Execute();},
            () => {bar.SignalAndWait();}
        );

        var q2 = new BlockingCollection<SpaceBattle.Lib.ICommand>();
        var st2 = new ServerThread(
            q2,
            () => {IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", currentScope).Execute();},
            () => {bar.SignalAndWait();}
        );

        var c1 = new Mock<SpaceBattle.Lib.ICommand>();
        var c2 = new Mock<SpaceBattle.Lib.ICommand>();

        threadList.TryAdd(id1, st1);
        threadList.TryAdd(id2, st2);

        c1.Setup(c => c.Execute()).Verifiable(Times.AtLeast(2));
        c2.Setup(c => c.Execute()).Verifiable(Times.AtLeast(2));

        IoC.Resolve<SpaceBattle.Lib.ICommand>("Test.CreateSoftStopConfiguration", id1, c1.Object, c2.Object).Execute();
        IoC.Resolve<SpaceBattle.Lib.ICommand>("Test.CreateSoftStopConfiguration", id2, c1.Object, c2.Object).Execute();

        st1.Start();
        st2.Start();

        bar.SignalAndWait();
        bar.Dispose();

        threadList[id1].Wait(100);
        threadList[id2].Wait(100);

        Assert.False(st1.Status());
        Assert.False(st2.Status());
        Mock.Verify(c1, c2);
    }

    [Fact]
    public void HardStopThreadFromAnotherThread()
    {
        var bar = new Barrier(2);
        var currentScope = IoC.Resolve<object>("Scopes.Current");
        var id = Guid.NewGuid();

        IoC.Resolve<SpaceBattle.Lib.ICommand>(
            "Game.Struct.ServerThread.CreateAndStart", 
            id,
            () => {IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", currentScope).Execute();},
            () => {bar.SignalAndWait();}
        ).Execute();

        var th = threadList[id];
        var hsThread = new HardStopCommand(th, () => { });
        Assert.Throws<Exception>(() => hsThread.Execute());
        IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Struct.ServerThread.HardStop", id, () => {}).Execute();
        bar.SignalAndWait();
        th.Wait(100);
    }

    [Fact]
    public void SoftStopThreadFromAnotherThread()
    {
        var bar = new Barrier(2);
        var currentScope = IoC.Resolve<object>("Scopes.Current");
        var id = Guid.NewGuid();

        IoC.Resolve<SpaceBattle.Lib.ICommand>(
            "Game.Struct.ServerThread.CreateAndStart", 
            id,
            () => {IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", currentScope).Execute();},
            () => {bar.SignalAndWait();}
        ).Execute();

        var th = threadList[id];
        var queue = th.GetQueue();

        var ssThread = new SoftStopCommand(th, queue, () => { });
        Assert.Throws<Exception>(() => ssThread.Execute());
        IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Struct.ServerThread.HardStop", id, () => {}).Execute();
        bar.SignalAndWait();
        th.Wait(100);
    }

    [Fact]
    public void NullThreadComparation()
    {
        var currentScope = IoC.Resolve<object>("Scopes.Current");
        var bar = new Barrier(2);
        var id = Guid.NewGuid();

        IoC.Resolve<SpaceBattle.Lib.ICommand>(
            "Game.Struct.ServerThread.CreateAndStart", 
            id,
            () => {IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", currentScope).Execute();},
            () => {bar.SignalAndWait();}
        ).Execute();
        
        var th = threadList[id];

        th.GetHashCode();
        Assert.False(th.Equals(null));
        IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Struct.ServerThread.HardStop", id, () => {}).Execute();
        bar.SignalAndWait();
        th.Wait(100);
    }
}

