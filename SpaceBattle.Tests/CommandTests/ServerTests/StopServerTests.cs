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

        var dict = new Dictionary<int, object>();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.Thread.Handle", (object[] args) => dict).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.Stop", (object[] args) => new StopServerCommand()).Execute();
    }

    [Fact]
    public void SuccefulStopServer()
    {
        var dict = IoC.Resolve<Dictionary<int, object>>("Server.Thread.Handle");
        var MoqCommand = new Mock<SpaceBattle.Lib.ICommand>();

        MoqCommand.Setup(i => i.Execute()).Verifiable();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.Command.Send", (object[] args) => MoqCommand.Object).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.Thread.Stop", (object[] args) => MoqCommand.Object).Execute();

        dict[1] = 1;
        
        IoC.Resolve<SpaceBattle.Lib.ICommand>("Server.Stop").Execute();

        MoqCommand.Verify(i => i.Execute(), Times.Exactly(1));
    }
}

