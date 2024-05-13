using System.Collections;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib;
public class InitialPositionSetter
{
    public void Call()
    {
        IoC.Resolve<Hwdtech.ICommand>(
        "IoC.Register",
        "Game.IUObject.SetStartPosition",
        (object[] args) =>
        {
            var objs = (IUObject[])args[0];
            //var updObjs = GetObjsWithUpdPos(objs);

            var fPlayerObjs = new ArraySegment<IUObject>(objs, 0, objs.Length / 2 + 1).ToArray();
            var sPlayerObjs = new ArraySegment<IUObject>(objs, objs.Length / 2 + 1, objs.Length).ToArray();

            var fPlayerUpdObjs = GetObjsWithUpdPos(fPlayerObjs, new Vector(new int[] { 0, 0 }), new Vector(new int[] { 1, 0 }));
            var sPlayerUpdObjs = GetObjsWithUpdPos(sPlayerObjs, new Vector(new int[] { 0, 1 }), new Vector(new int[] { 1, 0 }));

            var updObjs = fPlayerUpdObjs.Concat(sPlayerUpdObjs).ToArray();

            return updObjs;
        }
        ).Execute();
    }


    private static IEnumerable<IUObject> GetObjsWithUpdPos(IUObject[] objs, Vector start, Vector step)
    {
        var len = objs.Length;
        var index = 0;

        while (index < len)
        {
            IoC.Resolve<ICommand>("Game.IUObject.SetProperty", objs[index], "Position", start).Execute();
            start += step;
            yield return objs[index];
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