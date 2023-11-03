using SpaceBattleLib;
using TechTalk.SpecFlow;
using Moq;

namespace SpaceBattleTest;

[Binding]
public class VectorTest
{ 
    private List<Vector> vectors = new List<Vector>();
  
    [Given(@"вектор с координатами \((.*), (.*)\)")]
    public void CreateGoodVector(int x, int y)
    {
       vectors.Add(new Vector(new int[] {x, y}));
    }

    [Given(@"вектор с координатами \((.*), (.*), (.*)\)")]
    public void CreateBigVector(int x, int y, int z)
    {
        vectors.Add(new Vector(new int[] {x, y, z}));
    }

    [When(@"происходит сложение векторов")]
    public void Sum()
    {

    }

    [Then(@"получаем вектор \((.*), (.*)\)")]
    public void ReturningVector(int x, int y)
    {
        var result = vectors[0] + vectors[1];
        var expected = new Vector(new int[] {x, y});
        
        Assert.Equal(expected.Coords, result.Coords);
        vectors.Clear();
    }   

    [Then(@"возникает IndexOutOfRangeException")]
    public void ThrowingIndexOutOfRangeException()
    {
        Assert.Throws<IndexOutOfRangeException>(() => vectors[0] + vectors[1]);
        vectors.Clear();
    }
    
}
