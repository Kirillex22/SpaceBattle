using System.Collections;
using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib;
public class GameObjectsContainer
{
    private Dictionary<Guid, IUObject> gameObjects = new();
    public void Call()
    {
        IoC.Resolve<Hwdtech.ICommand>(
        "IoC.Register",
        "Game.Container.Push",
        (object[] args) =>
        {
            var id = (Guid)args[0];
            var obj = (IUObject)args[1];
            gameObjects[id] = obj;
        }
        ).Execute();

        IoC.Resolve<Hwdtech.ICommand>(
        "IoC.Register",
        "Game.Container.Get",
        (object[] args) =>
        {
            var id = (Guid)args[0];
            return gameObjects[id];
        }
        ).Execute();
    }
}