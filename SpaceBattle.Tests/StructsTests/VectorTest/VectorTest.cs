using SpaceBattleLib;
using TechTalk.SpecFlow;
using Moq;

namespace SpaceBattleTest;

[Binding]
public class VectorTest
{ 
    private List<Vector> vectors =  new List<Vector>();
  
    [Given(@"вектор с координатами \((.*), (.*)\)")]
    public void CreateGoodVector(double x, double y)
    {
       vectors.Add(new Vector(new double[] {x, y}));
    }

    [Given(@"двумерный вектор, одна из координат которого пуста")]
    public void CreateBadVector()
    {
         vectors.Add(new Vector(new double[] {1, double.NaN}));
    }

    [Given(@"вектор с координатами \((.*), (.*), (.*)\)")]
    public void CreateBigVector(double x, double y, double z)
    {
        vectors.Add(new Vector(new double[] {x, y, z}));
    }

    [When(@"происходит сложение векторов")]
    public void Sum()
    {

    }

    [Then(@"получаем вектор \((.*), (.*)\)")]
    public void ReturningVector(double x, double y)
    {
        var result = vectors[0] + vectors[1];
        var expected = new Vector(new double[] {x, y});
        
        Assert.Equal(expected.coords, result.coords);
        vectors.Clear();
    }   

    [Then(@"возникает ArgumentException")]
    public void ThrowingArgException()
    {
        Assert.Throws<ArgumentException>(() => vectors[0] + vectors[1]);
        vectors.Clear();
    }

    [Then(@"возникает IndexOutOfRangeException")]
    public void ThrowingIndexOutOfRangeException()
    {
        Assert.Throws<IndexOutOfRangeException>(() => vectors[0] + vectors[1]);
        vectors.Clear();
    }
    
}
