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
            var start = (Vector)args[1];
            var step = (Vector)args[2];
            var objs = GeneratePosition((IEnumerable<IUObject>)args[0], start, step);
            return objs;
        }
        ).Execute();
    }

    private static IEnumerable<IUObject> GeneratePosition(IEnumerable<IUObject> objs, Vector start, Vector step)
    {
        foreach (var obj in objs)
        {
            var mv = IoC.Resolve<IMovable>("Game.Adapters.IMovable", obj);
            mv.Position = start; //в реализации set для свойства mv.Position будет использоваться IoС стратегия, которая обновит текущий obj
            start += step;
            yield return obj;
        }
    }
}
