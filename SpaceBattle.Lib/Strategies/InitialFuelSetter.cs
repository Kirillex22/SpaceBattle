using System.Collections;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib;
public class InitialFuelSetter
{
    public void Call()
    {
        IoC.Resolve<Hwdtech.ICommand>(
        "IoC.Register",
        "Game.IUObject.SetStartFuelCapacity",
        (object[] args) =>
        {
            var objs = (IUObject[])args[0];
            var fuelCapacity = (int)args[1];
            var updObjs = GetObjsWithUpdFuel(objs, fuelCapacity);

            return updObjs;
        }
        ).Execute();
    }


    private static IEnumerable<IUObject> GetObjsWithUpdFuel(IUObject[] objs, int fuelCapacity)
    {
        var len = objs.Length;
        var index = 0;

        while (index < len)
        {
            IoC.Resolve<ICommand>("Game.IUObject.SetProperty", objs[index], "Fuel", fuelCapacity).Execute();
            yield return objs[index];
        }
    }
}