using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib;

public class RegisterExcHandler : ICommand
{
    private object _cmd;
    private object _exc;
    private ICommand _handler;

    public RegisterExcHandler(object cmd, object exc, ICommand handler)
    {
        _cmd = cmd;
        _exc = exc;
        _handler = handler;
    } 

    public void Execute()
    {
        var BuildExceptionTree = IoC.Resolve<Dictionary<object, object>>("Game.Get.ExceptionTree");
        var ExceptionTree = BuildExceptionTree;
        ExceptionTree.TryAdd(_cmd.ToString(), new Dictionary<object, object>());
        ExceptionTree = (Dictionary<object, object>)ExceptionTree[_cmd.ToString()];
        ExceptionTree.TryAdd(_exc.ToString(), _handler);
    }
}

