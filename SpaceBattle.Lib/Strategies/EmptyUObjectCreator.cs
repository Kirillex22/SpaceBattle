using System.Collections;
using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib;
public class EmptyUObjectCreator
{
    private GameObjectsContainer _container;

    public EmptyUObjectCreator() => _container = new();
    public void Call()
    {
        IoC.Resolve<Hwdtech.ICommand>(
        "IoC.Register",
        "Game.Create.EmptyObject",
        (object[] args) =>
        {
            return new UObject();
        }
        ).Execute();
    }
}