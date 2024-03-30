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

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Struct.ServerThread.Create", (object[] args) =>
        {
            var act = () => 
            {
                var guid = (Guid)args[0];
                var enterHook = (Action)args[1];
                var exitHook = (Action)args[2];
                var queue = new BlockingCollection<SpaceBattle.Lib.ICommand>();
                var st = new ServerThread(queue, enterHook, exitHook);

                IoC.Resolve<Hwdtech.ICommand>("IoC.Register", $"Game.Struct.ServerThread.HardStop{guid}", (object[] args) =>
                {
                    var hs = new HardStopCommand(st, (Action)args[0]);
                    var cmd = IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Struct.ServerThread.SendCommand", guid, hs);
                    return cmd;
                }).Execute();

                IoC.Resolve<Hwdtech.ICommand>("IoC.Register", $"Game.Struct.ServerThread.SoftStop{guid}", (object[] args) =>
                {
                    var ss = new SoftStopCommand(st, queue, (Action)args[0]);
                    var cmd = IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.Struct.ServerThread.SendCommand", guid, ss);
                    return cmd;
                }).Execute();

                IoC.Resolve<Hwdtech.ICommand>("IoC.Register", $"Game.Struct.ServerThread.Queue{guid}", (object[] args) =>
                {
                    return queue;
                }).Execute();

                IoC.Resolve<Hwdtech.ICommand>("IoC.Register", $"Game.Struct.ServerThread.Get{guid}", (object[] args) =>
                {
                    return st;
                }).Execute();
            };
            
            return new ActionCommand(act);
     
        }).Execute();

        new CreateAndStartThread().Call();
        new SendCommand().Call();
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

        var st1 = IoC.Resolve<ServerThread>($"Game.Struct.ServerThread.Get{id1}");
        var st2 = IoC.Resolve<ServerThread>($"Game.Struct.ServerThread.Get{id2}");

        c.Setup(c => c.Execute()).Throws(new NotImplementedException()).Verifiable(Times.AtLeast(2));
        c1.Setup(c => c.Execute()).Verifiable(Times.Never());

        IoC.Resolve<SpaceBattle.Lib.ICommand>($"Game.Struct.ServerThread.SendCommand", id1,  c.Object).Execute();
        IoC.Resolve<SpaceBattle.Lib.ICommand>($"Game.Struct.ServerThread.SendCommand", id1,  c.Object).Execute();

        IoC.Resolve<SpaceBattle.Lib.ICommand>($"Game.Struct.ServerThread.HardStop{id1}", () => {}).Execute();
        IoC.Resolve<SpaceBattle.Lib.ICommand>($"Game.Struct.ServerThread.HardStop{id2}", () => {}).Execute();

        IoC.Resolve<SpaceBattle.Lib.ICommand>($"Game.Struct.ServerThread.SendCommand", id1, c1.Object).Execute();
        IoC.Resolve<SpaceBattle.Lib.ICommand>($"Game.Struct.ServerThread.SendCommand", id2, c1.Object).Execute();

        bar.SignalAndWait();
        bar.Dispose();

        st1.Wait(100);
        st2.Wait(100);
      
        Assert.False(st1.Status());
        Assert.False(st2.Status());

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

        var st1 = IoC.Resolve<ServerThread>($"Game.Struct.ServerThread.Get{id1}");
        var st2 = IoC.Resolve<ServerThread>($"Game.Struct.ServerThread.Get{id2}");

        c.Setup(c => c.Execute()).Verifiable(Times.AtLeast(2));
        c1.Setup(c => c.Execute()).Verifiable(Times.Never());

        IoC.Resolve<SpaceBattle.Lib.ICommand>($"Game.Struct.ServerThread.SendCommand", id1, c.Object).Execute();
        IoC.Resolve<SpaceBattle.Lib.ICommand>($"Game.Struct.ServerThread.SendCommand", id1,  c.Object).Execute();

        IoC.Resolve<SpaceBattle.Lib.ICommand>($"Game.Struct.ServerThread.HardStop{id1}", () => {}).Execute();
        IoC.Resolve<SpaceBattle.Lib.ICommand>($"Game.Struct.ServerThread.HardStop{id2}", () => {}).Execute();

        IoC.Resolve<SpaceBattle.Lib.ICommand>($"Game.Struct.ServerThread.SendCommand", id1, c1.Object).Execute();
        IoC.Resolve<SpaceBattle.Lib.ICommand>($"Game.Struct.ServerThread.SendCommand", id2, c1.Object).Execute();

        bar.SignalAndWait();
        bar.Dispose();

        st1.Wait(100);
        st2.Wait(100);
      
        Assert.False(st1.Status());
        Assert.False(st2.Status());

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

                IoC.Resolve<SpaceBattle.Lib.ICommand>(
                    "Game.Struct.ServerThread.SendCommand",
                    (Guid)args[0],
                    (SpaceBattle.Lib.ICommand)args[1]
                ).Execute();

                IoC.Resolve<SpaceBattle.Lib.ICommand>(
                    $"Game.Struct.ServerThread.SoftStop{(Guid)args[0]}",
                     () => {}
                ).Execute();

                IoC.Resolve<SpaceBattle.Lib.ICommand>(
                    "Game.Struct.ServerThread.SendCommand",
                     (Guid)args[0],
                    (SpaceBattle.Lib.ICommand)args[2]
                ).Execute();
            };

            return new ActionCommand(act);

        }).Execute();

        var currentScope = IoC.Resolve<object>("Scopes.Current");

        IoC.Resolve<SpaceBattle.Lib.ICommand>(
            "Game.Struct.ServerThread.Create",
            id1, 
            () => {IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", currentScope).Execute();},
            () => {bar.SignalAndWait();}
        ).Execute();

        IoC.Resolve<SpaceBattle.Lib.ICommand>(
            "Game.Struct.ServerThread.Create", 
            id2,
            () => {IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", currentScope).Execute();},
            () => {bar.SignalAndWait();}
        ).Execute();

        var st1 = IoC.Resolve<ServerThread>($"Game.Struct.ServerThread.Get{id1}");
        var st2 = IoC.Resolve<ServerThread>($"Game.Struct.ServerThread.Get{id2}");

        var c1 = new Mock<SpaceBattle.Lib.ICommand>();
        var c2 = new Mock<SpaceBattle.Lib.ICommand>();

        c1.Setup(c => c.Execute()).Throws(new NotImplementedException()).Verifiable(Times.AtLeast(2));
        c2.Setup(c => c.Execute()).Verifiable(Times.AtLeast(2));

        IoC.Resolve<SpaceBattle.Lib.ICommand>("Test.CreateSoftStopConfiguration", id1, c2.Object, c1.Object).Execute();
        IoC.Resolve<SpaceBattle.Lib.ICommand>("Test.CreateSoftStopConfiguration", id2, c2.Object, c1.Object).Execute();

        st1.Start();
        st2.Start();

        bar.SignalAndWait();
        bar.Dispose();

        st1.Wait(100);
        st2.Wait(100);

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

                IoC.Resolve<SpaceBattle.Lib.ICommand>(
                    "Game.Struct.ServerThread.SendCommand",
                    (Guid)args[0],
                    (SpaceBattle.Lib.ICommand)args[1]
                ).Execute();

                IoC.Resolve<SpaceBattle.Lib.ICommand>(
                    $"Game.Struct.ServerThread.SoftStop{(Guid)args[0]}",
                     () => {}
                ).Execute();

                IoC.Resolve<SpaceBattle.Lib.ICommand>(
                    "Game.Struct.ServerThread.SendCommand",
                     (Guid)args[0],
                    (SpaceBattle.Lib.ICommand)args[2]
                ).Execute();
            };

            return new ActionCommand(act);

        }).Execute();

        var currentScope = IoC.Resolve<object>("Scopes.Current");

        IoC.Resolve<SpaceBattle.Lib.ICommand>(
            "Game.Struct.ServerThread.Create",
            id1, 
            () => {IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", currentScope).Execute();},
            () => {bar.SignalAndWait();}
        ).Execute();

        IoC.Resolve<SpaceBattle.Lib.ICommand>(
            "Game.Struct.ServerThread.Create", 
            id2,
            () => {IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", currentScope).Execute();},
            () => {bar.SignalAndWait();}
        ).Execute();

        var st1 = IoC.Resolve<ServerThread>($"Game.Struct.ServerThread.Get{id1}");
        var st2 = IoC.Resolve<ServerThread>($"Game.Struct.ServerThread.Get{id2}");

        var c1 = new Mock<SpaceBattle.Lib.ICommand>();
        var c2 = new Mock<SpaceBattle.Lib.ICommand>();

        c1.Setup(c => c.Execute()).Verifiable(Times.AtLeast(2));
        c2.Setup(c => c.Execute()).Verifiable(Times.AtLeast(2));

        IoC.Resolve<SpaceBattle.Lib.ICommand>("Test.CreateSoftStopConfiguration", id1, c1.Object, c2.Object).Execute();
        IoC.Resolve<SpaceBattle.Lib.ICommand>("Test.CreateSoftStopConfiguration", id2, c1.Object, c2.Object).Execute();

        st1.Start();
        st2.Start();

        bar.SignalAndWait();
        bar.Dispose();

        st1.Wait(100);
        st2.Wait(100);

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

        var st = IoC.Resolve<ServerThread>($"Game.Struct.ServerThread.Get{id}");

        var hsThread = new HardStopCommand(st, () => { });
        Assert.Throws<Exception>(() => hsThread.Execute());
        IoC.Resolve<SpaceBattle.Lib.ICommand>($"Game.Struct.ServerThread.HardStop{id}", () => {}).Execute();
        bar.SignalAndWait();
        st.Wait(100);
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

        var st = IoC.Resolve<ServerThread>($"Game.Struct.ServerThread.Get{id}");
        var queue = IoC.Resolve<BlockingCollection<SpaceBattle.Lib.ICommand>>($"Game.Struct.ServerThread.Queue{id}");

        var ssThread = new SoftStopCommand(st, queue, () => { });
        Assert.Throws<Exception>(() => ssThread.Execute());
        IoC.Resolve<SpaceBattle.Lib.ICommand>($"Game.Struct.ServerThread.HardStop{id}", () => {}).Execute();
        bar.SignalAndWait();
        st.Wait(100);
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

        var st = IoC.Resolve<ServerThread>($"Game.Struct.ServerThread.Get{id}");

        st.GetHashCode();
        Assert.False(st.Equals(null));
        IoC.Resolve<SpaceBattle.Lib.ICommand>($"Game.Struct.ServerThread.HardStop{id}", () => {}).Execute();
        bar.SignalAndWait();
        st.Wait(100);
    }
}

