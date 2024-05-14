using Hwdtech;
namespace SpaceBattle.Lib;

public class EmptyUObjectsGenerator
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
            return objs.ToList();
        }
        ).Execute();
    }
    private IEnumerable<IUObject> GetEmptyUObjects(int countOfObjects)
    {
        var iter = 0;
        while (iter < countOfObjects)
        {
            var id = Guid.NewGuid();
            var obj = IoC.Resolve<IUObject>("Game.Create.EmptyObject", id);
            yield return obj;
            iter++;
        }
    }
}