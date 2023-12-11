namespace SpaceBattle.Lib;

public interface IBridgeCommand
{
    void Execute();
    void Inject(ICommand cmd);
}

