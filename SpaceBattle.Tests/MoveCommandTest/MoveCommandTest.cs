using SpaceBattleLib;
using TechTalk.SpecFlow;
using Moq;

namespace SpaceBattleTest;

[Binding]
public class ConstantMotionTest
{ 
    
    [Given(@"космический корабль находится в точке пространства с координатами (12, 5)")]
    public void SetCoords()
    {

    }

    [Given(@"имеет мгновенную скорость (-5, 3)")]
    public void SetVelocity()
    {

    }

    [Given(@"скорость корабля определить невозможно")] 
    public void VelocityNan()
    {

    }

    [Given(@"изменить положение в пространстве космического корабля невозможно")]
    public void CordsChangeError()
    {

    }

    [Given(@"космический корабль, положение в пространстве которого невозможно определить")]
    public void CordsNan()
    {
        
    }

    [When(@"происходит прямолинейное равномерное движение без деформации")]
    public void Moving()
    {
        
    }

    [Then(@"космический корабль перемещается в точку пространства с координатами (7, 8)")]
    public void NewCoords()
    {
        
    }   

    [Then(@"возникает ошибка Exception ")]
    public void ThrowingException()
    {
        
    }
 
    

}
