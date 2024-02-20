using Hwdtech;

namespace SpaceBattle.Lib;

public class CreateThreadList
{
    public void Call()
    {
        var stList = new Dictionary<int, ServerThread>();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Struct.ServerThread.List", (object[] args) =>
        {
            return stList;
        }).Execute();
    }
}