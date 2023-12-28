using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib;

public class RegisterExcHandler : ICommand
{
    private Type _cmd;
    private Type _exc;
    private ICommand _handler;

    public RegisterExcHandler(Type cmd, Type exc, ICommand handler)
    {
        _cmd = cmd;
        _exc = exc;
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

