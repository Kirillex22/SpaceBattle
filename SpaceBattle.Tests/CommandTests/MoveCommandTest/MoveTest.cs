using SpaceBattle.Lib;
using TechTalk.SpecFlow;
using Moq;

namespace SpaceBattleTest;

[Binding]
public class MoveTest
{
    private readonly Mock<IMovable> m;

    private Action commandExecutionLambda;

    public MoveTest()
    {
        m = new Mock<IMovable>();

        commandExecutionLambda = () => { };
    }

    [Given(@"космический корабль находится в точке пространства с координатами \((.*), (.*)\)")]
    public void SetCoords(int x, int y)
    {
        m.SetupProperty(_m => _m.Position, new Vector(new int[] { x, y }));
    }

    [Given(@"имеет мгновенную скорость \((.*), (.*)\)")]
    public void SetVelocity(int x, int y)
    {
        m.SetupGet(_m => _m.Velocity).Returns(new Vector(new int[] { x, y }));
    }

    [Given(@"скорость корабля определить невозможно")]
    public void VelocityNan()
    {
        m.SetupGet(_m => _m.Velocity).Throws<NullReferenceException>();
    }

    [Given(@"изменить положение в пространстве космического корабля невозможно")]
    public void CordsChangeError()
    {
        m.SetupSet(_m => _m.Position).Throws<NullReferenceException>();
    }

    [Given(@"космический корабль, положение в пространстве которого невозможно определить")]
    public void CordsNan()
    {
        m.SetupGet(_m => _m.Position).Throws<NullReferenceException>();
    }

    [When(@"происходит прямолинейное равномерное движение без деформации")]
    public void Moving()
    {
        var mc = new MoveCommand(m.Object);
        commandExecutionLambda = () => mc.Execute();
    }

    [Then(@"космический корабль перемещается в точку пространства с координатами \((.*), (.*)\)")]
    public void NewCoords(int x, int y)
    {
        commandExecutionLambda();

        var expected = new Vector(new int[] { x, y });
        var result = m.Object.Position;

        Assert.Equal(expected.Coords, result.Coords);
    }

    [Then(@"возникает ошибка Exception")]
    public void ThrowingException()
    {
        Assert.Throws<NullReferenceException>(() => commandExecutionLambda());
    }
}

