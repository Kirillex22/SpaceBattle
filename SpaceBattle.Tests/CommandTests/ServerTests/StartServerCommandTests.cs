using Hwdtech;
using Hwdtech.Ioc;
using Moq;

namespace SpaceBattle.Lib.Tests;

public class StartServerCommandTests
{
    [Fact]
    public void SuccefulStartServer()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", 
            IoC.Resolve<object>("Scopes.New", 
            IoC.Resolve<object>("Scopes.Root"))
        ).Execute();

        int count = 5;
        var MoqCommand = new Mock<SpaceBattle.Lib.ICommand>();

        MoqCommand.Setup(i => i.Execute()).Verifiable();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.Create.Thread", (object[] args) => MoqCommand.Object).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Server.Start", (object[] args) => new StartServerCommand((int)args[0])).Execute();
        IoC.Resolve<SpaceBattle.Lib.ICommand>("Server.Start", count).Execute();

        MoqCommand.Verify(i => i.Execute(), Times.Exactly(count));
    }
}

