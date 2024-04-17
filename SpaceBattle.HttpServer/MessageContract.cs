namespace SpaceBattle.HttpServer;

public class MessageContract
{
    public string Type { get; set; } = "";
    public string GameId { get; set; } = "";
    public string ItemId { get; set; } = "";
    public Dictionary<string, object> InitialValues { get; set; }
}