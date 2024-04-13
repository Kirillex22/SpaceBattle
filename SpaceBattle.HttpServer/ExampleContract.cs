using System.Collections.Generic;
using System.Runtime.Serialization;
using CoreWCF.OpenApi.Attributes;

namespace SpaceBattle.HttpServer;

    [DataContract(Name = "ExampleContract", Namespace = "http://example.com")]
    internal class ExampleContract
    {
        [DataMember(Name = "Type", Order = 1)]
        [OpenApiProperty(Description = "type of game command")]
        public string Type { get; set; }

        [DataMember(Name = "GameId", Order = 2)]
        [OpenApiProperty(Description = "id of the target game")]
        public string GameId { get; set; }

        [DataMember(Name = "GameItemId", Order = 3)]
        [OpenApiProperty(Description = "id of the target game")]
        public string GameItemId { get; set; }


        [DataMember(Name = "InitialValues", Order = 4)]
        [OpenApiProperty(Description = "initial values for the game command")]
        public IDictionary<string, object> InitalValues { get; set; }
    }

