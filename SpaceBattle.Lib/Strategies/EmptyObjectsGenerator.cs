using Hwdtech;
namespace SpaceBattle.Lib;

public class EmptyObjectsGenerator
{
    public void Call()
    {
        IoC.Resolve<Hwdtech.ICommand>(
        "IoC.Register",
        "Game.Generators.IUObject",
        (object[] args) =>
        {
            var countOfObjects = (int)args[0];

            var objs = GetEmptyUObjects(countOfObjects);
            return objs;
        }
        ).Execute();
    }
    private static IEnumerable<IUObject> GetEmptyUObjects(int countOfObjects)
    {
        var iter = 0;
        while (iter < countOfObjects)
        {
            var id = new Guid();
            var obj = IoC.Resolve<IUObject>("Game.Create.EmptyObject", id);
            iter++;
            yield return obj;
        }
    }
}