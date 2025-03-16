using HelteOgHulerServer.Events;
using HelteOgHulerShared.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

static class DatabaseSetupUtility
{
    public static void SetupDatabaseSerializers()
    {
#pragma warning disable CS0618 // Type or member is obsolete
        BsonDefaults.GuidRepresentationMode = GuidRepresentationMode.V3;
#pragma warning restore CS0618 // Type or member is obsolete

        // Allow all mongodb serialization
        BsonSerializer.RegisterSerializer(new ObjectSerializer(ObjectSerializer.AllAllowedTypes));
        BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
        BsonSerializer.RegisterSerializer(new EnumSerializer<ActionName>(BsonType.String));

        // Register mongodb event types
        BsonClassMap.RegisterClassMap<Inn>();
        BsonClassMap.RegisterClassMap<Hero>();
        BsonClassMap.RegisterClassMap<Monster>();
        BsonClassMap.RegisterClassMap<AdventureEvent_V1>();
        BsonClassMap.RegisterClassMap<NewPlayerEvent_V1>();
        BsonClassMap.RegisterClassMap<RecruitHeroEvent_V1>();
        BsonClassMap.RegisterClassMap<UpgradeInnEvent_V1>();
        BsonClassMap.RegisterClassMap<CompleteObjectiveEvent_V1>();

        BsonClassMap.RegisterClassMap<GameState>(gameState =>
        {
            gameState.AutoMap();

            var playerSerializer = new DictionaryInterfaceImplementerSerializer<
                Dictionary<Guid, Player>
            >(
                dictionaryRepresentation: MongoDB
                    .Bson
                    .Serialization
                    .Options
                    .DictionaryRepresentation
                    .Document,
                keySerializer: new GuidToStringSerializer(),
                valueSerializer: BsonSerializer.SerializerRegistry.GetSerializer<Player>()
            );

            var playerPublicSerializer = new DictionaryInterfaceImplementerSerializer<
                Dictionary<Guid, PlayerPublic>
            >(
                dictionaryRepresentation: MongoDB
                    .Bson
                    .Serialization
                    .Options
                    .DictionaryRepresentation
                    .Document,
                keySerializer: new GuidToStringSerializer(),
                valueSerializer: BsonSerializer.SerializerRegistry.GetSerializer<PlayerPublic>()
            );

            gameState.GetMemberMap(g => g.PrivatePlayerDict).SetSerializer(playerSerializer);
            gameState.GetMemberMap(g => g.PublicPlayerDict).SetSerializer(playerPublicSerializer);
        });
    }
}

class GuidToStringSerializer : IBsonSerializer
{
    public Type ValueType => typeof(Guid);

    public object Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        var code = context.Reader.ReadString();

        return new Guid(code);
    }

    public void Serialize(
        BsonSerializationContext context,
        BsonSerializationArgs args,
        object value
    )
    {
        context.Writer.WriteString(value.ToString());
    }
}
