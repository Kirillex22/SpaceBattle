using Hwdtech;
using Hwdtech.Ioc;
using Moq;

namespace SpaceBattle.Lib.Test;

public class GetUObjectStrategyTests
{
    Dictionary<int, IUObject> objMap = new Dictionary<int, IUObject>(){ {1, new Mock<IUObject>().Object} };

    public GetUObjectStrategyTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.UObject", (object[] args) => this.objMap).Execute();
    }

    [Fact]
    public void GetUObjectStrategySuccessful()
    {
        var id = 1;
        var strategy = new GetUObjectStrategy();
        var q = strategy.Run(id);

        Assert.Equal(this.objMap[id], q);
    }

    [Fact]
    public void GetUObjectStrategyUnsuccessful()
    {
        var falseID = 2;
        var strategy = new GetUObjectStrategy();

        Assert.Throws<Exception>(() => strategy.Run(falseID));
    }
}