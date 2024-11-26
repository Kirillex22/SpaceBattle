using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib;

public class RegisterExcHandler : ICommand
{
    private object _cmd;
    private object _exc;
    private ICommand _handler;

    public RegisterExcHandler(ICommand handler, object cmd = null, object exc = null)
    {
        _cmd = cmd ?? "default";
        _exc = exc ?? "default";
        _handler = handler;
    } 

    public void Execute()
    {
        var BuildExceptionTree = IoC.Resolve<Dictionary<object, object>>("Game.Get.ExceptionTree");
        var ExceptionTree = BuildExceptionTree;
        ExceptionTree.TryAdd(_cmd, new Dictionary<object, object>());
        ExceptionTree = (Dictionary<object, object>)ExceptionTree[_cmd];
        ExceptionTree.TryAdd(_exc, _handler);
    }
}

