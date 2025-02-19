using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Product.Event.Handler.Service.Models
{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        [BsonElement(Order = 0)]
        public ObjectId Id { get; set; }


        [BsonRepresentation(BsonType.String)]
        [BsonElement(Order = 1)]
        public string ProductName { get; set; }



        [BsonRepresentation(BsonType.Int64)]
        [BsonElement(Order =2)]
        public int Count { get; set; }


        [BsonRepresentation(BsonType.Boolean)]
        [BsonElement(Order = 3)]
        public bool IsAvailable { get; set; }


        [BsonRepresentation(BsonType.String)]
        [BsonElement(Order = 4)]
        public decimal Price { get; set; }
    }
}
