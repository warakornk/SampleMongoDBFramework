using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace SampleMongoDBFramework.Models.Entities
{
	public class Amphur
	{
		public string AmphurId { get; set; }
		public string AmphurName { get; set; }
	}
}