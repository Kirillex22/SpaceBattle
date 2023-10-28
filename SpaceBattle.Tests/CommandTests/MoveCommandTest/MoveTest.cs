using SpaceBattleLib;
using TechTalk.SpecFlow;
using Moq;

namespace SpaceBattleTest;

[Binding]
public class MoveTest
{ 
    private Mock<IMovable> m = new Mock<IMovable>();
    private MoveCommand mc;

    
    [Given(@"космический корабль находится в точке пространства с координатами \((.*), (.*)\)")]
    public void SetCoords(double x, double y)
    {
        m.SetupProperty(_m => _m.position, new Vector(x, y)); 
    }

    [Given(@"имеет мгновенную скорость \((.*), (.*)\)")]
    public void SetVelocity(double x, double y)
    {
         m.SetupGet(_m => _m.velocity).Returns(new Vector(x, y));
    }

    [Given(@"скорость корабля определить невозможно")] 
    public void VelocityNan()
    {
        m.SetupGet(_m => _m.velocity).Throws<NullReferenceException>();
    }

    [Given(@"изменить положение в пространстве космического корабля невозможно")]
    public void CordsChangeError()
    {
        m.Setup(_m => _m.position).Throws<NullReferenceException>();
    }

    [Given(@"космический корабль, положение в пространстве которого невозможно определить")]
    public void CordsNan()
    {
        m.SetupGet(_m => _m.position).Throws<NullReferenceException>();
    }

    [When(@"происходит прямолинейное равномерное движение без деформации")]
    public void Moving()
    {
        mc = new(m.Object);
    }

    [Then(@"космический корабль перемещается в точку пространства с координатами \((.*), (.*)\)")]
    public void NewCoords(double x, double y)
    {
        mc.Execute();

        var expected = new Vector(x, y);
        var result = m.Object.position;
        
        Assert.Equal(expected.ToList(), result.ToList());
    }   

    [Then(@"возникает ошибка Exception")]
    public void ThrowingException()
    {
        Assert.Throws<NullReferenceException>(() => mc.Execute());
    }
}
