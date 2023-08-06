using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace SampleMongoDBFramework.Models.Entities
{
	public class Province
	{
		[BsonId][BsonRepresentation(BsonType.ObjectId)] public ObjectId Id { get; set; }
		public string ProvinceId { get; set; }
		public string ProvinceName { get; set; }
		public List<Amphur> Amphurs { get; set; }
	}
}