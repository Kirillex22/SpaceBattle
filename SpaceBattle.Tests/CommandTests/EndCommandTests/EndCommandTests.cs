using Hwdtech;
using Hwdtech.Ioc;
using Moq;
using TechTalk.SpecFlow.FileAccess;

namespace SpaceBattle.Lib.Tests;

public class EndMoveCommnandTest
{
    public EndMoveCommnandTest()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        var EndCommand = (object[] args) => {
            var mcommand = (IMoveCommandStopable)args[0];
            return new EndMoveCommand(mcommand); };
        
        var InjectCommand = (object[] args) => {
            var LocateInject = (IBridgeCommand)args[0];
            var CommandInject = (ICommand)args[1];
            LocateInject.Inject(CommandInject);
            return LocateInject; };

        var MoqCommand = new Mock<ICommand>();
        MoqCommand.Setup(mc => mc.Execute());

        var MoqInject = new Mock<IBridgeCommand>();
        MoqInject.Setup(mc => mc.Inject(It.IsAny<ICommand>()));

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetProperty", (object[] args) => MoqInject.Object).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "DeleteProperty", (object[] args) => MoqCommand.Object).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Command.Empty", (object[] args) => new EmptyCommand()).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Command.Inject", InjectCommand).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Command.End", EndCommand).Execute();
    }

    [Fact]
    public void EndCommandTest()
    {
        var MoqUobject = new Mock<IUobject>();
        var MoqEnd = new Mock<IMoveCommandStopable>();

        MoqEnd.Setup(i => i.Uobject).Returns(MoqUobject.Object).Verifiable();
        MoqEnd.Setup(i => i.NameCommand).Returns("Turn").Verifiable();
        MoqEnd.Setup(i => i.Properties).Returns(new List<string> {"Angle"}).Verifiable();

        IoC.Resolve<ICommand>("Command.End", MoqEnd.Object).Execute();
        MoqEnd.VerifyAll();
    }

    [Fact]
    public void BridgeCommandTest()
    {
        var MoqFirstCommand = new Mock<ICommand>();
        MoqFirstCommand.Setup(i => i.Execute()).Verifiable();

        var MoqSecondCommand = new Mock<ICommand>();
        MoqSecondCommand.Setup(i=>i.Execute()).Verifiable();

        var BridgeCommand = new BridgeCommand(MoqFirstCommand.Object);
        BridgeCommand.Execute();

        MoqSecondCommand.Verify(i=>i.Execute(), Times.Never);
        BridgeCommand.Inject(MoqSecondCommand.Object);
        BridgeCommand.Execute();
        MoqSecondCommand.Verify(i=>i.Execute(), Times.Once);
    }

    [Fact]
    public void EmptyCommandTest()
    {
        var Empty = IoC.Resolve<ICommand>("Command.Empty");
        Empty.Execute();

        Assert.NotNull(Empty);
    }
}

