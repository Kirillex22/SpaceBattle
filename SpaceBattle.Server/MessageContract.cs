using CoreWCF;
using System;
using System.Runtime.Serialization;

namespace SpaceBattle.Server;

[DataContract]
public class MessageContract
{
    [DataMember(Name = "Type", Order = 1)]
    public string Type { get; set; } = "";

    [DataMember(Name = "GameId", Order = 2)]
    public string GameId { get; set; } = "";

    [DataMember(Name = "ItemId", Order = 3)]
    public string ItemId { get; set; } = "";

    [DataMember(Name = "InitialValues", Order = 4)]
    public Dictionary<string, object>? InitialValues { get; set; }
}