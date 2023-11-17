using SpaceBattleLib;
using TechTalk.SpecFlow;
using Moq;

namespace SpaceBattleTest;

[Binding]
public class VectorTest
{ 
    private List<Vector> vectors;
    private Vector operationResult;
    private Action vectorSumLambda;

    public VectorTest()
    {
        vectors = new List<Vector>();
        operationResult = new Vector(new int[]{});
        vectorSumLambda = () => {};
    }
    
  
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
        vectorSumLambda = () => operationResult = vectors[0] + vectors[1];
    }

    [Then(@"получаем вектор \((.*), (.*)\)")]
    public void ReturningVector(int x, int y)
    {
        vectorSumLambda();
        var result = operationResult;
        var expected = new Vector(new int[] {x, y});
        
        Assert.Equal(expected.Coords, result.Coords);
        vectors.Clear();
    }   

    [Then(@"возникает IndexOutOfRangeException")]
    public void ThrowingIndexOutOfRangeException()
    {
        Assert.Throws<IndexOutOfRangeException>(() => vectorSumLambda());
        vectors.Clear();
    }
    
}
