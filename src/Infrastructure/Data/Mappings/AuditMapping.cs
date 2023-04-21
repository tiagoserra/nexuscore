using Domain.Core.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;

namespace Infrastructure.Data.Mappings;

public class AuditMapping
{
      public static void Configure()
        {
            BsonClassMap.RegisterClassMap<Audit>(map =>
            {
                map.MapIdProperty(c => c.Id).SetIdGenerator(StringObjectIdGenerator.Instance).SetSerializer(new StringSerializer(BsonType.ObjectId));
                map.MapMember(x => x.CreatedOn).SetIsRequired(true);
                map.MapMember(x => x.ExecutionContextId).SetIsRequired(true);
                map.MapMember(x => x.Request).SetIsRequired(true);
                map.MapMember(x => x.Response).SetIsRequired(true);
                map.MapMember(x => x.CommandName).SetIsRequired(true);
                map.MapMember(x => x.ExecutionTime).SetIsRequired(true);

                map.AutoMap();
                map.SetIgnoreExtraElements(true);
            });
        }
}