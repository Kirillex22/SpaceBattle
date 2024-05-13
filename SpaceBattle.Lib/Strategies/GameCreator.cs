using System.Collections;
using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib;
public class GameCreator
{
    public void Call()
    {
        IoC.Resolve<Hwdtech.ICommand>(
        "IoC.Register",
        "Game.CreateInitialState",
        (object[] args) =>
        {
            var countOfObjects = (int)args[0];

            var objs = GetEmptyUObjects(countOfObjects);

            objs = IoC.Resolve<IUObject[]>("Game.IUObject.SetStartPosition", objs);
            objs = IoC.Resolve<IUObject[]>("Game.IUObject.SetStartFuelCapacity", objs);
        }
        ).Execute();
    }

    private static IEnumerable<IUObject> GetEmptyUObjects(int countOfObjects)
    {
        var iter = 0;

        while (iter < countOfObjects)
        {
            var obj = IoC.Resolve<IUObject>("Game.Create.EmptyObject");
            iter++;
            yield return obj;
        }

        // IoC.Resolve<ICommand>("Game.IUObject.SetProperty", objs[0], "Position", new Vector(new int[] { 0, 0 })).Execute();
        // yield return objs[0];
        // IoC.Resolve<ICommand>("Game.IUObject.SetProperty", objs[1], "Position", new Vector(new int[] { 1, 0 })).Execute();
        // yield return objs[1];
        // IoC.Resolve<ICommand>("Game.IUObject.SetProperty", objs[2], "Position", new Vector(new int[] { 2, 0 })).Execute();
        // yield return objs[2];
        // IoC.Resolve<ICommand>("Game.IUObject.SetProperty", objs[3], "Position", new Vector(new int[] { 1, 1 })).Execute();
        // yield return objs[3];
        // IoC.Resolve<ICommand>("Game.IUObject.SetProperty", objs[4], "Position", new Vector(new int[] { 1, 2 })).Execute();
        // yield return objs[4];
        // IoC.Resolve<ICommand>("Game.IUObject.SetProperty", objs[5], "Position", new Vector(new int[] { 1, 3 })).Execute();
        // yield return objs[5];
    }
}



