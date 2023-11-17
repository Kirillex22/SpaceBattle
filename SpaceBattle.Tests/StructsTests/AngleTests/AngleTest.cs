using SpaceBattle.Lib;
using TechTalk.SpecFlow;

namespace SpaceBattle.Tests;

[Binding]
public class AngleTest
{ 
    private List<Angle> angles = new List<Angle>();
    private Angle sum;
    private Action lambda;

    [Given(@"имеется угол \((.*)\) градусов")]
    public void SetAngle(int x)
    {
        angles.Add(new Angle(x/45, 8));
    }

    [When("происходит сложение векторов")]
    public void Sum()
    {
        lambda = () =>  this.sum = this.angles[0] + this.angles[1];
    }

    [Then(@"получается угол \((.*)\) градусов")]
    public void NewAngle(int y)
    {
        lambda();
        var expect = new Angle(y/45, 8);
        var result = sum;
        
        Assert.Equal(expect.ToString(), result.ToString());
    }
    
    [Given(@"имеется другой угол \((.*)\) градусов")]
    public void SetAngle2(int x)
    {
        angles.Add(new Angle(x/72, 5));
    }

    [Then("возникает ошибка Exception ")]
    public void ThrowException()
    {
        Assert.ThrowsAny<Exception>(() => lambda());
    }
}

