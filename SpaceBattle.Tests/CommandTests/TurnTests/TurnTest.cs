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
        mq.SetupProperty(_mq => _mq.Angle, new Angle(x/45, 8)); 
    }

    [Given(@"имеет угловую скорость \((.*)\) градусов")]
    public void SetAngleVelocity(int v)
    {
        mq.SetupGet(_mq => _mq.AngleVelocity).Returns(new Angle(v/45, 8));
    }


    [When(@"происходит поворот вокруг собственной оси")]
    public void Turning()
    {
        turn = new(mq.Object);
    }

    [Then(@"космический корабль оказывается под углом \((.*)\) градусов к горизонту")]
    public void NewCoords(int x)
    {
        turn.Execute();

        var expect = new Angle(x/45, 8);
        var result = mq.Object.Angle;
        
        Assert.Equal(expect.ToString(), result.ToString());
    }  

    [Given(@"космический корабль, угол наклона к горизонту которого невозможно определить")] 
    public void NanAngle()
    {
        mq.SetupGet(_mq => _mq.Angle).Throws<NullReferenceException>();
    }

    [Then(@"возникает ошибка Exception")]
    public void ThrowException()
    {
        Assert.Throws<NullReferenceException>(() => turn.Execute());
    }

    [Given(@"угловую скорость корабля определить неозможно")]
    public void NanAngleVelocity()
    {
        mq.SetupGet(_mq => _mq.AngleVelocity).Throws<NullReferenceException>();
    }

    [Given(@"изменить угол наклона к горизонту невозможно")]
    public void NoneChangeCoords()
    {
        mq.SetupSet(_mq => _mq.Angle).Throws<NullReferenceException>();
    }
}

