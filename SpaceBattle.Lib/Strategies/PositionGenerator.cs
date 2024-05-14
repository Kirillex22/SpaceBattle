using System.Collections;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib;
public class PositionGenerator
{
    public void Call()
    {
        IoC.Resolve<Hwdtech.ICommand>(
        "IoC.Register",
        "Game.Generators.Movable.Position",
        (object[] args) =>
        {
            var objs = (List<IUObject>)args[0];
            var start = (Vector)args[1];
            var step = (Vector)args[2];

            var objsWithPosition = GeneratePosition(objs, start, step);
            return objsWithPosition.ToList();
        }
        ).Execute();
    }

    private IEnumerable<IUObject> GeneratePosition(List<IUObject> objs, Vector start, Vector step)
    {
        foreach (var obj in objs)
        {
            var mv = IoC.Resolve<IMovable>("Game.Adapters.IMovable", obj);
            mv.Position = start;
            start += step;
            yield return obj;
        }
    }
}

