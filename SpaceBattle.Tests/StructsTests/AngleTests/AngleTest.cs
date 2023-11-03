using SpaceBattle.Lib;
using TechTalk.SpecFlow;

namespace SpaceBattle.Tests;

[Binding]
public class AngleTest
{ 
    private List<Angle> angles = new List<Angle>();
    private Angle result;

    [Given(@"имеется угол \((.*)\) градусов")]
    public void SetAngle(int x)
    {
        angles.Add(new Angle(x/45));
    }

    [When("происходит сложение векторов")]
    public void Sum()
    {
        result = angles[0] + angles[1];
    }

    [Then(@"получается угол \((.*)\) градусов")]
    public void NewAngle(int y)
    {
        var expect = new Angle(y/45);
        Assert.Equal(expect.ToString(), result.ToString());
    }
   
}
