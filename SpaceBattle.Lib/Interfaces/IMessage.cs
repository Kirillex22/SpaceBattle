namespace SpaceBattle.Lib;

public interface IMessage
{
    public string CommandType {get; }
    public int GameId {get; }
    public int ItemId {get; }
    public IDictionary<string, object> Properties {get; }
}

