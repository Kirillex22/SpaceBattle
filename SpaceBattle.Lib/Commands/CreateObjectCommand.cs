using System.Data.Common;
using Hwdtech;

namespace SpaceBattle.Lib;

public class CreateObjectCommand : ICommand
{
    private Guid _id;

    public CreateObjectCommand(Guid id) => _id = id;

    public void Execute()
    {
        IoC.Resolve<ICommand>("Game.Create.EmptyObject", _id).Execute();
    }
}

