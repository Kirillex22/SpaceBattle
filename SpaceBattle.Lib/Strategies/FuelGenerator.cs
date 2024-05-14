using System.Collections;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib;
public class FuelGenerator
{
    public void Call()
    {
        IoC.Resolve<Hwdtech.ICommand>(
        "IoC.Register",
        "Game.Generators.Fuelable.Capacity",
        (object[] args) =>
        {
            var cpcty = (int)args[1];
            var objs = GenerateFuel((IEnumerable<IUObject>)args[0], cpcty);
            return objs;
        }
        ).Execute();
    }

    private static IEnumerable<IUObject> GenerateFuel(IEnumerable<IUObject> objs, int capacity)
    {
        foreach (var obj in objs)
        {
            var fble = IoC.Resolve<IFuelable>("Game.Adapters.IFuelable", obj);
            fble.Capacity = capacity; //в реализации set для свойства fble.Capacity будет использоваться IoС стратегия, которая обновит текущий obj
            yield return obj;
        }
    }
}