using Hwdtech;
using Hwdtech.Ioc;
using Moq;

namespace SpaceBattle.Lib.Tests;

public class StopServerCommandTests
{
    public StopServerCommandTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", 
            IoC.Resolve<object>("Scopes.New", 
            IoC.Resolve<object>("Scopes.Root"))
        ).Execute();

        var map = new Dictionary<int, object>();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.Thread.Map", (object[] args) => map).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.Stop", (object[] args) => new StopServerCommand()).Execute();
    }

    [Fact]
    public void SuccefulStopServer()
    {
        var map = IoC.Resolve<Dictionary<int, object>>("Server.Thread.Map");
        var MoqCommand = new Mock<SpaceBattle.Lib.ICommand>();

        MoqCommand.Setup(i => i.Execute()).Verifiable();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.Command.Send", (object[] args) => MoqCommand.Object).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.Thread.Stop", (object[] args) => MoqCommand.Object).Execute();

        map[0] = 1;
        map[1] = 2;
        map[2] = 3;

        var MoqBlocker = new Mock<ICommand>();
        MoqBlocker.Setup(i => i.Execute());

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.Thread.Barrier.Create", (object[] args) => MoqBlocker.Object).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.Thread.Barrier.Check", (object[] args) => MoqBlocker.Object).Execute();
        
        IoC.Resolve<SpaceBattle.Lib.ICommand>("Server.Stop").Execute();

        MoqCommand.Verify(i => i.Execute(), Times.Exactly(3));
    }
}

