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

        c.Setup(c => c.Execute()).Throws(new NotImplementedException()).Verifiable(Times.AtLeast(2));
        c1.Setup(c => c.Execute()).Verifiable(Times.Never());

        using(var st1 = IoC.Resolve<ServerThread>($"Game.Struct.ServerThread.Get{id1}"))
        using(var st2 = IoC.Resolve<ServerThread>($"Game.Struct.ServerThread.Get{id1}"))
        {
            IoC.Resolve<SpaceBattle.Lib.ICommand>($"Game.Struct.ServerThread.SendCommand", id1,  c.Object).Execute();
            IoC.Resolve<SpaceBattle.Lib.ICommand>($"Game.Struct.ServerThread.SendCommand", id1,  c.Object).Execute();

            IoC.Resolve<SpaceBattle.Lib.ICommand>($"Game.Struct.ServerThread.HardStop{id1}", () => {}).Execute();
            IoC.Resolve<SpaceBattle.Lib.ICommand>($"Game.Struct.ServerThread.HardStop{id2}", () => {}).Execute();

            IoC.Resolve<SpaceBattle.Lib.ICommand>($"Game.Struct.ServerThread.SendCommand", id1, c1.Object).Execute();
            IoC.Resolve<SpaceBattle.Lib.ICommand>($"Game.Struct.ServerThread.SendCommand", id2, c1.Object).Execute();

            bar.SignalAndWait();
            bar.Dispose();
        
            Assert.True(st1.Status());
            Assert.True(st2.Status());

            Mock.Verify(c, c1);
        }
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

        using(var st1 = IoC.Resolve<ServerThread>($"Game.Struct.ServerThread.Get{id1}"))
        using(var st2 = IoC.Resolve<ServerThread>($"Game.Struct.ServerThread.Get{id1}"))
        {
            IoC.Resolve<SpaceBattle.Lib.ICommand>($"Game.Struct.ServerThread.SendCommand", id1,  c.Object).Execute();
            IoC.Resolve<SpaceBattle.Lib.ICommand>($"Game.Struct.ServerThread.SendCommand", id1,  c.Object).Execute();

            IoC.Resolve<SpaceBattle.Lib.ICommand>($"Game.Struct.ServerThread.HardStop{id1}", () => {}).Execute();
            IoC.Resolve<SpaceBattle.Lib.ICommand>($"Game.Struct.ServerThread.HardStop{id2}", () => {}).Execute();

            IoC.Resolve<SpaceBattle.Lib.ICommand>($"Game.Struct.ServerThread.SendCommand", id1, c1.Object).Execute();
            IoC.Resolve<SpaceBattle.Lib.ICommand>($"Game.Struct.ServerThread.SendCommand", id2, c1.Object).Execute();

            bar.SignalAndWait();
            bar.Dispose();
        
            Assert.True(st1.Status());
            Assert.True(st2.Status());

            Mock.Verify(c, c1);
        }
    }

    [Fact]
    public void SuccesfulSoftStopThreadsWithCommandThrowsException()
    {
        var bar = new Barrier(3);
        var ssExecuted = new Barrier(3);

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
                    new ActionCommand(() => {ssExecuted.SignalAndWait();})
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
        

        var c1 = new Mock<SpaceBattle.Lib.ICommand>();
        var c2 = new Mock<SpaceBattle.Lib.ICommand>();

        c1.Setup(c => c.Execute()).Throws(new NotImplementedException()).Verifiable(Times.AtLeast(2));
        c2.Setup(c => c.Execute()).Verifiable(Times.AtLeast(2));

        using(var st1 = IoC.Resolve<ServerThread>($"Game.Struct.ServerThread.Get{id1}"))
        using(var st2 = IoC.Resolve<ServerThread>($"Game.Struct.ServerThread.Get{id2}"))
        {
            IoC.Resolve<SpaceBattle.Lib.ICommand>("Test.CreateSoftStopConfiguration", id1, c2.Object, c1.Object).Execute();
            IoC.Resolve<SpaceBattle.Lib.ICommand>("Test.CreateSoftStopConfiguration", id2, c2.Object, c1.Object).Execute();

            ssExecuted.SignalAndWait();

            bar.SignalAndWait();
            bar.Dispose();

            Assert.True(st1.Status());
            Assert.True(st2.Status());

            Mock.Verify(c1, c2);
        }
    }

    [Fact]
    public void SuccesfulSoftStopThreadsWithoutExceptions()
    {
        var bar = new Barrier(3);
        var ssExecuted = new Barrier(3);

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
                    new ActionCommand(() => {ssExecuted.SignalAndWait();})
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

        var c1 = new Mock<SpaceBattle.Lib.ICommand>();
        var c2 = new Mock<SpaceBattle.Lib.ICommand>();

        c1.Setup(c => c.Execute()).Verifiable(Times.AtLeast(2));
        c2.Setup(c => c.Execute()).Verifiable(Times.AtLeast(2));

        using(var st1 = IoC.Resolve<ServerThread>($"Game.Struct.ServerThread.Get{id1}"))
        using(var st2 = IoC.Resolve<ServerThread>($"Game.Struct.ServerThread.Get{id2}"))
        {
            IoC.Resolve<SpaceBattle.Lib.ICommand>("Test.CreateSoftStopConfiguration", id1, c2.Object, c1.Object).Execute();
            IoC.Resolve<SpaceBattle.Lib.ICommand>("Test.CreateSoftStopConfiguration", id2, c2.Object, c1.Object).Execute();

            ssExecuted.SignalAndWait();

            bar.SignalAndWait();
            bar.Dispose();

            Assert.True(st1.Status());
            Assert.True(st2.Status());

            Mock.Verify(c1, c2);
        }
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

        using(var st = IoC.Resolve<ServerThread>($"Game.Struct.ServerThread.Get{id}"))
        {
            var hsThread = new HardStopCommand(st, () => { });
            Assert.Throws<Exception>(() => hsThread.Execute());
            IoC.Resolve<SpaceBattle.Lib.ICommand>($"Game.Struct.ServerThread.HardStop{id}", () => {}).Execute();
            bar.SignalAndWait();
        }    
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

        using(var st = IoC.Resolve<ServerThread>($"Game.Struct.ServerThread.Get{id}"))
        {
            var queue = IoC.Resolve<BlockingCollection<SpaceBattle.Lib.ICommand>>($"Game.Struct.ServerThread.Queue{id}");
            var ssThread = new SoftStopCommand(st, queue, () => { });
            Assert.Throws<Exception>(() => ssThread.Execute());
            IoC.Resolve<SpaceBattle.Lib.ICommand>($"Game.Struct.ServerThread.HardStop{id}", () => {}).Execute();
            bar.SignalAndWait();
        }
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

        using(var st = IoC.Resolve<ServerThread>($"Game.Struct.ServerThread.Get{id}"))
        {
            st.GetHashCode();
            Assert.False(st.Equals(null));
            IoC.Resolve<SpaceBattle.Lib.ICommand>($"Game.Struct.ServerThread.HardStop{id}", () => {}).Execute();
            bar.SignalAndWait();
        }
    }
}

