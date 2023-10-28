using SpaceBattleLib;
using TechTalk.SpecFlow;
using Moq;
using System.Diagnostics.CodeAnalysis;

namespace SpaceBattleTest;

[Binding]
public class VectorTest
{ 
    private List<VectorModificated> vectors =  new List<VectorModificated>();
  
    [Given(@"вектор с координатами \((.*), (.*)\)")]
    public void CreateGoodVector(double x, double y)
    {
       vectors.Add(new VectorModificated(new double[] {x, y}));
    }

    [Given(@"двумерный вектор, одна из координат которого пуста")]
    public void CreateBadVector()
    {
         vectors.Add(new VectorModificated(new double[] {1, double.NaN}));
    }

    [Given(@"вектор с координатами \((.*), (.*), (.*)\)")]
    public void CreateBigVector(double x, double y, double z)
    {
        vectors.Add(new VectorModificated(new double[] {x, y, z}));
    }

    [When(@"происходит сложение векторов")]
    public void Sum()
    {

    }

    [Then(@"получаем вектор \((.*), (.*)\)")]
    public void ReturningVector(double x, double y)
    {
        var result = vectors[0] + vectors[1];
        var expected = new VectorModificated(new double[] {x, y});
        
        Assert.Equal(expected.ToList(), result.ToList());
    }   

    [Then(@"возникает ArgumentException")]
    public void ThrowingException()
    {
        Assert.Throws<ArgumentException>(() => vectors[0] + vectors[1]);
    }
}
