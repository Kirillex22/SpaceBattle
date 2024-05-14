using System.Collections;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib;

public class InitialFuelSetter
{
    private FuelGenerator _fuelGen = new();

    public InitialFuelSetter() => _fuelGen.Call();

    public void Call()
    {
        IoC.Resolve<Hwdtech.ICommand>(
        "IoC.Register",
        "Game.Initialize.StartFuelCapacity",
        (object[] args) =>
        {
            var objs = (List<IUObject>)args[0];
            var capacity = (int)args[1];
            var updObjs = IoC.Resolve<List<IUObject>>("Game.Generators.Fuelable.Capacity", objs, capacity);
            return updObjs;
        }
        ).Execute();
    }
}

