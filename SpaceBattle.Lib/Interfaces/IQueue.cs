namespace SpaceBattle.Lib;

public interface IQueue 
{
    public void Push(ICommand cmd);
    public ICommand Take();
}
