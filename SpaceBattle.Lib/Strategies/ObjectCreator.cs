using System.Collections;
using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib;
public class ObjectCreator
{
    public void Call()
    {
        IoC.Resolve<Hwdtech.ICommand>(
        "IoC.Register",
        "Game.Create.EmptyObject",
        (object[] args) =>
        {
            var act = () =>
            {
                var id = (Guid)args[0];
                var obj = IoC.Resolve<IUObject>("Game.Create.IUObject");
                IoC.Resolve<ICommand>("Game.IUObject.Container.Push", id, obj).Execute();
            };

            return new ActionCommand(act);
        }
        ).Execute();
    }
}

