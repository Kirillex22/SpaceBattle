using SpaceBattle.Lib;
using TechTalk.SpecFlow;
using Moq;

namespace SpaceBattle.Tests;

[Binding]
public class TurnTest
{ 
    
    private Mock<ITurnable> mq = new Mock<ITurnable>();

    private Turn turn;


    [Given(@"космический корабль находится под углом к горизонту в \((.*)\) градусов")]
    public void SetAngle(int x)
    {
        x /= 45;
        mq.SetupProperty(_mq => _mq.angle, new Angle(x, 8)); 
    }

    [Given(@"имеет угловую скорость \(.*\) градусов")]
    public void SetAngleVelocity(int v)
    {
        v /= 45;
        mq.SetupGet(_mq => _mq.angle_velocity).Returns(new Angle(v, 8));
    }


    [When(@"происходит поворот вокруг собственной оси")]
    public void Turning()
    {
        turn = new(mq.Object);
    }

    [Then(@"космический корабль оказывается под углом \(.*\) градусов к горизонту")]
    public void NewCoords(int x)
    {
        turn.Execute();

        var expect = new Angle(x, 8);
        var result = mq.Object.angle;
        
        Assert.Equal(expect, result);
    }  

    [Given(@"космический корабль, угол наклона к горизонту которого невозможно определить")] 
    public void NanAngle()
    {
        mq.SetupGet(_mq => _mq.angle).Throws<NullReferenceException>();
    }

    [Then(@"возникает ошибка Exception")]
    public void ThrowException()
    {
        Assert.Throws<NullReferenceException>(() => turn.Execute());
    }

    [Given(@"угловую скорость корабля определить неозможно")]
    public void NanAngleVelocity()
    {
        mq.SetupGet(_mq => _mq.angle_velocity).Throws<NullReferenceException>();
    }

    [Given(@"изменить угол наклона к горизонту невозможно")]
    public void NoneChangeCoords()
    {
        mq.Setup(_mq => _mq.angle).Throws<NullReferenceException>();
    }
}
